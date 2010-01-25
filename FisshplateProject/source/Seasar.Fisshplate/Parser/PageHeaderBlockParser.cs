using System;
using System.Collections.Generic;

using System.Text;
using System.Text.RegularExpressions;
using Seasar.Fisshplate.Core.Element;

namespace Seasar.Fisshplate.Parser
{
    public class PageHeaderBlockParser : RowParser
    {
        private static readonly Regex _patPageHeaderStart = new Regex(@"#pageHeaderStart");

        #region RowParser メンバ

        public bool Process(Seasar.Fisshplate.Wrapper.CellWrapper cell, FPParser parser)
        {
            string value = cell.StringValue;
            Match mat = _patPageHeaderStart.Match(value);
            if (mat.Success == false)
            {
                return false;
            }
            AbstractBlock block = new PageHeaderBlock();
            parser.PushBlockToStack(block);

            return true;
        }

        #endregion
    }
}
