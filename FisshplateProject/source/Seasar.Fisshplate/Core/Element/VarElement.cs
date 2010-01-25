using System;
using System.Collections.Generic;

using System.Text;
using Seasar.Fisshplate.Wrapper;
using System.Text.RegularExpressions;
using Seasar.Fisshplate.Consts;
using Seasar.Fisshplate.Util;
using Seasar.Fisshplate.Exception;

namespace Seasar.Fisshplate.Core.Element
{
    public class VarElement : TemplateElement
    {
        private string[] _expressions;
        private RowWrapper _row;
        private Regex _patDeclr = new Regex(@"([^=\s]+)\s*(=\s*[^=\s]+)?");


        public VarElement(string expression, RowWrapper row)
        {
            Regex regex = new Regex(@"\s*,\s*");
            this._expressions = regex.Split(expression);
            this._row = row;
        }

        #region TemplateElement メンバ

        public void Merge(Seasar.Fisshplate.Context.FPContext context)
        {
            IDictionary<string, object> data = context.Data;
            foreach (string expression in _expressions)
            {
                EvalExpression(data, expression);
            }
        }

        #endregion

        private void EvalExpression(IDictionary<string, object> data, string expression)
        {
            Match mat = _patDeclr.Match(expression);
            if (mat.Success == false)
            {
                ThrowMergeException(FPConsts.MessageIdVarDeclarationInvalid, expression, _row);
            }
            string varName = mat.Groups[1].Value;
            AssignVariable(data, varName);

            if (String.IsNullOrEmpty(mat.Groups[2].Value) == false)
            {
                InitializeVariable(data, expression);
            }
        }

        private void InitializeVariable(IDictionary<string, object> data, string expression)
        {
            try
            {
                OgnlUtil.GetValue(expression, data);
            }
            catch (ApplicationException e)
            {
                ThrowMergeException(FPConsts.MessageIdVarDeclarationInvalid, expression, _row);
            }
        }

        private void AssignVariable(IDictionary<string, object> data, string varName)
        {
            if (data.ContainsKey(varName))
            {
                ThrowMergeException(FPConsts.MessageIdAlreadyExists, varName, _row);
            }
            data[varName] = "";
        }

        private void ThrowMergeException(string messageId, string value, RowWrapper _row)
        {
            throw new FPMergeException(messageId, new Object[] { value }, _row);
        }
    }
}
