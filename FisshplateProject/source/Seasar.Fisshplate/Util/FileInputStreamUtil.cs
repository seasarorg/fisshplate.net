using System;
using System.Collections.Generic;

using System.Text;
using System.IO;

namespace Seasar.Fisshplate.Util
{
    public static class FileInputStreamUtil
    {
        public static FileStream CreateFileStream(string filePath)
        {
            try
            {
                return new FileStream(filePath, FileMode.Open, FileAccess.Read);
            }
            catch (FileNotFoundException e)
            {
                throw new ApplicationException("対象:" + filePath + ":ファイルがみつかりません", e);
            }
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
