using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

#region Enums

public enum SystemState : byte
{
    Disable,
    Enable
}

public enum BotLevel : byte
{
    Easy,
    Medium,
    Hard
}

public enum Turn : byte
{
    Player,
    Bot
}

public enum MarkType : byte
{
    Leaf,
    Star
}
#endregion

#region Classes

public static class Utilities
{
    private static Vector3 BUTTON_CLICK_FEEDBACK_STRENGH = new Vector3(0.2f, 0.2f, 0.2f);
    private const float BUTTON_CLICK_FEEDBACK_DURATION = 0.15f;
    private const float ZOOM_POPUP_DURATION = 0.25f;
    public static Tweener PunchButtonFeedback(this Button btn, Action callback = null)
    {
        return btn.transform.DOPunchScale(BUTTON_CLICK_FEEDBACK_STRENGH, BUTTON_CLICK_FEEDBACK_DURATION).OnComplete(() => callback?.Invoke());
    }

}

#endregion