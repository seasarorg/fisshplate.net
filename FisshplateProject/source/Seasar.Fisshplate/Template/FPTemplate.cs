using System;
using System.Collections.Generic;

using System.Text;
using Seasar.Fisshplate.Parser;
using NPOI.HSSF.UserModel;
using System.IO;
using Seasar.Fisshplate.Util;
using NPOI.POIFS.FileSystem;
using Seasar.Fisshplate.Wrapper;
using Seasar.Fisshplate.Core.Element;
using Seasar.Fisshplate.Context;
using Seasar.Fisshplate.Consts;

namespace Seasar.Fisshplate.Template
{
    /// <summary>
    /// FiSSH PlateでHSSFWorkbookを生成する際のエントリポイントとなるクラスです。
    /// </summary>
    /// <see cref="HSSFWorkbook"/>
    public class FPTemplate
    {
        private FPParser _parser = new FPParser();

        public FPTemplate()
        {
        }

        /// <summary>
        /// 独自でカスタマイズしたTemplateElementを適用するRowParserを追加します。
        /// </summary>
        /// <param name="rowParser"></param>
        public void AddRowParser(RowParser rowParser)
        {
            _parser.AddRowParser(rowParser);
        }

        public HSSFWorkbook Process(string templateName, Dictionary<string, object> data)
        {
            FileStream fs = InputStreamUtil.GetResourceAsStream(templateName);
            HSSFWorkbook workbook = new HSSFWorkbook(new POIFSFileSystem(fs));
            InputStreamUtil.Close(fs);
            return Process(workbook, data);
        }

        public HSSFWorkbook Process(Stream ins, Dictionary<string, object> data)
        {
            return Process(new HSSFWorkbook(new POIFSFileSystem(ins)), data);
        }

        public HSSFWorkbook Process(HSSFWorkbook hssfWorkbook, Dictionary<string, object> data)
        {
            WorkbookWrapper workbook = new WorkbookWrapper(hssfWorkbook);
            for (int i = 0; i < workbook.SheetCount; i++)
            {
                SheetWrapper sheet = workbook.GetSheetAt(i);
                if (sheet.RowCount < 1)
                {
                    continue;
                }
                Root root = _parser.Parse(sheet);

                sheet.PrepareForMerge();
                if (data == null)
                {
                    data = new Dictionary<string, object>();
                }
                FPContext context = new FPContext(sheet.HSSFSheet, data);
                PageContext pageContext = new PageContext();
                data[FPConsts.PageContextName] = pageContext;

                root.Merge(context);
            }
            return hssfWorkbook;
        }


    }
}
