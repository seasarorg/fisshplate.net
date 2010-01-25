using System;
using System.Collections.Generic;

using System.Text;

namespace Seasar.Fisshplate.Parser.Handler
{
    public class RowParserHandler
    {
        private RowParser[] _buildInRowParsers = 
        {
            new IteratorBlockParser(),
            new IfBlockParser(),
            new ElseIfBlockParser(),
            new ElseBlockParser(),
            new EndParser(),
            new PageBreakParser(),
            new CommentParser(),
            new VarParser(),
            new ExecParser(),
            new PageHeaderBlockParser(),
            new PageFooterBlockParser(),
            new ResumeParser(),
            new WhileParser(),
            new HorizontalIteratorBlockParser()
        };

        private IList<RowParser> _addOnRowParser = new List<RowParser>();

        /// <summary>
        /// 独自にカスタマイズしたパーサを追加します。
        /// </summary>
        /// <param name="parser"></param>
        public void AddRowParser(RowParser parser)
        {
            _addOnRowParser.Add(parser);
        }

        /// <summary>
        /// 自身に登録されたRowParserを使ってcellを解析します。
        /// 解析対象であればTemplateElementを生成し、
        /// 呼び出し元FPParserに追加してtrueを戻します。
        /// 解析対象外であれば、falseを戻します。
        /// </summary>
        /// <param name="cell">パースするセル</param>
        /// <param name="fPParser">呼び出し元FPParser</param>
        /// <returns>パース対象であればtrue</returns>
        /// <exception cref="Seasar.Fisshplate.Exception.FPParseException">解析時にエラーが発生した際に投げられます。 </exception>
        public bool Parse(Seasar.Fisshplate.Wrapper.CellWrapper cell, FPParser fPParser)
        {
            foreach (RowParser rowParser in _buildInRowParsers)
            {
                if (rowParser.Process(cell, fPParser))
                {
                    return true;
                }
            }
            foreach (RowParser rowParser in _addOnRowParser)
            {
                if (rowParser.Process(cell, fPParser))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
