using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Seasar.Fisshplate.Core.Element;

namespace Seasar.Fisshplate.Parser
{
    public class FormulaParser : CellParser
    {
        private static readonly Regex _patFormula = new Regex(@"^\s*#formula.*");

        #region CellParser メンバ

        public Seasar.Fisshplate.Core.Element.AbstractCell GetElement(Seasar.Fisshplate.Wrapper.CellWrapper cell, string value)
        {
            AbstractCell cellElement = null;
            Match mat = _patFormula.Match(value);
            if (mat.Success)
            {
                cellElement = new Formula(cell);
            }
            return cellElement;
        }

        #endregion
    }
}
