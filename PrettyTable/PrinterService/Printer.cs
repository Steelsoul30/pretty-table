using System.Text;
using PrettyTable.Interfaces;

namespace PrettyTable.PrinterService;

internal class Printer
{
	private IGrid _grid;
	private static StringBuilder _sb = new();
	private static readonly SeparatorLine _topLine = new((char)BoxCharacters.DownRight, (char)BoxCharacters.DownHorizontal, (char)BoxCharacters.DownLeft);
	private static readonly SeparatorLine _midLine = new((char)BoxCharacters.VerticalRight, (char)BoxCharacters.VerticalHorizontal, (char)BoxCharacters.VerticalLeft);
	private static readonly SeparatorLine _bottomLine = new((char)BoxCharacters.UpRight, (char)BoxCharacters.UpHorizontal, (char)BoxCharacters.UpLeft);

	public Printer(IGrid grid)
	{
		_grid = grid;
	}

	public string Print()
	{
		_sb = new StringBuilder();
		for (var i = 0; i < _grid.RowCount; i++)
		{
			PrintRow(i);
		}

		return _sb.ToString();
	}

	private void PrintRow(int row)
	{
		var isFirstRow = row == 0;
		var isLastRow = row == _grid!.RowCount - 1;
		var cellCount = _grid.GetCellCount(row);
		if (cellCount == 0) return;
		if (isFirstRow)
			PrintSeparator(cellCount, _topLine);
		else
			PrintSeparator(cellCount, _midLine);
		// main body
		var height = _grid.GetCellHeight(row, 0); // assuming all cells are the same height
		for (var i = 0; i < height; i++)
		{
			if (i == height / 2) // Content line
			{
				PrintLine(row, _grid.GetRowContent(row));
			}
			else
				PrintLine(row);
		}
		//end main body
		if (isLastRow)
			PrintSeparator(cellCount, _bottomLine);
	}

	private void PrintSeparator(int cellCount, SeparatorLine separator)
	{
		_sb.Append(separator.First);
		for (var i = 0; i < cellCount; i++)
		{
			var cellSize = _grid!.GetCellWidth(0, i);
			_sb.Append((char)BoxCharacters.Horizontal, cellSize);
			if (i < cellCount - 1)
			{
				_sb.Append(separator.Middle);
			}
		}
		_sb.Append(separator.Last).AppendLine();
	}

	private void PrintLine(int row, string[]? content = null)
	{
		var cellCount = _grid!.GetCellCount(row);
		_sb.Append((char)BoxCharacters.Vertical);
		for (var i = 0; i < cellCount; i++)
		{
			var cellSize = _grid!.GetCellWidth(row, i);
			var padding = _grid!.GetCellHPadding(row, i);
			if (content == null)
				_sb.Append(' ', cellSize);
			else
				_sb.Append(' ', padding.left).Append(content[i]).Append(' ', padding.right);
			if (i < cellCount - 1)
			{
				_sb.Append((char)BoxCharacters.Vertical);
			}
		}
		_sb.Append((char)BoxCharacters.Vertical).AppendLine();
	}

	private record SeparatorLine(char First, char Middle, char Last);
}