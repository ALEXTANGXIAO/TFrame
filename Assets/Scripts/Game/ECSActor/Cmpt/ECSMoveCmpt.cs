﻿using System.Collections;
using System.Collections.Generic;
using ECS;
using UnityEngine;

public class ECSMoveCmpt : ECSComponent,IFixedUpdate
{
    private ActorEntity actorEntity;
    private Vector3 Move;
    float m_TurnAmount;
    float m_ForwardAmount;
    float m_MovingTurnSpeed = 360;
    float m_StationaryTurnSpeed = 180;
    Rigidbody m_Rigidbody;
    public override void Awake()
    {
        actorEntity = Entity as ActorEntity;

        if (actorEntity == null)
        {
            return;
        }

        m_Rigidbody = actorEntity.gameObject.GetComponent<Rigidbody>();

        Entity.Event.AddEventListener<Vector3>(ActorEventDefine.ActorMove, ActorMove);
    }

    public void ActorMove(Vector3 vector3)
    {
        Move = vector3;
    }

    public override void OnDestroy()
    {
        m_Rigidbody = null;

        Entity.Event?.RemoveEventListener<Vector3>(ActorEventDefine.ActorMove, ActorMove);
    }

    void ApplyExtraTurnRotation()
    {
        float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);

        actorEntity.transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
    }

    public void FixedUpdate()
    {
        Move = actorEntity.transform.InverseTransformDirection(Move);

        Move = Vector3.ProjectOnPlane(Move, Vector3.up);

        m_TurnAmount = Mathf.Atan2(Move.x, Move.z);

        m_ForwardAmount = Move.z;

        actorEntity.transform.Translate(Move);

        ApplyExtraTurnRotation();
    }
}