using System;
using System.Collections.Generic;

using System.Text;
using NPOI.HSSF.UserModel;
using Seasar.Fisshplate.Util;

namespace Seasar.Fisshplate.Wrapper
{
    public class CellWrapper
    {
        private HSSFCell _hssfCell;
        private RowWrapper _row;

        public CellWrapper(HSSFCell cell, RowWrapper row)
        {
            this._row = row;
            this._hssfCell = cell;
        }

        public HSSFCell HSSFCell
        {
            get { return _hssfCell; }
        }

        public RowWrapper Row
        {
            get { return _row; }
        }

        public int CellIndex
        {
            get
            {
                if (IsNullCell())
                {
                    return -1;
                }
                return _hssfCell.ColumnIndex;
            }
        }

        public bool IsNullCell()
        {
            return _hssfCell == null;
        }

        public String StringValue
        {
            get { return FPPoiUtil.GetStringValue(_hssfCell); }
        }

        public Object ObjectValue
        {
            get { return FPPoiUtil.GetCellValueAsObject(_hssfCell); }
        }

    }
}
