using System;
using System.Collections.Generic;

using System.Text;
using Seasar.Fisshplate.Context;
using NPOI.HSSF.UserModel;

namespace Seasar.Fisshplate.Core.Element
{
    public class Root : TemplateElement
    {
        private TemplateElement _pageHeaderBlock = new NullElement();
        private IList<TemplateElement> _bodyElementList = new List<TemplateElement>();
        private TemplateElement _pageFooterBlock = new NullElement();

        #region TemplateElement メンバ

        public void Merge(FPContext context)
        {
            context.ShouldHeaderOut = false;
            _pageHeaderBlock.Merge(context);
            MergeBodyElement(context);
            if (context.ShouldFooterOut)
            {
                _pageFooterBlock.Merge(context);
            }
            RemoveUnwantedRows(context);
        }

        #endregion

        private void MergeBodyElement(FPContext context)
        {
            foreach (var elem in _bodyElementList)
            {
                elem.Merge(context); 
            }
        }
        
        private void RemoveUnwantedRows(FPContext context)
        {
            //ゴミ掃除
            HSSFSheet outSheet = context.OutSheet;
            int currentRowNum = context.CurrentRowNum;
            int lasRowNum = outSheet.LastRowNum;

            for (int i = currentRowNum; i <= lasRowNum; i++)
            {
                outSheet.RemoveRow(outSheet.GetRow(i));
            }
        }

        /// <summary>
        /// ページヘッダの要素
        /// </summary>
        public TemplateElement PageHeader
        {
            get { return _pageHeaderBlock; }
            set { _pageHeaderBlock = value; }
        }

        /// <summary>
        /// ページフッタの要素
        /// </summary>
        public TemplateElement PageFooter
        {
            get { return _pageFooterBlock; }
            set { _pageFooterBlock = value; }
        }

        /// <summary>
        /// ボディの要素を追加します。
        /// </summary>
        /// <param name="element"></param>
        public void AddBody(TemplateElement element)
        {
            _bodyElementList.Add(element);
        }

        /// <summary>
        /// ボディ要素のリストを戻します
        /// </summary>
        public IList<TemplateElement> BodyElementList
        {
            get { return _bodyElementList; }
        }
    }
}
