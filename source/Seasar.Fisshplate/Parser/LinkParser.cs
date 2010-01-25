using System;
using System.Collections.Generic;

using System.Text;
using System.Text.RegularExpressions;
using Seasar.Fisshplate.Consts;
using Seasar.Fisshplate.Enum;
using Seasar.Fisshplate.Core.Element;

namespace Seasar.Fisshplate.Parser
{
    public class LinkParser : CellParser
    {
        private static readonly Regex _patLink = new Regex(FPConsts.RegexLink);

        #region CellParser メンバ

        public Seasar.Fisshplate.Core.Element.AbstractCell GetElement(Seasar.Fisshplate.Wrapper.CellWrapper cell, string value)
        {
            Match mat = _patLink.Match(value);
            if (mat.Success == false || IsValidType(mat) == false)
            {
                return null;
            }
            return new Link(cell);
        }
        #endregion

        private bool IsValidType(Match mat)
        {
            string type = mat.Groups[1].Value;
            LinkElementType elemType = LinkElementType.Get(type);
            return elemType != null;
        }

    }
}
