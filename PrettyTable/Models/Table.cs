using System.Data;
using PrettyTable.Interfaces;
using PrettyTable.PrinterService;

namespace PrettyTable.Models;

public class Table : DataTable, IGrid
{
	public int RowCount => Rows.Count;
	public int ColumnCount => Columns.Count;

	private readonly GridOptions _gridOptions;
	private List<string>? _headers;

	public static Table FromDataTable(DataTable source, GridOptions? options = null)
	{
		var result = new Table(options);
		if (options?.WithHeaders == true)
		{
			result.AddHeaders(source.Columns.Cast<DataColumn>().Select(x => x.ColumnName));
		}
		foreach (DataRow sourceRow in source.Rows)
		{
			result.AddRow(sourceRow.ItemArray);
		}
		return result;
	}

	public Table(GridOptions? options = null)
	{
		_gridOptions = options ?? new GridOptions();
	}

    public Table AddHeaders(IEnumerable<string> headers)
    {
        _headers = headers.ToList();
        _gridOptions.WithHeaders = true;
        AddRow(_headers);
        if (RowCount != 1) // Headers wasn't the first row added
        {
	        var headerRow = Rows[RowCount - 1];
			var newHeaderRow = NewRow();
			newHeaderRow.ItemArray = (object[])headerRow.ItemArray.Clone();
	        Rows.Remove(headerRow);
	        Rows.InsertAt(newHeaderRow, 0);
		}
		return this;
    }

	public Table AddRow(IEnumerable<object?> rowData)
	{
		return AddRow(rowData.Select(x => x?.ToString() ?? string.Empty));
	}

	public Table AddRow(IEnumerable<string> rowData)
	{
		var data = rowData.ToList();
		var diff = data.Count - ColumnCount;
		switch (diff)
		{
			case > 0: // more data than columns
				for (var i = 0; i < diff; i++)
				{
                    var column = new DataColumn(null, typeof(Cell))
                    {
                        DefaultValue = new Cell(string.Empty)
                    };
                    Columns.Add(column);
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

	public Table AddColumn(IEnumerable<string> columnData)
	{
		var data = columnData.ToList();
		Columns.Add(null, typeof(Cell), string.Empty);
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

    public string[] GetHeaders()
    {
		if (!_gridOptions.WithHeaders) return Array.Empty<string>();
        if (_headers != null) return _headers.ToArray();
        var result = new string[ColumnCount];
        for (var i = 0; i < ColumnCount; i++)
        {
			result[i] = Columns[i].ColumnName;
        }
        return result;
    }

	public override string ToString()
	{
		PrepareForPrinting();
		return new Printer(this).Print();
	}

	private void PrepareForPrinting()
	{
		if (GridOptions.EqualizeCellWidthInColumn)
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