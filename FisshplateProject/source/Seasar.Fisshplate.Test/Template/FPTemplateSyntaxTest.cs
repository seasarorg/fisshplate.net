using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Seasar.Fisshplate.Template;
using System.Collections;
using NPOI.HSSF.UserModel;
using System.IO;

namespace Seasar.Fisshplate.Test.Template
{
    [TestFixture]
    public class FPTemplateSyntaxTest
    {
        [Test]
        public void Test_ForEachテスト_１カラム目にデータを出力しない()
        {
            IDictionary<string, object> data = new Dictionary<string, object>();
            IList<Foo> fooList = new List<Foo>()
            {
                new Foo(){Num = 10, Name = "1行目"},
                new Foo(){Num = 20, Name = "2行目"},
                new Foo(){Num = 30, Name = "3行目"},
                new Foo(){Num = 10, Name = "4行目"},
                new Foo(){Num = 20, Name = "5行目"},
                new Foo(){Num = 30, Name = "6行目"},
            };
            data["title"] = "タイトルです";
            data["fooList"] = fooList;
            FPTemplate template = new FPTemplate();
            HSSFWorkbook wb = template.Process(@"TestResource\Template\FPTemplate_ForEachTest.xls", data);
            using (Stream s = new FileStream("out_FPTemplate_ForEachTest.xls", FileMode.Create, FileAccess.Write))
            {
                wb.Write(s);
            }
        }

        [Test]
        public void Test_Whileテスト_ドキュメント用()
        {
            FPTemplate template = new FPTemplate();
            HSSFWorkbook wb = template.Process(@"TestResource\Template\FPTemplate_WhileTest.xls", new Dictionary<string, object>());
            using (Stream s = new FileStream("out_FPTemplate_WhileTest.xls", FileMode.Create, FileAccess.Write))
            {
                wb.Write(s);
            }
        }
        [Test]
        public void Test_Ifテスト_ドキュメント用()
        {
            IDictionary<string, object> data = new Dictionary<string, object>();
            IList<Foo> fooList = new List<Foo>()
            {
                new Foo(){Num = 10, Name = "1行目"},
                new Foo(){Num = 20, Name = "2行目"},
                new Foo(){Num = 30, Name = "3行目"},
                new Foo(){Num = 10, Name = "4行目"},
                new Foo(){Num = 20, Name = "5行目"},
                new Foo(){Num = 30, Name = "6行目"},
            };
            data["fooList"] = fooList;
            FPTemplate template = new FPTemplate();
            HSSFWorkbook wb = template.Process(@"TestResource\Template\FPTemplate_IfTest.xls", data);
            using (Stream s = new FileStream("out_FPTemplate_IfTest.xls", FileMode.Create, FileAccess.Write))
            {
                wb.Write(s);
            }
        }

        [Test]
        public void Test_PageBreakテスト_ドキュメント用()
        {
            IDictionary<string, object> data = new Dictionary<string, object>();
            IList<Foo> fooList = new List<Foo>()
            {
                new Foo(){Num = 10, Name = "1行目"},
                new Foo(){Num = 20, Name = "2行目"},
                new Foo(){Num = 30, Name = "3行目"},
                new Foo(){Num = 10, Name = "4行目"},
                new Foo(){Num = 20, Name = "5行目"},
                new Foo(){Num = 30, Name = "6行目"},
                new Foo(){Num = 10, Name = "7行目"},
                new Foo(){Num = 20, Name = "8行目"},
                new Foo(){Num = 30, Name = "9行目"},
           };
            data["title"] = "タイトル部分";
            data["b"] = fooList;
            FPTemplate template = new FPTemplate();
            HSSFWorkbook wb = template.Process(@"TestResource\Template\FPTemplate_PageBreakTest.xls", data);
            using (Stream s = new FileStream("out_FPTemplate_PageBreakTest.xls", FileMode.Create, FileAccess.Write))
            {
                wb.Write(s);
            }
        }

