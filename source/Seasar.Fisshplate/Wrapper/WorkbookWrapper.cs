using System;
using System.Collections.Generic;

using System.Text;
using NPOI.HSSF.UserModel;

namespace Seasar.Fisshplate.Wrapper
{
    public class WorkbookWrapper
    {
        private HSSFWorkbook _hssfWorkbook;
        private IList<SheetWrapper> _sheetList = new List<SheetWrapper>();

        public WorkbookWrapper(HSSFWorkbook workbook)
        {
            this._hssfWorkbook = workbook;
            for (var i = 0; i < workbook.NumberOfSheets; i++)
            {
                _sheetList.Add(new SheetWrapper(workbook.GetSheetAt(i), this, i));
            }
        }

        public HSSFWorkbook HSSFWorkbook
        {
            get { return _hssfWorkbook; }
        }

        public SheetWrapper GetSheetAt(int index)
        {
            return _sheetList[index];
        }

        public SheetWrapper GetSheetByName(string sheetName)
        {
            foreach (var sheet in _sheetList)
            {
                if (sheetName == sheet.SheetName)
                {
                    return sheet;
                }
            }
            return null;
        }

        public int SheetCount
        {
            get { return _sheetList.Count; }
        }
    }
}
