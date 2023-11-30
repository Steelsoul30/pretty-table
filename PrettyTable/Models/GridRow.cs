//namespace PrettyTable.Models;

//internal class GridRow
//{
//	private readonly List<Cell> _cells = new();
//	private readonly object _cellsLock = new();
//	public int Count => _cells.Count;
//	private int _maxVPadding;
//	public Cell? this[int index] => index < this.Count ? _cells[index] : null;

//	public void Add(Cell cell)
//	{
//		lock (_cellsLock)
//		{
//			_cells.Add(cell);
//			if (cell.VerticalPadding <= _maxVPadding) return;
//			_maxVPadding = cell.VerticalPadding;
//			EqualizeCellHeight();
//		}
//	}

//	public void AddRange(IEnumerable<Cell> cells)
//	{
//		lock (_cellsLock)
//		{
//			var collection = cells.ToList();
//			_cells.AddRange(collection);
//			var localMaxVPadding = collection.Max(c => c.VerticalPadding);
//			if (localMaxVPadding <= _maxVPadding) return;
//			_maxVPadding = localMaxVPadding;
//			EqualizeCellHeight();
//		}
//	}

//	public int GetWidth()
//	{
//		lock (_cellsLock)
//		{
//			return _cells.Sum(cell => cell.Width + 1) + 1;
//		}
//	}

//	public int GetCellWidth(int index)
//	{
//		return _cells[index].Width;
//	}

//	public int GetCellHeight(int index)
//	{
//		return _cells[index].Height;
//	}

//	public string GetCellContent(int index)
//	{
//		return _cells[index].Content;
//	}

//	private void EqualizeCellHeight()
//	{
//		lock (_cellsLock)
//		{
//			_cells.ForEach(c => c.VerticalPadding = _maxVPadding);
//		}

//	}
//}