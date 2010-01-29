using System;
using System.Collections.Generic;
using System.Text;

namespace Seasar.S2Fisshplate.Sample.Dto
{
    public class SampleDto
    {
        public string Title { get; set; }
        public int Number { get; set; }
        public IList<HogeDto> HogeItems { get; set; }
    }
}
