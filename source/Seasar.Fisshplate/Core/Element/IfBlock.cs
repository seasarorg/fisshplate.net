using System;
using System.Collections.Generic;

using System.Text;
using Seasar.Fisshplate.Util;

namespace Seasar.Fisshplate.Core.Element
{
    public class IfBlock : AbstractBlock
    {
        private string _condition;

        private TemplateElement _nextBlock = new NullElement();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="condition">条件式</param>
        public IfBlock(string condition)
        {
            this._condition = condition;
        }

        /// <summary>
        /// 次の条件のブロック要素
        /// </summary>
        public TemplateElement NextBlock
        {
            get { return _nextBlock; }
        }

        /// <summary>
        /// 次の条件のブロック要素を設定します。
        /// 具体的には、ElseIfBlockかElseBlockになります。
        /// </summary>
        /// <param name="next">次の条件のブロックを保持する要素。</param>
        public void SetNextBlock(AbstractBlock next)
        {
            this._nextBlock = next;
        }

        public string Condition
        {
            get { return _condition; }
        }

        public override void Merge(Seasar.Fisshplate.Context.FPContext context)
        {
            IDictionary<string, object> data = context.Data;
            bool isTarget = (bool)(OgnlUtil.GetValue("(" + _condition + ")", data));
            if (isTarget)
            {
                MergeChildren(context);
            }
            else
            {
                _nextBlock.Merge(context);
            }
        }
    }
}
