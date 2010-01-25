using System;
using System.Collections.Generic;

using System.Text;
using Seasar.Fisshplate.Wrapper;

namespace Seasar.Fisshplate.Exception
{
    public class FPMergeException : FPException
    {
        public FPMergeException(string messageId)
            : base(messageId)
        {
        }
        public FPMergeException(string messageId, object[] args)
            : base(messageId, args, null, null)
        {
        }

        public FPMergeException(string messageId, RowWrapper row)
            : base(messageId, null, row, null)
        {
        }

        public FPMergeException(string messageId, object[] args, RowWrapper row)
            : base(messageId, args, row, null)
        {
        }

        public FPMergeException(string messageId, object[] args, System.Exception ex)
            : base(messageId, args, null, ex)
        {
        }

        public FPMergeException(string messageId, object[] args, RowWrapper row, System.Exception ex)
            : base(messageId, args, row, ex)
        {
        }

    }
}
