using ECS;
using UnityEngine;

public class ECSInputCmpt : ECSComponent, IUpdate
{
    #region 属性

    private float m_Speed = 3f;
    float m_TurnAmount;
    float m_ForwardAmount;
    float m_MovingTurnSpeed = 360;
    float m_StationaryTurnSpeed = 180;

    public Transform MainCameraTrans;
    private Vector3 m_CamForward;
    private Vector3 Move;
    #endregion

    public override void Awake()
    {
        MainCameraTrans = Camera.main.transform;
    }

    #region 输入
    public float Horizontal { private set; get; }
    public float Vertical { private set; get; }
    public bool Splash { private set; get; }
    public bool Jump { private set; get; }
    #endregion


    public void Update()
    {
        Splash = Input.GetKey(KeyCode.LeftShift);

        if (Splash)
        {
            Horizontal = InputSys.GetAxis("Horizontal");
        }
        else
        {
            Horizontal = InputSys.GetAxis("Horizontal") / 2;
        }

        if (Splash)
        {
            Vertical = InputSys.GetAxis("Vertical");
        }
        else
        {
            Vertical = InputSys.GetAxis("Vertical") / 2;
        }

        Jump = Input.GetKeyDown(KeyCode.Space);

        var speed = Time.deltaTime * m_Speed;

        if (MainCameraTrans != null)
        {
            m_CamForward = Vector3.Scale(MainCameraTrans.forward, new Vector3(1, 0, 1)).normalized;

            Move = Vertical * m_CamForward * speed + Horizontal * MainCameraTrans.right * speed;
        }
        else
        {
            Move = new Vector3(Horizontal * speed, 0, Vertical * speed);
        }

        if (Move.magnitude > 1f)
        {
            Move.Normalize();
        }

        ECSEventHelper.Send(Entity,ActorEventDefine.ActorMove, Move);
    }
}
