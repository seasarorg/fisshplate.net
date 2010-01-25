using System;
using System.Collections.Generic;

using System.Text;
using System.Text.RegularExpressions;
using Seasar.Fisshplate.Core.Element;

namespace Seasar.Fisshplate.Parser
{
    public class PageBreakParser : RowParser
    {
        private static readonly Regex _patPageBreak = new Regex("#pageBreak");

        #region RowParser メンバ

        public bool Process(Seasar.Fisshplate.Wrapper.CellWrapper cell, FPParser parser)
        {
            string value = cell.StringValue;
            Match mat = _patPageBreak.Match(value);
            if (mat.Success == false)
            {
                return false;
            }
            Root root = parser.Root;
            TemplateElement elem = new PageBreak(root);
            parser.AddTemplateElement(elem);
            return true;
        }

        #endregion
    }
}
