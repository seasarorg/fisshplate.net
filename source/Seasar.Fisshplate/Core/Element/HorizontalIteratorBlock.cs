using System;
using System.Collections.Generic;

using System.Text;
using Seasar.Fisshplate.Wrapper;
using Seasar.Fisshplate.Consts;
using Seasar.Fisshplate.Util;
using System.Collections;
using NPOI.HSSF.UserModel;

namespace Seasar.Fisshplate.Core.Element
{
    public class HorizontalIteratorBlock : AbstractBlock
    {
        private string _varName;
        private string _iteratorName;
        private string _indexName;
        private int _startCellIndex;
        private RowWrapper _row;

        public HorizontalIteratorBlock(string varName, string iteratorName, string indexName, CellWrapper cell)
        {
            this._varName = varName;
            this._iteratorName = iteratorName;
            this._startCellIndex = cell.CellIndex;
            this._row = cell.Row;
            if (indexName == null || indexName.Trim() == string.Empty)
            {
                this._indexName = FPConsts.DefaultIteratorIndexName;
            }
            else
            {
                this._indexName = indexName;
            }
        }

        public override void Merge(Seasar.Fisshplate.Context.FPContext context)
        {
            IDictionary<string, object> data = context.Data;
            object o = OgnlUtil.GetValue(_iteratorName, data);
            IEnumerator ite = EnumeratorUtil.GetEnumerator(o, _iteratorName, _row);
            MergeIteratively(context, ite, data);
        }

        private void MergeIteratively(Seasar.Fisshplate.Context.FPContext context, IEnumerator ite, IDictionary<string, object> data)
        {
            int index = 0;
            int startRowNum = context.CurrentRowNum;
            int startCell = _startCellIndex;
            int maxCellNum = GetMaxCellElementSize() - _startCellIndex;

            MergeNoIterationBlock(context);

            while (ite.MoveNext())
            {
                object value = ite.Current;
                data[_varName] = value;
                data[_indexName] = index;
                index++;
                context.MoveCurrentRowTo(startRowNum);
                MergeBlock(context, startCell);
                startCell += maxCellNum;
            }
        }

        private void MergeBlock(Seasar.Fisshplate.Context.FPContext context, int startCell)
        {
            foreach (TemplateElement elem in _childList)
            {
                if (elem is Row)
                {
                    context.MoveCurrentCellTo(startCell);
                    MergeRow(context, (Row)elem);
                }
                else
                {
                    elem.Merge(context);
                }
            }
        }

        private void MergeRow(Seasar.Fisshplate.Context.FPContext context, Row row)
        {
            HSSFRow outRow = context.CurrentRow;
            outRow.Height = row.RowHeight;
            IDictionary<string, object> data = context.Data;
            data[FPConsts.RowNumberName] = context.CurrentRowNum + 1;
            for (int i = 0; i < row.CellElementList.Count; i++)
            {
                if (i < _startCellIndex)
                {
                    continue;
                }
                AdjustColumnWidth(context, (short)i);
                TemplateElement elem = (TemplateElement)row.CellElementList[i];
                elem.Merge(context);
            }
            context.NextRow();
        }

        private void AdjustColumnWidth(Seasar.Fisshplate.Context.FPContext context, int column)
        {
            int cellWidth = _row.Sheet.HSSFSheet.GetColumnWidth(column);
            context.OutSheet.SetColumnWidth(context.CurrentCellNum, cellWidth);
        }

        private int GetMaxCellElementSize()
        {
            int max = 0;
            foreach (TemplateElement elem in _childList)
            {
                if (elem is Row)
                {
                    int listSize = ((Row)elem).CellElementList.Count;
                    max = max > listSize ? max : listSize;
                }
            }
            return max;
        }

        private void MergeNoIterationBlock(Seasar.Fisshplate.Context.FPContext context)
        {
            foreach (TemplateElement elem in _childList)
            {
                if (elem is Row)
                {
                    MergeNoIterationRow(context, (Row)elem);
                }
                else
                {
                    elem.Merge(context);
                }
            }
        }

        private void MergeNoIterationRow(Seasar.Fisshplate.Context.FPContext context, Row row)
        {
            HSSFRow outRow = context.CreateCurrentRow();
            outRow.Height = row.RowHeight;
            IDictionary<string, object> data = context.Data;
            data[FPConsts.RowNumberName] = context.CurrentRowNum + 1;
            for (int i = 0; i < row.CellElementList.Count; i++)
            {
                if (i > _startCellIndex)
                {
                    break;
                }
                TemplateElement elem = (TemplateElement)row.CellElementList[i];
                elem.Merge(context);
            }
            context.NextRow();

        }
    }
}
