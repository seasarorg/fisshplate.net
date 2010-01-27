using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using System.IO;
using Seasar.Fisshplate.Wrapper;
using NPOI.HSSF.UserModel;
using Seasar.Fisshplate.Parser;
using Seasar.Fisshplate.Core.Element;
using System.Collections;

namespace Seasar.Fisshplate.Test.Parser
{
    [TestFixture]
    public class HForEachParserTest
    {
        [Test]
        public void TestHForEach解析()
        {
            using (Stream s = new FileStream("HorizontalIteratorTest.xls", FileMode.Open, FileAccess.Read))
            {
                WorkbookWrapper workbook = new WorkbookWrapper(new HSSFWorkbook(s));
                FPParser parser = new FPParser();

                Root root = parser.Parse(workbook.GetSheetAt(0));
                IList<TemplateElement> bodyList = root.BodyElementList;

                // 2行目がHForEachのはず
                TemplateElement row = bodyList[1];
                Assert.AreEqual(typeof(HorizontalIteratorBlock), row.GetType());

            }

        }
    }
}
