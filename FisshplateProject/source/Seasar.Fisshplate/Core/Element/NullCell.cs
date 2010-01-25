using System;
using System.Collections.Generic;
using System.Text;

namespace Seasar.Fisshplate.Core.Element
{
    public class NullCell : TemplateElement
    {
        public NullCell() { }

        #region TemplateElement メンバ

        public void Merge(Seasar.Fisshplate.Context.FPContext context)
        {
            context.NextCell();
        }

        #endregion
    }
}
