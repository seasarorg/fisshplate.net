using System;
using System.Collections.Generic;

using System.Text;
using System.Text.RegularExpressions;
using Seasar.Fisshplate.Core.Element;

namespace Seasar.Fisshplate.Parser
{
    public class IfBlockParser : RowParser
    {
        private static readonly Regex _patIf = new Regex(@"^\s*#if\s*(.+)");

        #region RowParser メンバ

        public bool Process(Seasar.Fisshplate.Wrapper.CellWrapper cell, FPParser parser)
        {
            string value = cell.StringValue;
            Match mat = _patIf.Match(value);
            if (mat.Success == false)
            {
                return false;
            }
            string condition = mat.Groups[1].Value;
            AbstractBlock block = new IfBlock(condition);

            parser.AddBlockElement(block);

            return true;
        }

        #endregion
    }
}
