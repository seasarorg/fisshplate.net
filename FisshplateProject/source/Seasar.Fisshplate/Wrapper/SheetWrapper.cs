using System;
using System.Collections.Generic;

using System.Text;
using NPOI.HSSF.UserModel;

namespace Seasar.Fisshplate.Wrapper
{
    public class SheetWrapper
    {
        private HSSFSheet _hssfSheet;
        private WorkbookWrapper _workbook;
        private IList<RowWrapper> _rowList = new List<RowWrapper>();
        private int _sheetIndex;

        public SheetWrapper(HSSFSheet sheet, WorkbookWrapper workbook, int sheetIndex)
        {
            this._workbook = workbook;
            this._hssfSheet = sheet;
            this._sheetIndex = sheetIndex;
            for (int i = 0; i <= sheet.LastRowNum; i++)
            {
                _rowList.Add(new RowWrapper(sheet.GetRow(i), this));
            }
        }

        public HSSFSheet HSSFSheet
        {
            get { return _hssfSheet; }
        }

        public WorkbookWrapper Workbook
        {
            get { return _workbook; }
        }

        public RowWrapper GetRow(int index)
        {
            return _rowList[index];
        }

        public int RowCount
        {
            get { return _rowList.Count; }
        }
        public int SheetIndex
        {
            get { return _sheetIndex; }
        }

        public string SheetName
        {
            get { return _workbook.HSSFWorkbook.GetSheetName(_sheetIndex); }
        }

        public void RemoveRow(int i)
        {
            RowWrapper row = this.GetRow(i);
            //全てのセルを削除　NPOI1.2.1のバグ対応
            row.HSSFRow.RemoveAllCells();

            _hssfSheet.RemoveRow(row.HSSFRow);
            _rowList.RemoveAt(i);
        }

        /// <summary>
        /// データ埋め込み準備のために、シートを初期化します
        /// </summary>
        public void PrepareForMerge()
        {
            RemoveAllRow();
            RemoveAllMergedRegion();
        }

        private void RemoveAllRow()
        {
            for (int i = 0; i < RowCount - 1; i++)
            {
                HSSFRow hssfRow = GetRow(i).HSSFRow;
                if (hssfRow != null)
                {
                    // 全てのセルを削除　NPOI1.2.1のバグ対応
                    hssfRow.RemoveAllCells();

                    _hssfSheet.RemoveRow(hssfRow);
                }
            }
        }

        private void RemoveAllMergedRegion()
        {
            int numMergedRegions = _hssfSheet.NumMergedRegions;

            for (int i = 0; i < numMergedRegions; i++)
            {
                _hssfSheet.RemoveMergedRegion(0);
            }
        }

    }
}
