using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seasar.S2Fisshplate.Attr;
using NPOI.HSSF.UserModel;
using Seasar.Quill.Attrs;

namespace Seasar.S2Fisshplate.Test.Interceptors
{
    [Implementation]
    [Aspect(typeof(Seasar.S2Fisshplate.Interceptors.FPQuillInterceptor))]
    public interface HogeFpao
    {
        [FPTemplate("TemplateS2Fisshplate.xls")]
        HSSFWorkbook GetHogeFisshplate(HogeDto dto);

        [FPTemplate(@"Template\S2FPTemplateTest.xls")]
        HSSFWorkbook GetFolderTemplate(S2TestDto dto);

        [FPTemplate(@"Template\Hoge\S2FPTemplateTest_List.xls")]
        HSSFWorkbook GetInterceptorIListTest(IList<HogeDto> itemList);
    }
}
