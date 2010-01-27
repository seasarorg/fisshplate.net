using System;
using System.Collections.Generic;
using System.Text;

namespace Seasar.S2Fisshplate.Attr
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple=false)]
    public class FPTemplateAttribute : Attribute
    {
        private readonly string _path;

        public string Path
        {
            get { return _path; }
        }

        public FPTemplateAttribute(string path)
        {
            _path = path;
        }

    }
}
