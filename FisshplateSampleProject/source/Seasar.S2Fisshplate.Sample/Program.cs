using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Seasar.Quill;

namespace Seasar.S2Fisshplate.Sample
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Form form1 = new Form1();
            QuillInjector.GetInstance().Inject(form1);

            Application.Run(form1);
        }
    }
}
