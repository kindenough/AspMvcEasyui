using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zephyr.Utils.EPPlus.DataValidation.Contracts
{
    /// <summary>
    /// Represents a validation with an operator
    /// </summary>
    public interface IExcelDataValidationWithOperator
    {
        /// <summary>
        /// Operator type
        /// </summary>
        ExcelDataValidationOperator Operator { get; set; }
    }
}
