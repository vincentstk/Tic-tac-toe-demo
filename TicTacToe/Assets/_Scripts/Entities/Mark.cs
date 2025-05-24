using System;
using Hiraishin.ObserverPattern;
using Hiraishin.Utilities;
using UnityEngine;
using Zenject;

public class Mark : MonoBehaviour
{
    #region Defines

    private const string LEAF = "leaf";
    private const string STAR = "star";

    #endregion
    #region Component Configs

    [SerializeField]
    private MarkType type;

    [Inject] private ObjectPooling _poolerPrefab;
    
    private Action<object> OnEndGame;

    #endregion

    #region Properties
    
    public MarkType Type
    {
        get => type;
    }

    #endregion
    private void Awake()
    {
        OnEndGame = (param) => EndGame();
        this.RegisterListener(EventID.EndGame, OnEndGame);
    }

    private void EndGame()
    {
        if (type == MarkType.Leaf)
        {
            _poolerPrefab.ReturnToPool(LEAF, gameObject, true);
            return;
        }
        _poolerPrefab.ReturnToPool(STAR, gameObject, true);
    }
}
