using System;
using System.Collections;
using Hiraishin.ObserverPattern;
using Hiraishin.Utilities;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;
using WebSocketSharp;
using Random = UnityEngine.Random;

public class GameController : BaseSingleton<GameController>
{
    #region Component Configs

    [SerializeField] private Turn currentTurn;
    
    private Player player;
    private Bot bot;
    private WaitForSeconds turnDelay = new WaitForSeconds(0.5f);
    private WaitForSeconds gameDelay = new WaitForSeconds(1f);
    private WebSocket webSocket;
    private bool isConnected;
    private Match match;

    #endregion

    private void Start()
    {
        try
        {
            webSocket = new WebSocket("ws://localhost:8080");
            webSocket.OnOpen += (sender, args) =>
            {
                isConnected = true;
            };
            webSocket.OnMessage += (sender, message) =>
            {
                Debug.Log("OnMessage!");
                Debug.Log("url " + ((WebSocket)sender).Url + " " + message.Data);
                SetBotData(message.Data);
            };
            webSocket.Connect();
            player = Player.Instance;
            bot = Bot.Instance;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    private IEnumerator BotTurnDelay()
    {
        yield return turnDelay;
        bot.Move();
    }

    private IEnumerator EndGameDelay(int resultValue)
    {
        yield return gameDelay;
        this.PostEvent(EventID.EndGame);
        if (resultValue == 0)
        {
            GameplayHUD.Instance.ShowDrawResult();
            yield break;
        }

        GameplayHUD.Instance.ShowResult(resultValue == 1);
    }

    private IEnumerator StartGameDelay()
    {
        yield return gameDelay;
        player.InputSystem.State = SystemState.Enable;
        GameplayHUD.Instance.SetBotData();
    }

    private void SetBotData(string data)
    {
        match = JsonConvert.DeserializeObject<Match>(data);
        bot.SetDataFromRemote(match);
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
                StartCoroutine(BotTurnDelay());
                break;
        }
    }

    public void SetBotLevel()
    {
        if (!isConnected)
        {
            BotLevel level = (BotLevel)Random.Range(0, 3);
            bot.SetLevel(level);
            StartCoroutine(StartGameDelay());
        }
        else
        {
            match = new Match()
            {
                packageType = "req/match",
                msgId = (byte)MessageId.RequestOpponent
            };
            webSocket.Send(JsonConvert.SerializeObject(match));
            StartCoroutine(StartGameDelay());
        }

    }

    public void EndGame(int resultValue)
    {
        currentTurn = Turn.Player;
        player.InputSystem.State = SystemState.Disable;
        StartCoroutine(EndGameDelay(resultValue));
    }
}
