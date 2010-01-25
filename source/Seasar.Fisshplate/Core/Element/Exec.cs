using System;
using System.Collections.Generic;

using System.Text;
using Seasar.Fisshplate.Util;

namespace Seasar.Fisshplate.Core.Element
{
    public class Exec : TemplateElement
    {
        private string _expression;

        public Exec(string expression)
        {
            this._expression = expression;
        }

        #region TemplateElement メンバ

        public void Merge(Seasar.Fisshplate.Context.FPContext context)
        {
            IDictionary<string, object> data = context.Data;
            OgnlUtil.GetValue(_expression, data);
        }

        #endregion
    }
}
