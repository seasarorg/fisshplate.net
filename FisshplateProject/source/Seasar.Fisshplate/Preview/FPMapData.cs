using System;
using System.Collections.Generic;
using System.Text;
using Seasar.Fisshplate.Wrapper;
using Seasar.Fisshplate.Consts;
using System.Collections;

namespace Seasar.Fisshplate.Preview
{
    public class FPMapData
    {
        private String _keyName;

        protected SheetWrapper _sheet;
        protected IList<FPMapData> _childList = new List<FPMapData>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="sheet">シート</param>
        /// <param name="keyName">テンプレートから参照するキー文字列</param>
        internal FPMapData(SheetWrapper sheet, String keyName)
        {
            this._sheet = sheet;
            this._keyName = keyName;
        }

        /// <summary>
        /// このデータをテンプレートから参照するキー文字列
        /// </summary>
        public String KeyName
        {
            get { return _keyName; }
        }

        /// <summary>
        /// 子要素を追加します。
        /// </summary>
        /// <param name="sheet">子要素データが記載されたシート</param>
        /// <param name="keyName">子要素をテンプレートから参照するキー文字列</param>
        public void AddChild(SheetWrapper sheet, String keyName)
        {
            _childList.Add(new FPMapData(sheet, keyName));
        }

        public FPMapData GetChildByKey(String keyName)
        {
            foreach (var item in _childList)
            {
                if (item._keyName == keyName)
                {
                    return item;
                }
            }
            return null;
        }

        /// <summary>
        /// 埋め込み用データを生成します。
        /// 複数行の場合はDictionaryのListを戻します。
        /// ヘッダ行に「empty list」と書いてある場合は、0件のListを戻します。
        /// </summary>
        /// <returns>埋め込み用データ</returns>
        public object BuildData()
        {
            if (_sheet == null)
            {
                return BuildMapData();
            }
            else if (_sheet.RowCount <= 2)
            {
                String firstCell = _sheet.GetRow(0).GetCell(0).StringValue;
                if (FPConsts.PreviewEmptyListSign.Equals(firstCell))
                {
                    return new List<IDictionary>();
                }
                else
                {
                    return BuildMapData();
                }
            }
            else
            {
                return BuildListData();
            }
        }

        /// <summary>
        /// Dictionaryとして埋込みデータを生成します。
        /// </summary>
        /// <returns>埋込みデータ</returns>
        public IDictionary<string, object> BuildMapData()
        {
            IDictionary<string, object> data = new Dictionary<string, object>();
            if (_sheet != null)
            {
                RowWrapper keys = _sheet.GetRow(0);
                RowWrapper vals = _sheet.GetRow(1);
                PutValueToMap(data, keys, vals);
            }
            BuildChildData(data);

            return data;
        }

        /// <summary>
        /// DictionaryのListとして埋込みデータを生成します。
        /// </summary>
        /// <returns>埋め込みデータ</returns>
        protected IList<IDictionary<string, object>> BuildListData()
        {
            RowWrapper keys = _sheet.GetRow(0);
            IList<IDictionary<string, object>> list = new List<IDictionary<string, object>>();
            for (int i = 1; i < _sheet.RowCount; i++)
            {
                IDictionary<string, object> item = new Dictionary<string, object>();
                RowWrapper vals = _sheet.GetRow(i);
                BuildChildData(item);
                list.Add(item);
            }
            return list;
        }

        /// <summary>
        /// このデータが保持する子要素の埋め込みデータを生成します。
        /// </summary>
        /// <param name="data">子要素データを追加するDictionary</param>
        protected void BuildChildData(IDictionary<string, object> data)
        {
            foreach (var item in _childList)
            {
                object childData = item.BuildData();
                data[item.KeyName] = childData;
            }
        }

        /// <summary>
        /// 埋め込み用データが記載されたExcelシートからDictionaryにデータを追加します。
        /// </summary>
        /// <param name="data">埋め込みデータ用Dictionary</param>
        /// <param name="keys">キーとなる行</param>
        /// <param name="vals">値となる行</param>
        protected void PutValueToMap(IDictionary<string, object> data, RowWrapper keys, RowWrapper vals)
        {
            for (int i = 0; i < keys.CellCount; i++)
            {
                CellWrapper key = keys.GetCell(i);
                if (key.IsNullCell())
                {
                    continue;
                }
                CellWrapper val = vals.GetCell(i);

                String keyStr = key.StringValue;
                object valObj = val.ObjectValue;
                data[keyStr] = valObj;
            }
        }
    }
}
