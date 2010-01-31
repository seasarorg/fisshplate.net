using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Seasar.S2Fisshplate.Sample.Dto;
using Seasar.Quill;
using Seasar.S2Fisshplate.Sample.Fpao;
using NPOI.HSSF.UserModel;
using System.IO;
using Seasar.Fisshplate.Template;

namespace Seasar.S2Fisshplate.Sample
{
    public partial class Form1 : Form
    {
        // Interceptor
        protected SampleFpao sampleFpao;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // SampleDtoを作成します。
            SampleDto dto = new SampleDto()
            {
                Title = "Quillと連携しました",
                Number = 123456,
                HogeItems = new List<HogeDto>()
                {
                    new HogeDto() { Number = 10, Name = "1番目", Date = DateTime.Now },
                    new HogeDto() { Number = 20, Name = "2番目", Date = DateTime.Now },
                    new HogeDto() { Number = 30, Name = "3番目", Date = DateTime.Now },
                    new HogeDto() { Number = 10, Name = "4番目", Date = DateTime.Now },
                    new HogeDto() { Number = 20, Name = "5番目", Date = DateTime.Now },
                    new HogeDto() { Number = 10, Name = "6番目", Date = DateTime.Now },
                }
            };

            HSSFWorkbook wb = sampleFpao.GetSampleExcel(dto);
            if (wb != null)
            {
                using (Stream s = new FileStream("out_quillSample.xls", FileMode.Create, FileAccess.Write))
                {
                    wb.Write(s);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            IDictionary<string, object> data = new Dictionary<string, object>();
            IList<Foo> list = new List<Foo>();
            list.Add(new Foo() { Name = "1行目", Num = 10 });
            list.Add(new Foo() { Name = "2行目", Num = 20 });
            list.Add(new Foo() { Name = "3行目", Num = 30 });
            list.Add(new Foo() { Name = "1行目", Num = 10 });
            list.Add(new Foo() { Name = "2行目", Num = 20 });
            list.Add(new Foo() { Name = "3行目", Num = 30 });
            data["fooList"] = list;

            FPTemplate template = new FPTemplate();
            HSSFWorkbook wb = null;
            using(Stream s = new FileStream(@"template\ForEachSample.xls", FileMode.Open, FileAccess.Read))
            {
                wb = template.Process(s, data);
            }
            using (Stream s = new FileStream(@"out_ForEachSample.xls", FileMode.Create, FileAccess.Write))
            {
                wb.Write(s);
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            IList<Data> list = new List<Data>();
            list.Add(new Data() { Month = 1, AmountA = 10, AmountB = 20, AmountC = 30, AmountD = 40 });
            list.Add(new Data() { Month = 2, AmountA = 12, AmountB = 21, AmountC = 31, AmountD = 50 });
            list.Add(new Data() { Month = 3, AmountA = 14, AmountB = 19, AmountC = 32, AmountD = 60 });
            list.Add(new Data() { Month = 4, AmountA = 16, AmountB = 18, AmountC = 33, AmountD = 70 });
            IDictionary<string, object> data = new Dictionary<string, object>();
            data["list"] = list;

            FPTemplate template = new FPTemplate();
            HSSFWorkbook wb = null;
            using (Stream s = new FileStream(@"template\HForEachSample.xls", FileMode.Open, FileAccess.Read))
            {
                wb = template.Process(s, data);
            }
            using (Stream s = new FileStream(@"out_HForEachSample.xls", FileMode.Create, FileAccess.Write))
            {
                wb.Write(s);
            }

        }

        public class Foo
        {
            public string Name { get; set; }
            public int Num { get; set; }
        }

        public class Data
        {
            public int AmountA { get; set; }
            public int AmountB { get; set; }
            public int AmountC { get; set; }
            public int AmountD { get; set; }
            public int Month { get; set; }
        }

    }
}
