using ECS;
using UnityEngine;

public class ECSInputCmpt : ECSComponent, IUpdate
{
    public float Horizontal { private set; get; }
    public float Vertical { private set; get; }
    public bool Splash { private set; get; }
    public bool Jump { private set; get; }
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

        ECSEventHelper.Send(Entity,ActorEventDefine.ActorMove,Vector3.up);
    }
}
