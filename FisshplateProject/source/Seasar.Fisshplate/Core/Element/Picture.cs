using System;
using System.Collections.Generic;

using System.Text;
using Seasar.Fisshplate.Wrapper;
using System.Text.RegularExpressions;
using Seasar.Fisshplate.Exception;
using Seasar.Fisshplate.Consts;
using NPOI.HSSF.UserModel;
using System.IO;
using Seasar.Fisshplate.Util;
using System.Drawing;

namespace Seasar.Fisshplate.Core.Element
{
    public class Picture : AbstractCell
    {
        public Picture(CellWrapper cell)
            :base(cell)
        {
        }

        public override void MergeImpl(Seasar.Fisshplate.Context.FPContext context, NPOI.HSSF.UserModel.HSSFCell outCell)
        {
            string cellValue = CellValue.ToString();
            Regex pat = new Regex(@"^\s*#picture\s*\(\s*(.*)\s+cell\s*=\s*([^\s]+)\s+row\s*=\s*([^\s]+)\s*\)");
            Match mat = pat.Match(cellValue);
            if (mat.Success == false)
            {
                throw new FPMergeException(FPConsts.MessageIdPictureMergeError,
                    new object[] { cellValue }, _cell.Row);
            }

            string picturePath = mat.Groups[0].Value;
            string cellRange = mat.Groups[2].Value;
            string rowRange = mat.Groups[3].Value;
            int cellRangeIntVal = int.Parse(cellRange);
            int rowRangeIntVal = int.Parse(rowRange);
            if (IsWritePicture(picturePath))
            {
                WritePicture(picturePath, cellRangeIntVal, rowRangeIntVal, context);
            }

        }

        private void WritePicture(string picturePath, int cellRangeIntVal, int rowRangeIntVal, Seasar.Fisshplate.Context.FPContext context)
        {
            FileStream imageFs = FileInputStreamUtil.CreateFileStream(picturePath);
            using (Image img = Image.FromStream(imageFs))
            {
                HSSFWorkbook workbook = _cell.Row.Sheet.Workbook.HSSFWorkbook;
                HSSFPatriarch patriarch = context.Patriarch;

                int imgWidth = img.Width;
                int imgHeight = img.Height;
                int cellNo = context.CurrentCellNum;
                int rowNo = context.CurrentRowNum;

                byte[] pictureData = ImageIOUtil.ConvertToBytes(img);
                HSSFClientAnchor anchor = CreateAnchor(imgWidth, imgHeight, cellNo, rowNo, cellRangeIntVal, rowRangeIntVal);

                string suffix = StringUtil.ParseSuffix(picturePath);
                int pictureType = SetupPictureType(suffix);
                int pictureIndex = workbook.AddPicture(pictureData, pictureType);
                patriarch.CreatePicture(anchor, pictureIndex);
            }
        }

        private int SetupPictureType(string suffix)
        {
            if (suffix.ToLower() == "jpg")
            {
                return HSSFWorkbook.PICTURE_TYPE_JPEG;
            }
            if (suffix.ToLower() == "png")
            {
                return HSSFWorkbook.PICTURE_TYPE_PNG;
            }
            throw new FPMergeException(FPConsts.MessagePictureTypeInvalid);
        }

        private HSSFClientAnchor CreateAnchor(int imgWidth, int imgHeight, int cellNo, int rowNo, int cellRangeIntVal, int rowRangeIntVal)
        {
            HSSFClientAnchor anchor = new HSSFClientAnchor();
            anchor.Dx1 = 0;
            anchor.Dx2 = 0;
            anchor.Dy1 = 0;
            anchor.Dy2 = 255;

            int fromCellNo = cellNo;
            int toCellNo = cellNo + cellRangeIntVal;
            int fromRowNo = rowNo;
            int toRowNo = rowNo + rowRangeIntVal;

            anchor.Col1 = fromCellNo;
            anchor.Col2 = toCellNo;
            anchor.Row1 = fromRowNo;
            anchor.Row2 = toRowNo;
            anchor.AnchorType = 2;

            return anchor;
        }

        private bool IsWritePicture(string picturePath)
        {
            if (picturePath == string.Empty)
            {
                return false;
            }
            if (picturePath.Length <= 0)
            {
                return false;
            }
            return true;
        }
    }
}
