using System.Data;
using System.Security.Cryptography;
using PrettyTable.PrinterService;

namespace PrettyTable.Models;

public class Table : DataTable, IWithRow, IWithColumn, IGrid
{
	public Options Options { get; }
	public int RowCount => Rows.Count;
	public int ColumnCount => Columns.Count;

	private List<string>? _headers;

	public Table(Options? options = null)
	{
		Options = options ?? new Options();
	}

	public IWithRow AddRow(IEnumerable<string> rowData)
	{
		var data = rowData.ToList();
		var diff = data.Count - ColumnCount;
		switch (diff)
		{
			case > 0: // more data than columns
				for (var i = 0; i < diff; i++)
				{
					Columns.Add(null, typeof(Cell), string.Empty);
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
			row[i] = new Cell(data[i]);
		}

		Rows.Add(row);
		return this;
	}

	public IWithColumn AddColumn(IEnumerable<string> columnData)
	{
		var data = columnData.ToList();
		var diff = data.Count - RowCount;
		switch (diff)
		{
			case > 0: // more data than columns
				for (var i = 0; i < diff; i++)
				{
					AddRow(Enumerable.Repeat(string.Empty, ColumnCount));
				}
				break;
			case < 0: // less data than columns
				for (var i = 0; i < -diff; i++)
				{
					data.Add(string.Empty);
				}
				break;
		}

		Columns.Add(null, typeof(Cell), string.Empty);
		for (var i = 0 ; i < RowCount ; i++)
		{
			Rows[i][ColumnCount - 1] = new Cell(data[i]);
		}
		return this;
	}

	public string GetCellContent(int row, int column)
	{
		return Rows[row][column].ToString() ?? string.Empty;
	}

	public int GetCellCount(int row)
	{
		return Rows[row].ItemArray.Length;
	}

	public int GetCellHeight(int row, int column)
	{
		var cell = Rows[row][column] as Cell;
		return cell?.Height ?? 0;
	}

	public (int left, int right) GetCellHPadding(int row, int column)
	{
		return Rows[row][column] is not Cell cell ? (0, 0) : (cell.HorizontalPaddingLeft, cell.HorizontalPaddingRight);
	}

	public int GetCellWidth(int row, int column)
	{
		var cell = Rows[row][column] as Cell;
		return cell?.Width ?? 0;
	}

	public string[] GetRowContent(int row)
	{
		var dataRow = Rows[row];
		return dataRow.ItemArray.Select(x => x?.ToString() ?? string.Empty).ToArray();
	}

	public override string ToString()
	{
		Normalize();
		return new Printer(this).Print();
	}

	private void Normalize()
	{
		if (Options.EqualizeCellWidthInColumn)
		{
			for (var i = 0; i < ColumnCount; i++)
			{
				var cells = new List<Cell>(RowCount);
				foreach (DataRow row in Rows)
				{
					if (row[i] is not Cell cell) continue;
					cells.Add(cell);
				}

				var maxWidth = cells.Max(c => c.Width);
				cells.ForEach(c => c.SetTotalWidth(maxWidth));
			}
		}
	}
}

public interface IWithRow
{
	public IWithRow AddRow(IEnumerable<string> rowData);
}

public interface IWithColumn
{
	public IWithColumn AddColumn(IEnumerable<string> colData);
}