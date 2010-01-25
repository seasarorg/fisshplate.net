using System;
using System.Collections.Generic;

using System.Text;
using Seasar.Fisshplate.Wrapper;
using Seasar.Fisshplate.Util;
using Seasar.Fisshplate.Exception;
using Seasar.Fisshplate.Consts;

namespace Seasar.Fisshplate.Core.Element
{
    public class WhileBlock : AbstractBlock
    {
        private string _condition;
        private RowWrapper _row;

        public WhileBlock(RowWrapper row, string condition)
        {
            _row = row;
            _condition = condition;
        }

        public override void Merge(Seasar.Fisshplate.Context.FPContext context)
        {
            while (IsConditionTrue(context))
            {
                MergeChildren(context);
            }
        }

        private bool IsConditionTrue(Seasar.Fisshplate.Context.FPContext context)
        {
            IDictionary<string, object> data = context.Data;
            try
            {
                return (bool)(OgnlUtil.GetValue("(" + _condition + ")", data));
            }
            catch (ApplicationException e)
            {
                throw new FPMergeException(FPConsts.MessageIdWhileInvalidCondition, new object[] { _condition }, _row);
            }
            throw new NotImplementedException();
        }
    }
}
