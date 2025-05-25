using System;
using System.Collections;
using Hiraishin.Utilities;
using UnityEngine;
using Zenject;


public class Bot : BaseSingleton<Bot>
{
    #region Defines

    private const string STAR_TAG = "star";
    private const string CLICK = "click";

    #endregion
    #region Component Configs

    [SerializeField] private string botName;
    [SerializeField] private BotLevel botLevel;
    
    [Inject] private ObjectPooling _poolerPrefab;
    private DecideSystem _decideSystem;
    private AudioSystem _audioSystem;

    #endregion

    #region Properties

    public string BotName
    {
        get => botName;
    }

    public BotLevel BotLevel
    {
        get => botLevel;
    }

    #endregion

    protected override void OnAwake()
    {
        _decideSystem = GetComponent<DecideSystem>();
        _audioSystem = GetComponent<AudioSystem>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        _decideSystem.Init();
        _audioSystem.Init();
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
        _audioSystem.PlayAudio(CLICK);
    }

    public void SetLevel(BotLevel level)
    {
        _decideSystem.SetLevel(level);
    }

    public void SetDataFromRemote(Match match)
    {
        botName = match.botName;
        botLevel = match.botRank;
        _decideSystem.SetLevel(match.botRank);
    }
}
