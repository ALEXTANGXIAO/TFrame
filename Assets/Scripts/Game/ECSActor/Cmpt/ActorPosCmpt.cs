using System;
using ECS;
using UnityEngine;

class ActorPosCmpt : ECSComponent,IUpdate
{
    private Rigidbody rootRigidbody;

    private GameObject gameObject;

    private GameTimerTick m_gameTimerTick;

    public ECSInputCmpt EcsInputComponent;

    public override void Awake()
    {
        var actorEntity = Entity as ActorEntity;
        if (actorEntity!= null)
        {
            gameObject = actorEntity.gameObject;
        }
        m_gameTimerTick = new GameTimerTick(0.01f, OnTick);

        EcsInputComponent = Entity.GetComponent<ECSInputCmpt>();
    }

    private Vector2 MoveInput;

    private int m_animation;
    private int Dirt;

    public void Update()
    {
        if (gameObject == null)
        {
            return;
        }

        if (m_gameTimerTick != null)
        {
            m_gameTimerTick.OnUpdate();
        }

        MoveInput = new Vector2(EcsInputComponent.Horizontal,EcsInputComponent.Vertical);
    }

    public void OnTick()
    {
        if (gameObject == null)
        {
            return;
        }

        Vector3 pos = gameObject.transform.position;

        var dir = gameObject.transform.eulerAngles.y;

        ActorDataMgr.Instance.UpCachePosReq(pos, dir,MoveInput,EcsInputComponent.Jump?1:0);
    }
}