using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using System.IO;
using System.Collections;
using Seasar.Fisshplate.Template;
using NPOI.HSSF.UserModel;
using System.Reflection;

namespace Seasar.Fisshplate.Test.Element
{
    [TestFixture]
    public class HorizontalIteratorBlockTest
    {
        private string _appDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        [Test]
        public void Test横展開()
        {
            HSSFWorkbook wb = null;
            using (Stream s = new FileStream(@"HorizontalIteratorTest.xls", FileMode.Open))
            {
                IList<Sample> list = new List<Sample>()
                {
                    new Sample(){ Month=1,AmountA=10,AmountB=20,AmountC=30,AmountD=40 },
                    new Sample(){ Month=2,AmountA=12,AmountB=21,AmountC=31,AmountD=50 },
                    new Sample(){ Month=3,AmountA=14,AmountB=19,AmountC=32,AmountD=60 },
                    new Sample(){ Month=4,AmountA=16,AmountB=18,AmountC=33,AmountD=70 },
               };
                IDictionary<string, object> data = new Dictionary<string, object>();
                data["list"] = list;
                FPTemplate fp = new FPTemplate();
                wb = fp.Process(s, data);
            }
            using (Stream fos = new FileStream(_appDirectory + @"\HorizonalIterator.xls", FileMode.OpenOrCreate, FileAccess.Write))
            {
                wb.Write(fos);
            }
        }

        public class Sample
        {
            public int AmountA { get; set; }
            public int AmountB { get; set; }
            public int AmountC { get; set; }
            public int AmountD { get; set; }
            public int Month { get; set; }
        }
    }
}
