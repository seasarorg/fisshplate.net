using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using System.IO;
using NPOI.HSSF.UserModel;
using Seasar.Fisshplate.Template;
using System.Collections;

namespace Seasar.Fisshplate.Test.Template
{
    [TestFixture]
    public class FPTemplateFormulaTest
    {
        [Test]
        public void TestFormula出力()
        {
            //HSSFWorkbook wb = null;
            //using (Stream s = new FileStream(@"TestResource\Template\FPTemplateFormulaTest.xls", FileMode.Open, FileAccess.Read))
            //{
            //    FPTemplate template = new FPTemplate();
            //    IDictionary<string, object> data = new Dictionary<string, object>();

            //    data["formula"] = @"D1+D2+D3";

            //    data["formulaSum"] = @"SUM(D1:D3)";

            //    wb = template.Process(s, data);
            //}
            //using (Stream fos = new FileStream("out_formula.xls", FileMode.Create, FileAccess.Write))
            //{
            //    wb.Write(fos);
            //}
        }
    }
}
