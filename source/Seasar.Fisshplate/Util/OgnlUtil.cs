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
        /// "data.title" -> "data['title']
        /// "code" -> "data['code']
        /// "data.title.hoge -> "data['title'].hoge
        /// に変換してJSciprtでevalを行う。
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="data"></param>
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
            //data['～']形式の場合
            if (Regex.Match(expression.Trim(), @"^data\['\S+'\]").Success)
            {
                return expression;
            }
            // data.～の形式の場合
            Match mat = Regex.Match(expression, @"^data\.(\S[^\.\[\]]+)(\S*)");
            if (mat.Success)
            {
                return "data['" + mat.Groups[1].Value + "']" + mat.Groups[2].Value;
            }
            
            // "data"以外の値の場合(変数一つか、最初が"."区切りか、"[" がある)
            Match mat2 = Regex.Match(expression, @"^([^\s\.\[]+)((\.|\[).*)?");
            if (mat2.Success)
            {
                return "data['" + mat2.Groups[1].Value + "']" + mat2.Groups[2].Value;
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
