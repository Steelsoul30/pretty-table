using System.Data;
using System.Security.Cryptography;

namespace PrettyTable.Models;

public class Table : DataTable, IGrid
{
	public Options Options { get; }
	public int RowCount => Rows.Count;
	public int ColumnCount => Columns.Count;

	private List<string>? _headers;

	public Table(Options? options = null)
	{
		Options = options ?? new Options();
	}

	public Table AddRow(IEnumerable<string> rowData)
	{
		var data = rowData.ToList();
		var diff = data.Count - Columns.Count;
		switch (diff)
		{
			case > 0: // more data than columns
				for (var i = 0; i < diff; i++)
				{
					Columns.Add();
				}
				break;
			case < 0: // less data than columns
				for (var i = 0; i < -diff; i++)
				{
					data.Add(string.Empty);
				}
				break;
		}
		var row = NewRow();
		
		for (var i = 0 ; i < data.Count; i++)
		{
			row[i] = data[i];
		}

		return this;
	}

	public string? GetCellContent(int row, int column)
	{
		return Rows[row][column].ToString();
	}

	public int GetCellCount(int row)
	{
		throw new NotImplementedException();
	}

	public int GetCellHeight(int row, int column)
	{
		throw new NotImplementedException();
	}

	public (int left, int right) GetCellHPadding(int row, int column)
	{
		throw new NotImplementedException();
	}

	public int GetCellWidth(int row, int column)
	{
		throw new NotImplementedException();
	}

	public string[] GetRowContent(int row)
	{
		var dataRow = Rows[row];
		return dataRow.ItemArray.Select(x => x?.ToString() ?? string.Empty).ToArray();
	}

	public override string ToString()
	{
		return base.ToString();
	}
}