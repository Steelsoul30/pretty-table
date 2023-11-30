namespace PrettyTable.Models;

public class GridOptions
{
    public const bool AddEmptyCellsAsNeeded = true;
    public const bool EqualizeCellWidthInColumn = true;
    public bool WithHeaders { get; set; } = false;
}