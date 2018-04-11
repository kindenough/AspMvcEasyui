using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zephyr.Utils.EPPlus.DataValidation.Formulas.Contracts;

namespace Zephyr.Utils.EPPlus.DataValidation.Contracts
{
    /// <summary>
    /// Interface for a data validation with two formulas
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IExcelDataValidationWithFormula2<T> : IExcelDataValidationWithFormula<T>
        where T : IExcelDataValidationFormula
    {
        /// <summary>
        /// Formula 2
        /// </summary>
        T Formula2 { get; }
    }
}
