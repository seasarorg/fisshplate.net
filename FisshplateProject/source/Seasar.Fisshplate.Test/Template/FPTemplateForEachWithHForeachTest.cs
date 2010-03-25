using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using NPOI.HSSF.UserModel;
using System.IO;
using Seasar.Fisshplate.Template;
using Seasar.Fisshplate.Exception;
using System.Reflection;


namespace Seasar.Fisshplate.Test.Template
{
    [TestFixture]
    public class FPTemplateForEachWithHForeachTest
    {
        private string _appDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        [Test]
        public void test_単純なForEachとHForEach()
        {
            HSSFWorkbook wb = null;
            using (FileStream fs = new FileStream(@"TestResource\Template\FPTemplate_ForEach_HForEachTest.xls", FileMode.Open, FileAccess.Read))
            {
                try
                {
                    FPTemplate template = new FPTemplate();
                    Dictionary<string, object> data = new Dictionary<string, object>();
                    data.Add("title", "タイトルである");
                    List<A> aList = new List<A>();
                    aList.Add(new A("1行目", 10, DateTime.Now, new String[] {"aa", "bb", "cc"}));
                    aList.Add(new A("2行目", 20, DateTime.Now, new String[] { "aa" }));
                    aList.Add(new A("3行目", 30, DateTime.Now, new String[] { "aa", "bb", "cc" }));
                    aList.Add(new A("4行目", 10, DateTime.Now, new String[] { "aa", "cc" }));
                    aList.Add(new A("5行目", 20, DateTime.Now, new String[] { "aa", "bb", "cc" }));
                    aList.Add(new A("6行目", 30, DateTime.Now, new String[] { "bb", "cc" }));
                    aList.Add(new A("7行目", 10, DateTime.Now, new String[] { "aa", "bb", "cc" }));
                    aList.Add(new A("8行目", 20, DateTime.Now, new String[] { "aa", "bb"}));
                    aList.Add(new A("9行目", 30, DateTime.Now, new String[] { "cc" }));

                    data.Add("fooList", aList);

                    wb = template.Process(fs, data);

                }
                catch (FPParseException ex)
                {
                    System.Console.WriteLine(ex.Message);
                    Assert.Fail();
                }
            }
            using (FileStream fos = new FileStream(_appDirectory + @"\out_FPTemplate_ForEach_HForEachTest.xls", FileMode.OpenOrCreate, FileAccess.Write))
            {
                wb.Write(fos);
            }
        }

        [Test]
        public void test_カラム項目と行が可変な帳票()
        {
            HSSFWorkbook wb = null;
            using (FileStream fs = new FileStream(@"TestResource\Template\FPTemplate_ForEach_HForEachTest2.xls", FileMode.Open, FileAccess.Read))
            {
                try
                {
                    FPTemplate template = new FPTemplate();
                    Dictionary<string, object> data = new Dictionary<string, object>();

                    ViewDto view = new ViewDto()
                    {
                        Title = "メダルリスト",
                        ColumnList = new List<ColumnDto>()
                        {
                            new ColumnDto() { Name = "金"},
                            new ColumnDto() { Name = "銀"},
                            new ColumnDto() { Name = "銅"},
                            new ColumnDto() { Name = "-"}
                        },
                    };

                    view.RowList = new List<RowDto>();
                    for (int i = 1; i <= 10; i++)
                    {
                        var item = new RowDto();
                        item.Name = "国" + i.ToString();
                        item.Nums = new Dictionary<String, int?>();
                        int? count = i % 2;
                        if (count == 0)
                        {
                            count = null;
                        }
                        item.Nums["金"] = count;
                        count = i % 3;
                        if (count == 0)
                        {
                            count = null;
                        }
                        item.Nums["銀"] = count;
                        count = i % 4;
                        if (count == 0)
                        {
                            count = null;
                        }
                        item.Nums["銅"] = count;
                        item.Nums["-"] = null;

                        item.Sum = (item.Nums["金"] ?? 0) + (item.Nums["銀"] ?? 0) + (item.Nums["銅"] ?? 0);

                        view.RowList.Add(item);
                    }

                    data.Add("data", view);

                    wb = template.Process(fs, data);

                }
                catch (FPParseException ex)
                {
                    System.Console.WriteLine(ex.Message);
                    Assert.Fail();
                }
            }
            using (FileStream fos = new FileStream(_appDirectory + @"\out_FPTemplate_ForEach_HForEachTest2.xls", FileMode.OpenOrCreate, FileAccess.Write))
            {
                wb.Write(fos);
            }
        }

        public class ViewDto
        {
            public String Title { get; set; }
            public IList<RowDto> RowList { get; set; }
            public IList<ColumnDto> ColumnList { get; set; }
        }
        public class RowDto
        {
            public String Name { get; set; }
            public IDictionary<String, int?> Nums { get; set; }
            public int? Sum { get; set; }
        }
        public class ColumnDto
        {
            public String Name { get; set; }
        }


        public class A
        {
            public A()
            {
            }
            public A(String name, int num, DateTime time, IList<String> bList)
            {
                this.Name = name;
                this.Num = num;
                this.Time = time;
                this.BarList = new List<B>();
                foreach (var item in bList)
                {
                    this.BarList.Add(new B(item));
                }
            }
            public String Name { get; set; }
            public int Num { get; set; }
            public DateTime Time { get; set; }

            public IList<B> BarList { get; set; }
        }

        public class B
        {
            public B() { }
            public B(String kana)
            {
                this.Kana = kana;
            }
            public String Kana { get; set; }
        }

    }



}
