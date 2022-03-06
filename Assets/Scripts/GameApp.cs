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
        var entity = EcsGameSystem.Create<Entity>();
        ECSActor actor = entity.AddComponent<ECSActor>();
        actor.Name = typeof(ECSActor).ToString();
        actor.gameObject = Instantiate(Player);
        actor.transform = actor.gameObject.GetComponent<Transform>();
        entity.AddComponent<ECSInputComponent>();
        entity.AddComponent<ECSMoveComponent>();
        entity.AddComponent<ECSAnimatorCmpt>();
        entity.CheckDebugInfo(actor.gameObject);
        Debug.Log(entity.ToString());
    }

    void Update()
    {
        EcsGameSystem.OnUpdate();
    }
}

public class EcsGameSystem : ECSSystem
{
    public void OnUpdate()
    {
        Update();
    }
}