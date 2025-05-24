using Hiraishin.Utilities;
using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private GameObject poolerPrefab;
    
    public override void InstallBindings()
    {
        Container.Bind<ObjectPooling>().FromComponentInNewPrefab(poolerPrefab).AsSingle();
    }
}
