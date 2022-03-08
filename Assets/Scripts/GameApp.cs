using System.Collections;
using System.Collections.Generic;
using ECS;
using UnityEngine;

sealed partial class GameApp : UnitySingleton<GameApp>
{
    public EcsGameSystem EcsGameSystem = new EcsGameSystem();
    public GameObject Player;

    public override void Awake()
    {
        base.Awake();
        InitLibImp();
        RegistAllSystem();
        SetTargetFrameRate();
        InitGame();
    }

    private void InitGame()
    {
        MonoManager.Instance.AddUpdateListener(EcsGameSystem.OnUpdate);
        MonoManager.Instance.AddFixedUpdateListener(EcsGameSystem.OnFixedUpdate);
        var entity = EcsFactory.Instance.CreateActorEntity(ActorType.PlayerActor,Instantiate(Player),true);
        entity.AddComponent<ECSInputCmpt>();
        entity.AddComponent<ECSMoveCmpt>();
        entity.AddComponent<ECSAnimatorCmpt>();
        Debug.Log(entity.ToString());
        //StartCoroutine(Test(entity));
    }

    IEnumerator Test(Entity entity)
    {
        yield return new WaitForSeconds(3.0f);
        ECSObject.Destroy(entity);
    }
}