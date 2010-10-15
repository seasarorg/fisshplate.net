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
            string ss = "__obj__['hoge']";
            string ee = OgnlUtil.ToEvalFormula(ss);
            Assert.AreEqual("__obj__['hoge']", ee);

            string str = "name = __obj__['hoge'] + (__obj__.fuga - moge) && || sage";
            string exp = OgnlUtil.ToEvalFormula(str);
            Assert.AreEqual("__obj__['name']=__obj__['hoge']+(__obj__['__obj__'].fuga-__obj__['moge'])&&||__obj__['sage']", exp);
        }

        [Test]
        public void Test変数名が__obj__の場合()
        {
            //__obj__は予約後なので、普通には使えない（制限事項）
            //必ず連想配列形式で利用すること！！
            string str = "__obj__['__obj__'] = __obj__ + __obj__['fuga']";
            string exp = OgnlUtil.ToEvalFormula(str);
            Assert.AreEqual("__obj__['__obj__']=__obj__+__obj__['fuga']", exp);
            //この形式の文字列は正常には動作しない。
        }

        [Test]
        public void TestEnumerable系の使い方の場合()
        {
            string str = "name = hoge['moge'] + foo[123].age";
            string exp = OgnlUtil.ToEvalFormula(str);
            Assert.AreEqual("__obj__['name']=__obj__['hoge']['moge']+__obj__['foo'][123].age", exp);
        }

        [Test]
        public void TestBoolean条件の場合()
        {
            string str = "idx < 12";
            string exp = OgnlUtil.ToEvalFormula(str);
            Assert.AreEqual("__obj__['idx']<12", exp);

            str = @"prevKey!=item.Key";
            exp = OgnlUtil.ToEvalFormula(str);
            Assert.AreEqual(@"__obj__['prevKey']!=__obj__['item'].Key", exp);

            str = @"itemList.Count == index+1||prevKey != itemList[index + 1].Key";
            exp = OgnlUtil.ToEvalFormula(str);
            Assert.AreEqual(@"__obj__['itemList'].Count==__obj__['index']+1||__obj__['prevKey']!=__obj__['itemList'][__obj__['index']+1].Key", exp);

        }

        [Test]
        public void Test空白がない場合()
        {
            string str = "index%2==0";
            string exp = OgnlUtil.ToEvalFormula(str);
            Assert.AreEqual("__obj__['index']%2==0", exp);

        }

        [Test]
        public void Test_プロパティの場合()
        {
            string str = "data.val";
            string exp = OgnlUtil.ToEvalFormula(str);
            Assert.AreEqual("__obj__['data'].val", exp);
        }

        [Test]
        public void Test_文字列中の何かの場合()
        {
            string str = "'hoge'";
            string exp = OgnlUtil.ToEvalFormula(str);
            Assert.AreEqual("'hoge'", exp);

            str = "\"h\\\"oge\"";
            exp = OgnlUtil.ToEvalFormula(str);
            Assert.AreEqual("\"h\\\"oge\"", exp);

            System.Console.WriteLine(exp);
        }

        [Test]
        public void Test_値のセット()
        {
            IDictionary<String, object> data = new Dictionary<String, object>();

            data["template"] = "";
            JScriptUtil.Evaluate("__obj__['template']='D1+D2+D3'", data);
        }
    }
}
