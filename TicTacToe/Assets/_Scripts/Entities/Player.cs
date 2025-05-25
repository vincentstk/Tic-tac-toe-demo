using Hiraishin.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class Player : BaseSingleton<Player>
{
    #region Defines

    private const string LEAF_TAG = "leaf";
    private const string CLICK = "click";

    #endregion
    #region Component Configs

    [Inject] private ObjectPooling _poolerPrefab;
    private InputCallbackSystem _inputSystem;
    private AudioSystem _audioSystem;

    private Camera mainCam;
    
    #endregion

    #region Properties

    public InputCallbackSystem InputSystem
    {
        get => _inputSystem;
    }

    #endregion

    protected override void OnAwake()
    {
        _inputSystem = new InputCallbackSystem();
        _audioSystem = GetComponent<AudioSystem>();
        _inputSystem.OnClick += Choose;
        mainCam = Camera.main;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _audioSystem.Init();
    }

    private void Choose(Vector2 mousePosition)
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        Vector2 worldPosition = mainCam.ScreenToWorldPoint(mousePosition);
        Tile tile = Board.Instance.GetTile(worldPosition);
        if (!tile || tile.HasMark())
        {
            return;
        }
        GameObject leafObj = _poolerPrefab.SpawnObject(LEAF_TAG, tile.transform.position, Quaternion.identity);
        Mark mark = leafObj.GetComponent<Mark>();
        tile.SetMark(mark);
        _audioSystem.PlayAudio(CLICK);
    }
}
