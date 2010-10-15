using System;
using System.Collections.Generic;
using System.Text;
using Seasar.Fisshplate.Wrapper;
using System.Text.RegularExpressions;
using NPOI.HSSF.UserModel;

namespace Seasar.Fisshplate.Core.Element
{
    public class Formula : AbstractCell
    {

        private static readonly Regex _patFormula = new Regex(@"^\s*#formula\s*\((.*)\)");
        
        public Formula(CellWrapper cell)
            : base(cell)
        {
        }

        public override void MergeImpl(Seasar.Fisshplate.Context.FPContext context, NPOI.HSSF.UserModel.HSSFCell outCell)
        {
            String value = base.CellValue.ToString();
            Match mat = _patFormula.Match(value);
            String formula = mat.Groups[1].Value;
            if (IsWritePicture(formula))
            {
                outCell.SetCellType(HSSFCell.CELL_TYPE_FORMULA);
                outCell.SetCellFormula(formula);
                
            }
        }

        private bool IsWritePicture(string formula)
        {
            if (String.IsNullOrEmpty(formula))
            {
                return false;
            }
            return true;
        }
    }
}
