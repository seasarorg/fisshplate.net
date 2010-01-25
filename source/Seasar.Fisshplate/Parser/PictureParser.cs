using System;
using System.Collections.Generic;

using System.Text;
using System.Text.RegularExpressions;
using Seasar.Fisshplate.Core.Element;

namespace Seasar.Fisshplate.Parser
{
    public class PictureParser : CellParser
    {
        private static readonly Regex _patPicture = new Regex(@"^\s*#picture\s*\(\s*\S+\s+cell\s*=\s*\S+\s+row\s*=\s*\S+\s*\)");

        #region CellParser メンバ

        public Seasar.Fisshplate.Core.Element.AbstractCell GetElement(Seasar.Fisshplate.Wrapper.CellWrapper cell, string value)
        {
            AbstractCell cellElem = null;
            Match mat = _patPicture.Match(value);

            if (mat.Success)
            {
                cellElem = new Picture(cell);
            }
            return cellElem;
        }

        #endregion
    }
}