        [Test]
        public void Test_Scriptテスト_ドキュメント用()
        {
            IDictionary<string, object> data = new Dictionary<string, object>();
            IList<ScriptItem> fooList = new List<ScriptItem>()
            {
                new ScriptItem() { Key = "K0001", Name = "1-1です", Date = new DateTime(2008, 2, 4), Quantity = 2 },
                new ScriptItem() { Key = "K0001", Name = "1-2です", Date = new DateTime(2008, 2, 5), Quantity = 3 },
                new ScriptItem() { Key = "K0001", Name = "1-3です", Date = new DateTime(2008, 2, 6), Quantity = 2 },
                new ScriptItem() { Key = "K0001", Name = "1-4です", Date = new DateTime(2008, 2, 7), Quantity = 3 },
                new ScriptItem() { Key = "K0002", Name = "2-1だ", Date = new DateTime(2008, 2, 8), Quantity = 2 },
                new ScriptItem() { Key = "K0002", Name = "2-2だ", Date = new DateTime(2008, 2, 9), Quantity = 3 },
                new ScriptItem() { Key = "K0002", Name = "2-3だ", Date = new DateTime(2008, 2, 10), Quantity = 2 },
                new ScriptItem() { Key = "K0002", Name = "2-4だ", Date = new DateTime(2008, 2, 11), Quantity = 3 },
                new ScriptItem() { Key = "K0002", Name = "2-5だ", Date = new DateTime(2008, 2, 12), Quantity = 2 },
                new ScriptItem() { Key = "K0002", Name = "2-6だ", Date = new DateTime(2008, 2, 13), Quantity = 3 },
                new ScriptItem() { Key = "K0002", Name = "2-7だ", Date = new DateTime(2008, 2, 14), Quantity = 2 },
                new ScriptItem() { Key = "K0002", Name = "2-8だ", Date = new DateTime(2008, 2, 15), Quantity = 3 },
                new ScriptItem() { Key = "K0002", Name = "2-9だ", Date = new DateTime(2008, 2, 16), Quantity = 2 },
                new ScriptItem() { Key = "K0003", Name = "3-1", Date = new DateTime(2008, 2, 17), Quantity = 3 },
                new ScriptItem() { Key = "K0004", Name = "4-1ですよ", Date = new DateTime(2008, 2, 18), Quantity = 2 },
                new ScriptItem() { Key = "K0004", Name = "4-2ですよ", Date = new DateTime(2008, 2, 19), Quantity = 3 },
                new ScriptItem() { Key = "K0004", Name = "4-3ですよ", Date = new DateTime(2008, 2, 20), Quantity = 2 },
                new ScriptItem() { Key = "K0004", Name = "4-4ですよ", Date = new DateTime(2008, 2, 21), Quantity = 3 },
          };
            data["itemList"] = fooList;
            FPTemplate template = new FPTemplate();
            HSSFWorkbook wb = template.Process(@"TestResource\Template\FPTemplate_ScriptTest.xls", data);
            using (Stream s = new FileStream("out_FPTemplate_ScriptTest.xls", FileMode.Create, FileAccess.Write))
            {
                wb.Write(s);
            }
            IDictionary<string, object> data2 = new Dictionary<string, object>();
            data2["itemList"] = fooList;
            FPTemplate template2 = new FPTemplate();
            HSSFWorkbook wb2 = template2.Process(@"TestResource\Template\FPTemplate_SuspendTest.xls", data2);
            using (Stream s = new FileStream("out_FPTemplate_SuspendTest.xls", FileMode.Create, FileAccess.Write))
            {
                wb2.Write(s);
            }
        }

        [Test]
        public void TestHyperLink()
        {
            IDictionary<string, object> data = new Dictionary<string, object>();
            IList<Bar> fooList = new List<Bar>()
            {
                new Bar(){Num=10,Name="1行目",Address="http://www.google.com"},
                new Bar(){Num=20,Name="2行目",Address="http://www.seasar.org"},
                new Bar(){Num=30,Name="3行目",Address="http://www.apple.com"},
                new Bar(){Num=10,Name="4行目",Address="http://ja.openoffice.org/"},
            };
            data["fooList"] = fooList;
            FPTemplate template = new FPTemplate();
            HSSFWorkbook wb = template.Process(@"TestResource\Template\FPTemplate_LinkTest.xls", data);
            using (Stream s = new FileStream("out_FPTemplate_LinkTest.xls", FileMode.Create, FileAccess.Write))
            {
                wb.Write(s);
            }

        }

        public class Bar
        {
            public int Num { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
        }

        public class Foo
        {
            public int Num { get; set; }
            public string Name { get; set; }
        }

        public class ScriptItem
        {
            public string Key { get; set; }
            public string Name { get; set; }
            public DateTime Date { get; set; }
            public int Quantity { get; set; }
        }

    }
}
