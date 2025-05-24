using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class DecideSystem : MonoBehaviour
{
    #region Component Configs

    [SerializeField] private SystemState state;
    [SerializeField] private BotLevel level;
    [SerializeField] private Mark leaf;
    [SerializeField] private Mark star;

    private Board board;
    #endregion

    #region Properties

    public SystemState State
    {
        get => state;
    }

    #endregion
    public void Init()
    {
        board = Board.Instance;
    }

    private Tile EasyMove()
    {
        Tile tile = null;
        int x = Random.Range(0, board.GridSystem.Width);
        int y = Random.Range(0, board.GridSystem.Height);
        tile = board.GridSystem.GetTile(x, y);
        while (tile.HasMark())
        {
            if (board.GridSystem.IsBoardFull())
            {
                break;
            }
            x = Random.Range(0, board.GridSystem.Width);
            y = Random.Range(0, board.GridSystem.Height);
            tile = board.GridSystem.GetTile(x, y);
        }

        return tile;
    }

    private Tile MediumMove()
    {
		float ratio = Random.Range(0f, 1f);
        if (ratio <= 0.5f)
            return EasyMove();
        return HardMove();
    }
    private Tile HardMove()
    {
        (int row, int col) tilePosition = FindBestMove();
        return board.GetTile(tilePosition.row, tilePosition.col);
    }
    private (int row, int col) FindBestMove()
    {
        int bestRow = -1;
        int bestCol = -1;
        int bestValue = int.MinValue;
        int alpha = int.MinValue;
        int beta = int.MaxValue;

        for (int x = 0; x < board.GridSystem.Width; x++)
        {
            for (int y = 0; y < board.GridSystem.Height; y++)
            {
                if (board.GridSystem.GetTile(x, y).HasMark()) 
                    continue;
                board.GridSystem.GetTile(x, y).SetMarkNonCheck(star);
                int moveValue = Minimax(0, false, alpha, beta);
                board.GridSystem.GetTile(x, y).SetMarkNonCheck(null);
                if (moveValue > bestValue)
                {
                    bestValue = moveValue;
                    bestRow = x;
                    bestCol = y;
                }

            }
        }
        return (bestRow, bestCol);
    }

    private int Minimax(int depth, bool isMaximizing, int alpha, int beta)
    {
        if (board.CheckWin(MarkType.Star))
        {
            return 1;
        }
        if (board.CheckWin(MarkType.Leaf))
        {
            return -1;
        }
        if (board.GridSystem.IsBoardFull())
        {
            return 0;
        }

        if (isMaximizing)
        {
            int bestValue = int.MinValue;
            for (int x = 0; x < board.GridSystem.Width; x++)
            {
                for (int y = 0; y < board.GridSystem.Height; y++)
                {
                    if (board.GridSystem.GetTile(x, y).HasMark()) 
                        continue;
                    board.GridSystem.GetTile(x, y).SetMarkNonCheck(star);
                    int value = Minimax(depth + 1, false, alpha, beta);
                    board.GridSystem.GetTile(x, y).SetMarkNonCheck(null);
                    bestValue = Mathf.Max(bestValue, value);
                    alpha = Mathf.Max(alpha, bestValue);
                    if (beta <= alpha)
                        break;
                }
            }
            return bestValue;
        }
        else
        {
            int bestValue = int.MaxValue;
            for (int x = 0; x < board.GridSystem.Width; x++)
            {
                for (int y = 0; y < board.GridSystem.Height; y++)
                {
                    if (board.GridSystem.GetTile(x, y).HasMark()) 
                        continue;
                    board.GridSystem.GetTile(x, y).SetMarkNonCheck(leaf);
                    int value = Minimax(depth + 1, true, alpha, beta);
                    board.GridSystem.GetTile(x, y).SetMarkNonCheck(null);
                    bestValue = Mathf.Min(bestValue, value);
                    beta = Mathf.Min(beta, bestValue);
                    if (beta <= alpha)
                        break;
                }
            }
            return bestValue;
        }
    }
    
    
    public Tile ChooseMove()
    {
        switch (level)
        {
            case BotLevel.Easy:
                return EasyMove();
            case BotLevel.Medium:
                return MediumMove();
            case BotLevel.Hard:
                return HardMove();
        }
        return null;
    }

    public void SetLevel(BotLevel newLevel)
    {
        level = newLevel;
        state = SystemState.Enable;
    }
}
