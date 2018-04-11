using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zephyr.Utils.EPPlus.DataValidation.Formulas.Contracts;

namespace Zephyr.Utils.EPPlus.DataValidation.Contracts
{
    public interface IExcelDataValidationInt : IExcelDataValidationWithFormula2<IExcelDataValidationFormulaInt>, IExcelDataValidationWithOperator
    {
    }
}
