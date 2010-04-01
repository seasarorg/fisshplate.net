using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using System.IO;
using NPOI.HSSF.UserModel;
using Seasar.Fisshplate.Util;

namespace Seasar.Fisshplate.Test.Util
{
    [TestFixture]
    public class FPPoiUtilTest
    {
        [Test]
        public void Test_型のチェック()
        {
            using (FileStream fs = new FileStream(@"TestResource\Template\Util\FPPoiuUtil.xls", FileMode.Open, FileAccess.Read))
            {
                HSSFWorkbook wb = new HSSFWorkbook(fs);

                HSSFSheet sheet = wb.GetSheetAt(0);
                HSSFRow row1 = sheet.GetRow(0);
                HSSFRow row2 = sheet.GetRow(1);
                System.Console.WriteLine(row1.GetCell(2).ToString());

                Assert.AreEqual(new DateTime(2010, 3, 11), FPPoiUtil.GetCellValueAsObject(row1.GetCell(2)));

            }
        }


        [Test]
        public void Test_セルの値のみ比較()
        {
            using (FileStream fs = new FileStream(@"TestResource\Template\Util\FPPoiuUtil.xls", FileMode.Open, FileAccess.Read))
            {
                HSSFWorkbook wb = new HSSFWorkbook(fs);
                
                HSSFSheet sheet = wb.GetSheetAt(0);
                HSSFRow row1 = sheet.GetRow(0);
                HSSFRow row2 = sheet.GetRow(1);
                Assert.IsTrue(FPPoiUtil.IsSameCellValue(row1.GetCell(0), row2.GetCell(0)));
                Assert.IsTrue(FPPoiUtil.IsSameCellValue(row1.GetCell(1), row2.GetCell(1)));
                Assert.IsTrue(FPPoiUtil.IsSameCellValue(row1.GetCell(2), row2.GetCell(2)));
                Assert.IsTrue(FPPoiUtil.IsSameCellValue(row1.GetCell(3), row2.GetCell(3)));
                Assert.IsTrue(FPPoiUtil.IsSameCellValue(row1.GetCell(4), row2.GetCell(4)));
            }
        }
        [Test]
        public void Test_セル比較_スタイルまで含め()
        {
            using (FileStream fs = new FileStream(@"TestResource\Template\Util\FPPoiuUtil.xls", FileMode.Open, FileAccess.Read))
            {
                HSSFWorkbook wb = new HSSFWorkbook(fs);

                HSSFSheet sheet1 = wb.GetSheetAt(0);
                HSSFSheet sheet2 = wb.GetSheetAt(1);

                HSSFRow row1 = sheet1.GetRow(0);
                HSSFRow row2 = sheet2.GetRow(0);

                Assert.IsTrue(FPPoiUtil.IsSameCell(row1.GetCell(0), row2.GetCell(0)));
                Assert.IsTrue(FPPoiUtil.IsSameCell(row1.GetCell(1), row2.GetCell(1)));
                Assert.IsTrue(FPPoiUtil.IsSameCell(row1.GetCell(2), row2.GetCell(2)));
                Assert.IsTrue(FPPoiUtil.IsSameCell(row1.GetCell(3), row2.GetCell(3)));
                Assert.IsTrue(FPPoiUtil.IsSameCell(row1.GetCell(4), row2.GetCell(4)));
            }
        }

        [Test]
        public void Test_行の値のみ比較_最後のセルIndexが違っても空白なら同じ()
        {
            using (FileStream fs = new FileStream(@"TestResource\Template\Util\FPPoiuUtil.xls", FileMode.Open, FileAccess.Read))
            {
                HSSFWorkbook wb = new HSSFWorkbook(fs);

                HSSFSheet sheet1 = wb.GetSheetAt(0);
                HSSFSheet sheet2 = wb.GetSheetAt(0);

                Assert.IsTrue(FPPoiUtil.IsSameRowValue(sheet1.GetRow(0), sheet1.GetRow(0)));
                Assert.IsTrue(FPPoiUtil.IsSameRowValue(sheet1.GetRow(0), sheet1.GetRow(1)));
                Assert.IsTrue(FPPoiUtil.IsSameRowValue(sheet1.GetRow(1), sheet1.GetRow(1)));
                Assert.IsTrue(FPPoiUtil.IsSameRowValue(sheet1.GetRow(0), sheet2.GetRow(0)));
                Assert.IsTrue(FPPoiUtil.IsSameRowValue(sheet1.GetRow(1), sheet2.GetRow(1)));
                Assert.IsTrue(FPPoiUtil.IsSameRowValue(sheet1.GetRow(0), sheet2.GetRow(1)));
                Assert.IsTrue(FPPoiUtil.IsSameRowValue(sheet1.GetRow(1), sheet2.GetRow(0)));
            }
        }
        [Test]
        public void Test_行比較_スタイルを含めて()
        {
            using (FileStream fs = new FileStream(@"TestResource\Template\Util\FPPoiuUtil.xls", FileMode.Open, FileAccess.Read))
            {
                HSSFWorkbook wb = new HSSFWorkbook(fs);

                HSSFSheet sheet1 = wb.GetSheetAt(0);
                HSSFSheet sheet2 = wb.GetSheetAt(1);

                Assert.IsTrue(FPPoiUtil.IsSameRow(sheet1.GetRow(0), sheet2.GetRow(0)));
                Assert.IsTrue(FPPoiUtil.IsSameRow(sheet1.GetRow(1), sheet2.GetRow(1)));
            }
        }

