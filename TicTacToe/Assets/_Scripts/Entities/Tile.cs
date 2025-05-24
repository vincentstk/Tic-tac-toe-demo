using System;
using Hiraishin.ObserverPattern;
using UnityEngine;

public class Tile : MonoBehaviour
{
    #region Component Configs
    
    private Mark currentMark;

    private Action<object> OnEndGame;

    #endregion

    #region Properties

    public int X { get; set; }
    public int Y { get; set; }

    public Mark CurrentMark
    {
        get => currentMark;
    }

    #endregion

    private void Start()
    {
        OnEndGame = (param) => EndGame();
        this.RegisterListener(EventID.EndGame, OnEndGame);
    }

    private void EndGame()
    {
        SetMarkNonCheck(null);
    }

    public bool HasMark()
    {
        return currentMark;
    }
    public void SetMark(Mark mark)
    {
        currentMark = mark;
        Board.Instance.CheckWin(this);
    }
    public void SetMarkNonCheck(Mark mark)
    {
        currentMark = mark;
    }
}
