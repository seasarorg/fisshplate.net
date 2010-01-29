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
    }
}
