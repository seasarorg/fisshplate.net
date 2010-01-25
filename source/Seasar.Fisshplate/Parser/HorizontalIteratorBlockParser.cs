using System;
using System.Collections.Generic;

using System.Text;
using System.Text.RegularExpressions;
using Seasar.Fisshplate.Core.Element;

namespace Seasar.Fisshplate.Parser
{
    public class HorizontalIteratorBlockParser : RowParser
    {
        private static readonly Regex _patHor = new Regex(@"^\s*#hforeach\s+(\S+)\s*:\s*(\S+)(\s+index\s*=\s*(\S+))*\s*$");

        #region RowParser メンバ

        public bool Process(Seasar.Fisshplate.Wrapper.CellWrapper cell, FPParser parser)
        {
            string value = cell.StringValue;
            Match mat = _patHor.Match(value);
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
            string varName = mat.Groups[1].Value;
            string iteratorName = mat.Groups[2].Value;
            string indexName = mat.Groups[4].Value;

            return new HorizontalIteratorBlock(varName, iteratorName, indexName, cell);
        }


    }
}
