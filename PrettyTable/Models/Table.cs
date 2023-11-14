namespace PrettyTable.Models;

public class Table : Grid
{
	private IList<Cell> _headers;

	internal Table(GridOptions options) : base(options)
	{

	}
}