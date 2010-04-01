using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using System.IO;
using NPOI.HSSF.UserModel;
using Seasar.Fisshplate.Preview;
using System.Collections;

namespace Seasar.Fisshplate.Test.Preview
{
    [TestFixture]
    public class MapBuilderTest
    {
        [Test]
        public void Test_Map生成()
        {
            //期待値
            IDictionary<string, object> expected = new Dictionary<string, object>();
            expected["repnum"] = 101d;
            expected["title"] = "タイトルです。";

            //実行
            using (Stream s = new FileStream(@"TestResource\Template\Preview\MapBuilderTest.xls", FileMode.Open, FileAccess.Read))
            {
                HSSFWorkbook wb = new HSSFWorkbook(s);
                MapBuilder builder = new MapBuilder();
                IDictionary<string, object> actual = builder.BuildMapFrom(wb);

                System.Console.WriteLine(actual["repnum"].GetType().ToString());
                Assert.AreEqual(101d, actual["repnum"]);
                Assert.AreEqual("タイトルです。", actual["title"]);
                Assert.AreEqual(new DateTime(2008, 1, 28), actual["date"]);
                IDictionary<string, object> rootData = (IDictionary<string, object>)actual["data"];
                Assert.AreEqual("ループじゃないの", rootData["val"]);

                IList<IDictionary<string, object>> itemList = (IList<IDictionary<string, object>>)actual["itemList"];
                Assert.AreEqual(10, itemList.Count);
                System.Console.WriteLine(itemList.Count.ToString());
                for (int i = 0; i < itemList.Count; i++)
                {
                    IDictionary<string, object> item = itemList[i];
                    foreach (var v in item)
                    {
                        System.Console.WriteLine(v.Key + "=" + v.Value);
                    }
                    Assert.AreEqual((Double)(i + 1), item["num"]);
                }
                IDictionary<string, object> item2 = (IDictionary<string, object>)itemList[0];
                IList<IDictionary<string, object>> childList = (IList<IDictionary<string, object>>)item2["childList"];

                Assert.AreEqual(5, childList.Count);

                IDictionary<string, object> data = (IDictionary<string, object>)actual["data"];
                Assert.AreEqual("ループじゃないの", data["val"]);
                itemList = (IList<IDictionary<string, object>>)data["itemList"];
                Assert.AreEqual(6, itemList.Count);

                IDictionary<string, object> dataChild = (IDictionary<string, object>)data["child"];
                Assert.AreEqual("子供のデータ", dataChild["childVal"]);

                IDictionary<string, object> dataGrandChild = (IDictionary<string, object>)dataChild["grandChild"];
                Assert.AreEqual("dataの孫の値", dataGrandChild["grandChildVal"]);

                IDictionary<string, object> dataGrand2 = (IDictionary<string, object>)dataGrandChild["grand2"];
                Assert.AreEqual("dataのひ孫", dataGrand2["val"]);

            }

        }
    }
}
