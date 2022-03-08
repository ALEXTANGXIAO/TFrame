using System.Collections;
using System.Collections.Generic;
using ECS;
using GameProtocol;
using UnityEngine;

public class ActorOnlineCmpt : ECSComponent,IUpdate
{
    private Vector3 Pos;
    private float dir;
    private GameObject gameObject;

    public override void Awake()
    {
        var actorEntity = Entity as ActorEntity;
        if (actorEntity != null)
        {
            gameObject = actorEntity.gameObject;
        }
    }

    public void Update()
    {
        if (gameObject == null)
        {
            return;
        }

        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, Pos, 0.2f);
        gameObject.transform.eulerAngles = Vector3.Lerp(gameObject.transform.eulerAngles, new Vector3(0,dir,0), 0.2f);
    }

    public void UpdatePosPack(PosPack posPack)
    {
        Debug.Log(posPack);

        Pos = new Vector3(posPack.PosX, posPack.PosY, posPack.PosZ);

        dir = posPack.Dirt;
    }
}
