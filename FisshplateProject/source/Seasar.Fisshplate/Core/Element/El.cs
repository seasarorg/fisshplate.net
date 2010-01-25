using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Seasar.Fisshplate.Consts;
using Seasar.Fisshplate.Context;
using Seasar.Fisshplate.Util;
using Microsoft.JScript;
using Seasar.Fisshplate.Exception;
using System.Collections;

namespace Seasar.Fisshplate.Core.Element
{
    /// <summary>
    /// テンプレートのセルの値にバインド変数が含まれる場合の要素クラスです。OGNLで評価します。
    /// </summary>
    public class El : TemplateElement
    {
        private Dictionary<string, object> _expressionMap = new Dictionary<string, object>();
        private AbstractCell _targetElement;

        private string _originalCellValue;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="target">バインド変数を評価する対象となるセル要素</param>
        public El(AbstractCell target)
        {
            _targetElement = target;
            // TODO : toString で大丈夫か？
            _originalCellValue = target.CellValue.ToString();
            Regex regex = new Regex(FPConsts.RegexBindVar);

            MatchCollection matches = regex.Matches(_originalCellValue);
            foreach (Match match in matches)
            {
                _expressionMap.Add(match.Value, null);
            }
        }

        #region TemplateElement メンバ

        public void Merge(FPContext context)
        {
            object value = GetBoundValue(context);
            _targetElement.CellValue = value;
            _targetElement.Merge(context);
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="Seasar.Fisshplate.Exception.FPMergeException"></exception>
        public object GetBoundValue(FPContext context)
        {
            object value = null;
            if (context.SkipMerge)
            {
                value = "";
            }
            else
            {
                PutValueToMap(context);
                value = BuildValue();
            }
            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="Seasar.Fisshplate.Exception.FPMergeException"></exception>
        private void PutValueToMap(FPContext context)
        {

            IDictionary<string, object> data = context.Data;
            Dictionary<string, object>.KeyCollection keys = _expressionMap.Keys;
            Dictionary<string, object> back = new Dictionary<string,object>();

            foreach (string expString in keys)
            {
                BindVariable bindVar = new BindVariable(expString);
                object value = GetValue(data, bindVar);
                value = ConvertLineFeed(value);
                back[expString] = value;
            }
            foreach (string expString in back.Keys)
            {
                _expressionMap[expString] = back[expString];
            }
        }

        protected object ConvertLineFeed(object value)
        {
            if (value is string)
            {
                value = ((string)value).Replace("\r\n", "\n").Replace("\r", "\n");
            }
            return value;
        }

        private object BuildValue()
        {
            if (OnlySingleBindVarIn(_originalCellValue))
            {
                Dictionary<string, object>.KeyCollection keys = _expressionMap.Keys;
                IEnumerator ie = keys.GetEnumerator();
                ie.MoveNext();
                
                return _expressionMap[(string)ie.Current];
            }
            else
            {
                return ReplaceAllBindVariable(_originalCellValue);
            }
        }

        private object ReplaceAllBindVariable(string cellValue)
        {
            Dictionary<string, object>.KeyCollection keys = _expressionMap.Keys;
            foreach (string key in keys)
            {
                // セル中の文字列を、値に置換している。
                // Java版は、Patter.quoteなどを利用しているが・・・？
                cellValue = cellValue.Replace(key, _expressionMap[key].ToString());
            }
            return cellValue;
        }

        private bool OnlySingleBindVarIn(string value)
        {
            if (_expressionMap.Count != 1)
            {
                return false;
            }
            foreach (string exp in _expressionMap.Keys)
            {
                value = value.Replace(exp, "");
            }
            return (value.Trim().Length == 0);
        }

        private object GetValue(IDictionary<string, object> data, BindVariable bindVar)
        {
            object value = null;
            try
            {
                value = OgnlUtil.GetValue(bindVar.Name, data);
            }
            catch (ApplicationException ex)
            {
                HandleGetValueRuntimeException(ex);
            }

            return (value == null) ? GetNullValue(bindVar) : value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bindVar"></param>
        /// <returns></returns>
        /// <exception cref="Seasar.Fisshplate.Exception.FPMergeException"></exception>
        private object GetNullValue(BindVariable bindVar)
        {
            if (bindVar.NullAllowed)
            {
                return bindVar.NullValue;
            }
            throw new FPMergeException(FPConsts.MessageIdBindVarUndefined,
                new object[] { bindVar.Name }, _targetElement.Cell.Row);
        }

        private void HandleGetValueRuntimeException(ApplicationException ex)
        {
            if (ex.InnerException == null || ex.InnerException.InnerException == null)
            {
                throw ex;
            }
 
            // JScript例外はスルー
            if (ex.InnerException.InnerException is JScriptException)
            {
                return;
            }
            throw ex;
        }

        public string OriginalCellValue
        {
            get { return _originalCellValue; }
        }

        public AbstractCell TargetElement
        {
            get { return _targetElement; }
        }
    }
}
