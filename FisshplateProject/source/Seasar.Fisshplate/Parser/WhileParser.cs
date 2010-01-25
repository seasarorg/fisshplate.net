using System;
using System.Collections.Generic;

using System.Text;
using System.Text.RegularExpressions;
using Seasar.Fisshplate.Wrapper;
using Seasar.Fisshplate.Core.Element;

namespace Seasar.Fisshplate.Parser
{
    public class WhileParser : RowParser
    {
        private static readonly Regex _patWhile = new Regex(@"^\s*#while\s+(.+)");

        #region RowParser メンバ

        public bool Process(Seasar.Fisshplate.Wrapper.CellWrapper cell, FPParser parser)
        {
            string value = cell.StringValue;
            Match mat = _patWhile.Match(value);
            if (mat.Success == false)
            {
                return false;
            }
            string condition = mat.Groups[1].Value;
            RowWrapper row = cell.Row;
            AbstractBlock block = new WhileBlock(row, condition);
            parser.AddBlockElement(block);

            return true;
        }

        #endregion
    }
}
