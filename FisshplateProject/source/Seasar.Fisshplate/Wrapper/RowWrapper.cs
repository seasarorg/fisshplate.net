using System;
using System.Collections.Generic;

using System.Text;
using NPOI.HSSF.UserModel;

namespace Seasar.Fisshplate.Wrapper
{
    public class RowWrapper
    {
        private HSSFRow _hssfRow;
        private SheetWrapper _sheet;
        private IList<CellWrapper> _cellList = new List<CellWrapper>();

        public RowWrapper(HSSFRow row, SheetWrapper sheet)
        {
            this._sheet = sheet;
            this._hssfRow = row;
            if (row != null)
            {
                AddCellsToList(row);
            }
        }

        private void AddCellsToList(HSSFRow row)
        {
            for (int i = 0; i < row.LastCellNum; i++)
            {
                _cellList.Add(new CellWrapper(row.GetCell(i), this));
            }
        }

        public bool IsNullRow()
        {
            return _hssfRow == null;
        }

        public HSSFRow HSSFRow
        {
            get { return _hssfRow; }
        }

        public SheetWrapper Sheet
        {
            get { return _sheet; }
        }
        public CellWrapper GetCell(int index)
        {
            if (index + 1 > _cellList.Count)
            {
                return null;
            }
            return _cellList[index];
        }

        public int CellCount
        {
            get { return _cellList.Count; }
        }
    }
}
