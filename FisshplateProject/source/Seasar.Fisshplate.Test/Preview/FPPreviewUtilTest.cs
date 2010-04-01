using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using System.IO;
using NPOI.HSSF.UserModel;
using Seasar.Fisshplate.Util;
using Seasar.Fisshplate.Preview;

namespace Seasar.Fisshplate.Test.Preview
{
    [TestFixture]
    public class FPPreviewUtilTest
    {
        //[Test]
        public void Test_プレビュー()
        {
            using (FileStream template = new FileStream(@"TestResource\Template\Preview\MapBuilderTest_template.xls", FileMode.Open, FileAccess.Read))
            using (FileStream data = new FileStream(@"TestResource\Template\Preview\MapBuilderTest.xls", FileMode.Open, FileAccess.Read))
            using (FileStream template_out = new FileStream(@"out_FPPreviewTest_stream_out.xls", FileMode.Create, FileAccess.ReadWrite))
            {
                HSSFWorkbook o = FPPreviewUtil.GetWorkbook(template, data);
                o.Write(template_out);
            }

        }
    }
}
