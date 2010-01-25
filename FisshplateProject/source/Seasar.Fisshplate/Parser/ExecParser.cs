using System;
using System.Collections.Generic;

using System.Text;
using System.Text.RegularExpressions;
using Seasar.Fisshplate.Core.Element;

namespace Seasar.Fisshplate.Parser
{
    public class ExecParser : RowParser
    {
        private static readonly Regex _patExec = new Regex(@"#exec\s+(.+)");

        #region RowParser メンバ

        public bool Process(Seasar.Fisshplate.Wrapper.CellWrapper cell, FPParser parser)
        {
            string value = cell.StringValue;
            Match mat = _patExec.Match(value);
            if (mat.Success == false)
            {
                return false;
            }
            string expression = mat.Groups[1].Value;
            Exec elem = new Exec(expression);
            parser.AddTemplateElement(elem);

            return true;
        }

        #endregion
    }
}
