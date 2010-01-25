using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;
using NPOI.HSSF.UserModel;
using Seasar.Fisshplate.Wrapper;
using Seasar.Fisshplate.Core.Element;
using Seasar.Fisshplate.Parser;

namespace Seasar.Fisshplate.Test.Parser
{
    [TestFixture]
    public class LinkParserTest
    {
        private LinkParser parser = new LinkParser();

        [Test]
        public void Testパース成功()
        {
            HSSFWorkbook wb = new HSSFWorkbook();
            wb.CreateSheet().CreateRow(0).CreateCell(0);
            WorkbookWrapper ww = new WorkbookWrapper(wb);

            CellWrapper cell = ww.GetSheetAt(0).GetRow(0).GetCell(0);
            string value = "#link-url link=http://www.gyoizo.com text=ほげ";
            cell.HSSFCell.SetCellValue(new HSSFRichTextString(value));

            AbstractCell actual = parser.GetElement(cell, value);
            Assert.AreEqual(typeof(Link), actual.GetType());

            value = "#link-file link=http://www.gyoizo.com text=ほげ";
            actual = parser.GetElement(cell, value);
            Assert.AreEqual(typeof(Link), actual.GetType());

            value = "#link-this link=http://www.gyoizo.com text=ほげ";
            actual = parser.GetElement(cell, value);
            Assert.AreEqual(typeof(Link), actual.GetType());

            value = "#link-email link=http://www.gyoizo.com text=ほげ";
            actual = parser.GetElement(cell, value);
            Assert.AreEqual(typeof(Link), actual.GetType());
        }

        [Test]
        public void Testパース失敗()
        {
            HSSFWorkbook wb = new HSSFWorkbook();
            wb.CreateSheet().CreateRow(0).CreateCell(0);
            WorkbookWrapper ww = new WorkbookWrapper(wb);

            CellWrapper cell = ww.GetSheetAt(0).GetRow(0).GetCell(0);
            String value = "#link-hoge link=http://www.gyoizo.com text=ほげ";
            cell.HSSFCell.SetCellValue(new HSSFRichTextString(value));

            AbstractCell actual = parser.GetElement(cell, value);
            Assert.IsNull(actual);
        }
    }
}
