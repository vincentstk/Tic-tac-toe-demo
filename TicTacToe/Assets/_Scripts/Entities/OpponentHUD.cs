using Hiraishin.Utilities;
using TMPro;
using UnityEngine;

public class OpponentHUD : BaseSingleton<OpponentHUD>
{
    #region UI Elements

    [SerializeField] private TextMeshProUGUI txtName;
    [SerializeField] private TextMeshProUGUI txtRank;

    #endregion
    public void SetBotData()
    {
        txtName.SetText(Bot.Instance.BotName);
        txtRank.SetText(Bot.Instance.BotLevel.ToString());
    }
}
