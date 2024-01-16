using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Common.Validator
{
    public class ArgumentValidation
    {
        public static void ThrowIfNullOrEmpty([NotNull] string input,string name) 
        {
            if(string.IsNullOrEmpty(input))
            {
                throw new ArgumentNullException(name);
            }
        }
        public static void ThrowIfNull<T>(T value, [CallerArgumentExpression("value")] string expression = null) where T : class
        {
            if(value == null)
            {
                Throw (expression);
            }
        }
        private static void Throw(string expression)
        => throw new ArgumentException($"Argument {expression} must not be null");
    }
}
