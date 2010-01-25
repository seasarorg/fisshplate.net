using System;
using System.Collections.Generic;

using System.Text;
using NPOI.Util.Collections;
using System.Collections;

namespace Seasar.Fisshplate.Core.Element
{
    public class Resume : TemplateElement
    {
        private string _targetVar;

        public Resume(string targetVar)
        {
            _targetVar = targetVar;
        }

        #region TemplateElement メンバ

        public void Merge(Seasar.Fisshplate.Context.FPContext context)
        {
            HashSet susSet = context.SuspendedSet;

            for (IEnumerator itr = susSet.GetEnumerator(); itr.MoveNext(); )
            {
                Suspend sus = (Suspend)itr.Current;
                string targetstr = sus.El.OriginalCellValue;
                if (targetstr.Contains(_targetVar))
                {
                    sus.Resume(context);
                    susSet.Remove(sus);
                    break;
                }
            }

        }

        #endregion
    }
}
