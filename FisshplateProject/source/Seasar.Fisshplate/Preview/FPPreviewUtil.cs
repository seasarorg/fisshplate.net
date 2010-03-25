using System;
using System.Collections.Generic;
using System.Text;
using NPOI.HSSF.UserModel;
using Seasar.Fisshplate.Template;
using System.Collections;
using System.IO;

namespace Seasar.Fisshplate.Preview
{
    public static class FPPreviewUtil
    {
        /// <summary>
        /// テンプレートファイルにデータファイル内のデータを埋め込んだ出力ファイルを戻します。
        /// </summary>
        /// <param name="template">テンプレート用ファイル</param>
        /// <param name="data">データ用ファイル</param>
        /// <exception cref="">FPException</exception>
        /// <returns></returns>
        public static HSSFWorkbook GetWorkbook(HSSFWorkbook template, HSSFWorkbook data)
        {
            FPTemplate fptemp = new FPTemplate();
            MapBuilder mb = new MapBuilder();
            IDictionary<string, object> map = mb.BuildMapFrom(data);
            return fptemp.Process(template, map);
        }

        /// <summary>
        /// テンプレートファイルのストリームと、データ用ファイルのストリームから出力ファイルを生成して戻します。
        /// </summary>
        /// <param name="template">テンプレート用ストリーム</param>
        /// <param name="data">データ用ストリーム</param>
        /// <returns>データを埋め込んだワークブック</returns>
        /// <exception cref="">FPException</exception>
        public static HSSFWorkbook GetWorkbook(Stream template, Stream data)
        {
            HSSFWorkbook tempWb = new HSSFWorkbook(template);
            HSSFWorkbook dataWb = new HSSFWorkbook(data);
            return GetWorkbook(tempWb, dataWb);
        }
    }
}
