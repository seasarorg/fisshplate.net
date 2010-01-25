using System;
using System.Collections.Generic;

using System.Text;
using Seasar.Fisshplate.Context;
using NPOI.HSSF.UserModel;
using Seasar.Fisshplate.Consts;

namespace Seasar.Fisshplate.Core.Element
{
    public class PageBreak : TemplateElement
    {
        private Root _root;

        public PageBreak(Root root)
        {
            this._root = root;
        }
        #region TemplateElement メンバ

        public void Merge(Seasar.Fisshplate.Context.FPContext context)
        {
            WriteFooter(context);

            DoPageBreak(context);
            PageCountUp(context);

            context.ShouldHeaderOut = true;
            context.ShouldFooterOut = false;
            if (context.InIteratorBlock())
            {
                IteratorBlock currentIterator = context.CurrentIterator;
                currentIterator.InitLineNumPerPage();
            }
        }


        #endregion

        private void WriteFooter(Seasar.Fisshplate.Context.FPContext context)
        {
            TemplateElement footer = _root.PageFooter;
            footer.Merge(context);
        }

        private void DoPageBreak(FPContext context)
        {
            HSSFSheet sheet = context.OutSheet;
            int currentRowNum = context.CurrentRowNum;
            sheet.SetRowBreak(currentRowNum - 1);
        }

        private void PageCountUp(FPContext context)
        {
            ((PageContext)(context.Data[FPConsts.PageContextName])).AddPageNum();
        }

    }
}
