using System;
using System.Collections.Generic;

using System.Text;
using Seasar.Fisshplate.Wrapper;
using System.Resources;

namespace Seasar.Fisshplate.Exception
{
    //TODO いずれSeasar.NET の MessageFormatterを利用するように変更
    public class FPException : System.Exception
    {
        private static object[] EmptyArgs = new object[] { };

        private string _messageId;

        public string MessageId
        {
            get { return _messageId; }
        }

        private object[] _args;

        public object[] Args
        {
            get 
            {
                if (_args == null)
                {
                    return null;
                }
                return (object[])_args.Clone();
            }
        }

        private string _message;

        public override string Message
        {
            get
            {
                return _message;
            }
        }

        public FPException(string messageId)
            : this(messageId, EmptyArgs)
        {
        }

        public FPException(string messageId, object[] args)
            : this(messageId, args, null, null)
        {
        }

        public FPException(string messageId, RowWrapper row)
            : this(messageId, null, row, null)
        {
        }

        public FPException(string messageId, object[] args, RowWrapper row)
            : this(messageId, args, row, null)
        {
        }

        public FPException(string messageId, object[] args, System.Exception ex)
            : this(messageId, args, null, ex)
        {
        }

        /// <summary>
        /// リソースバンドルのキーを受け取って例外をラップします。
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="args"></param>
        /// <param name="row"></param>
        /// <param name="ex"></param>
        public FPException(string messageId, object[] args, RowWrapper row, System.Exception ex)
        {
            this._messageId = messageId;
            this._args = GetParam(args, row);

            // 自分のリソースマネージャを取得
            ResourceManager rm = EFPMessages.ResourceManager;
            
            string msg = rm.GetString(messageId);
            // [EFP99999]メッセージ のような形式にする。
            this._message = "[" + messageId + "]" + String.Format(msg, _args);
        }

        private object[] GetParam(object[] args, RowWrapper row)
        {
            if (row == null || row.IsNullRow())
            {
                return args;
            }
            return GetParamIncludingRowNum(args, row);
        }

        private object[] GetParamIncludingRowNum(object[] args, RowWrapper row)
        {
            int rowNum = row.HSSFRow.RowNum + 1;
            int paramLength = (args == null) ? 1 : args.Length + 1;
            object[] parameters = new object[paramLength];
            for (int i = 0; i < paramLength -1; i++)
            {
                parameters[i] = args[i];
            }
            parameters[paramLength - 1] = row.Sheet.SheetName + " : " + rowNum;
            return parameters;
        }


    }
}
