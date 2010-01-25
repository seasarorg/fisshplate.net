using System;
using System.Collections.Generic;

using System.Text;
using Seasar.Fisshplate.Wrapper;
using System.Collections;
using Seasar.Fisshplate.Exception;
using Seasar.Fisshplate.Consts;

namespace Seasar.Fisshplate.Util
{
    /// <summary>
    /// 繰り返し処理を行うクラスに共通するユーティリティクラスです。
    /// </summary>
    public static class EnumeratorUtil
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o">配列もしくはリスト</param>
        /// <param name="iteratorName">イテレータの名前（エラー表示用）</param>
        /// <param name="row">行ラッパクラス（エラー表示用）</param>
        /// <returns></returns>
        /// <exception cref="Seasar.Fisshplate.Exception.FPMergeException">第1引数がEnumeratorを持たないクラスだった場合に投げられます。</exception>
        public static IEnumerator GetEnumerator(object o, string iteratorName, RowWrapper row)
        {
            if (o is IEnumerable)
            {
                return ((IEnumerable)o).GetEnumerator();
            }
            throw new FPMergeException(FPConsts.MessageIdNotIteratable, new object[] { iteratorName }, row);
        }
    }
}
