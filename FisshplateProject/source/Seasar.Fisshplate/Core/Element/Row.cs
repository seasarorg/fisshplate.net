using System;
using System.Collections.Generic;

using System.Text;
using Seasar.Fisshplate.Wrapper;
using Seasar.Fisshplate.Parser.Handler;
using NPOI.HSSF.UserModel;
using Seasar.Fisshplate.Consts;

namespace Seasar.Fisshplate.Core.Element
{
    public class Row : TemplateElement
    {
        private IList<TemplateElement> _cellElementList = new List<TemplateElement>();
        private short _rowHeight;
        private Root _root;

        /// <summary>
        /// コンストラクタです。テンプレート側の行オブジェクトを受け取り、その行内のセル情報を解析して保持します。
        /// </summary>
        /// <param name="templateRow">テンプレート側の行オブジェクト</param>
        /// <param name="root">自分自身が属してるルート要素クラス</param>
        /// <param name="cellParserHandler">セルを解析するクラス</param>
        public Row(RowWrapper templateRow, Root root, CellParserHandler cellParserHandler)
        {
            this._root = root;
            if (templateRow.IsNullRow())
            {
                this._rowHeight = templateRow.Sheet.HSSFSheet.DefaultRowHeight;
                _cellElementList.Add(new NullElement());
                return;
            }
            HSSFRow hssfRow = templateRow.HSSFRow;
            this._rowHeight = hssfRow.Height;
            for (int i = 0; i < templateRow.CellCount; i++)
            {
                CellWrapper templateCell = templateRow.GetCell(i);
                TemplateElement element = cellParserHandler.GetElement(templateCell);
                _cellElementList.Add(element);
            }
        }



        #region TemplateElement メンバ

        public void Merge(Seasar.Fisshplate.Context.FPContext context)
        {
            // ヘッダ・フッタ制御
            if (context.ShouldHeaderOut)
            {
                context.ShouldHeaderOut = false;
                _root.PageHeader.Merge(context);
            }
            context.ShouldFooterOut = true;

            HSSFRow outRow = context.CreateCurrentRow();
            outRow.Height = _rowHeight;
            IDictionary<string, object> data = context.Data;
            data[FPConsts.RowNumberName] = context.CurrentRowNum + 1;
            foreach (TemplateElement elem in _cellElementList)
            {
                elem.Merge(context);
            }
            context.NextRow();
        }

        #endregion

        public short RowHeight
        {
            get { return _rowHeight; }
        }

        public IList<TemplateElement> CellElementList
        {
            get { return _cellElementList; }
        }
    }
}
