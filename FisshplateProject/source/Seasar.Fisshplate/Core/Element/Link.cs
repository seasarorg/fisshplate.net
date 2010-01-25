using System;
using System.Collections.Generic;

using System.Text;
using System.Text.RegularExpressions;
using Seasar.Fisshplate.Consts;
using Seasar.Fisshplate.Wrapper;
using Seasar.Fisshplate.Exception;
using NPOI.HSSF.UserModel;
using Seasar.Fisshplate.Enum;

namespace Seasar.Fisshplate.Core.Element
{
    public class Link : AbstractCell
    {
        private static readonly Regex _patLink = new Regex(FPConsts.RegexLink);

        public Link(CellWrapper cell)
            :base(cell)
        {
        }

        public override void MergeImpl(Seasar.Fisshplate.Context.FPContext context, NPOI.HSSF.UserModel.HSSFCell outCell)
        {
            string cellValue = CellValue.ToString();
            Match mat = _patLink.Match(cellValue);
            if (mat.Success == false)
            {
                throw new FPMergeException(FPConsts.MessageIdLinkMergeError,
                    new object[]{cellValue}, _cell.Row);
            }
            string type = mat.Groups[1].Value;
            string link = mat.Groups[2].Value;
            string text = mat.Groups[3].Value;

            LinkElementType linkType = LinkElementType.Get(type);
            HSSFHyperlink hyperLink = linkType.CreateHyperlink(type);
            hyperLink.Address = link;
            outCell.Hyperlink = hyperLink;
            outCell.SetCellValue(new HSSFRichTextString(text));

        }
    }
}
