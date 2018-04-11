using System;
using System.Collections.Generic;
using System.Text;

namespace Zephyr.Utils.NPOI.SS.Formula.Eval
{
    public class MissingArgEval : ValueEval
    {
        public static MissingArgEval instance = new MissingArgEval();

        private MissingArgEval()
        {
        }
    }
}
