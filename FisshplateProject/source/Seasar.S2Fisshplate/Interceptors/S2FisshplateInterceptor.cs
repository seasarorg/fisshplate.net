using System;
using System.Collections.Generic;
using System.Text;
using Seasar.Framework.Aop.Interceptors;
using NPOI.HSSF.UserModel;
using System.Reflection;
using Seasar.S2Fisshplate.Attr;
using System.IO;
using Seasar.Fisshplate.Template;

namespace Seasar.S2Fisshplate.Interceptors
{
    public class S2FisshplateInterceptor : AbstractInterceptor
    {

        public override object Invoke(Seasar.Framework.Aop.IMethodInvocation invocation)
        {
            MethodBase method = invocation.Method;
            if (!method.IsAbstract)
            {
                return invocation.Proceed();
            }
            // メソッド引数
            object[] args = invocation.Arguments;
            if (args == null || args.Length == 0)
            {
                return null;
            }

            object bean = args[0];
            Dictionary<string, object> __obj__ = new Dictionary<string, object>();
            __obj__["data"] = bean;
            HSSFWorkbook workbook = GetWorkbook(method);

            FPTemplate template = new FPTemplate();

            return template.Process(workbook, __obj__);
        }

        private HSSFWorkbook GetWorkbook(MethodBase method)
        {
            //属性の取得
            object[] attributes = method.GetCustomAttributes(typeof(FPTemplateAttribute), false);
            foreach (object o in attributes)
	        {
                FPTemplateAttribute attribute = (FPTemplateAttribute)o;

                // テンプレートのパスを取得する。
                string path = attribute.Path;

                try
                {
                    using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        HSSFWorkbook wb = new HSSFWorkbook(fs);
                        return wb;
                    }

                }
                catch (IOException e)
                {
                    throw new ApplicationException("ファイルが開けません", e);
                }
	        }
            return null;
        }
    }
}
