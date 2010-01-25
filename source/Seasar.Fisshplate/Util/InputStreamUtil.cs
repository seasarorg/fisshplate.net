using System;
using System.Collections.Generic;

using System.Text;
using System.IO;

namespace Seasar.Fisshplate.Util
{
    public class InputStreamUtil
    {
        public static FileStream GetResourceAsStream(string name)
        {
            return new FileStream(name, FileMode.Open, FileAccess.Read);
        }

        public static void Close(FileStream fs)
        {
            if (fs != null)
            {
                fs.Close();
            }
        }
    }
}
