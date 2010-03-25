using System;
using System.Collections.Generic;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Seasar.Fisshplate.Template;
using System.IO;
using NPOI.HSSF.UserModel;

namespace Seasar.Fisshplate.Test.Template
{
    [TestFixture]
    public class TemplateValueTest
    {
        [Test]
        public void TestSimpleValue()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["title"] = "タイトルなのですよー";
            data["name"] = 123456;

            FPTemplate tamplate = new FPTemplate();
            HSSFWorkbook wb = null;
            using (FileStream fs = new FileStream(@"TestResource\Template\TemplateValueTest.xls", FileMode.Open, FileAccess.Read))
            {
                wb = tamplate.Process(fs, data);
            }
            Assert.AreEqual("タイトルなのですよー", wb.GetSheetAt(0).GetRow(0).GetCell(0).StringCellValue);
            Assert.AreEqual(123456, wb.GetSheetAt(0).GetRow(1).GetCell(0).NumericCellValue);

        }
    }
}
