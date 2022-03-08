using ECS;
using UnityEngine;

public enum ActorType
{
    ActorNone,

    PlayerActor,

    MonsterActor,

    PetActor,

    DropActor,

    PointActor,

    MaxType,
}

public class ActorEntity : Entity
{
    public ActorType ActorType = ActorType.ActorNone;

    public string Name;
    public UnityEngine.GameObject gameObject { get; private set; }
    public UnityEngine.Transform transform { get; private set; }
    public uint ActorId { get; private set; }

    public void Bind(uint actorId,GameObject obj)
    {
        this.gameObject = obj;
        this.transform = gameObject.transform;
        this.ActorId = actorId;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        Object.Destroy(gameObject);
        this.gameObject = null;
        this.transform = null;
        this.ActorId = 0;
    }
}
