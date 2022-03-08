using System.Collections;
using System.Collections.Generic;
using ECS;
using GameProtocol;
using UnityEngine;

public class ActorOnlineCmpt : ECSComponent,IUpdate
{
    private Vector2 Move;
    private Vector3 Pos;
    private float dir;
    private GameObject gameObject;
    private ECSAnimatorCmpt ecsAnimatorCmpt;
    public override void Awake()
    {
        var actorEntity = Entity as ActorEntity;
        if (actorEntity != null)
        {
            gameObject = actorEntity.gameObject;
        }

        ecsAnimatorCmpt = Entity.GetComponent<ECSAnimatorCmpt>();
    }

    public void Update()
    {
        if (gameObject == null)
        {
            return;
        }

        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, Pos, 0.2f);

        gameObject.transform.eulerAngles = Vector3.Lerp(gameObject.transform.eulerAngles, new Vector3(0,dir,0), 0.2f);

        if (ecsAnimatorCmpt!=null)
        {
            ecsAnimatorCmpt.Move(Move);
        }
    }

    public void UpdatePosPack(PosPack posPack)
    {
        Debug.Log(posPack);

        Pos = new Vector3(posPack.PosX, posPack.PosY, posPack.PosZ);

        dir = posPack.Dirt;

        Move = new Vector2(posPack.RotaX, posPack.RotaY);
    }
}
