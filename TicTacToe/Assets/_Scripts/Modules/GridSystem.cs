using Hiraishin.Utilities;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    #region Component Configs

    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float cellSize;
    [SerializeField] private Vector3 origin;
    [SerializeField] private Tile tile;

    private Grid<Tile> grid;

    #endregion

    #region Properties

    public int Width
    {
        get => width;
    }

    public int Height
    {
        get => height;
    }

    #endregion

    public void Init()
    {
        grid = new Grid<Tile>(width, height, cellSize, origin, GridType.Grid2D, true);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Tile tileObj = Instantiate(tile, grid.GetCenterPositionInWorld(x, y), Quaternion.identity);
                tileObj.X = x;
                tileObj.Y = y;
                tileObj.transform.localScale = Vector3.one * cellSize;
                grid.SetValue(x, y, tileObj);
            }
        }
    }

    public bool IsBoardFull()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Tile tileObj = grid.GetValue(x, y);
                if (!tileObj.HasMark())
                {
                    return false;
                }
            }
        }
        return true;
    }
    public Tile GetTile(Vector2 position)
    {
        return grid.GetValue(position);
    }

    public Tile GetTile(int x, int y)
    {
        return grid.GetValue(x, y);
    }
    public bool CheckWin(MarkType type)
    {
        // Check columns
        for (int x = 0; x < width; x++)
        {
            if (grid.GetValue(x, 0).CurrentMark?.Type == type && grid.GetValue(x, 1).CurrentMark?.Type == type && grid.GetValue(x, 2).CurrentMark?.Type == type) 
                return true;
        }
        // Check rows
        for (int y = 0; y < 3; y++)
        {
            if (grid.GetValue(0, y).CurrentMark?.Type == type && grid.GetValue(1, y).CurrentMark?.Type == type && grid.GetValue(2, y).CurrentMark?.Type == type) 
                return true;
        }
        // Check diagonals
        int index = 0;
        for (int x = 0; x < width; x++)
        {
            if (grid.GetValue(index, index).CurrentMark?.Type != type)
                break;
            index++;
        }

        if (index == width)
            return true;
        index = 0;
        for (int i = 0; i < width; i++)
        {
            if (grid.GetValue(index, width - index - 1).CurrentMark?.Type != type)
                break;
            index++;
        }
        if (index == width) 
            return true;
        return false;
    }
}
