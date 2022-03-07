using System.Collections;
using System.Collections.Generic;
using ECS;
using UnityEngine;

public class GameApp : UnitySingleton<GameApp>
{
    public EcsGameSystem EcsGameSystem = new EcsGameSystem();
    public GameObject Player;

    void Start()
    {
        MonoManager.Instance.AddUpdateListener(EcsGameSystem.OnUpdate);

        var entity = EcsFactory.Instance.Create(Instantiate(Player));
        entity.AddComponent<ECSInputCmpt>();
        entity.AddComponent<ECSMoveCmpt>();
        entity.AddComponent<ECSAnimatorCmpt>();
        Debug.Log(entity.ToString());
        

    }
}