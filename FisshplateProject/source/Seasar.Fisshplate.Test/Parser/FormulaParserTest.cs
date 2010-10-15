using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using System.IO;
using Seasar.Fisshplate.Wrapper;
using Seasar.Fisshplate.Parser;
using Seasar.Fisshplate.Core.Element;
using NPOI.HSSF.UserModel;

namespace Seasar.Fisshplate.Test.Parser
{
    [TestFixture]
    public class FormulaParserTest
    {
        [Test]
        public void TestFormula解析()
        {
            //using (Stream s = new FileStream(@"TestResource\Template\FPTemplateFormulaTest.xls", FileMode.Open, FileAccess.Read))
            //{
            //    WorkbookWrapper workbook = new WorkbookWrapper(new HSSFWorkbook(s));
            //    FPParser parser = new FPParser();

            //    Root root = parser.Parse(workbook.GetSheetAt(0));
            //    Assert.AreEqual(typeof(NullElement), root.PageHeader.GetType());
            //    Assert.AreEqual(typeof(NullElement), root.PageFooter.GetType());

            //    IList<TemplateElement> bodyList = root.BodyElementList;

            //    // 空行
            //    TemplateElement row = bodyList[0];
            //    IList<TemplateElement> cellList = ((Row)row).CellElementList;

            //    //２行目
            //    row = bodyList[1];
            //    Assert.AreEqual(typeof(Row), row.GetType());
            //    cellList = ((Row)row).CellElementList;
            //    TemplateElement cell = cellList[0];
            //    Assert.AreEqual(typeof(NullCell), cell.GetType());
            //    cell = cellList[1];
            //    Assert.AreEqual(typeof(El), cell.GetType());
            //    // 中身がPicture
            //    Assert.AreEqual(typeof(Formula), ((El)cell).TargetElement.GetType());
            //}

        }
    }
}
