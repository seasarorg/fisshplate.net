using System;
using System.Collections.Generic;

using System.Text;
using Seasar.Fisshplate.Core.Element;
using Seasar.Fisshplate.Wrapper;
using System.Text.RegularExpressions;
using Seasar.Fisshplate.Consts;
using NPOI.HSSF.UserModel;

namespace Seasar.Fisshplate.Parser.Handler
{
    public class CellParserHandler
    {
        private static readonly Regex _patEl = new Regex(FPConsts.RegexBindVar);
        private static readonly Regex _patSuspend = new Regex(@"\s*#suspend\s+(.*" + FPConsts.RegexBindVar + @".*)");

        private static CellParser[] _buildInCellParser = 
        {
            new PictureParser(),
            new LinkParser()
        };

        public TemplateElement GetElement(CellWrapper templateCell)
        {
            if (templateCell.HSSFCell == null)
            {
                return new NullCell();
            }
            string value = GetCellValue(templateCell);
            if (value == null)
            {
                return new GenericCell(templateCell);
            }
            AbstractCell cellElem = GetElementByParsers(templateCell, value);

            Match mat = _patEl.Match(value);
            if (mat.Success)
            {
                return CreateEl(cellElem, value);
            }
            return cellElem;
        }

        private TemplateElement CreateEl(AbstractCell cellElem, string value)
        {
            Match mat = _patSuspend.Match(value);
            if (mat.Success)
            {
                cellElem.CellValue = mat.Groups[1].Value;
                El el = new El(cellElem);
                return new Suspend(el);
            }
            else
            {
                return new El(cellElem);
            }
        }

        private AbstractCell GetElementByParsers(CellWrapper templateCell, string value)
        {
            AbstractCell cellElem = null;
            foreach (CellParser parser in _buildInCellParser)
            {
                cellElem = parser.GetElement(templateCell, value);
                if (cellElem != null)
                {
                    break;
                }
            }
            if (cellElem == null)
            {
                cellElem = new GenericCell(templateCell);
            }
            return cellElem;
        }

        private string GetCellValue(CellWrapper templateCell)
        {
            HSSFCell hssfCell = templateCell.HSSFCell;
            string value = null;
            if (hssfCell.CellType == HSSFCell.CELL_TYPE_STRING)
            {
                value = hssfCell.RichStringCellValue.String;
            }
            else if (hssfCell.CellType == HSSFCell.CELL_TYPE_FORMULA)
            {
                value = hssfCell.CellFormula;
            }
            return value;
        }
    }
}
