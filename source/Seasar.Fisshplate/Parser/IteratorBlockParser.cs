using System;
using System.Collections.Generic;

using System.Text;
using System.Text.RegularExpressions;
using Seasar.Fisshplate.Core.Element;
using Seasar.Fisshplate.Wrapper;
using Seasar.Fisshplate.Exception;
using Seasar.Fisshplate.Consts;

namespace Seasar.Fisshplate.Parser
{
    public class IteratorBlockParser : RowParser
    {
        private static readonly Regex _patIterator = new Regex(@"^\s*#foreach\s+(\S+)\s*:\s*(\S+)(\s+index\s*=\s*(\S+))*(\s+max\s*=\s*(\S+))*\s*$");

        #region RowParser メンバ

        public bool Process(Seasar.Fisshplate.Wrapper.CellWrapper cell, FPParser parser)
        {
            string value = cell.StringValue;
            Match mat = _patIterator.Match(value);
            if (mat.Success == false)
            {
                return false;
            }
            AbstractBlock block = BuildElement(cell, mat);

            parser.AddBlockElement(block);
            return true;
        }
        #endregion

        private AbstractBlock BuildElement(Seasar.Fisshplate.Wrapper.CellWrapper cell, Match mat)
        {
            RowWrapper row = cell.Row;
            string varName = mat.Groups[1].Value;
            string iteratorName = mat.Groups[2].Value;
            string indexName = mat.Groups[4].Value;
            string maxString = mat.Groups[6].Value;
            int max = 0;

            if (String.IsNullOrEmpty(maxString) == false)
            {
                if (int.TryParse(maxString, out max) == false)
                {
                    throw new FPParseException(FPConsts.MessageIdNotIteratorInvalidMax, row);
                }
            }
            return new IteratorBlock(row, varName, iteratorName, indexName, max);
        }

    }
}
