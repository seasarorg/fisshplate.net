using System;
using System.Collections.Generic;
using System.Text;
using Seasar.Fisshplate.Wrapper;

namespace Seasar.Fisshplate.Parser
{
    public interface RowParser
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="parser"></param>
        /// <returns></returns>
        /// <exception cref="Seasar.Fisshplate.Exception.FPParseException"></exception>
        bool Process(CellWrapper cell, FPParser parser);
    }
}
