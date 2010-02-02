using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;
using NPOI.HSSF.UserModel;
using Seasar.Fisshplate.Wrapper;
using Seasar.Fisshplate.Core.Element;
using Seasar.Fisshplate.Context;
using Seasar.Fisshplate.Exception;

namespace Seasar.Fisshplate.Test.Element
{
    [TestFixture]
    public class WhileBlockTest
    {
        [Test]
        public void Test例外テスト()
        {
            HSSFWorkbook hssfWb = new HSSFWorkbook();
            hssfWb.CreateSheet().CreateRow(0).CreateCell(0);
            WorkbookWrapper wb = new WorkbookWrapper(hssfWb);
            WhileBlock block = new WhileBlock(wb.GetSheetAt(0).GetRow(0), "hogehoge");
            try
            {
                block.Merge(new FPContext(null, null));
                Assert.Fail();
            }
            catch (FPMergeException)
            {
                Assert.IsTrue(true);
            }
        }

        [Test]
        public void Test条件テスト()
        {

        }
    }
}
