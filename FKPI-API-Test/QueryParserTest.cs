using Xunit;
using System.Collections.Generic;
using FKPI_API.Core;
using SqlKata.Compilers;
using System;
using System.Linq;

public class QueryParserTest
{
    private QueryParser queryParser = new QueryParser();
    private SqlServerCompiler compiler = new SqlServerCompiler();

    public QueryParserTest() { }

    [Fact]
    public void ItShouldThrowExceptionWhenEmpty()
    {
        Assert.Throws<System.Exception>(() => queryParser.Parse(new List<string> { }));
    }

    [Fact]
    public void ItShouldThrowExceptionWhenStartingWithOperator()
    {
        Assert.Throws<System.Exception>(() => queryParser.Parse(new List<string> { "/", "53", "+", "183" }));
    }

    [Fact]
    public void ItShouldThrowExceptionWhenEndingWithOperator()
    {
        Assert.Throws<System.Exception>(() => queryParser.Parse(new List<string> { "53", "+", "183", "*" }));
    }

    [Fact]
    public void ItShouldThrowExceptionWhenTwoSuccessiveAccounts()
    {
        Assert.Throws<System.Exception>(() => queryParser.Parse(new List<string> { "53", "*", "183", "170", "/", "12" }));
    }

    [Fact]
    public void ItShouldThrowExceptionWhenTwoSuccessiveOperators()
    {
        Assert.Throws<System.Exception>(() => queryParser.Parse(new List<string> { "53", "*", "+", "183" }));
    }

    [Fact]
    public void ItShouldContainJoinsWhenFormulaValid()
    {
        var sql = compiler.Compile(queryParser.Parse(new List<string> { "53", "+", "183" })).ToString();
        Assert.Contains("JOIN", sql);
    }

    [Fact]
    public void NumberOfJoinsShouldMatchAccountsNumberWhenFormulaValid()
    {
        var sql = compiler.Compile(queryParser.Parse(new List<string> { "53", "+", "183", "*", "12", "/", "26" })).ToString();
        var joinsNumber = sql.Split(" ").Count(x => x.ToLower() == "join");
        Assert.Equal(4, joinsNumber);
    }

    [Fact]
    public void SelectStatementShouldMatchExpectedSelectWhenValid()
    {
        var sql = compiler.Compile(queryParser.Parse(new List<string> { "53", "+", "183", "-", "353" })).ToString();

        var expected = @"SELECT [Year], Account1 + Account2 - Account3 as [Value] FROM [Accounts]";

        Assert.Contains(expected, sql);
    }
}