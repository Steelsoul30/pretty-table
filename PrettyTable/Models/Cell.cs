namespace PrettyTable.Models;

internal class Cell
{
	public int Width => Content.Length + HorizontalPaddingLeft + HorizontalPaddingRight;
	public int Height => VerticalPadding * 2 + 1;
	public string Content { get; }
	public int VerticalPadding { get; set; }
	public int HorizontalPaddingLeft { get; private set; }
	public int HorizontalPaddingRight { get; private set; }

	public Cell(string content, int verticalPadding = 1, int horizontalPaddingLeft = 1, int horizontalPaddingRight = 1)
	{
		Content = content;
		VerticalPadding = verticalPadding;
		HorizontalPaddingLeft = horizontalPaddingLeft;
		HorizontalPaddingRight = horizontalPaddingRight;
	}

	public void SetTotalWidth(int newWidth)
	{
		if (Width >= newWidth) return;
		var diff = newWidth - Width;
		do
		{
			HorizontalPaddingRight++;
			diff--;
			if (diff == 0) break;
			HorizontalPaddingLeft++;
			diff--;
		} while (diff > 0);
	}
}