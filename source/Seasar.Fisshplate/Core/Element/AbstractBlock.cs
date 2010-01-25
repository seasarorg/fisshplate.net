using System;
using System.Collections.Generic;

using System.Text;
using Seasar.Fisshplate.Context;

namespace Seasar.Fisshplate.Core.Element
{
    public abstract class AbstractBlock : TemplateElement
    {
        protected IList<TemplateElement> _childList = new List<TemplateElement>();

        /// <summary>
        /// ブロック内の子要素を追加します。
        /// </summary>
        /// <param name="element"></param>
        public void AddChild(TemplateElement element)
        {
            _childList.Add(element);
        }

        /// <summary>
        /// ブロック内の子要素のリスト
        /// </summary>
        public IList<TemplateElement> ChildList
        {
            get { return _childList; }
        }

        /// <summary>
        /// 子要素にデータを埋め込みます
        /// </summary>
        /// <param name="context">コンテキスト</param>
        /// <exception cref="Seasar.Fisshplate.Exception.FPMergeException">データ埋め込み時にエラーが発生した際に投げられます。</exception>
        protected void MergeChildren(FPContext context)
        {
            foreach (TemplateElement elem in _childList)
            {
                elem.Merge(context);
            }
        }

        #region TemplateElement メンバ

        public abstract void Merge(FPContext context);

        #endregion
    }
}
