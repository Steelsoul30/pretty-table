using System.Security.Cryptography;
using PrettyTable.Models;
using Xunit.Abstractions;

namespace PrettyTable.Tests
{
	public class PrinterTests
	{
		private readonly ITestOutputHelper _output;

		public PrinterTests(ITestOutputHelper output)
		{
			_output = output;
		}

		[Fact]
		public void Test1()
		{
			var expected = 
@"┌─────────┬────────┬───────┐
│         │        │       │
│  first  │ second │ third │
│         │        │       │
├─────────┼────────┼───────┤
│         │        │       │
│ fourth  │ fifth  │ sixth │
│         │        │       │
├─────────┼────────┼───────┤
│         │        │       │
│ seventh │ eighth │ ninth │
│         │        │       │
└─────────┴────────┴───────┘
";
			var firstRow = new[] { "first", "second", "third" };
			var secondRow = new[] { "fourth", "fifth", "sixth" };
			var thirdRow = new[] { "seventh", "eighth", "ninth" };
			var actual = new Grid(new Options()).AddRow(firstRow).AddRow(secondRow).AddRow(thirdRow).ToString();
			_output.WriteLine(actual);
			_output.WriteLine(expected);
			Assert.Equal(expected, actual);
		}

		[Fact]
		public void TableTestRows()
		{
			var expected = 
				@"┌─────────┬────────┬───────┐
│         │        │       │
│  first  │ second │ third │
│         │        │       │
├─────────┼────────┼───────┤
│         │        │       │
│ fourth  │ fifth  │ sixth │
│         │        │       │
├─────────┼────────┼───────┤
│         │        │       │
│ seventh │ eighth │ ninth │
│         │        │       │
└─────────┴────────┴───────┘
";
			var firstRow = new[] { "first", "second", "third" };
			var secondRow = new[] { "fourth", "fifth", "sixth" };
			var thirdRow = new[] { "seventh", "eighth", "ninth" };
			var actual = new Table(new Options()).AddRow(firstRow).AddRow(secondRow).AddRow(thirdRow).ToString();
			_output.WriteLine(actual);
			_output.WriteLine(expected);
			Assert.Equal(expected, actual);
		}

		[Fact]
		public void TableTestColumns()
		{
			var expected =
				@"┌─────────┬────────┬───────┐
│         │        │       │
│  first  │ second │ third │
│         │        │       │
├─────────┼────────┼───────┤
│         │        │       │
│ fourth  │ fifth  │ sixth │
│         │        │       │
├─────────┼────────┼───────┤
│         │        │       │
│ seventh │ eighth │ ninth │
│         │        │       │
└─────────┴────────┴───────┘
";
			var firstCol = new[] { "first", "fourth", "seventh" };
			var secondCol = new[] { "second", "fifth", "eighth" };
			var thirdCol = new[] { "third", "sixth", "ninth" };
			var actual = new Table(new Options()).AddColumn(firstCol).AddColumn(secondCol).AddColumn(thirdCol).ToString();
			_output.WriteLine(actual);
			_output.WriteLine(expected);
			Assert.Equal(expected, actual);
		}

		private string GetUniqueKey(int size)
		{
			var bitCount = size * 6;
			var byteCount = (bitCount + 7) / 8;
			var data = new byte[byteCount];
			using var crypto = RandomNumberGenerator.Create();
			crypto.GetBytes(data);
			return Convert.ToBase64String(data);
		}
	}
}