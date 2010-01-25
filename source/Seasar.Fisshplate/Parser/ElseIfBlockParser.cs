using System;
using System.Collections.Generic;

using System.Text;
using System.Text.RegularExpressions;
using Seasar.Fisshplate.Core.Element;

namespace Seasar.Fisshplate.Parser
{
    public class ElseIfBlockParser : AbstractElseParser
    {
        private static readonly Regex _patElseIf = new Regex(@"^\s*#else\s+if\s*(.+)");

        protected override Seasar.Fisshplate.Core.Element.AbstractBlock CreateElement(string condition)
        {
            return new ElseIfBlock(condition);
        }

        protected override System.Text.RegularExpressions.Regex GetPattern()
        {
            return _patElseIf;
        }
    }
}
