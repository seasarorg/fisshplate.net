using System;
using System.Collections.Generic;

using System.Text;
using Seasar.Fisshplate.Consts;
using System.Text.RegularExpressions;

namespace Seasar.Fisshplate.Core
{
    /// <summary>
    /// バインド変数を表すクラスです。
    /// </summary>
    public class BindVariable
    {
        private string _name;

        private bool _nullAllowed;

        private object _nullValue;


        /// <summary>
        /// コンストラクタです。セル上に記載されたバインド変数を受け取ります。
        /// 式の中に!がある場合は、NULLを許可します。
        /// !の後に値が続く場合は、NULL時のデフォルト値とします。
        /// </summary>
        /// <param name="var">セル上に記載されたバインド変数</param>
        public BindVariable(string variable)
        {
            string baseVar = Regex.Replace(variable, "^" + FPConsts.RegexBindVarStart + "(?<1>.+)" + FPConsts.RegexBindVarEnd + "$", "${1}");
            int idx = baseVar.IndexOf(FPConsts.NullValueOperator);
            _nullAllowed = (idx >= 1);
            if (_nullAllowed)
            {
                _name = baseVar.Substring(0, idx);
                _nullValue = baseVar.Substring(idx + 1);
            }
            else
            {
                _name = baseVar;
            }
        }

        /// <summary>
        /// 変数名
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// NULL時のデフォルト値
        /// </summary>
        public object NullValue
        {
            get { return _nullValue; }
        }

        /// <summary>
        /// NULLを許可するか否か
        /// </summary>
        public bool NullAllowed
        {
            get { return _nullAllowed; }
        }
    }
}
