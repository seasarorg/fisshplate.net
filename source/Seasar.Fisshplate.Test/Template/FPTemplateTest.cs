using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;
using Seasar.Fisshplate.Template;
using System.IO;
using NPOI.HSSF.UserModel;
using Seasar.Fisshplate.Exception;
using System.Reflection;

namespace Seasar.Fisshplate.Test.Template
{
    [TestFixture]
    public class FPTemplateTest
    {
        private FPTemplate _template;

        private string _appDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        [Test]
        public void Test行の要素がリストの場合()
        {
            HSSFWorkbook wb = null;
            using (FileStream fs = new FileStream("FPTemplateTest.xls", FileMode.Open, FileAccess.Read))
            {
                try
                {
                    _template = new FPTemplate();
                    Dictionary<string, object> data = new Dictionary<string, object>();
                    data["title"] = "タイトルである";
                    List<A> aList = new List<A>()
                    {
	                    new A() { Name = "1行目", Num = 10, Date = DateTime.Now },
	                    new A() { Name = "2行目", Num = 20, Date = DateTime.Now },
	                    new A() { Name = "3行目", Num = 30, Date = DateTime.Now },
	                    new A() { Name = "4行目", Num = 10, Date = DateTime.Now },
	                    new A() { Name = "5行目", Num = 20, Date = DateTime.Now },
	                    new A() { Name = "6行目", Num = 30, Date = DateTime.Now },
                    };

                    data["b"] = aList;

                    wb = _template.Process(fs, data);

                }
                catch (FPParseException)
                {
                    Assert.Fail();
                }
            }
            using (FileStream fos = new FileStream(_appDirectory + @"\out.xls", FileMode.OpenOrCreate, FileAccess.Write))
            {
                wb.Write(fos);
            }
        }
        [Test]
        public void Test行の要素がリストの場合_1件()
        {
            HSSFWorkbook wb = null;
            using (FileStream fs = new FileStream("FPTemplateTest.xls", FileMode.Open, FileAccess.Read))
            {
                try
                {
                    _template = new FPTemplate();
                    Dictionary<string, object> data = new Dictionary<string, object>();
                    data.Add("title", "タイトルである");
                    List<A> aList = new List<A>();
                    aList.Add(new A("1行目", 10, DateTime.Now));
                    data.Add("b", aList);

                    wb = _template.Process(fs, data);

                }
                catch (FPParseException)
                {
                    Assert.Fail();
                }
            }
            using (FileStream fos = new FileStream(_appDirectory + @"\out_1.xls", FileMode.OpenOrCreate, FileAccess.Write))
            {
                wb.Write(fos);
            }
        }

        [Test]
        public void Test行の要素が配列の場合()
        {
            HSSFWorkbook wb = null;
            using (FileStream fs = new FileStream("FPTemplateTest.xls", FileMode.Open, FileAccess.Read))
            {
                try
                {
                    _template = new FPTemplate();
                    Dictionary<string, object> data = new Dictionary<string, object>();
                    data.Add("title", "配列tのテスト");
                    A[] aList = new A[]
                    {
                        new A("1行目", 10, DateTime.Now),
                        new A("2行目", 20, DateTime.Now),
                        new A("3行目", 30, DateTime.Now),
                        new A("4行目", 10, DateTime.Now)
                    };
                    data.Add("b", aList);

                    wb = _template.Process(fs, data);

                }
                catch (FPParseException)
                {
                    Assert.Fail();
                }
            }
            using (FileStream fos = new FileStream(_appDirectory + @"\out_array.xls", FileMode.OpenOrCreate, FileAccess.Write))
            {
                wb.Write(fos);
            }
        }

        [Test]
        public void Testループのテスト()
        {
            HSSFWorkbook wb = null;
            using (FileStream fs = new FileStream("FPTemplateTest_nestedLoop.xls", FileMode.Open, FileAccess.Read))
            {
                try
                {
                    _template = new FPTemplate();
                    Dictionary<string, object> data = new Dictionary<string, object>();
                    object[] parentList = new object[]
                    {
                        new string[]{"子供1","子供2","子供3","子供4"},
                        new string[]{"子供5","子供6","子供7","子供8"},
                        new string[]{"子供9","子供10","子供11","子供12"},
   
                    };

                    data.Add("parentList", parentList);

                    wb = _template.Process(fs, data);

                }
                catch (FPParseException)
                {
                    Assert.Fail();
                }
            }
            using (FileStream fos = new FileStream(_appDirectory + @"\out_nestedLoop.xls", FileMode.OpenOrCreate, FileAccess.Write))
            {
                wb.Write(fos);
            }
        }