        [Test]
        public void Test_セル比較_スタイルのみ()
        {
            using (FileStream fs = new FileStream(@"TestResource\Template\Util\FPPoiuUtil.xls", FileMode.Open, FileAccess.Read))
            {
                HSSFWorkbook wb = new HSSFWorkbook(fs);

                HSSFSheet sheet1 = wb.GetSheetAt(0);
                HSSFSheet sheet2 = wb.GetSheetAt(1);

                HSSFRow row1 = sheet1.GetRow(0);
                HSSFRow row2 = sheet2.GetRow(0);

                Assert.IsNotNull(row1.GetCell(0).CellStyle);
                Assert.IsNotNull(row2.GetCell(0).CellStyle);

                Assert.IsTrue(FPPoiUtil.IsSameCellStyle(row1.GetCell(0).CellStyle, row2.GetCell(0).CellStyle));
                Assert.IsTrue(FPPoiUtil.IsSameCellStyle(row1.GetCell(1).CellStyle, row2.GetCell(1).CellStyle));
                Assert.IsTrue(FPPoiUtil.IsSameCellStyle(row1.GetCell(2).CellStyle, row2.GetCell(2).CellStyle));
                Assert.IsTrue(FPPoiUtil.IsSameCellStyle(row1.GetCell(3).CellStyle, row2.GetCell(3).CellStyle));
                Assert.IsTrue(FPPoiUtil.IsSameCellStyle(row1.GetCell(4).CellStyle, row2.GetCell(4).CellStyle));
            }
        }

        [Test]
        public void Test_セル比較_コメントのみ()
        {
            using (FileStream fs = new FileStream(@"TestResource\Template\Util\FPPoiuUtil.xls", FileMode.Open, FileAccess.Read))
            {
                HSSFWorkbook wb = new HSSFWorkbook(fs);

                HSSFSheet sheet1 = wb.GetSheetAt(0);
                HSSFSheet sheet2 = wb.GetSheetAt(1);

                HSSFRow row1 = sheet1.GetRow(0);
                HSSFRow row2 = sheet2.GetRow(0);

                Assert.IsNotNull(row1.GetCell(0).CellStyle);
                Assert.IsNotNull(row2.GetCell(0).CellStyle);

                Assert.IsTrue(FPPoiUtil.IsSameCellComment(row1.GetCell(0).CellComment, row2.GetCell(0).CellComment));
                Assert.IsTrue(FPPoiUtil.IsSameCellComment(row1.GetCell(1).CellComment, row2.GetCell(1).CellComment));
                Assert.IsTrue(FPPoiUtil.IsSameCellComment(row1.GetCell(2).CellComment, row2.GetCell(2).CellComment));
                Assert.IsTrue(FPPoiUtil.IsSameCellComment(row1.GetCell(3).CellComment, row2.GetCell(3).CellComment));
                Assert.IsTrue(FPPoiUtil.IsSameCellComment(row1.GetCell(4).CellComment, row2.GetCell(4).CellComment));
            }
        }



        [Test]
        public void Test_シート比較_値のみ()
        {
            using (FileStream fs = new FileStream(@"TestResource\Template\Util\FPPoiuUtil.xls", FileMode.Open, FileAccess.Read))
            {
                HSSFWorkbook wb = new HSSFWorkbook(fs);

                HSSFSheet sheet1 = wb.GetSheetAt(0);
                HSSFSheet sheet2 = wb.GetSheetAt(1);
                Assert.IsTrue(FPPoiUtil.IsSameSheetValue(sheet1, sheet2));
            }
        }

        [Test]
        public void Test_シート比較_スタイルまで含め()
        {
            using (FileStream fs = new FileStream(@"TestResource\Template\Util\FPPoiuUtil.xls", FileMode.Open, FileAccess.Read))
            {
                HSSFWorkbook wb = new HSSFWorkbook(fs);

                HSSFSheet sheet1 = wb.GetSheetAt(0);
                HSSFSheet sheet2 = wb.GetSheetAt(1);

                Assert.IsTrue(FPPoiUtil.IsSameSheet(sheet1, sheet2));
            }
        }
    }
}
