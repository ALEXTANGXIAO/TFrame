using System;
using ECS;
using UnityEngine;

class ActorPosCmpt : ECSComponent,IUpdate
{
    private Rigidbody rootRigidbody;

    private GameObject gameObject;

    private GameTimerTick m_gameTimerTick;

    public override void Awake()
    {
        var actorEntity = Entity as ActorEntity;
        if (actorEntity!= null)
        {
            gameObject = actorEntity.gameObject;
        }
        m_gameTimerTick = new GameTimerTick(0.1f, OnTick);

        RegisterEvent();
    }

    private void RegisterEvent()
    {
        Entity.Event.AddEventListener<Vector2>(ActorEventDefine.ActorVector2, ActorVector2);
    }

    private Vector2 MoveInput;
    public void ActorVector2(Vector2 vector2)
    {
        MoveInput = vector2;
    }

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
    }

    public void OnTick()
    {
        if (gameObject == null)
        {
            return;
        }

        Vector3 pos = gameObject.transform.position;

        var dir = gameObject.transform.eulerAngles.y;//ctrl.Root.transform.eulerAngles;

        ActorDataMgr.Instance.UpCachePosReq(pos, dir,MoveInput);
    }
}