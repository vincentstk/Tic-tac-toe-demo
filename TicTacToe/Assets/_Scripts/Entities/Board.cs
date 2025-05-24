using System;
using Hiraishin.Utilities;
using UnityEngine;

public class Board : BaseSingleton<Board>
{
    #region Component Configs

    private GridSystem _gridSystem;

    #endregion

    #region Properties

    public GridSystem GridSystem
    {
        get => _gridSystem;
    }

    #endregion

    protected override void OnAwake()
    {
        _gridSystem = GetComponent<GridSystem>();
    }

    private void Start()
    {
        _gridSystem.Init();
    }

    public Tile GetTile(Vector2 position)
    {
        return _gridSystem.GetTile(position);
    }
    public Tile GetTile(int x, int y)
    {
        return _gridSystem.GetTile(x, y);
    }

    public void CheckWin(Tile tile)
    {
        if (_gridSystem.CheckWin(tile.CurrentMark.Type))
        {
            switch (tile.CurrentMark.Type)
            {
                case MarkType.Leaf:
                    GameController.Instance.EndGame(1);
                    return;
                case MarkType.Star:
                    GameController.Instance.EndGame(-1);
                    return;
            }
        }
        else if (_gridSystem.IsBoardFull())
        {
            GameController.Instance.EndGame(0);
            return;
        }
        GameController.Instance.MoveDone();
    }
    public bool CheckWin(MarkType type)
    {
        if (_gridSystem.CheckWin(type))
        {
            switch (type)
            {
                case MarkType.Leaf:
                    return true;
                case MarkType.Star:
                    return true;
            }
        }
        return false;
    }
}
