using System;
using System.Collections.Generic;
using System.Text;
using Seasar.Fisshplate.Core.Element;
using NPOI.HSSF.UserModel;
using Seasar.Fisshplate.Parser.Handler;
using Seasar.Fisshplate.Wrapper;
using MbUnit.Framework;
namespace Seasar.Fisshplate.Test.Element
{
    [TestFixture]
    public class SuspendTest
    {
        [Test]
        public void Test埋込()
        {
            Root root = new Root();
            HSSFWorkbook templateWb = new HSSFWorkbook();
            HSSFSheet templateSheet = templateWb.CreateSheet();
            HSSFRow templateRow = templateSheet.CreateRow(0);
            HSSFCell cell = templateRow.CreateCell(0);
            cell.SetCellValue(new HSSFRichTextString("#suspend id=test expr=TEST is ${hoge}"));

            WorkbookWrapper workbook = new WorkbookWrapper(templateWb);

            Row row = new Row(workbook.GetSheetAt(0).GetRow(0), root,new CellParserHandler());
            IList<TemplateElement> elementList = row.CellElementList;
            TemplateElement elem = (TemplateElement) elementList[0];
            Assert.AreEqual(typeof(Suspend), elem.GetType());
        }
    }
}
