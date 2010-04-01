using System;
using System.Collections.Generic;

using System.Text;
using Seasar.Fisshplate.Wrapper;

namespace Seasar.Fisshplate.Exception
{
    public class FPPreviewException : FPException
    {
        public FPPreviewException(string messageId)
            : base(messageId)
        {
        }
        public FPPreviewException(string messageId, object[] args)
            : base(messageId, args)
        {
        }


    }
}
