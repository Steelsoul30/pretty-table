using PrettyTable.Models;

namespace PrettyTable.Interfaces;

public interface IGrid
{
    int RowCount { get; }
    int ColumnCount { get; }
    GridOptions GridOptions { get; }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="row">zero based index</param>
    /// <param name="column">zero based index</param>
    /// <returns></returns>
    int GetCellWidth(int row, int column);
    int GetCellHeight(int row, int column);
    string GetCellContent(int row, int column);
    string[] GetRowContent(int row);
    string[] GetHeaders();

    int GetCellCount(int row);
    (int left, int right) GetCellHPadding(int row, int column);
}