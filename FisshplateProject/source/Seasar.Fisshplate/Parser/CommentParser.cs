using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Seasar.Fisshplate.Parser
{
    public class CommentParser : RowParser
    {
        private static readonly Regex _patComment = new Regex(@"^\s*#comment.*");

        #region RowParser メンバ

        public bool Process(Seasar.Fisshplate.Wrapper.CellWrapper cell, FPParser parser)
        {
            string value = cell.StringValue;
            Match mat = _patComment.Match(value);

            return mat.Success;
        }

        #endregion
    }
}
