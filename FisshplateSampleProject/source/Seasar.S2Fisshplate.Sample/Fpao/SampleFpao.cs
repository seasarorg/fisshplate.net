using System;
using System.Collections.Generic;
using System.Text;
using Seasar.Quill.Attrs;
using Seasar.S2Fisshplate.Interceptors;
using NPOI.HSSF.UserModel;
using Seasar.S2Fisshplate.Attr;
using Seasar.S2Fisshplate.Sample.Dto;

namespace Seasar.S2Fisshplate.Sample.Fpao
{
    [Implementation]
    [Aspect(typeof(QuillFPInterceptor))]
    public interface SampleFpao
    {
        [FPTemplate(@"Template\FPSample.xls")]
        HSSFWorkbook GetSampleExcel(SampleDto dto);
    }
}
