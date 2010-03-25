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

        /// <summary>
        /// セルの書式・スタイル・座標・値などが同一かどうか
        /// </summary>
        /// <param name="cell1"></param>
        /// <param name="cell2"></param>
        /// <returns></returns>
        public static bool IsSameCell(HSSFCell cell1, HSSFCell cell2)
        {
            if (cell1 == null && cell2 == null) { return true; }
            if (cell1 == null || cell2 == null) { return false; }
            if (IsSameCellStyle(cell1.CellStyle, cell2.CellStyle) == false) { return false; }
            if (cell1.CellType != cell2.CellType) { return false; }
            if (cell1.ColumnIndex != cell2.ColumnIndex) { return false; }
            if (cell1.RowIndex != cell2.RowIndex) { return false; }
            if (IsSameHyperLink(cell1.Hyperlink, cell2.Hyperlink) == false) { return false; }
            if (IsSameCellComment(cell1.CellComment, cell2.CellComment) == false) { return false; }
            if (cell1.Sheet.GetColumnWidth(cell1.ColumnIndex) != cell2.Sheet.GetColumnWidth(cell2.ColumnIndex)) { return false; }
            if (cell1.Sheet.GetRow(cell1.RowIndex).Height != cell2.Sheet.GetRow(cell2.RowIndex).Height) { return false; }
      
            return IsSameCellValue(cell1, cell2);
        }

        /// <summary>
        /// セルの値が同一かどうか
        /// </summary>
        /// <param name="cell1"></param>
        /// <param name="cell2"></param>
        /// <returns></returns>
        public static bool IsSameCellValue(HSSFCell cell1, HSSFCell cell2)
        {
            if (cell1 == null && cell2 == null) { return true; }
            if (cell1 == null || cell2 == null) { return false; }
            switch (cell1.CellType)
            {
                case HSSFCell.CELL_TYPE_FORMULA:
                    return cell1.CellFormula == cell2.CellFormula;
                case HSSFCell.CELL_TYPE_BOOLEAN:
                    return cell1.BooleanCellValue == cell2.BooleanCellValue;
                case HSSFCell.CELL_TYPE_ERROR:
                    return cell1.ErrorCellValue == cell2.ErrorCellValue;
                case HSSFCell.CELL_TYPE_STRING:
                    return cell1.StringCellValue == cell2.StringCellValue;
                case HSSFCell.CELL_TYPE_NUMERIC:
                    return cell1.NumericCellValue == cell2.NumericCellValue;
                default:
                    return cell1.ToString() == cell2.ToString();
            }
        }
        public static bool IsSameRow(HSSFRow row1, HSSFRow row2)
        {
            if (row1 == null && row2 == null) { return true; }
            if (row1 == null || row2 == null) { return false; }

            if (row1.Height != row2.Height) { return false; }
            if (row1.HeightInPoints != row2.HeightInPoints) { return false; }
            if (row1.IsFormatted != row2.IsFormatted) { return false; }
            if (row1.OutlineLevel != row2.OutlineLevel) { return false; }
            if (row1.ZeroHeight != row2.ZeroHeight) { return false; }

            if (row1.LastCellNum != row2.LastCellNum) { return false; }
            if (row1.FirstCellNum != row2.FirstCellNum) { return false; }

            int lastCellNum = row1.LastCellNum;

            for (int i = 0; i < lastCellNum; i++)
            {
                if (IsSameCell(row1.GetCell(i), row2.GetCell(i)) == false)
                {
                    return false;
                }
            }
            return true;
        }
        public static bool IsSameRowValue(HSSFRow row1, HSSFRow row2)
        {
            int lastCellNum = row1.LastCellNum;
            if (lastCellNum < row2.LastCellNum)
            {
                lastCellNum = row2.LastCellNum;
            }
            for (int i = 0; i < lastCellNum; i++)
            {
                HSSFCell cell1 = row1.GetCell(i);
                if (cell1 == null)
                {
                    cell1 = row1.CreateCell(i);
                }
                HSSFCell cell2 = row2.GetCell(i);
                if (cell2 == null)
                {
                    cell2 = row2.CreateCell(i);
                }
                if (IsSameCellValue(cell1, cell2) == false)
                {
                    return false;
                }
            }
            
            return true;
        }
        public static bool IsSameSheet(HSSFSheet sheet1, HSSFSheet sheet2)
        {
            if (sheet1 == null && sheet2 == null) { return true; }
            if (sheet1 == null || sheet2 == null) { return false; }

            //TODO sheetの

            if (sheet1.LastRowNum != sheet2.LastRowNum) { return false; }
            if (sheet1.FirstRowNum != sheet2.FirstRowNum) { return false; }

            int lastRowNum = sheet1.LastRowNum;

            for (int i = 0; i < lastRowNum; i++)
            {
                if (IsSameRow(sheet1.GetRow(i), sheet2.GetRow(i)) == false)
                {
                    return false;
                }
            }

            return true;
        }
        public static bool IsSameSheetValue(HSSFSheet sheet1, HSSFSheet sheet2)
        {
            if (sheet1 == null && sheet2 == null) { return true; }
            if (sheet1 == null || sheet2 == null) { return false; }

            int lastRowNum = sheet1.LastRowNum;
            if (lastRowNum < sheet2.LastRowNum)
            {
                lastRowNum = sheet2.LastRowNum;
            }
            for (int i = 0; i < lastRowNum; i++)
            {
                HSSFRow row1 = sheet1.GetRow(i);
                if (row1 == null)
                {
                    row1 = sheet1.CreateRow(i);
                }
                HSSFRow row2 = sheet2.GetRow(i);
                if (row2 == null)
                {
                    row2 = sheet2.CreateRow(i);
                }
                if (IsSameRowValue(row1, row2) == false) { return false; }
            }
            return true;
        }
        public static bool IsSameWorkbook(HSSFWorkbook book1, HSSFWorkbook book2)
        {
            return true;
        }
        public static bool IsSameWorkbookValue(HSSFWorkbook book1, HSSFWorkbook book2)
        {
            return true;
        }

        public static bool IsSameCellStyle(HSSFCellStyle style1, HSSFCellStyle style2)
        {
            if (style1 == null && style2 == null) { return true; }
            if (style1 == null || style2 == null) { return false; }

            //if (style1.Equals(style2) == false) { return false; }

            if (style1.Alignment != style2.Alignment) { return false; }
            if (style1.BorderBottom != style2.BorderBottom) { return false; }
            if (style1.BorderLeft != style2.BorderLeft) { return false; }
            if (style1.BorderRight != style2.BorderRight) { return false; }
            if (style1.BorderTop != style2.BorderTop) { return false; }
            if (style1.DataFormat != style2.DataFormat) { return false; }
            if (style1.FillBackgroundColor != style2.FillBackgroundColor) { return false; }
            if (style1.FillForegroundColor != style2.FillForegroundColor) { return false; }
            if (style1.FillPattern != style2.FillPattern) { return false; }
            if (style1.FontIndex != style2.FontIndex) { return false; }
            if (style1.Hidden != style2.Hidden) { return false; }
            if (style1.Indention != style2.Indention) { return false; }
            if (style1.Index != style2.Index) { return false; } //Indexはシートごとに違うのでは？
            if (style1.IsLocked != style2.IsLocked) { return false; }
            if (style1.BottomBorderColor != style2.BottomBorderColor) { return false; }
            if (style1.LeftBorderColor != style2.LeftBorderColor) { return false; }
            if (style1.RightBorderColor != style2.RightBorderColor) { return false; }
            if (style1.TopBorderColor != style2.TopBorderColor) { return false; }
            if (style1.Rotation != style2.Rotation) { return false; }
            //if (style1.UserStyleName != style2.UserStyleName) { return false; } //スタイル名は一致しなくてもいい？
            if (style1.VerticalAlignment != style2.VerticalAlignment) { return false; }
            if (style1.WrapText != style2.WrapText) { return false; }

            return true;
        }

        public static bool IsSameHyperLink(HSSFHyperlink hyperlink1, HSSFHyperlink hyperlink2)
        {
            if (hyperlink1 == null && hyperlink2 == null) { return true; }
            if (hyperlink1 == null || hyperlink2 == null) { return false; }

            if (hyperlink1.Equals(hyperlink2) == false) { return false; }

            return true;
        }
        public static bool IsSameCellComment(HSSFComment hSSFComment1, HSSFComment hSSFComment2)
        {
            if (hSSFComment1 == null && hSSFComment2 == null) { return true; }
            if (hSSFComment1 == null || hSSFComment2 == null) { return false; }

            //if (hSSFComment1.Equals(hSSFComment2) == false) { return false; }
            
            if (hSSFComment1.Author != hSSFComment2.Author) { return false; }
            if (hSSFComment1.FillColor != hSSFComment2.FillColor) { return false; }
            if (hSSFComment1.HorizontalAlignment != hSSFComment2.HorizontalAlignment) { return false; }
            if (hSSFComment1.IsNoFill != hSSFComment2.IsNoFill) { return false; }
            if (hSSFComment1.LineStyle != hSSFComment2.LineStyle) { return false; }
            if (hSSFComment1.LineStyleColor != hSSFComment2.LineStyleColor) { return false; }
            if (hSSFComment1.LineWidth != hSSFComment2.LineWidth) { return false; }
            if (hSSFComment1.MarginBottom != hSSFComment2.MarginBottom) { return false; }
            if (hSSFComment1.MarginLeft != hSSFComment2.MarginLeft) { return false; }
            if (hSSFComment1.MarginRight != hSSFComment2.MarginRight) { return false; }
            if (hSSFComment1.MarginTop != hSSFComment2.MarginTop) { return false; }
            if (hSSFComment1.ShapeType != hSSFComment2.ShapeType) { return false; }
            if (hSSFComment1.VerticalAlignment != hSSFComment2.VerticalAlignment) { return false; }
            if (hSSFComment1.Visible != hSSFComment2.Visible) { return false; }
            if (hSSFComment1.String.String != hSSFComment2.String.String) { return false; }


            return true;
        }
    }
}
