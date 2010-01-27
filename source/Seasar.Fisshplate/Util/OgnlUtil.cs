using System;
using System.Collections.Generic;

using System.Text;
using System.Text.RegularExpressions;

namespace Seasar.Fisshplate.Util
{
    public static class OgnlUtil
    {
        /// <summary>
        /// Java版と違い、 JScript.NETで式を解決する。
        /// JScript.NETだと"data.title"のような式や "code" のような式が認識出来ない。
        /// "code" -> "__obj__['code']
        /// "data.title.hoge -> "__obj__['data'].title.hoge
        /// "__obj__.title" -> "__obj__['__obj__'].title
        /// に変換してJSciprtでevalを行う。
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="data"></param>
        /// <remarks>内部変数として、「__obj__」を利用します。
        /// 「__obj__」を変数として利用したい場合は、<code>__obj__['__obj__']</code>と記述してください。
        /// ただし、変数名やプロパティ名として「__obj__」を利用することはオススメしません。
        /// </remarks>
        /// <returns></returns>
        public static object GetValue(string expression, IDictionary<string, object> data)
        {
            try
            {
                string expr = ToEvalFormula(expression);

                return JScriptUtil.Evaluate(expr, data);
            }
            catch (System.Exception e)
            {
                throw new ApplicationException("JScript実行時例外", e);
            }
        }

        private static string ToEvalExpr(string expression)
        {
            // ''形式の場合、ただの文字列なのでスルー
            if (Regex.Match(expression.Trim(), @"^'.*'$").Success)
            {
                return expression;
            }

            // "true", "false" の場合、 boolean値なのでスルー
            if (expression.Trim() == "true" || expression.Trim() == "false")
            {
                return expression;
            }
            //__obj__['～']形式の場合、正しい形式なので何もしない。
            if (Regex.Match(expression.Trim(), @"^__obj__\['\S+'\]").Success)
            {
                return expression;
            }
           
            // 変数一つか、最初が"."区切り、もしくは"["(配列)の場合)
            Match mat2 = Regex.Match(expression, @"^\s*([^\s\.\[]+)((\.|\[).*)?");
            if (mat2.Success)
            {
                return "__obj__['" + mat2.Groups[1].Value + "']" + mat2.Groups[2].Value;
            }
            throw new ApplicationException("JSciprtで解析出来ない式です。[" + expression + "]");
        }

        /// <summary>
        /// 変数宣言用に置換してから処理を行う。
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="data"></param>
        public static object DeclareVar(string expression, Dictionary<string, object> data)
        {
            string exp = ToEvalFormula(expression);

            return JScriptUtil.Evaluate(exp, data);
        }

        public static string ToEvalFormula(string expression)
        {
            Regex _varPat = new Regex(@"[^\s\+\-\(\)&|\=/%\*,0-9]{1}[^\s\+\-\(\)&|\=/%\*]*");
            MatchCollection matCol = _varPat.Matches(expression);

            string exp = String.Empty;

            int idx = 0;
            foreach (Match mat in matCol)
            {
                exp += expression.Substring(idx, mat.Index - idx);
                string varExp = ToEvalExpr(mat.Value);
                exp += varExp;
                idx = mat.Index + mat.Length;
            }
            if (idx < expression.Length)
            {
                exp += expression.Substring(idx);
            }
            return exp;
        }

    }
}
