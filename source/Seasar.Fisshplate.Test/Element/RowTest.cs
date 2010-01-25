using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;
using Seasar.Fisshplate.Core.Element;
using NPOI.HSSF.UserModel;
using Seasar.Fisshplate.Wrapper;
using Seasar.Fisshplate.Parser.Handler;

namespace Seasar.Fisshplate.Test.Element
{
    [TestFixture]
    public class RowTest
    {
        [Test]
        public void Testコンストラクタ()
        {
            Root root = new Root();
            HSSFWorkbook templateWb = new HSSFWorkbook();
            HSSFSheet templateSheet = templateWb.CreateSheet();
            HSSFRow templateRow = templateSheet.CreateRow(0);
            HSSFCell cell = templateRow.CreateCell(0);
            cell.SetCellValue(new HSSFRichTextString("リテラル"));
            cell = templateRow.CreateCell(1);
            cell.SetCellValue(10D);
            //cellNum 2 は設定しない。
            cell = templateRow.CreateCell(3);
            cell.SetCellValue(new HSSFRichTextString("${data}"));
            cell = templateRow.CreateCell(4);
            cell.SetCellValue(new HSSFRichTextString("#picture(/hoge/fuga.png cell=1 row=1)"));
            cell = templateRow.CreateCell(5);
            cell.SetCellValue(new HSSFRichTextString("#picture(${data.path} cell=1 row=1)"));
            cell = templateRow.CreateCell(6);
            cell.SetCellValue(new HSSFRichTextString("#suspend TEST is ${hoge}"));
            cell = templateRow.CreateCell(7);
            cell.SetCellFormula("TEXT(VALUE(\"20040101\"),\"yyyy/mm/dd\")");
            cell = templateRow.CreateCell(8);
            cell.SetCellFormula("TEXT(VALUE(\"${hoge}\"),\"yyyy/mm/dd\")");
            cell = templateRow.CreateCell(9);
            cell.SetCellValue(new HSSFRichTextString("#link-url  link = http://www.gyoizo.com text = ほげー"));
            cell = templateRow.CreateCell(10);
            cell.SetCellValue(new HSSFRichTextString("#link-url  link = ${data.hoge} text = ほげー"));


            WorkbookWrapper workbook = new WorkbookWrapper(templateWb);

            Row row = new Row(workbook.GetSheetAt(0).GetRow(0), root, new CellParserHandler());
            IList<TemplateElement> elementList = row.CellElementList;

            TemplateElement elem = (TemplateElement) elementList[0];
            Assert.AreEqual(elem.GetType(), typeof(GenericCell));
            elem = (TemplateElement) elementList[1];
            Assert.AreEqual(elem.GetType(), typeof(GenericCell));
            elem = (TemplateElement) elementList[2];
            Assert.AreEqual(elem.GetType(), typeof(NullCell));
            elem = (TemplateElement) elementList[3];
            Assert.AreEqual(elem.GetType(), typeof(El));
            Assert.AreEqual(((El)elem).TargetElement.GetType(), typeof(GenericCell));
            elem = (TemplateElement) elementList[4];
            Assert.AreEqual(elem.GetType(), typeof(Picture));
            elem = (TemplateElement) elementList[5];
            Assert.AreEqual(elem.GetType(), typeof(El));
            Assert.AreEqual(((El)elem).TargetElement.GetType(), typeof(Picture));
            elem = (TemplateElement) elementList[6];
            Assert.AreEqual(elem.GetType(), typeof(Suspend));
            Assert.AreEqual(((Suspend)elem).El.TargetElement.CellValue, "TEST is ${hoge}");
            elem = (TemplateElement) elementList[7];
            Assert.AreEqual(elem.GetType(), typeof(GenericCell));
            elem = (TemplateElement) elementList[8];
            Assert.AreEqual(elem.GetType(), typeof(El));
            elem = (TemplateElement) elementList[9];
            Assert.AreEqual(elem.GetType(), typeof(Link));
            elem = (TemplateElement) elementList[10];
            Assert.AreEqual(((El)elem).TargetElement.GetType(), typeof(Link));

        }
    }
}
