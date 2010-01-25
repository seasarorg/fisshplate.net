using System;
using System.Collections.Generic;

using System.Text;
using System.Text.RegularExpressions;
using Seasar.Fisshplate.Core.Element;
using Seasar.Fisshplate.Exception;
using Seasar.Fisshplate.Consts;
using Seasar.Fisshplate.Wrapper;

namespace Seasar.Fisshplate.Parser
{
    public abstract class AbstractElseParser : RowParser
    {
        #region RowParser メンバ

        public bool Process(Seasar.Fisshplate.Wrapper.CellWrapper cell, FPParser parser)
        {
            string value = cell.StringValue;
            Match mat = GetPattern().Match(value);
            if (mat.Success == false)
            {
                return false;
            }
            CheckBlockStack(parser, cell);
            ProcessElse(GetCondition(mat), cell, parser);
            return true;
        }

        private void ProcessElse(string condition, Seasar.Fisshplate.Wrapper.CellWrapper cell, FPParser parser)
        {
            RowWrapper row = cell.Row;
            AbstractBlock parent = GetParentIfBlock(row, parser);
            AbstractBlock block = CreateElement(condition);
            ((IfBlock)parent).SetNextBlock(block);
            parser.PushBlockToStack(block);
        }

        private AbstractBlock GetParentIfBlock(RowWrapper row, FPParser parser)
        {
            AbstractBlock parent = parser.LastElementFromStack;
            if ((parent is IfBlock) == false)
            {
                throw new FPParseException(FPConsts.MessageIdLackIf, row);
            }
            return parent;
        }

        private string GetCondition(Match mat)
        {
            string condition = null;
            if (mat.Groups.Count > 0)
            {
                condition = mat.Groups[1].Value;
            }
            return condition;
        }

        private void CheckBlockStack(FPParser parser, Seasar.Fisshplate.Wrapper.CellWrapper cell)
        {
            if (parser.IsBlockStackBlank())
            {
                throw new FPParseException(FPConsts.MessageIdLackIf, cell.Row);
            }
        }

        #endregion

        protected abstract AbstractBlock CreateElement(string condition);

        /// <summary>
        /// 解析対象か否かの判定に使うRegexを戻します。
        /// </summary>
        /// <returns></returns>
        protected abstract Regex GetPattern();
    }
}
