using System;
using System.Collections.Generic;

using System.Text;
using Seasar.Fisshplate.Wrapper;

namespace Seasar.Fisshplate.Exception
{
    /// <summary>
    /// テンプレート解析時に投げられる例外クラスです。
    /// </summary>
    public class FPParseException : FPException
    {
        public FPParseException(string messageId)
            : base(messageId)
        {
        }
        public FPParseException(string messageId, object[] args)
            : base(messageId, args, null, null)
        {
        }

        public FPParseException(string messageId, RowWrapper row)
            : base(messageId, null, row, null)
        {
        }

        public FPParseException(string messageId, object[] args, RowWrapper row)
            : base(messageId, args, row, null)
        {
        }

        public FPParseException(string messageId, object[] args, System.Exception ex)
            : base(messageId, args, null, ex)
        {
        }

        public FPParseException(string messageId, object[] args, RowWrapper row, System.Exception ex)
            : base(messageId, args, row, ex)
        {
        }
    }
}
