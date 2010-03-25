using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;
using NPOI.HSSF.UserModel;
using System.IO;
using Seasar.Fisshplate.Wrapper;
using Seasar.Fisshplate.Context;
using Seasar.Fisshplate.Core.Element;
using Seasar.Fisshplate.Exception;

namespace Seasar.Fisshplate.Test.Element
{
    [TestFixture]
    public class ElTest
    {

        [Test]
        public void Testデータは数字だけど文字列型の場合は文字列型()
        {
            using (FileStream fs = new FileStream(@"TestResource\Template\ElTest.xls", FileMode.Open, FileAccess.Read))
            {
                HSSFWorkbook template = new HSSFWorkbook(fs);
                WorkbookWrapper workbook = new WorkbookWrapper(template);

                Dictionary<string, object> data = new Dictionary<string, object>();
                data["code"] = "01234";
                data["num"] = -1234;

                FPContext context = new FPContext(template.GetSheetAt(0), data);
                CellWrapper cell0 = workbook.GetSheetAt(0).GetRow(0).GetCell(0); // ${code}
                CellWrapper cell1 = workbook.GetSheetAt(0).GetRow(0).GetCell(1); // ${num}

                El el = new El(new GenericCell(cell0));
                el.Merge(context);
                el = new El(new GenericCell(cell1));
                el.Merge(context);

                HSSFCell actual = template.GetSheetAt(0).GetRow(0).GetCell(0);
                Assert.AreEqual(HSSFCell.CELL_TYPE_STRING, actual.CellType);
                Assert.AreEqual("01234", actual.RichStringCellValue.String);

                actual = template.GetSheetAt(0).GetRow(0).GetCell(1);
                Assert.AreEqual(HSSFCell.CELL_TYPE_NUMERIC, actual.CellType);
                Assert.AreEqual(-1234D, actual.NumericCellValue);

            }
            
        }

        [Test]
        public void Test変数にビックリマークをつけた場合はNull回避()
        {
            using (FileStream fs = new FileStream(@"TestResource\Template\ElTest.xls", FileMode.Open, FileAccess.Read))
            {
                HSSFWorkbook template = new HSSFWorkbook(fs);
                WorkbookWrapper workbook = new WorkbookWrapper(template);

                Dictionary<string, object> data = new Dictionary<string, object>();

                FPContext context = new FPContext(template.GetSheetAt(0), data);

                CellWrapper cell = workbook.GetSheetAt(0).GetRow(0).GetCell(2); // ${hoge}
                CellWrapper cellNull = workbook.GetSheetAt(0).GetRow(0).GetCell(3); // ${hoge!}
                CellWrapper cellNullValue = workbook.GetSheetAt(0).GetRow(0).GetCell(4); // ${hoge!NULL時デフォルト値}

                El el = new El(new GenericCell(cell));
                try
                {
                    el.Merge(context);
                    Assert.Fail();
                }
                catch (FPMergeException)
                {
                    // True
                }
                el = new El(new GenericCell(cellNull));
                el.Merge(context);
                HSSFCell actual = template.GetSheetAt(0).GetRow(0).GetCell(0);
                Assert.AreEqual("", actual.RichStringCellValue.String);

                el = new El(new GenericCell(cellNullValue));
                el.Merge(context);
                actual = template.GetSheetAt(0).GetRow(0).GetCell(1);
                Assert.AreEqual("NULL時デフォルト値", actual.RichStringCellValue.String);

                data["hoge"] = null;
                el = new El(new GenericCell(cellNull));
                el.Merge(context);
                actual = template.GetSheetAt(0).GetRow(0).GetCell(2);
                Assert.AreEqual("", actual.RichStringCellValue.String);
            }
        }

        [Test]
        public void Test文字列に埋め込み()
        {
            using (FileStream fs = new FileStream(@"TestResource\Template\ElTest.xls", FileMode.Open, FileAccess.Read))
            {
                HSSFWorkbook template = new HSSFWorkbook(fs);
                WorkbookWrapper workbook = new WorkbookWrapper(template);
                Dictionary<string, object> data = new Dictionary<string, object>();
                data["embeded"] = 123;
                FPContext context = new FPContext(template.GetSheetAt(0), data);
                context.NextRow();

                CellWrapper cell = workbook.GetSheetAt(0).GetRow(1).GetCell(0);
                El el = new El(new GenericCell(cell));

                el.Merge(context);
                HSSFCell actual = template.GetSheetAt(0).GetRow(1).GetCell(0);
                Assert.AreEqual("埋め込み番号は123です。", actual.RichStringCellValue.String);
            }
        }

