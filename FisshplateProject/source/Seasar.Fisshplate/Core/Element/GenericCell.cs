using System;
using System.Collections.Generic;

using System.Text;
using Seasar.Fisshplate.Wrapper;
using NPOI.HSSF.UserModel;

namespace Seasar.Fisshplate.Core.Element
{
    /// <summary>
    /// 値を持つ通常のセルの情報を保持する要素クラスです。
    /// </summary>
    public class GenericCell : AbstractCell
    {
        public GenericCell(CellWrapper cell)
            : base(cell)
        {
        }

        public override void MergeImpl(Seasar.Fisshplate.Context.FPContext context, NPOI.HSSF.UserModel.HSSFCell outCell)
        {
            HSSFCell templateCell = _cell.HSSFCell;
            object cellValue = CellValue;

            int cellType = templateCell.CellType;

            if (cellType == HSSFCell.CELL_TYPE_FORMULA)
            {
                outCell.SetCellFormula((string)cellValue);
            }
            else if (cellType == HSSFCell.CELL_TYPE_ERROR)
            {
                outCell.SetCellErrorValue((byte)cellValue);
            }
            else if (cellValue is DateTime)
            {
                outCell.SetCellValue((DateTime)cellValue);
                outCell.SetCellType(HSSFCell.CELL_TYPE_NUMERIC);
            }
            else if (cellValue is string)
            {
                outCell.SetCellValue(new HSSFRichTextString((string)cellValue));
                outCell.SetCellType(HSSFCell.CELL_TYPE_STRING);
            }
            else if (cellValue is bool)
            {
                outCell.SetCellValue((bool)cellValue);
                outCell.SetCellType(HSSFCell.CELL_TYPE_BOOLEAN);
            }
            else if (IsNumber(cellValue))
            {
                outCell.SetCellValue(double.Parse(cellValue.ToString()));
                outCell.SetCellType(HSSFCell.CELL_TYPE_NUMERIC);
            }
        }

        private bool IsNumber(object cellValue)
        {
            if (cellValue == null || cellValue is string)
            {
                return false;
            }
            double result = double.NaN;
            return double.TryParse(cellValue.ToString(), out result);
        }

    }
}
