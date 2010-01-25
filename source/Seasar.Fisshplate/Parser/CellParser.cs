using System;
using System.Collections.Generic;

using System.Text;
using Seasar.Fisshplate.Core.Element;
using Seasar.Fisshplate.Wrapper;

namespace Seasar.Fisshplate.Parser
{
    /// <summary>
    /// セル単位のタグから各要素を解析するインタフェースです。
    /// </summary>
    public interface CellParser
    {
        /// <summary>
        /// セルの内容を解析し、このパーサに合致した場合はそれに紐つくTemplateElementを実装したクラスを戻します。
        /// 合致しない場合はnullを戻します。
        /// </summary>
        /// <param name="cell">解析対象セル</param>
        /// <param name="value">解析対象セルの値</param>
        /// <returns>紐ついた要素クラス</returns>
        AbstractCell GetElement(CellWrapper cell, string value);
    }
}