        [Test]
        public void Test文字列埋込複数()
        {
            using (FileStream fs = new FileStream(@"TestResource\Template\ElTest_multi.xls", FileMode.Open, FileAccess.Read))
            {
                HSSFWorkbook template = new HSSFWorkbook(fs);
                WorkbookWrapper workbook = new WorkbookWrapper(template);
                Dictionary<string, object> data = new Dictionary<string, object>();
                data["embeded1"] = 123;
                data["embeded2"] = 456;
                FPContext context = new FPContext(template.GetSheetAt(0), data);

                CellWrapper cell = workbook.GetSheetAt(0).GetRow(0).GetCell(0);
                El el = new El(new GenericCell(cell));

                el.Merge(context);
                HSSFCell actual = template.GetSheetAt(0).GetRow(0).GetCell(0);
                Assert.AreEqual("埋め込み番号は123と456です。", actual.RichStringCellValue.String);

                cell = workbook.GetSheetAt(0).GetRow(1).GetCell(0);
                context.NextRow();

                el = new El(new GenericCell(cell));

                el.Merge(context);
                actual = template.GetSheetAt(0).GetRow(1).GetCell(0);
                Assert.AreEqual("123と456", actual.RichStringCellValue.String);

                cell = workbook.GetSheetAt(0).GetRow(2).GetCell(0);
                context.NextRow();

                el = new El(new GenericCell(cell));

                el.Merge(context);
                actual = template.GetSheetAt(0).GetRow(2).GetCell(0);
                Assert.AreEqual("123456", actual.RichStringCellValue.String);


            }

        }

        [Test]
        public void Test改行コード変換()
        {
            using (FileStream fs = new FileStream(@"TestResource\Template\ElTest.xls", FileMode.Open, FileAccess.Read))
            {
                HSSFWorkbook template = new HSSFWorkbook(fs);
                WorkbookWrapper workbook = new WorkbookWrapper(template);
                Dictionary<string, object> data = new Dictionary<string, object>();

                data["code"] = "0\n12\r34";
                data["num"] = "番号\r\nはこれ";

                FPContext context = new FPContext(template.GetSheetAt(0), data);

                CellWrapper cell0 = workbook.GetSheetAt(0).GetRow(0).GetCell(0);
                CellWrapper cell1 = workbook.GetSheetAt(0).GetRow(0).GetCell(1);

                El el = new El(new GenericCell(cell0));
                el.Merge(context);
                el = new El(new GenericCell(cell1));
                el.Merge(context);

                HSSFCell actual = template.GetSheetAt(0).GetRow(0).GetCell(0);
                Assert.AreEqual(HSSFCell.CELL_TYPE_STRING, actual.CellType);
                Assert.AreEqual("0\n12\n34", actual.RichStringCellValue.String);

                actual = template.GetSheetAt(0).GetRow(0).GetCell(1);
                Assert.AreEqual(HSSFCell.CELL_TYPE_STRING, actual.CellType);
                Assert.AreEqual("番号\nはこれ", actual.RichStringCellValue.String);

            }
        }

        [Test]
        public void Testバインド変数でメソッドや配列要素アクセス()
        {
            using (FileStream fs = new FileStream(@"TestResource\Template\ElTest_method.xls", FileMode.Open, FileAccess.Read))
            {
                HSSFWorkbook template = new HSSFWorkbook(fs);
                WorkbookWrapper workbook = new WorkbookWrapper(template);
                Dictionary<string, object> data = new Dictionary<string, object>();

                List<object> dataList = new List<object>();
                dataList.Add("ほげ");
                dataList.Add(123);
                data["dataList"] = dataList;

                FPContext context = new FPContext(template.GetSheetAt(0), data);

                CellWrapper cell = workbook.GetSheetAt(0).GetRow(0).GetCell(0);//2

                El el = new El(new GenericCell(cell));
                el.Merge(context);
                HSSFCell actual = template.GetSheetAt(0).GetRow(0).GetCell(0);
                Assert.AreEqual(2D, actual.NumericCellValue);

                cell = workbook.GetSheetAt(0).GetRow(0).GetCell(1);//ほげ
                el = new El(new GenericCell(cell));
                el.Merge(context);
                actual = template.GetSheetAt(0).GetRow(0).GetCell(1);

                Assert.AreEqual("ほげ", actual.RichStringCellValue.String);

                cell = workbook.GetSheetAt(0).GetRow(1).GetCell(0);//${dataList[1][0]}
                context.NextRow();

                el = new El(new GenericCell(cell));
                el.Merge(context);
                actual = template.GetSheetAt(0).GetRow(1).GetCell(0);
                Assert.AreEqual(123D, actual.NumericCellValue);

                cell = workbook.GetSheetAt(0).GetRow(2).GetCell(0);//リストのサイズは${dataList.Count}です。
                context.NextRow();

                el = new El(new GenericCell(cell));
                el.Merge(context);
                actual = template.GetSheetAt(0).GetRow(2).GetCell(0);
                Assert.AreEqual("リストのサイズは2です。", actual.RichStringCellValue.String);



            }
        }



    }
}
