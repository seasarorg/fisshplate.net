using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;
using NPOI.HSSF.UserModel;
using Seasar.Fisshplate.Wrapper;
using Seasar.Fisshplate.Parser;

namespace Seasar.Fisshplate.Test.Parser
{
    [TestFixture]
    public class WhileParserTest
    {
        [Test]
        public void Test解析()
        {
            HSSFWorkbook hssfWb = new HSSFWorkbook();
            hssfWb.CreateSheet().CreateRow(0).CreateCell(0).SetCellValue(new HSSFRichTextString(" #while hoge == 100  "));
            hssfWb.GetSheetAt(0).CreateRow(1).CreateCell(0).SetCellValue(new HSSFRichTextString(" #end  "));
            WorkbookWrapper wb = new WorkbookWrapper(hssfWb);

            FPParser fpParser = new FPParser();
            WhileParser parser = new WhileParser();

            bool actual = parser.Process(wb.GetSheetAt(0).GetRow(0).GetCell(0), fpParser);
            Assert.IsTrue(actual);

            hssfWb.GetSheetAt(0).GetRow(0).GetCell(0).SetCellValue(new HSSFRichTextString("hile hoge==100"));
            actual = parser.Process(wb.GetSheetAt(0).GetRow(0).GetCell(0), fpParser);
            Assert.IsFalse(actual);
        }
    }
}
