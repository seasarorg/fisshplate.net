using System;
using System.Collections.Generic;

using System.Text;
using Seasar.Fisshplate.Wrapper;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using Seasar.Fisshplate.Context;

namespace Seasar.Fisshplate.Core.Element
{
    /// <summary>
    /// セル要素の基底抽象クラスです。
    /// </summary>
    public abstract class AbstractCell : TemplateElement
    {
        /// <summary>
        /// テンプレート側のセル
        /// </summary>
        protected CellWrapper _cell;

        private bool _isMargedCell;
        private short _relativeMergedColumnTo;
        private int _relativeMergedRowNumTo;
        private object _cellValue;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="cell"></param>
        public AbstractCell(CellWrapper cell)
        {
            _cell = cell;
            HSSFSheet templateSheet = cell.Row.Sheet.HSSFSheet;
            int rowNum = cell.Row.HSSFRow.RowNum;

            // マージ情報をなめて、スタート地点が合致すれば保存しておく。
            for (int i = 0; i < templateSheet.NumMergedRegions; i++)
            {
                CellRangeAddress reg = templateSheet.GetMergedRegion(i);
                SetupMergedCellInfo(cell.HSSFCell.ColumnIndex, rowNum, reg);
                if (_isMargedCell)
                {
                    break;
                }
            }
            _cellValue = cell.ObjectValue;
        }

        #region TemplateElement メンバ

        public void Merge(FPContext context)
        {
            HSSFCell outCell = context.CurrentCell;
            CopyCellStyle(context, outCell);

            MergeImpl(context, outCell);

            context.NextCell();
        }

        /// <summary>
        /// このクラスを継承したクラスで実装される、データ埋め込み処理の実装です。
        /// </summary>
        /// <param name="context">コンテキスト</param>
        /// <param name="outCell">出力先のセル</param>
        /// <exception cref="Seasar.Fisshplate.Exception.FPMergeException">データ埋め込み時にエラーが発生した際に投げられる例外です。</exception>
        public abstract void MergeImpl(FPContext context, HSSFCell outCell);

        /// <summary>
        /// テンプレート側のセルのスタイルを出力側へコピーします。フォントやデータフォーマットも反映されます。
        /// </summary>
        /// <param name="context">コンテキスト</param>
        /// <param name="outCell">出力するセル</param>
        public void CopyCellStyle(FPContext context, HSSFCell outCell)
        {
            HSSFCell hssfCell = _cell.HSSFCell;
            HSSFCellStyle outStyle = hssfCell.CellStyle;
            outCell.CellStyle = outStyle;
            if (_isMargedCell)
            {
                MergeCell(context);
            }

        }

        private void MergeCell(FPContext context)
        {
            int columnFrom = context.CurrentCellNum;
            int rowFrom = context.CurrentRowNum;

            CellRangeAddress reg = new CellRangeAddress(
                rowFrom, rowFrom + _relativeMergedRowNumTo,
                columnFrom, columnFrom + _relativeMergedColumnTo);
            //HSSFSheet hssfSheet = _cell.Row.Sheet.HSSFSheet;
            //FISSHPLATE-52
            HSSFSheet hssfSheet = context.OutSheet;
            hssfSheet.AddMergedRegion(reg);
        }
        #endregion

        private void SetupMergedCellInfo(int cellNum, int rowNum, CellRangeAddress reg)
        {
            if (reg.FirstColumn != cellNum || reg.FirstRow != rowNum)
            {
                _isMargedCell = false;
                return;
            }
            _isMargedCell = true;
            _relativeMergedColumnTo = (short)(reg.LastColumn - reg.FirstColumn);
            _relativeMergedRowNumTo = reg.LastRow - reg.FirstRow;
        }

        public object CellValue
        {
            get { return _cellValue; }
            set { _cellValue = value; }
        }

        public CellWrapper Cell
        {
            get { return _cell; }
        }
    }
}
