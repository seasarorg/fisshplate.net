using System;
using System.Collections.Generic;
using System.Text;

namespace Seasar.Fisshplate.Util
{
    public static class StringUtil
    {
        public static string ParseSuffix(string picturePath)
        {
            int lastSeparatorIndex = picturePath.LastIndexOf('.');
            string suffix = picturePath.Substring(lastSeparatorIndex + 1);
            return suffix;
        }
    }
}
