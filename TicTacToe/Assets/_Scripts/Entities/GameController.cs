using System;
using System.Collections;
using Hiraishin.ObserverPattern;
using Hiraishin.Utilities;
using UnityEngine;

public class GameController : BaseSingleton<GameController>
{
    #region Component Configs

    [SerializeField] private Turn currentTurn;
    
    private Player player;
    private Bot bot;

    #endregion

    private void Start()
    {
        player = Player.Instance;
        bot = Bot.Instance;
    }
    
    public void MoveDone()
    {
        currentTurn = currentTurn == Turn.Player ? Turn.Bot : Turn.Player;
        switch (currentTurn)
        {
            case Turn.Player:
                player.InputSystem.State = SystemState.Enable;
                break;
            case Turn.Bot:
                player.InputSystem.State = SystemState.Disable;
                bot.Move();
                break;
        }
    }

    public void SetBotLevel(BotLevel level)
    {
        bot.SetLevel(level);
        player.InputSystem.State = SystemState.Enable;
    }

    public void EndGame(int resultValue)
    {
        player.InputSystem.State = SystemState.Disable;
        this.PostEvent(EventID.EndGame);
        if (resultValue == 0)
        {
            GameplayHUD.Instance.ShowDrawResult();
            return;
        }

        GameplayHUD.Instance.ShowResult(resultValue == 1);
    }
}
