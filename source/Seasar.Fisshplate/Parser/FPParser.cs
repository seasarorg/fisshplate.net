using System;
using System.Collections.Generic;

using System.Text;
using Seasar.Fisshplate.Parser.Handler;
using Seasar.Fisshplate.Core.Element;
using Seasar.Fisshplate.Wrapper;
using NPOI.HSSF.UserModel;
using Seasar.Fisshplate.Exception;
using Seasar.Fisshplate.Consts;

namespace Seasar.Fisshplate.Parser
{
    public class FPParser
    {
        private Root _rootElement;
        private Stack<AbstractBlock> _blockStack = new Stack<AbstractBlock>();

        private RowParserHandler _rowParserHandler = new RowParserHandler();
        private CellParserHandler _cellParserHandler = new CellParserHandler();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FPParser()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        /// <exception cref="Seasar.Fisshplate.Exception.FPParseException"></exception>
        public Root Parse(SheetWrapper sheet)
        {
            _rootElement = new Root();
            for (int i = 0; i < sheet.RowCount; i++)
            {
                ParseRow(sheet.GetRow(i));
            }
            // スタックにまだブロックが残ってたら#end不足
            if (_blockStack.Count > 0)
            {
                throw new FPParseException(FPConsts.MessageIdEndElement, new object[]{"?"});
            }

            return _rootElement;
        }

        /// <summary>
        /// ルートの要素リストを戻します。
        /// </summary>
        public Root Root
        {
            get { return _rootElement; }
        }

        /// <summary>
        /// 独自にカスタマイズした行単位のパーサーを追加します。
        /// </summary>
        /// <param name="rowParser">追加するパーサー</param>
        public void AddRowParser(RowParser parser)
        {
            _rowParserHandler.AddRowParser(parser);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <exception cref="Seasar.Fisshplate.Exception.FPParseException"></exception>
        private void ParseRow(RowWrapper row)
        {
            if (row.IsNullRow())
            {
                CreateRowElement(row);
                return;
            }

            for (int i = 0; i < row.CellCount; i++)
            {
                CellWrapper cell = row.GetCell(i);

                if (IsCellParsable(cell) == false)
                {
                    continue;
                }
                if (_rowParserHandler.Parse(cell, this))
                {
                    return;
                }
            }
            CreateRowElement(row);
        }

        private bool IsCellParsable(CellWrapper cell)
        {
            if (cell == null)
            {
                return false;
            }
            HSSFCell hssfCell = cell.HSSFCell;
            if (hssfCell == null || (hssfCell.CellType != HSSFCell.CELL_TYPE_STRING))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// ブロック要素に親要素がある場合、その親要素にブロック要素を子要素として追加します。
        /// </summary>
        /// <param name="block">ブロック要素</param>
        public void AddBlockElement(AbstractBlock block)
        {
            if (IsBlockStackBlank() == false)
            {
                AbstractBlock parentBlock = _blockStack.Peek();
                parentBlock.AddChild(block);
            }
            PushBlockToStack(block);
        }

        /// <summary>
        /// ブロックの閉じ判定用スタックにブロック要素を追加します。
        /// </summary>
        /// <param name="block">ブロック要素</param>
        public void PushBlockToStack(AbstractBlock block)
        {
            _blockStack.Push(block);
        }

        private void CreateRowElement(RowWrapper row)
        {
            Row rowElem = new Row(row, _rootElement, _cellParserHandler);
            AddTemplateElement(rowElem);
        }

        /// <summary>
        /// 要素を親要素があれば子要素として追加します。親要素がなければルートにボディ要素として追加します。
        /// </summary>
        /// <param name="elem">要素</param>
        public void AddTemplateElement(TemplateElement elem)
        {
            if (IsBlockStackBlank() == false)
            {
                AbstractBlock block = _blockStack.Peek();
                block.AddChild(elem);
            }
            else
            {
                _rootElement.AddBody(elem);
            }
        }

        /// <summary>
        /// ブロックの閉じ判定用スタックが空か否かを戻します。
        /// </summary>
        /// <returns>空ならばtrue。</returns>
        public bool IsBlockStackBlank()
        {
            return (_blockStack.Count < 1);
        }

        /// <summary>
        /// ブロックの閉じ判定用スタックからポップします。
        /// </summary>
        /// <returns>ブロック要素</returns>
        public AbstractBlock PopFromBlockStack()
        {
            return _blockStack.Pop();
        }

        /// <summary>
        /// ブロックの閉じ判定用スタックから最後の要素を取得して戻します。 
        /// </summary>
        /// <returns>最後の要素</returns>
        public AbstractBlock LastElementFromStack
        {
            get { return _blockStack.Peek(); }
        }
    }
}
