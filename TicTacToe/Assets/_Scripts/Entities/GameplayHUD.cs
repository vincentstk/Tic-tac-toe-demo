using System;
using System.Collections;
using Hiraishin.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameplayHUD : BaseSingleton<GameplayHUD>
{
    #region Defines

    private const string VICTORY = "VICTORY";
    private const string DEFEAT = "DEFEAT";
    private const string DRAW = "DRAW";

    #endregion
    #region Component Configs

    [SerializeField] private Color victoryColor;
    [SerializeField] private Color defeatColor;
    [SerializeField] private Color drawColor;

    #endregion
    #region UI Elements

    [SerializeField] private CanvasGroup cvsgrSetOpponent;
    [SerializeField] private CanvasGroup cvsgrResult;

    [SerializeField] private TextMeshProUGUI txtResult;
    [SerializeField] private TextMeshProUGUI txtname;
    [SerializeField] private TextMeshProUGUI txtrank;

    #endregion

    private void ShowPopup(CanvasGroup popup, bool isShow)
    {
        popup.alpha = isShow ? 1 : 0;
        popup.blocksRaycasts = isShow;
        popup.interactable = isShow;
    }

    private IEnumerator DelayGetBotData()
    {
        yield return new WaitForSeconds(2f);

    }

    public void ShowResult(bool isVictory)
    {
        txtResult.SetText(isVictory ? VICTORY : DEFEAT);
        txtResult.color = isVictory ? victoryColor : defeatColor;
        ShowPopup(cvsgrResult, true);
    }
    public void ShowDrawResult()
    {
        txtResult.SetText(DRAW);
        txtResult.color = drawColor;
        ShowPopup(cvsgrResult, true);
    }

    public void SetBotData()
    {
        txtname.SetText(Bot.Instance.BotName);
        txtrank.SetText(Bot.Instance.BotLevel.ToString());
    }
    
    #region UI Element Events

    public void OnClickSetOpponentLevel()
    {
        GameController.Instance.SetBotLevel();
        ShowPopup(cvsgrSetOpponent, false);
    }

    public void OnClickPlayAgain(Button btn)
    {
        btn.PunchButtonFeedback(() =>
        {
            ShowPopup(cvsgrResult, false);
            ShowPopup(cvsgrSetOpponent, true);
        });
    }
    public void OnClickQuit(Button btn)
    {
        btn.PunchButtonFeedback(() =>
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        });
    }

    #endregion
}
