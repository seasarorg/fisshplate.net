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
            string str = "name = __obj__['hoge'] + (__obj__.fuga - moge) && || sage";
            string exp = OgnlUtil.ToEvalFormula(str);
            Assert.AreEqual("__obj__['name'] = __obj__['hoge'] + (__obj__['__obj__'].fuga - __obj__['moge']) && || __obj__['sage']", exp);
        }

        [Test]
        public void Test変数名がdataの場合()
        {
            string str = "name = data + data.fuga";
            string exp = OgnlUtil.ToEvalFormula(str);
            Assert.AreEqual("__obj__['name'] = __obj__['data'] + __obj__['data'].fuga", exp);
        }

        [Test]
        public void TestEnumerable系の使い方の場合()
        {
            string str = "name = hoge['moge'] + foo[123].age";
            string exp = OgnlUtil.ToEvalFormula(str);
            Assert.AreEqual("__obj__['name'] = __obj__['hoge']['moge'] + __obj__['foo'][123].age", exp);
        }
    }
}