        [Test]
        public void Test最後のヘッダフッタ制御のテスト_ぴったりの場合()
        {
            HSSFWorkbook wb = null;
            using (FileStream fs = new FileStream("FPTemplateTest_lastPageHandling.xls", FileMode.Open, FileAccess.Read))
            {
                try
                {
                    _template = new FPTemplate();
                    Dictionary<string, object> data = new Dictionary<string, object>();
                    data.Add("title", "タイトルである");
                    List<A> aList = new List<A>();
                    aList.Add(new A("1行目", 10, DateTime.Now));
                    aList.Add(new A("2行目", 20, DateTime.Now));
                    aList.Add(new A("3行目", 30, DateTime.Now));
                    aList.Add(new A("4行目", 10, DateTime.Now));
                    aList.Add(new A("5行目", 20, DateTime.Now));
                    aList.Add(new A("6行目", 30, DateTime.Now));
                    aList.Add(new A("7行目", 10, DateTime.Now));
                    aList.Add(new A("8行目", 20, DateTime.Now));
                    aList.Add(new A("9行目", 30, DateTime.Now));

                    data.Add("b", aList);

                    wb = _template.Process(fs, data);

                }
                catch (FPParseException)
                {
                    Assert.Fail();
                }
            }
            using (FileStream fos = new FileStream(_appDirectory + @"\out_lastPageHandling.xls", FileMode.OpenOrCreate, FileAccess.Write))
            {
                wb.Write(fos);
            }
        }

        [Test]
        public void Test最後のヘッダフッタ制御のテスト_あまる場合()
        {
            HSSFWorkbook wb = null;
            using (FileStream fs = new FileStream("FPTemplateTest_lastPageHandling2.xls", FileMode.Open, FileAccess.Read))
            {
                try
                {
                    _template = new FPTemplate();
                    Dictionary<string, object> data = new Dictionary<string, object>();
                    data.Add("title", "タイトルである");
                    List<A> aList = new List<A>();
                    aList.Add(new A("1行目", 10, DateTime.Now));
                    aList.Add(new A("2行目", 20, DateTime.Now));
                    aList.Add(new A("3行目", 30, DateTime.Now));
                    aList.Add(new A("4行目", 10, DateTime.Now));
                    aList.Add(new A("5行目", 20, DateTime.Now));
                    aList.Add(new A("6行目", 30, DateTime.Now));
                    aList.Add(new A("7行目", 10, DateTime.Now));
                    aList.Add(new A("8行目", 20, DateTime.Now));
                    aList.Add(new A("9行目", 30, DateTime.Now));

                    data.Add("b", aList);

                    wb = _template.Process(fs, data);

                }
                catch (FPParseException)
                {
                    Assert.Fail();
                }
            }
            using (FileStream fos = new FileStream(_appDirectory + @"\out_lastPageHandling2.xls", FileMode.OpenOrCreate, FileAccess.Write))
            {
                wb.Write(fos);
            }
        }

        [Test]
        public void Test空行指定()
        {
            HSSFWorkbook wb = null;
            using (FileStream fs = new FileStream("FPTemplateTest_iteratorMax.xls", FileMode.Open, FileAccess.Read))
            {
                try
                {
                    _template = new FPTemplate();
                    Dictionary<string, object> data = new Dictionary<string, object>();
                    data.Add("title", "タイトルである");
                    List<A> aList = new List<A>();
                    aList.Add(new A("1行目", 10, DateTime.Now));
                    aList.Add(new A("2行目", 20, DateTime.Now));
                    aList.Add(new A("3行目", 30, DateTime.Now));
                    aList.Add(new A("4行目", 10, DateTime.Now));
                    aList.Add(new A("5行目", 20, DateTime.Now));
                    aList.Add(new A("6行目", 30, DateTime.Now));

                    data.Add("b", aList);

                    wb = _template.Process(fs, data);

                }
                catch (FPParseException)
                {
                    Assert.Fail();
                }
            }
            using (FileStream fos = new FileStream(_appDirectory + @"\out_iteratorMax.xls", FileMode.OpenOrCreate, FileAccess.Write))
            {
                wb.Write(fos);
            }
        }

        [Test]
        public void Test空行指定_改ページ対応()
        {
            HSSFWorkbook wb = null;
            using (FileStream fs = new FileStream("FPTemplateTest_iteratorMax_pageBreak.xls", FileMode.Open, FileAccess.Read))
            {
                try
                {
                    _template = new FPTemplate();
                    Dictionary<string, object> data = new Dictionary<string, object>();
                    data.Add("title", "タイトルである");
                    List<A> aList = new List<A>();
                    aList.Add(new A("1行目", 10, DateTime.Now));
                    aList.Add(new A("2行目", 20, DateTime.Now));
                    aList.Add(new A("3行目", 30, DateTime.Now));
                    aList.Add(new A("4行目", 10, DateTime.Now));
                    aList.Add(new A("5行目", 20, DateTime.Now));
                    aList.Add(new A("6行目", 30, DateTime.Now));
                    aList.Add(new A("7行目", 10, DateTime.Now));
                    aList.Add(new A("8行目", 20, DateTime.Now));
                    aList.Add(new A("9行目", 30, DateTime.Now));
                    aList.Add(new A("10行目", 10, DateTime.Now));
                    aList.Add(new A("11行目", 20, DateTime.Now));
                    aList.Add(new A("12行目", 30, DateTime.Now));

                    data.Add("b", aList);

                    wb = _template.Process(fs, data);

                }
                catch (FPParseException)
                {
                    Assert.Fail();
                }
            }
            using (FileStream fos = new FileStream(_appDirectory + @"\out_iteratorMax_pageBreak.xls", FileMode.OpenOrCreate, FileAccess.Write))
            {
                wb.Write(fos);
            }
        }

        public class A
        {
            public string Name { get; set; }
            public int Num { get; set; }
            public DateTime Date { get; set; }

            public A() { }

            public A(string name, int num, DateTime date)
            {
                this.Name = name;
                this.Num = num;
                this.Date = date;
            }
        }

    }
}
