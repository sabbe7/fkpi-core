using System;
using System.Collections.Generic;
using System.Linq;
using FKPI_API.Helpers;
using SqlKata;

namespace FKPI_API.Core
{
    public class QueryParser
    {
        /// <summary>
        /// Get query used for KPI evaluation
        /// </summary>
        /// <param name="tokens">List of tokens that form a KPI formula</param>        
        /// <returns>Returns query</returns>
        public Query Parse(List<string> tokens)
        {
            if (!Helpers.Helper.IsFormulaValid(tokens))
            {
                throw new Exception("Invalid formula");
            }

            var accounts = tokens.Where(x => !Helpers.Helper.IsOperator(x)).ToList();

            // get a query for the available years in the account value table
            var years = new Query("AccountValue").SelectRaw("Distinct year");

            // this cte is going to be the main query to build upon
            var cte = new Query().From(years.As("av")).Select("av.year");

            // loop over the accounts found in the formula and for each account add a join query by year and account value 
            for (int i = 0; i < accounts.Count; i++)
            {
                var tableName = $"av{i + 1}";

                var columnName = $"Account{i + 1}";

                tokens = tokens.Select(x => accounts[i] == x ? $"{columnName}" : x).ToList();

                cte.LeftJoin($"AccountValue as {tableName}", j =>
                    j.On("av.Year", $"av{i + 1}.Year").Where($"av{i + 1}.AccountId", accounts[i])
                )
                .Select($"av{i + 1}.Amount as {columnName}");
            }

            // the total query wich applies the formula over the joined queries
            var query = new Query("Accounts")
                .With("Accounts", cte)
                .Select("Year")
                .SelectRaw($"{string.Join(' ', tokens)} as [Value]");

            return query;
        }
    }
}