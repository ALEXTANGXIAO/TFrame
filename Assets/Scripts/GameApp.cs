using System.Collections;
using System.Collections.Generic;
using ECS;
using UnityEngine;

sealed partial class GameApp : UnitySingleton<GameApp>
{
    public EcsGameSystem EcsGameSystem = new EcsGameSystem();
    public GameObject Player;
    public bool LocalGame;
    public Transform StartPown;
    public override void Awake()
    {
        base.Awake();

        Debug.Log(DevicePerformanceUtil.GetDevicePerformanceLevel());

        //DevicePerformanceUtil.ModifySettingsBasedOnPerformance();
        InitLibImp();
        RegistAllSystem();
        SetTargetFrameRate();
        InitGame();
    }

    private void InitGame()
    {
        MonoManager.Instance.AddUpdateListener(EcsGameSystem.OnUpdate);
        MonoManager.Instance.AddFixedUpdateListener(EcsGameSystem.OnFixedUpdate);

        if (!LocalGame)
        {
            UISys.Mgr.ShowWindow<LoginMainUI>();
        }
        else
        {
            var entity = EcsFactory.Instance.CreateActorEntity(ActorType.PlayerActor, Instantiate(Player), true);
            if (GameApp.Instance.StartPown != null)
            {
                entity.gameObject.transform.position = new Vector3(0,4, 0);
            }
            entity.AddComponent<ECSShaderCmpt>();
            entity.AddComponent<ECSInputCmpt>();
            entity.AddComponent<ECSMoveCmpt>();
            entity.AddComponent<ECSAnimatorCmpt>();
            Debug.Log(entity.ToString());

            //entity.Event.Send(ActorEventDefine.SetBodyMaterial, ResMgr.Instance.Load<Material>("Dino/Materials/Dino_Color/Dino_03"));

#if UNITY_ANDROID
            UISys.Mgr.ShowWindow<JoyStickUI>();
#endif
        }

        //StartCoroutine(Test(entity));
    }

    IEnumerator Test(Entity entity)
    {
        yield return new WaitForSeconds(3.0f);
        ECSObject.Destroy(entity);
    }
}