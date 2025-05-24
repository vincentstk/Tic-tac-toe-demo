using System;
using System.Collections;
using Hiraishin.Utilities;
using UnityEngine;
using Zenject;


public class Bot : BaseSingleton<Bot>
{
    #region Defines

    private const string STAR_TAG = "star";

    #endregion
    #region Component Configs

    [Inject] private ObjectPooling _poolerPrefab;
    private DecideSystem _decideSystem;

    #endregion

    protected override void OnAwake()
    {
        _decideSystem = GetComponent<DecideSystem>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        _decideSystem.Init();
    }

    public void Move()
    {
        Tile tile = _decideSystem.ChooseMove();
        if (!tile)
        {
            return;
        }
        GameObject starObj = _poolerPrefab.SpawnObject(STAR_TAG, tile.transform.position, Quaternion.identity);
        Mark mark = starObj.GetComponent<Mark>();
        tile.SetMark(mark);
    }

    public void SetLevel(BotLevel level)
    {
        _decideSystem.SetLevel(level);
    }
}
