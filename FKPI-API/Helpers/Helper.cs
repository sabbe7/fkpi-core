using System;
using System.Collections.Generic;

namespace FKPI_API.Helpers
{
    public class Helper
    {
        public static bool IsOperator(string token)
        {
            return (new List<string>() { "+", "-", "*", "/" }).Contains(token);
        }

        public static string GenerateId()
        {
            return Guid.NewGuid().ToString();
        }

        public static bool IsFormulaValid(List<string> tokens)
        {
            // Invalid formula if we have less than 3 items
            if (tokens.Count < 3)
            {
                return false;
            }

            // first and last items cannot be operators
            if (Helper.IsOperator(tokens[0]) || Helper.IsOperator(tokens[tokens.Count - 1]))
            {
                return false;
            }

            // even-indexed items must be accounts and odd-indexed items must be operators
            for (var i = 1; i < tokens.Count - 1; i++)
            {
                var isOperator = Helper.IsOperator(tokens[i]);

                if (i % 2 == 0 && isOperator || i % 2 != 0 && !isOperator)
                {
                    return false;
                }
            }

            return true;
        }
    }
}