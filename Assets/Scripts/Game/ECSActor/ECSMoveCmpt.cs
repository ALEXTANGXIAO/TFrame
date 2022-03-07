using System.Collections;
using System.Collections.Generic;
using ECS;
using UnityEngine;

public class ECSMoveCmpt : ECSComponent, IUpdate
{
    public ECSInputCmpt EcsInputComponent;
    public ECSGameObjectCmpt EcsGameObject;
    public Transform MainCameraTrans;
    private Vector3 m_CamForward;
    private Vector3 Move;
    float m_TurnAmount;
    float m_ForwardAmount;
    float m_MovingTurnSpeed = 360;
    float m_StationaryTurnSpeed = 180;
    float Speed = 3f;
    Rigidbody m_Rigidbody;
    public override void Awake()
    {
        base.Awake();

        EcsInputComponent = Entity.GetComponent<ECSInputCmpt>();

        EcsGameObject = Entity.GetComponent<ECSGameObjectCmpt>();

        MainCameraTrans = Camera.main.transform;

        m_Rigidbody = EcsGameObject.gameObject.GetComponent<Rigidbody>();

        Entity.Event.AddEventListener<Vector3>(ActorEventDefine.ActorMove, ActorMove);
    }

    public void ActorMove(Vector3 vector3)
    {
        Debug.Log(vector3);
    }

    public override void OnDestroy()
    {
        EcsInputComponent = null;

        EcsGameObject = null;

        MainCameraTrans = null;

        m_Rigidbody = null;
    }

    public void Update()
    {
        if (EcsInputComponent == null || EcsGameObject == null)
        {
            Debug.Log(EcsInputComponent);
            Debug.Log(EcsGameObject);
            return;
        }

        var speed = Time.deltaTime * Speed;
        if (MainCameraTrans != null)
        {
            m_CamForward = Vector3.Scale(MainCameraTrans.forward, new Vector3(1, 0, 1)).normalized;
            Move = EcsInputComponent.Vertical * m_CamForward * speed + EcsInputComponent.Horizontal * MainCameraTrans.right * speed;
        }
        else
        {
            Move = new Vector3(EcsInputComponent.Horizontal * speed, 0, EcsInputComponent.Vertical * speed);
        }

        if (Move.magnitude > 1f)
        {
            Move.Normalize();
        }
        Move = EcsGameObject.transform.InverseTransformDirection(Move);
        Move = Vector3.ProjectOnPlane(Move, Vector3.up);

        m_TurnAmount = Mathf.Atan2(Move.x, Move.z);
        m_ForwardAmount = Move.z;

        EcsGameObject.transform.Translate(Move);

        ApplyExtraTurnRotation();
    }

    void ApplyExtraTurnRotation()
    {
        float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
        EcsGameObject.transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
    }
}
