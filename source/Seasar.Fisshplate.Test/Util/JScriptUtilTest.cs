using System;
using System.Collections.Generic;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Seasar.Fisshplate.Util;
using System.Reflection;
using Microsoft.JScript;

namespace Seasar.Fisshplate.Test.Util
{
    [TestFixture]
    public class JScriptUtilTest
    {
        [Test]
        public void TestString()
        {
            Assert.AreEqual<string>("文字列", (string)JScriptUtil.Evaluate("\"文字列\"", null));
        }

        [Test]
        public void TestInt()
        {
            Assert.AreEqual<int>(7, (int)JScriptUtil.Evaluate("7", null));
        }

        [Test]
        public void TestDouble()
        {
            Assert.AreEqual<double>((double)3.14, (double)(JScriptUtil.Evaluate("3.14", null)));
        }

        [Test]
        public void TestBool()
        {
            Assert.AreEqual<bool>(true, (bool)(JScriptUtil.Evaluate("true", null)));
        }

        [Test]
        public void TestCompareString()
        {
            Assert.AreEqual<bool>(true, (bool)(JScriptUtil.Evaluate("\"hoge\" == \"hoge\"", null)));
        }

        [Test]
        public void TestCompareStringNotEqual()
        {
            Assert.AreEqual<bool>(false, (bool)(JScriptUtil.Evaluate("\"hoge\" == \"moge\"", null)));
        }

        [Test]
        public void TestOut()
        {
            string outStr = "Out";
            Dictionary<string, object> hash = new Dictionary<string, object>();
            hash["out"] = outStr;

            Assert.AreEqual<string>("Out", (string)JScriptUtil.Evaluate("data['out']", hash));
        }

        [Test]
        public void TestReturn()
        {
            string str = @"'a
b'";
            Assert.AreEqual<string>("a\r\nb", (string)JScriptUtil.Evaluate(str, null));
        }

        [Test]
        public void TestNestValue()
        {
            Dictionary<string, object> hash = new Dictionary<string, object>();
            Dictionary<string, object> items = new Dictionary<string, object>();
            items["aaa"] = 123;
            items["bbb"] = 456;

            hash["items"] = items;

            Assert.AreEqual<int>(579, (int)JScriptUtil.Evaluate("data['items']['aaa'] + data['items']['bbb']", hash));
        }

        [Test]
        public void TestNestStringConcat()
        {
            Dictionary<string, object> hash = new Dictionary<string, object>();
            hash["hoge"] = "Hoge";
            hash["moge"] = "Moge";

            Assert.AreEqual<string>("HogeMoge", (string)JScriptUtil.Evaluate("\"Hoge\" + \"Moge\"", null));

        }

        [Test]
        public void TestValue()
        {
            string value = "hoge";

            Assert.AreEqual("hoge", JScriptUtil.Evaluate("data", value));
        }

        [Test]
        public void TestMethod()
        {
            HogeSample data = new HogeSample();
            Assert.AreEqual<string>("hoge", (string)JScriptUtil.Evaluate("data.GetHoge()", data));
        }

        [Test]
        public void TestPropery()
        {
            HogeSample data = new HogeSample();
            data.Hoge = "hhhh";
            Assert.AreEqual<string>("hhhh", (string)JScriptUtil.Evaluate("data.Hoge", data));
        }

        [Test]
        public void TestNoPropertyException()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["hoge"] = "Hoge";
            try
            {
                JScriptUtil.Evaluate("data['moge']", data);
            }
            catch (TargetInvocationException ex)
            {
                if (ex.InnerException != null && ex.InnerException is JScriptException)
                {
                    if (ex.InnerException.InnerException != null && ex.InnerException.InnerException is KeyNotFoundException)
                    {
                        // OK
                        return;
                    }
                }
            }
            Assert.Fail();
        }

        [Test]
        public void TestList()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            List<object> list = new List<object>();
            list.Add("ほげ");
            list.Add(123);

            data["dataList"] = list;

            Assert.AreEqual(2, JScriptUtil.Evaluate("data['dataList'].Count", data));
            Assert.AreEqual("ほげ", JScriptUtil.Evaluate("data['dataList'][0]", data));
            Assert.AreEqual(123, JScriptUtil.Evaluate("data['dataList'][1]", data));
            Assert.AreEqual(2, JScriptUtil.Evaluate("data['dataList'][0].Length", data));
        }

        [Test]
        public void TestOutParameter()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["hoge"] = "Hoge";

            JScriptUtil.Evaluate("data['moge'] = \"Moge\"", data);
            Assert.AreEqual<string>("Moge", (string)data["moge"]);
        }

        /// <summary>
        /// JScriptを実行する場所から見えてないとアウトっぽい？
        /// この場合、MbUnitの実行場所から見えるように、public の必要がある。
        /// </summary>
        public class HogeSample
        {
            public string Hoge { get; set; }
            public string Moge { get; set; }

            public int Aaa { get; set; }
            public int Bbb { get; set; }

            public string GetHoge() { return "hoge"; }
        }
    }
}
