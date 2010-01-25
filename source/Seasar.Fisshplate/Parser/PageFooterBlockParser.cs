using System;
using System.Collections.Generic;

using System.Text;
using Seasar.Fisshplate.Core.Element;
using System.Text.RegularExpressions;

namespace Seasar.Fisshplate.Parser
{
    public class PageFooterBlockParser : RowParser
    {
        private static readonly Regex _patPageFooterStart = new Regex(@"#pageFooterStart");

        #region RowParser メンバ

        public bool Process(Seasar.Fisshplate.Wrapper.CellWrapper cell, FPParser parser)
        {
            string value = cell.StringValue;
            Match mat = _patPageFooterStart.Match(value);
            if (mat.Success == false)
            {
                return false;
            }
            AbstractBlock block = new PageFooterBlock();
            parser.PushBlockToStack(block);

            return true;
        }

        #endregion
    }
}
