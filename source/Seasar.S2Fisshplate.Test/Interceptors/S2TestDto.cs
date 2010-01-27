using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seasar.S2Fisshplate.Test.Interceptors
{
    public class S2TestDto
    {
        public String Title { get; set; }

        public IList<HogeDto> ItemList { get; set; }
    }
}
