using System;
using System.Collections.Generic;

using System.Text;
using System.Collections.Specialized;
using System.CodeDom.Compiler;
using Microsoft.JScript;
using System.Reflection;

namespace Seasar.Fisshplate.Util
{
    public static class JScriptUtil
    {
        private static readonly CodeDomProvider _provider = new JScriptCodeProvider();
        private static readonly Type _evaluateType;

        private const string EvalSource = @"
            package Seasar.Fisshplate.Util.JScript
            {
                class Evaluator
                {
                    public static function Eval(expr : String, unsafe : boolean,
                        __obj__ : Object) : Object
                    {
                        if (unsafe)
                        {
                            return eval(expr, 'unsafe');
                        }
                        else
                        {
                            return eval(expr);
                        }
                    }
                }
            }";

        static JScriptUtil()
        {
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateInMemory = true;

            CompilerResults results = _provider.CompileAssemblyFromSource(parameters, EvalSource);

            Assembly assembly = results.CompiledAssembly;
            _evaluateType = assembly.GetType("Seasar.Fisshplate.Util.JScript.Evaluator");
        }

        public static object Evaluate(string exp, object root)
        {
            if (exp.Contains("\r")) { exp = exp.Replace("\r", "\\r"); }
            if (exp.Contains("\n")) { exp = exp.Replace("\n", "\\n"); }

            return _evaluateType.InvokeMember("Eval", BindingFlags.InvokeMethod,
                    null, null, new object[] { exp, true, root});
        }
    }
}
