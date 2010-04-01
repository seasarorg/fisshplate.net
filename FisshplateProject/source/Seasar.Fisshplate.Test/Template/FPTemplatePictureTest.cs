using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using System.IO;
using NPOI.HSSF.UserModel;
using Seasar.Fisshplate.Template;
using System.Collections;

namespace Seasar.Fisshplate.Test.Template
{
    [TestFixture]
    public class FPTemplatePictureTest
    {
        [Test]
        public void Test画像出力()
        {
            HSSFWorkbook wb = null;
            using (Stream s = new FileStream(@"TestResource\Template\FPTemplatePictureTest.xls", FileMode.Open, FileAccess.Read))
            {
                FPTemplate template = new FPTemplate();
                IDictionary<string, object> data = new Dictionary<string, object>();
                data["data"] = new A("1行目", 10, DateTime.Now, @"TestResource\picture1.png");

                wb = template.Process(s, data);
            }
            using (Stream fos = new FileStream("out_picture.xls", FileMode.Create, FileAccess.Write))
            {
                wb.Write(fos);
            }
        }


        [Test]
        public void Test行の要素がリストの場合の画像出力()
        {
            HSSFWorkbook wb = null;
            using (Stream s = new FileStream(@"TestResource\Template\FPTemplatePictureTest2.xls", FileMode.Open, FileAccess.Read))
            {
                FPTemplate template = new FPTemplate();
                IDictionary<string, object> data = new Dictionary<string, object>();
                data["title"] = "タイトルである";
                IList<A> aList = new List<A>()
                {
                    new A("1行目", 10, DateTime.Now, @"TestResource\picture1.png"),
                    new A("2行目", 20, DateTime.Now, null),
                    new A("3行目", 30, DateTime.Now, @"TestResource\picture2.jpg"),
                    new A("4行目", 10, DateTime.Now, @"TestResource\picture1.png"),
                    new A("5行目", 20, DateTime.Now, @"TestResource\picture1.png"),
                    new A("6行目", 30, DateTime.Now, @"TestResource\picture3.png"),
                };
                data["b"] = aList;

                wb = template.Process(s, data);
            }
            using (Stream fos = new FileStream("out_picture2.xls", FileMode.Create, FileAccess.Write))
            {
                wb.Write(fos);
            }
        }

        [Test]
        public void Test画像出力_パスをリテラルと変数で指定()
        {
            HSSFWorkbook wb = null;
            FPTemplate template = new FPTemplate();
            IDictionary<string, object> data = new Dictionary<string, object>();
            data["data"] = "hoge";
            data["picture"] = @"TestResource\seasar.jpg";
            wb = template.Process(@"TestResource\Template\FPTemplatePictureTest3.xls", data);

            using (Stream fos = new FileStream(@"TestResource\out_picture3.xls", FileMode.Create, FileAccess.Write))
            {
                wb.Write(fos);
            }
        }
        
        public class A
        {
            public string Name { get; set; }
            public int Num { get; set; }
            public DateTime Date { get; set; }
            public string Picture { get; set; }

            public A(string name, int num, DateTime date, string picture)
            {
                this.Name = name;
                this.Num = num;
                this.Date = date;
                this.Picture = picture;
            }
        }
    }
}
