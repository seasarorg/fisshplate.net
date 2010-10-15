using System;
using System.Collections.Generic;

using System.Text;
using System.Text.RegularExpressions;

namespace Seasar.Fisshplate.Util
{
    public static class OgnlUtil
    {
        private static readonly String RootObjName = "__obj__";

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
            //Trimしてから計算する。
            expression = expression.Trim();
            //からの場合、何もしない。
            if (expression.Length == 0)
            {
                return expression;
            }
            // ''形式の場合、ただの文字列なのでスルー
            if (Regex.Match(expression, @"^'.*'$").Success)
            {
                return expression;
            }
            //数字から始まる場合もスルー
            if (Regex.Match(expression, @"^[0-9]+").Success)
            {
                return expression;
            }
            //「.」から始まる場合もスルー
            if (expression.StartsWith("."))
            {
                return expression;
            }

            // "true", "false" の場合、 boolean値なのでスルー
            if (expression == "true" || expression == "false")
            {
                return expression;
            }
            //__obj__の場合、予約語なので何もしない。
            if (expression == RootObjName)
            {
                return expression;
            }
           
            // 変数一つか、最初が"."区切り、もしくは"["(配列)の場合)
            Match mat2 = Regex.Match(expression, @"^\s*([^\s\.\[]+)((\.|\[).*)?");
            if (mat2.Success)
            {
                return RootObjName + "['" + mat2.Groups[1].Value + "']" + mat2.Groups[2].Value;
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
            expression = expression.Replace(" ", "");

            const char DEFAULT_MODE = '0';
            const char SINGLE_QUOTATION = '1';
            const char DOUBLE_QUOTATION = '3';
            const char ESCAPE_SINGLE_QUOTATION = '2';
            const char ESCAPE_DOUBE_QUOTATION = '4';
            const char TERM_MODE = '5';
            var mode = DEFAULT_MODE;
            var st = 0;
            var evaledExpr = "";
            //文字列を1字ずつ字句解析する。
            for (int i = 0; i < expression.Length; i++)
            {
                var expCh = expression[i];
                //シングルクォーテーションの場合
                if (expCh == '\'')
                {
                    if (mode == DEFAULT_MODE) // 空の状態
                    {
                        //文字列開始(Single)
                        mode = SINGLE_QUOTATION;
                        //開始位置記憶
                        st = i;
                    }
                    else if (mode == SINGLE_QUOTATION) //シングルクォーテーションの途中
                    {
                        // 文字列終了。解析終了文字列に足しこみ
                        evaledExpr += expression.Substring(st, i + 1 - st);
                        mode = DEFAULT_MODE;
                    }
                    else if (mode == ESCAPE_SINGLE_QUOTATION) //エスケープ対象
                    {
                        mode = SINGLE_QUOTATION;
                    }
                    else
                    {
                        throw new ApplicationException("式の解析中にエラーが発生しました(')");
                    }
                }
                else if (expCh == '\"')
                {
                    if (mode == DEFAULT_MODE) // 空の状態
                    {
                        //文字列開始(Double)
                        mode = DOUBLE_QUOTATION;
                        //開始位置記憶
                        st = i;
                    }
                    else if (mode == DOUBLE_QUOTATION) //ダブルクォーテーションの途中
                    {
                        // 文字列終了。解析終了文字列に足しこみ
                        evaledExpr += expression.Substring(st, i + 1 - st);
                        mode = '0';
                    }
                    else if (mode == ESCAPE_DOUBE_QUOTATION) //エスケープ対象
                    {
                        mode = DOUBLE_QUOTATION;
                    }
                }
                else if (expCh == '\\')
                {
                    if (mode == SINGLE_QUOTATION)
                    {
                        mode = ESCAPE_SINGLE_QUOTATION;
                    }
                    else if (mode == DOUBLE_QUOTATION)
                    {
                        mode = ESCAPE_DOUBE_QUOTATION;
                    }
                    else
                    {
                        throw new ApplicationException("式の解析中にエラーが発生しました(\\)");
                    }
                }
                else
                {
                    if (expCh == '+' || expCh == '-' || expCh == '*' || expCh == '/' ||
                        expCh == '(' || expCh == ')' || expCh == '[' || expCh == ']' ||
                        expCh == '&' || expCh == '|' || expCh == '=' || expCh == '!' ||
                        expCh == '>' || expCh == '<' || expCh == '%')
                    {
                        if (mode == SINGLE_QUOTATION || mode == DOUBLE_QUOTATION)
                        {
                            //Do Nothing
                        }
                        else if (mode == ESCAPE_SINGLE_QUOTATION || mode == ESCAPE_DOUBE_QUOTATION)
                        {
                            throw new ApplicationException("式の解析中にエラーが発生しました()");
                        }
                        else if (mode == TERM_MODE) //項が確定
                        {
                            var ex = expression.Substring(st, i - st);
                            evaledExpr += ToEvalExpr(ex);
                            evaledExpr += expCh;
                            mode = DEFAULT_MODE;
                        }
                        else
                        {
                            //そのまま足す
                            evaledExpr += expCh;
                        }
                    }
                    else
                    {
                        if (mode == DEFAULT_MODE) //項の開始
                        {
                            st = i;
                            mode = TERM_MODE;
                        }
                        else if (mode == SINGLE_QUOTATION || mode == DOUBLE_QUOTATION)
                        {
                            // Do Nothing
                        }
                        else if (mode == ESCAPE_SINGLE_QUOTATION || mode == ESCAPE_DOUBE_QUOTATION)
                        {
                            throw new ApplicationException("式の解析中にエラーが発生しました()");
                        }
                        //通常文字
                        else if (mode == TERM_MODE) // 項の途中
                        {
                            // Do Nothing
                        }
                    }
                }
                //TODO シングルクォーテーションの場合
                //   1 : 空の状態だったら、文字列開始
                //   2 : シングルクォーテーションの途中だったら、文字列終了
                //   3 : 文字列中の\の後だったら、エスケープ対象
                //TODO ダブルクォーテーションの場合
                //   1 : 空の状態だったら、文字列開始
                //   2 : ダブルクォーテーションの途中だったら、文字列終了
                //   3 : 文字列中の\の後だったら、エスケープ対象
                //TODO \の場合
                //   文字列中だったら、エスケープ対象
                //   それ以外だったら、エラー
                //TODO +-*/%()&|=!><[] の場合
                //   文字列中だったら、スルー
                //   それ以外だったら演算子なので、一つ前までが解析対象
            }
            //終了後、項の途中だったら最後に足し込む
            if (mode == TERM_MODE)
            {
                var ex = expression.Substring(st);
                evaledExpr += ToEvalExpr(ex);
            }
     
            return evaledExpr;
            //expression = expression.Replace(" ", "");
            //System.Console.WriteLine(expression);
            ////''や""で全てが囲まれていた場合は文字列とみなして何もしない。
            //if (expression[0] == '\'' && expression[expression.Length - 1] == '\'')
            //{
            //    return expression;
            //}
            //if (expression[0] == '"' && expression[expression.Length - 1] == '"')
            //{
            //    return expression;
            //}

            //Regex varPat = new Regex(@"[^\+\-\*/%(\)&|\=\!><\[\]]*");
            //MatchCollection matCol = varPat.Matches(expression);
            //string exp = String.Empty;

            //int idx = 0;
            //foreach (Match mat in matCol)
            //{
            //    String varExp = mat.Value;
            //    exp += expression.Substring(idx, mat.Index - idx);
            //    if (mat.Value.Trim().StartsWith(".") == false || mat.Value.Trim().Length != 0)
            //    {
            //        varExp = ToEvalExpr(mat.Value);
            //    }
            //    exp += varExp;
            //    idx = mat.Index + mat.Length;
            //}
            //if (idx < expression.Length)
            //{
            //    exp += expression.Substring(idx);
            //}
            //return exp;
        }

    }
}
