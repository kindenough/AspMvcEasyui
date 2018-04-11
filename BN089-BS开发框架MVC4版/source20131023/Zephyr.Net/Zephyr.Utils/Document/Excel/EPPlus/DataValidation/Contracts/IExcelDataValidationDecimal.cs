using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zephyr.Utils.EPPlus.DataValidation.Formulas.Contracts;

namespace Zephyr.Utils.EPPlus.DataValidation.Contracts
{
    /// <summary>
    /// Data validation interface for decimal values
    /// </summary>
    public interface IExcelDataValidationDecimal : IExcelDataValidationWithFormula2<IExcelDataValidationFormulaDecimal>, IExcelDataValidationWithOperator
    {
    }
}
