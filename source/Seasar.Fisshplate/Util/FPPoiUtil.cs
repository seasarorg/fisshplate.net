using System;
using System.Collections.Generic;

using System.Text;
using NPOI.HSSF.UserModel;
using System.Text.RegularExpressions;

namespace Seasar.Fisshplate.Util
{
    public class FPPoiUtil
    {
        private FPPoiUtil() { }

        /// <summary>
        /// セルの書式設定に基づいてセルの値を戻します。
        /// </summary>
        /// <param name="hssfCell"></param>
        /// <returns></returns>
        public static object GetCellValueAsObject(HSSFCell hssfCell)
        {
            if (hssfCell == null)
            {
                return null;
            }
            int cellType = hssfCell.CellType;
            object ret = null;
            switch (cellType)
            {
                case HSSFCell.CELL_TYPE_NUMERIC:
                    ret = GetValueFromNumericCell(hssfCell);
                    break;
                case HSSFCell.CELL_TYPE_STRING:
                    ret = hssfCell.RichStringCellValue.String;
                    break;
                case HSSFCell.CELL_TYPE_BOOLEAN:
                    ret = hssfCell.BooleanCellValue;
                    break;
                case HSSFCell.CELL_TYPE_FORMULA:
                    ret = hssfCell.CellFormula;
                    break;
                case HSSFCell.CELL_TYPE_ERROR:
                    ret = hssfCell.ErrorCellValue;
                    break;
                case HSSFCell.CELL_TYPE_BLANK:
                    break;
                default:
                    return null;
            }
            return ret;
        }

        private static object GetValueFromNumericCell(HSSFCell hssfCell)
        {
            String str = hssfCell.ToString();
            // TODO 日付の正規表現は間違えているかも。ToStringも？？
            if (Regex.Match(str, @"\\d+-.+-\\d+").Success)
            {
                return hssfCell.DateCellValue;
            }
            return hssfCell.NumericCellValue;
        }

        public static string GetStringValue(HSSFCell hssfCell)
        {
            if (hssfCell == null)
            {
                return null;
            }
            HSSFRichTextString richVal = hssfCell.RichStringCellValue;
            if (richVal == null)
            {
                return null;
            }
            return richVal.String;
        }
    }
}
