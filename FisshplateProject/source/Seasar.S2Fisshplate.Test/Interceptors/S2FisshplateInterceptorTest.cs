using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using NPOI.HSSF.UserModel;
using System.IO;
using Seasar.Quill;

namespace Seasar.S2Fisshplate.Test.Interceptors
{
    [TestFixture]
    public class S2FisshplateInterceptorTest
    {
        protected HogeFpao _hogeFpao;

        [Test]
        public void TestQuillとの連携テスト()
        {
            QuillInjector.GetInstance().Inject(this);

            HogeDto dto = new HogeDto()
            {
                Name = "名前",
                Num = 111,
                Date = DateTime.Now
            };
            HSSFWorkbook wb = _hogeFpao.GetHogeFisshplate(dto);
            Assert.IsNotNull(wb);

            using (Stream fos = new FileStream("out_InterceptorTest.xls", FileMode.Create, FileAccess.Write))
            {
                wb.Write(fos);
            }

        }

        [Test]
        public void TestQuillとの連携テスト_フォルダの中()
        {
            QuillInjector.GetInstance().Inject(this);

            S2TestDto dto = new S2TestDto();
            dto.Title = "Quill連携テスト";
            IList<HogeDto> itemList = new List<HogeDto>();
            itemList.Add(new HogeDto() { Name = "1行目", Num = 10, Date = DateTime.Now });
            itemList.Add(new HogeDto() { Name = "2行目", Num = 20, Date = DateTime.Now });
            itemList.Add(new HogeDto() { Name = "3行目", Num = 30, Date = DateTime.Now });

            dto.ItemList = itemList;

            HSSFWorkbook wb = _hogeFpao.GetFolderTemplate(dto);
            Assert.IsNotNull(wb);

            using (Stream fos = new FileStream("out_InterceptorFolderTest.xls", FileMode.Create, FileAccess.Write))
            {
                wb.Write(fos);
            }
        }

        [Test]
        public void TestQuillとの連携_IListの場合()
        {
            QuillInjector.GetInstance().Inject(this);

            IList<HogeDto> itemList = new List<HogeDto>();
            itemList.Add(new HogeDto() { Name = "1行目", Num = 10, Date = DateTime.Now });
            itemList.Add(new HogeDto() { Name = "2行目", Num = 20, Date = DateTime.Now });
            itemList.Add(new HogeDto() { Name = "3行目", Num = 30, Date = DateTime.Now });

            HSSFWorkbook wb = _hogeFpao.GetInterceptorIListTest(itemList);
            Assert.IsNotNull(wb);

            using (Stream fos = new FileStream("out_InterceptorListTest.xls", FileMode.Create, FileAccess.Write))
            {
                wb.Write(fos);
            }
        }

    }
}
