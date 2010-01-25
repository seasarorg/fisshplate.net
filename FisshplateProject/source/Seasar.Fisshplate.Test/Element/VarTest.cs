using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;
using Seasar.Fisshplate.Core.Element;
using Seasar.Fisshplate.Context;
using NPOI.HSSF.UserModel;
using Seasar.Fisshplate.Wrapper;
using Seasar.Fisshplate.Exception;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Seasar.Fisshplate.Test.Element
{
    [TestFixture]
    public class VarTest
    {
        [Test]
        public void Test変数1個()
        {
            string varName = " hoge ";
            VarElement varElem = new VarElement(varName, null);
            Dictionary<string, object> data = new Dictionary<string, object>();
            FPContext context = new FPContext(null, data);
            varElem.Merge(context);
            Assert.IsNotNull(data["hoge"]);
        }

        [Test]
        public void Test変数複数()
        {
            string varName = " hoge ,foo, bar,fuga";
            VarElement varElem = new VarElement(varName, null);
            Dictionary<string, object> data = new Dictionary<string, object>();
            FPContext context = new FPContext(null, data);
            varElem.Merge(context);
            Assert.IsNotNull(data["hoge"]);
            Assert.IsNotNull(data["foo"]);
            Assert.IsNotNull(data["bar"]);
            Assert.IsNotNull(data["fuga"]);
        }

        [Test]
        public void Test既存の変数を宣言したら例外()
        {
		    HSSFWorkbook wb = new HSSFWorkbook();
		    wb.CreateSheet();
		    wb.GetSheetAt(0).CreateRow(0);		
    		
		    WorkbookWrapper workbook = new WorkbookWrapper(wb);
    		
		    string varName = " hoge ,foo, bar,fuga";
		    VarElement varElem = new VarElement(varName, workbook.GetSheetAt(0).GetRow(0));
            Dictionary<string, object> data = new Dictionary<string, object>();
		    data["foo"] = null;
		    FPContext context = new FPContext(null, data);
		    try
            {
			    varElem.Merge(context);
			    Assert.Fail();
		    }
            catch (FPMergeException e)
            {
                Trace.Write(e.Message);
			    Assert.IsTrue(true);			
		    }		
        }

        [Test]
        public void Test変数宣言と初期化()
        {
            string varName = " hoge ,foo=0, bar,fuga = 'initVal'";
            VarElement varElem = new VarElement(varName, null);
            Dictionary<string, object> data = new Dictionary<string, object>();
            FPContext context = new FPContext(null, data);
            varElem.Merge(context);
            Assert.IsNotNull(data["hoge"]);
            Assert.IsNotNull(data["foo"]);
            Assert.AreEqual(0, data["foo"]);
            Assert.IsNotNull(data["bar"]);
            Assert.IsNotNull(data["fuga"]);
            Assert.AreEqual("initVal", data["fuga"]);
        }

        [Test]
        public void Test初期化でエラーで例外()
        {
            HSSFWorkbook wb = new HSSFWorkbook();
            wb.CreateSheet();
            wb.GetSheetAt(0).CreateRow(0);		

            WorkbookWrapper workbook = new WorkbookWrapper(wb);

            string varName = " foo =12fdk=df";
            VarElement varElem = new VarElement(varName,workbook.GetSheetAt(0).GetRow(0));
            Dictionary<string, object> data = new Dictionary<string, object>();
            FPContext context = new FPContext(null, data);
            try
            {
                varElem.Merge(context);
			    Assert.Fail();
            }
            catch (FPMergeException)
            {
                Assert.IsTrue(true);
            }	
        }

        [Test]
        public void Test初期化Pattern()
        {
            Regex pat = new Regex(@"([^=\s]+)\s*(=\s*[^=\s]+)?");
            Match mat = pat.Match("hoge");
            Assert.IsTrue(mat.Success);
            Assert.AreEqual("hoge", mat.Groups[0].Value);
            Assert.AreEqual("hoge", mat.Groups[1].Value);
            Assert.IsTrue(String.IsNullOrEmpty(mat.Groups[2].Value));

            mat = pat.Match("hoge=1");
            Assert.IsTrue(mat.Success);
            Assert.AreEqual("hoge=1", mat.Groups[0].Value);
            Assert.AreEqual("hoge", mat.Groups[1].Value);
            Assert.AreEqual("=1", mat.Groups[2].Value);

            mat = pat.Match("hoge =  1");
            Assert.IsTrue(mat.Success);
            Assert.AreEqual("hoge =  1", mat.Groups[0].Value);
            Assert.AreEqual("hoge", mat.Groups[1].Value);
            Assert.AreEqual("=  1", mat.Groups[2].Value);

        }

    }
}
