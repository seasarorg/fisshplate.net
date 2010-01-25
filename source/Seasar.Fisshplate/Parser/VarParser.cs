using System;
using System.Collections.Generic;

using System.Text;
using System.Text.RegularExpressions;
using Seasar.Fisshplate.Wrapper;
using Seasar.Fisshplate.Core.Element;

namespace Seasar.Fisshplate.Parser
{
    public class VarParser : RowParser
    {
        private static readonly Regex _patVar = new Regex(@"#var\s+(.+)");

        #region RowParser メンバ

        public bool Process(Seasar.Fisshplate.Wrapper.CellWrapper cell, FPParser parser)
        {
            string value = cell.StringValue;
            Match mat = _patVar.Match(value);
            if (mat.Success == false)
            {
                return false;
            }
            RowWrapper row = cell.Row;
            string vars = mat.Groups[1].Value;
            VarElement elem = new VarElement(vars, row);
            parser.AddTemplateElement(elem);

            return true;
        }

        #endregion
    }
}
