using System.Data;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using PrettyTable.Models;
using Xunit.Abstractions;

namespace PrettyTable.Tests
{
	public class PrinterFixture : IDisposable
	{
		private string _startTag = "test";
		private string _endTag = "end";
		public IReadOnlyDictionary<string, string> Expected { get; }
		public PrinterFixture()
		{
			var dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			var sb = new StringBuilder();
			foreach (var line in File.ReadLines("PrinterTests.txt"))
			{
				if (line.StartsWith(_startTag, true, CultureInfo.InvariantCulture))
				{
					dictionary.Add(line.Split(":")[1].Trim(), string.Empty);
				}
				else if (line.StartsWith(_endTag, true, CultureInfo.InvariantCulture))
				{
					var testName = line.Split(_endTag)[1].Trim();
					if (!dictionary.ContainsKey(testName))
					{
						throw new Exception($"Failed reading test file. end tag '{testName}' without a start tag");
					}
					dictionary[testName] = sb.ToString();
					sb.Clear();
				}
				else
				{
					if (line.Trim().Length == 0) continue;
					sb.AppendLine(line);
				}
			}

			if (sb.Length != 0)
			{
				throw new Exception("Missing start or end tags");
			}
			Expected = dictionary;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
	}
	public class PrinterTests : IClassFixture<PrinterFixture>
	{
		private readonly ITestOutputHelper _output;
		private readonly PrinterFixture _fixture;

		public PrinterTests(PrinterFixture fixture, ITestOutputHelper output)
		{
			_fixture = fixture;
			_output = output;
		}

		[Fact]
		public void TableTestRows()
		{
			var testName = nameof(TableTestRows);
			Assert.True(_fixture.Expected.TryGetValue(testName, out var expected), $"expected test-case {testName} not found");
			var firstRow = new[] { "first", "second", "third" };
			var secondRow = new[] { "fourth", "fifth", "sixth" };
			var thirdRow = new[] { "seventh", "eighth", "ninth" };
			var actual = new Table(new GridOptions()).AddRow(firstRow).AddRow(secondRow).AddRow(thirdRow).ToString();
			_output.WriteLine(actual);
			_output.WriteLine(expected);
			Assert.Equal(expected, actual);
		}

		[Fact]
		public void TableTestRowsWithObjects()
		{
			var testName = nameof(TableTestRowsWithObjects);
			Assert.True(_fixture.Expected.TryGetValue(testName, out var expected), $"expected test-case {testName} not found");
			var firstRow = new object[] { 1.1, "second", 3 };
			var secondRow = new object[] { "fourth", 5, 6.6 };
			var thirdRow = new object[] { 7, 8.8, "ninth" };
			var actual = new Table(new GridOptions()).AddRow(firstRow).AddRow(secondRow).AddRow(thirdRow).ToString();
			_output.WriteLine(actual);
			_output.WriteLine(expected);
			Assert.Equal(expected, actual);
		}

		[Fact]
		public void TableTestColumns()
		{
			var testName = nameof(TableTestColumns);
			Assert.True(_fixture.Expected.TryGetValue(testName, out var expected), $"expected test-case {testName} not found");
			var firstCol = new[] { "first", "fourth", "seventh" };
			var secondCol = new[] { "second", "fifth", "eighth" };
			var thirdCol = new[] { "third", "sixth", "ninth" };
			var actual = new Table(new GridOptions()).AddColumn(firstCol).AddColumn(secondCol).AddColumn(thirdCol).ToString();
			_output.WriteLine(actual);
			_output.WriteLine(expected);
			Assert.Equal(expected, actual);
		}

		[Fact]
		public void TableTestMixedColThenRow()
		{
			var testName = nameof(TableTestMixedColThenRow);
			Assert.True(_fixture.Expected.TryGetValue(testName, out var expected), $"expected test-case {testName} not found");
			var firstCol = new[] { "first", "fourth", "seventh" };
			var secondCol = new[] { "second", "fifth", "eighth" };
			var actual = new Table(new GridOptions()).AddColumn(firstCol).AddRow(secondCol).ToString();
			_output.WriteLine(actual);
			_output.WriteLine(expected);
			Assert.Equal(expected, actual);
		}

		[Fact]
		public void TableTestMixedRowThenCol()
		{
			var testName = nameof(TableTestMixedRowThenCol);
			Assert.True(_fixture.Expected.TryGetValue(testName, out var expected), $"expected test-case {testName} not found");
			var firstCol = new[] { "first", "second", "third" };
			var secondCol = new[] { "fourth", "fifth", "sixth" };
			var actual = new Table(new GridOptions()).AddRow(firstCol).AddColumn(secondCol).ToString();
			_output.WriteLine(actual);
			_output.WriteLine(expected);
			Assert.Equal(expected, actual);
		}

		[Fact]
		public void TableFromDataTable()
		{
			var testName = nameof(TableFromDataTable);
			Assert.True(_fixture.Expected.TryGetValue(testName, out var expected), $"expected test-case {testName} not found");
			var dataTable = new DataTable();
			var columns = new DataColumn[]
			{
				new ("", typeof(string)),
				new ("", typeof(int)),
				new ("", typeof(float))
			};
			dataTable.Columns.AddRange(columns);
			dataTable.Rows.Add("first", 2, 3.3f);
			dataTable.Rows.Add("fourth", 5, 6.6f);
			dataTable.Rows.Add("seventh", 8, 9.9f);
			var actual = Table.FromDataTable(dataTable).ToString();
			_output.WriteLine(actual);
			_output.WriteLine(expected);
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData (true)]
		[InlineData (false)]
		public void TableFromDataTableWithColumnNames(bool withHeader)
		{
			var testName = nameof(TableFromDataTableWithColumnNames) + withHeader;
			Assert.True(_fixture.Expected.TryGetValue(testName, out var expected), $"expected test-case {testName} not found");
			var dataTable = new DataTable();
			var columns = new DataColumn[]
			{
				new ("header1", typeof(string)),
				new ("header2", typeof(int)),
				new ("header3", typeof(float))
			};
			dataTable.Columns.AddRange(columns);
			dataTable.Rows.Add("first", 2, 3.3f);
			dataTable.Rows.Add("fourth", 5, 6.6f);
			dataTable.Rows.Add("seventh", 8, 9.9f);
			var actual = Table.FromDataTable(dataTable, new GridOptions() {WithHeaders = withHeader}).ToString();
			_output.WriteLine(actual);
			_output.WriteLine(expected);
			Assert.Equal(expected, actual);
		}

		[Fact]
		public void TableTestRowsWithHeader()
		{
			var testName = nameof(TableTestRowsWithHeader);
			Assert.True(_fixture.Expected.TryGetValue(testName, out var expected), $"expected test-case {testName} not found");
			var firstRow = new[] { "first", "second", "third" };
			var secondRow = new[] { "fourth", "fifth", "sixth" };
			var thirdRow = new[] { "seventh", "eighth", "ninth" };
			var headers = new[] { "header1", "header2", "header3" };
			var actual = new Table(new GridOptions()).AddHeaders(headers).AddRow(firstRow).AddRow(secondRow).AddRow(thirdRow).ToString();
			_output.WriteLine(actual);
			_output.WriteLine(expected);
			Assert.Equal(expected, actual);
			actual = new Table(new GridOptions()).AddRow(firstRow).AddRow(secondRow).AddHeaders(headers).AddRow(thirdRow).ToString();
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