using System;
using System.Collections.Generic;
using System.Text;
using NPOI.HSSF.UserModel;
using Seasar.Fisshplate.Wrapper;
using System.Text.RegularExpressions;
using Seasar.Fisshplate.Consts;
using Seasar.Fisshplate.Core;

namespace Seasar.Fisshplate.Preview
{
    /// <summary>
    /// EXCELからテンプレートに埋め込むDictionaryを生成するクラスです。
    /// </summary>
    public class MapBuilder
    {
        private static readonly String RootSheetName = "root";

        public IDictionary<string, object> BuildMapFrom(HSSFWorkbook wb)
        {
            WorkbookWrapper workbook = new WorkbookWrapper(wb);
            SheetWrapper sheet = workbook.GetSheetByName(RootSheetName);
            FPMapData root = new FPMapData(sheet, RootSheetName);
            BuildRootMapData(workbook, root);

            return (IDictionary<String, object>)root.BuildData();
        }

        private void BuildRootMapData(WorkbookWrapper workbook, FPMapData root)
        {
            for (int i = 0; i < workbook.SheetCount; i++)
            {
                SheetWrapper sheet = workbook.GetSheetAt(i);
                String keyName = GetKeyNameFrom(sheet);
                if (RootSheetName != keyName)
                {
                    BuildMapData(root, sheet, keyName);
                }
            }
        }

        private string GetKeyNameFrom(SheetWrapper sheet)
        {
            String col = sheet.GetRow(0).GetCell(0).StringValue;
            if (String.IsNullOrEmpty(col) && Regex.Match(FPConsts.RegexBindVar, col).Success)
            {
                sheet.RemoveRow(0);
                BindVariable var = new BindVariable(col);
                return var.Name;
            }
            else
            {
                return sheet.SheetName;
            }
        }

        private void BuildMapData(FPMapData parent, SheetWrapper sheet, string keyName)
        {
            int idx = keyName.IndexOf('#');
            if (idx == -1)
            {
                parent.AddChild(sheet, keyName);
            }
            else
            {
                BuildMapDataComposition(parent, sheet, keyName, idx);
            }
        }

        private void BuildMapDataComposition(FPMapData grandParent, SheetWrapper sheet, string keyName, int idx)
        {
            String selfKeyName = keyName.Substring(idx + 1);
            String parentKeyName = keyName.Substring(0, idx);
            FPMapData parent = grandParent.GetChildByKey(parentKeyName);
            if (parent == null)
            {
                //TODO throw new FPPreviewException(FPConsts.MessageIdPreviewLacckOfParent, new Object[] { parentKeyName });
            }
            BuildMapData(parent, sheet, selfKeyName);
        }


    }
}
