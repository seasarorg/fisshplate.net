using System;
using System.Collections.Generic;

using System.Text;
using NPOI.HSSF.UserModel;
using Seasar.Fisshplate.Context;

namespace Seasar.Fisshplate.Core.Element
{
    /// <summary>
    /// テンプレートのセルの値にバインド変数が含まれる場合の要素クラスです。
    /// JScriptで評価します。
    /// 評価は即時にはされず、Resumeクラスによって評価されます。
    /// </summary>
    public class Suspend : TemplateElement
    {
        private El _el;

        public El El
        {
            get { return _el; }
        }

        private Stack<HSSFCell> _targetCellStack = new Stack<HSSFCell>();

        public Suspend(El el)
        {
            this._el = el;
        }

        #region TemplateElement メンバ

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="Seasar.Fisshplate.Exception.FPMergeException"></exception>
        public void Merge(FPContext context)
        {
            context.AddSuspendedSet(this);
            HSSFCell outCell = context.CurrentCell;
            _targetCellStack.Push(outCell);
            _el.TargetElement.CopyCellStyle(context, outCell);
            context.NextCell();
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="Seasar.Fisshplate.Exception.FPMergeException"></exception>
        public void Resume(FPContext context)
        {
            object value = _el.GetBoundValue(context);
            _el.TargetElement.CellValue = value;
            while (_targetCellStack.Count != 0)
            {
                HSSFCell outCell = _targetCellStack.Pop();
                _el.TargetElement.MergeImpl(context, outCell);
            }
        }
    }
}
