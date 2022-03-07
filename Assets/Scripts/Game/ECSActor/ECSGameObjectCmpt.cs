using System.Collections;
using System.Collections.Generic;
using ECS;
using UnityEngine;

public class ECSGameObjectCmpt : ECSComponent
{
    public string Name;
    public UnityEngine.GameObject gameObject { get; private set; }
    public UnityEngine.Transform transform { get; private set; }
    public uint ActorId { get; private set; }

    public void BindCmpt(GameObject obj,uint actorId)
    {
        this.gameObject = obj;
        this.transform = gameObject.transform;
        this.ActorId = actorId;
    }

    public override void OnDestroy()
    {
        Object.Destroy(gameObject);
        this.gameObject = null;
        this.transform = null;
        this.ActorId = 0;
    }
}
