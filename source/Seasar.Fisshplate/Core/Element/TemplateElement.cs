using System;
using System.Collections.Generic;

using System.Text;
using Seasar.Fisshplate.Context;

namespace Seasar.Fisshplate.Core.Element
{
    /// <summary>
    /// テンプレートの各要素を反映するインターフェースです。
    /// </summary>
    public interface TemplateElement
    {
        /// <summary>
        /// コンテキストに格納されたデータをテンプレートに埋め込みます。
        /// </summary>
        /// <param name="context">コンテキスト</param>
        /// <exception cref="Seasar.Fisshplate.Exception.FPMergeException">データ埋め込み時にエラーが発生した際に投げられます。</exception>
        void Merge(FPContext context);
    }
}
