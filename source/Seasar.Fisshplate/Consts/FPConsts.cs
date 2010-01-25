using System;
using System.Collections.Generic;

using System.Text;

namespace Seasar.Fisshplate.Consts
{
    /// <summary>
    /// 定数を保持するインタフェースです。
    /// </summary>
    public static class FPConsts
    {
        /// <summary>
        /// イテレータ要素内で参照する現在行のインデックスの変数名です。「index」になります。
        /// </summary>
        public const string DefaultIteratorIndexName = "index";

        /// <summary>
        /// 出力されるシート内の行番号を表す変数名です。「rownum」になります。
        /// </summary>
        public const string RowNumberName = "rownum";

        /// <summary>
        /// 出力されるシート内のページ番号を表す変数名です。「pagenum」になります。
        /// </summary>
        public const string PageNumberName = "pagenum";

        /// <summary>
        /// 出力されるページのコンテキスト名です「pageになります」。
        /// </summary>
        public const string PageContextName = "page";

        /// <summary>
        /// 埋込みデータのNULL制御演算子
        /// </summary>
        public const string NullValueOperator = "!";

        public const string MessageIdEndElement = "EFP00001";

        public const string MessageIdLackIf = "EFP00002";

        public const string MessageIdNotIteratable = "EFP00003";

        public const string MessageIdBindVarUndefined = "EFP00004";

        public const string MessagePictureTypeInvalid = "EFP00007";

        public const string MessageIdNotIteratorInvalidMax = "EFP00008";

        public const string MessageIdAlreadyExists = "EFP00010";

        public const string MessageIdVarDeclarationInvalid = "EFP00011";

        public const string MessageIdWhileInvalidCondition = "EFP00012";

        public const string MessageIdLinkMergeError = "EFP00013";

        public const string MessageIdPictureMergeError = "EFP00014";

        public const string RegexBindVarStart = @"\$\{";
        public const string RegexBindVarEnd = @"\}";
        public const string RegexBindVar = RegexBindVarStart + "[^" + RegexBindVarStart + RegexBindVarEnd + "]" + "+" + RegexBindVarEnd;

        public const string PreviewEmptyListSign = "empty list";

        public const string RegexLink = @"^\s*#link-(\S+)\s+link\s*=\s*(.+)\s+text\s*=\s*(.+)$";
    }
}
