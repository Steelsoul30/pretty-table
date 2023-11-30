//using PrettyTable.PrinterService;

//namespace PrettyTable.Models;

//public interface IGrid
//{
//	int RowCount { get; }
//	int ColumnCount { get; }
//	GridOptions GridOptions { get; }
//	/// <summary>
//	/// 
//	/// </summary>
//	/// <param name="row">zero based index</param>
//	/// <param name="column">zero based index</param>
//	/// <returns></returns>
//	int GetCellWidth(int row, int column);
//	int GetCellHeight(int row, int column);
//	string GetCellContent(int row, int column);
//	string[] GetRowContent(int row);

//	int GetCellCount(int row);
//	(int left, int right) GetCellHPadding(int row, int column);
//}

//public class Grid : IGrid
//{
//	private IList<Cell> _cells;
//	private IList<GridRow> _cells2d = new List<GridRow>();
//	public int RowCount { get; private set; } = 0;
//	public int ColumnCount { get; private set; } = 0;
//	public GridOptions GridOptions { get; }

//	public Grid(GridOptions? options = null)
//	{
//		GridOptions = options ?? new GridOptions();
//	}

//	internal void AddRow(GridRow row)
//	{
//		var length = row.Count;
//		ColumnCount = ColumnCount < length ? length : ColumnCount;
//		_cells2d.Add(row);
//		RowCount++;
//	}

//	public int GetCellWidth(int row, int column)
//	{
//		return _cells2d[row].GetCellWidth(column);
//	}

//	public int GetCellHeight(int row, int column)
//	{
//		return _cells2d[row].GetCellHeight(column);
//	}

//	public string GetCellContent(int row, int column)
//	{
//		return _cells2d[row].GetCellContent(column);
//	}

//	public string[] GetRowContent(int row)
//	{
//		var cellCount = _cells2d[row].Count;
//		var result = new string[cellCount];
//		for (var i = 0; i < cellCount; i++)
//		{
//			result[i] = _cells2d[row].GetCellContent(i);
//		}
//		return result;
//	}

//	public (int left, int right) GetCellHPadding(int row, int column)
//	{
//		var cell = _cells2d[row][column];
//		return cell == null ? (0, 0) : (cell.HorizontalPaddingLeft, cell.HorizontalPaddingRight);
//	}

//	public int GetCellCount(int row)
//	{
//		return _cells2d[row].Count;
//	}

//	internal void Normalize()
//	{
//		if (GridOptions.AddEmptyCellsAsNeeded)
//		{
//			foreach (var row in _cells2d)
//			{
//				if (row.Count < ColumnCount)
//				{
//					row.AddRange(Enumerable.Repeat(0, ColumnCount - row.Count).Select(x => new Cell("")));
//				}
//			}
//		}

//		if (GridOptions.EqualizeCellWidthInColumn)
//		{
//			for (var i = 0; i < ColumnCount; i++)
//			{
//				var cells = new List<Cell>(RowCount);
//				foreach (var row in _cells2d)
//				{
//					var cell = row[i];
//					if (cell == null) continue;
//					cells.Add(cell);
//				}

//				var maxWidth = cells.Max(c => c.Width);
//				cells.ForEach(c => c.SetTotalWidth(maxWidth));
//			}
//		}
//	}

//	public override string ToString()
//	{
//		Normalize();
//		return this.Stringify();
//	}
//}

//public static class GridExtensions
//{
//	public static Grid AddRow(this Grid self, IEnumerable<string> row)
//	{
//		var gridRow = new GridRow();
//		foreach (var item in row)
//		{
//			gridRow.Add(new Cell(item));
//		}
//		self.AddRow(gridRow);
//		return self;
//	}

//	public static Grid Normalize(this Grid self)
//	{
//		self.Normalize();
//		return self;
//	}

//	public static string Stringify(this IGrid grid)
//	{
//		return new Printer(grid).Print();
//	}
//}

//public class GridOptions
//{
//	public const bool AddEmptyCellsAsNeeded = true;
//	public const bool EqualizeCellWidthInColumn = true;
//}