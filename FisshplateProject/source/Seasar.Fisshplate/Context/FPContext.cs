using System;
using System.Collections.Generic;

using System.Text;
using NPOI.HSSF.UserModel;
using Seasar.Fisshplate.Core.Element;
using NPOI.Util.Collections;

namespace Seasar.Fisshplate.Context
{
    public class FPContext
    {
        private HSSFSheet _outSheet;
        private int _currentRowNum;
        private int _currentCellNum;
        private Dictionary<string, object> _data;
        private bool _shouldHeaderOut;
        private bool _shouldFooterOut;
        private bool _skipMerge = false;

        private HSSFPatriarch _patriarch;

        private IteratorBlock _currentIterator;

        private HashSet _suspendedSet = new HashSet();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="outSheet">出力するシート</param>
        /// <param name="data">埋め込むデータ</param>
        public FPContext(HSSFSheet outSheet, Dictionary<string, object> data)
        {
            this._outSheet = outSheet;
            this._data = data;
            Init();
        }

        /// <summary>
        /// 埋め込むデータを戻します。
        /// </summary>
        public IDictionary<string, object> Data
        {
            get { return _data; }
        }

        /// <summary>
        /// 現在の出力対象行を戻します。
        /// まだ無ければ生成します。
        /// </summary>
        public HSSFRow CurrentRow
        {
            get
            {
                HSSFRow row = _outSheet.GetRow(_currentRowNum);
                if (row == null)
                {
                    row = _outSheet.CreateRow(_currentRowNum);
                }
                return row;
            }
        }

        /// <summary>
        /// 現在の出力対象行を新たに生成します。
        /// </summary>
        /// <returns></returns>
        public HSSFRow CreateCurrentRow()
        {
            return _outSheet.CreateRow(_currentRowNum);
        }

        /// <summary>
        /// 現在の出力対象行を任意の場所に移動し、その行を戻します。
        /// </summary>
        /// <param name="rowNum">移動先の行番号</param>
        /// <returns>出力対象行</returns>
        public HSSFRow MoveCurrentRowTo(int rowNum)
        {
            _currentRowNum = rowNum;
            return CurrentRow;
        }

        /// <summary>
        /// 現在の出力対象セルを戻します
        /// </summary>
        public HSSFCell CurrentCell
        {
            get
            {
                HSSFRow row = CurrentRow;
                HSSFCell cell = row.GetCell(_currentCellNum);
                if (cell == null)
                {
                    cell = row.CreateCell(_currentCellNum);
                }
                return cell;
            }
        }

        /// <summary>
        /// 現在の出力対象セルを任意の場所に移動し、そのセルを戻します。
        /// </summary>
        /// <param name="cellNum">移動先のセル番号</param>
        /// <returns>出力対象セル</returns>
        public HSSFCell MoveCurrentCellTo(int cellNum)
        {
            _currentCellNum = cellNum;
            return CurrentCell;
        }

        /// <summary>
        /// 現在の行の位置を戻します。
        /// </summary>
        public int CurrentRowNum
        {
            get
            {
                return _currentRowNum;
            }
        }

        /// <summary>
        /// 現在のセルの位置を戻します。
        /// </summary>
        public int CurrentCellNum
        {
            get
            {
                return _currentCellNum;
            }
        }

        /// <summary>
        /// 出力するシートを戻します。
        /// </summary>
        /// <returns>シート</returns>
        public HSSFSheet OutSheet
        {
            get
            {
                return _outSheet;
            }
        }

        public bool ShouldHeaderOut
        {
            get
            {
                return _shouldHeaderOut;
            }
            set
            {
                _shouldHeaderOut = value;
            }
        }

        public bool ShouldFooterOut
        {
            get
            {
                return _shouldFooterOut;
            }
            set
            {
                _shouldFooterOut = value;
            }
        }
        
        public bool SkipMerge
        {
            get { return _skipMerge; }
            set { _skipMerge = value; }
        }

        public HSSFPatriarch Patriarch
        {
            get 
            {
                if (_patriarch == null)
                {
                    _patriarch = _outSheet.CreateDrawingPatriarch();
                }
                return _patriarch;
            }
        }

        /// <summary>
        /// 現在のIteratorBlockを示します。
        /// </summary>
        /// <see cref="IteratorBlock"/>
        public IteratorBlock CurrentIterator
        {
            get { return _currentIterator; }
            set { _currentIterator = value; }
        }

        /// <summary>
        /// 現在のIteratorBlockを消去します。
        /// </summary>
        public void ClearCurrentIterator()
        {
            this._currentIterator = null;
        }

        /// <summary>
        /// 現在、ItaratorBlockの中にいるか否かを戻します。
        /// </summary>
        /// <returns>ItaratorBlockの中に居ればtrue</returns>
        public bool InIteratorBlock()
        {
            return _currentIterator != null;
        }

        /// <summary>
        /// 評価を保留するセルを保留リストに追加します。
        /// </summary>
        /// <param name="suspend">評価を保留するセル</param>
        public void AddSuspendedSet(Suspend suspend)
        {
            _suspendedSet.Add(suspend);
        }

        /// <summary>
        /// 保留リストを戻します。
        /// </summary>
        public HashSet SuspendedSet
        {
            get
            {
                return _suspendedSet;
            }
        }

        /// 現在の出力対象位置を初期化します。
        /// </summary>
        public void Init()
        {
            _currentCellNum = 0;
            _currentRowNum = 0;
        }

        /// <summary>
        /// 出力対象行を次の行に進めます。
        /// </summary>
        public void NextRow()
        {
            _currentCellNum = 0;
            _currentRowNum++;
        }

        /// <summary>
        /// 出力対象セルを次のセルに進めます。
        /// </summary>
        public void NextCell()
        {
            _currentCellNum++;
        }

    }
}
