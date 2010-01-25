using System;
using System.Collections.Generic;

using System.Text;
using System.Text.RegularExpressions;
using Seasar.Fisshplate.Core.Element;

namespace Seasar.Fisshplate.Parser
{
    public class ResumeParser : RowParser
    {
        private static readonly Regex _patResume = new Regex(@"^\s*#resume\s*(.+)");

        #region RowParser メンバ

        public bool Process(Seasar.Fisshplate.Wrapper.CellWrapper cell, FPParser parser)
        {
            string value = cell.StringValue;
            Match mat = _patResume.Match(value);
            if (mat.Success == false)
            {
                return false;
            }
            string targetVar = mat.Groups[1].Value;
            Resume elem = new Resume(targetVar);
            parser.AddTemplateElement(elem);

            return true;
        }

        #endregion
    }
}
