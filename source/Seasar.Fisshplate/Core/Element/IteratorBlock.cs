using System;
using System.Collections.Generic;

using System.Text;
using Seasar.Fisshplate.Wrapper;
using Seasar.Fisshplate.Consts;
using Seasar.Fisshplate.Context;
using Seasar.Fisshplate.Util;
using System.Collections;

namespace Seasar.Fisshplate.Core.Element
{
    public class IteratorBlock : AbstractBlock
    {
        private string _varName;
        private string _iteratorName;
        private string _indexName;
        private int _max;
        private RowWrapper _row;
        private int _lineNumPerPage;

        /// <summary>
        /// 要素を保持する変数名とイテレータ自身の名前とループのインデックス名とループの最大繰り返し回数を受け取ります。
        /// </summary>
        /// <param name="row">テンプレート上の行</param>
        /// <param name="varName">イテレータ内の要素を保持する変数名</param>
        /// <param name="iteratorName">イテレータ名</param>
        /// <param name="indexName">ループのインデックス名</param>
        /// <param name="max">ループの最大繰り返し回数</param>
        public IteratorBlock(RowWrapper row, string varName, string iteratorName, string indexName, int max)
        {
            this._varName = varName;
            this._iteratorName = iteratorName;
            this._max = max;
            this._row = row;

            if (String.IsNullOrEmpty(indexName))
            {
                this._indexName = FPConsts.DefaultIteratorIndexName;
            }
            else
            {
                this._indexName = indexName;
            }
        }

        public override void Merge(FPContext context)
        {
            IDictionary<string, object> data = context.Data;
            object o = OgnlUtil.GetValue(_iteratorName, data);
            IEnumerator ite = EnumeratorUtil.GetEnumerator(o, _iteratorName, _row);
            MergeIteratively(context, ite, data);
        }

        private void MergeIteratively(FPContext context, IEnumerator ite, IDictionary<string, object> data)
        {
            context.CurrentIterator = this;
            InitLineNumPerPage();
            int index = 0;
            while (ite.MoveNext())
            {
                object value = ite.Current;
                data[_varName] = value;
                data[_indexName] = index;
                _lineNumPerPage++;
                index++;

                MergeChildren(context);
            }
            context.SkipMerge = true;
            context.ClearCurrentIterator();
            while (_max > _lineNumPerPage)
            {
                data[_indexName] = index;
                _lineNumPerPage++;
                index++;
                MergeChildren(context);
            }
            context.SkipMerge = false;
        }

        /// <summary>
        /// 現在のページごとの行番号
        /// </summary>
        public int LineNumPerPage
        {
            get { return _lineNumPerPage; }
        }

        /// <summary>
        /// 現在のページごとの行番号を初期化します。
        /// </summary>
        public void InitLineNumPerPage()
        {
            _lineNumPerPage = 0;
        }
    }
}
