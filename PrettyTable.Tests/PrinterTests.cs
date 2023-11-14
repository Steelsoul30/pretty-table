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
			var sw = new StringWriter();
			Console.SetOut(sw);
			var firstRow = new[] { "first", "second", "third" };
			var secondRow = new[] { "fourth", "fifth", "sixth" };
			var thirdRow = new[] { "seventh", "eighth", "ninth" };
			new Grid(new GridOptions()).AddRow(firstRow).AddRow(secondRow).AddRow(thirdRow).Normalize().Print();
			_output.WriteLine(sw.ToString());
			_output.WriteLine(expected);
			Assert.Equal(expected, sw.ToString());
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