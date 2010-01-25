using System;
using System.Collections.Generic;

using System.Text;
using System.Text.RegularExpressions;
using Seasar.Fisshplate.Core.Element;

namespace Seasar.Fisshplate.Parser
{
    public class ElseBlockParser : AbstractElseParser
    {
        private static readonly Regex _patElse = new Regex(@"^\s*#else\s*$");

        protected override Seasar.Fisshplate.Core.Element.AbstractBlock CreateElement(string condition)
        {
            return new ElseBlock();
        }

        protected override System.Text.RegularExpressions.Regex GetPattern()
        {
            return _patElse;
        }
    }
}
