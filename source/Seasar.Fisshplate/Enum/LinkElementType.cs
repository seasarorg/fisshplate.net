using System;
using System.Collections.Generic;

using System.Text;
using NPOI.HSSF.UserModel;

namespace Seasar.Fisshplate.Enum
{
    public class LinkElementType
    {
        private int _linkType;

        public static readonly LinkElementType URL = new LinkElementType(HSSFHyperlink.LINK_URL);
        public static readonly LinkElementType EMAIL = new LinkElementType(HSSFHyperlink.LINK_EMAIL);
        public static readonly LinkElementType THIS = new LinkElementType(HSSFHyperlink.LINK_DOCUMENT);
        public static readonly LinkElementType FILE = new LinkElementType(HSSFHyperlink.LINK_FILE);

        private LinkElementType(int type)
        {
            this._linkType = type;
        }

        public static LinkElementType Get(string type)
        {
            switch (type)
            {
                case "url" :
                    return URL;
                case "email":
                    return EMAIL;
                case "file":
                    return FILE;
                case "this":
                    return THIS;
                default:
                    return null;
            }
        }

        internal NPOI.HSSF.UserModel.HSSFHyperlink CreateHyperlink(string type)
        {
            return new HSSFHyperlink(_linkType);
        }
    }
}
