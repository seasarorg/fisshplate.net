using System;
using System.Collections.Generic;

using System.Text;
using System.Text.RegularExpressions;
using Seasar.Fisshplate.Exception;
using Seasar.Fisshplate.Consts;
using Seasar.Fisshplate.Core.Element;

namespace Seasar.Fisshplate.Parser
{
    public class EndParser : RowParser
    {
        private static readonly Regex _patEnd = new Regex(@"(^\s*#end\s*|#pageHeaderEnd|#pageFooterEnd)");
        #region RowParser メンバ

        public bool Process(Seasar.Fisshplate.Wrapper.CellWrapper cell, FPParser parser)
        {
            string value = cell.StringValue;
            Match mat = _patEnd.Match(value);
            if (mat.Success == false)
            {
                return false;
            }
            CheckBlockStack(cell, parser);
            ProcessEnd(parser);
            return true;
        }

        private void ProcessEnd(FPParser parser)
        {
            Root root = parser.Root;
            AbstractBlock block = parser.PopFromBlockStack();
            Type type = block.GetType();

            if (type == typeof(ElseBlock) || type == typeof(ElseIfBlock))
            {
                block = GetIfBlockFromStack(parser);
            }
            else if (type == typeof(PageHeaderBlock))
            {
                root.PageHeader = block;
                return;
            }
            else if (type == typeof(PageFooterBlock))
            {
                root.PageFooter = block;
                return;
            }
            if (parser.IsBlockStackBlank())
            {
                root.AddBody(block);
            }
        }

        /// <summary>
        /// Else If か Elseの場合、元になるIfが出るまで継続してPopする。
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        private AbstractBlock GetIfBlockFromStack(FPParser parser)
        {
            AbstractBlock block = null;
            while (block == null || block.GetType() != typeof(IfBlock))
            {
                block = parser.PopFromBlockStack();
            }
            return block;
        }

        private void CheckBlockStack(Seasar.Fisshplate.Wrapper.CellWrapper cell, FPParser parser)
        {
            if (parser.IsBlockStackBlank())
            {
                throw new FPParseException(FPConsts.MessageIdEndElement, cell.Row);
            }
        }

        #endregion
    }
}
