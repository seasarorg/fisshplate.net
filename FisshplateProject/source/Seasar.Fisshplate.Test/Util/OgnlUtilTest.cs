using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;
using Seasar.Fisshplate.Util;

namespace Seasar.Fisshplate.Test.Util
{
    [TestFixture]
    public class OgnlUtilTest
    {
        [Test]
        public void TestDeclareVar()
        {
            string str = "name = data['hoge'] + (data.fuga - moge) && || sage";
            string exp = OgnlUtil.ToEvalFormula(str);
            Assert.AreEqual("data['name'] = data['hoge'] + (data['data'].fuga - data['moge']) && || data['sage']", exp);
        }

        public void Test変数名がdataの場合()
        {
            string str = "name = data + data.fuga";
            string exp = OgnlUtil.ToEvalFormula(str);
            Assert.AreEqual("data['name'] = data['data'] + data['data'].fuga", exp);
        }
    }
}
