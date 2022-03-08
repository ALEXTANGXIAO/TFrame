using System.Collections.Generic;
using ECS;
using UnityEngine;

public class EcsFactory : BaseLogicSys<EcsFactory>
{
    public EcsGameSystem EcsGameSys;
    private uint m_currentActorId = 1000;
    private Dictionary<uint, Entity> m_entityDic = new Dictionary<uint, Entity>();
    public Dictionary<uint, Entity> EntityDic => m_entityDic;

    public GameObject EntityRoot;
    public EcsFactory()
    {
        EcsGameSys = GameApp.Instance.EcsGameSystem;
        InitActorRoot();
    }

    private void InitActorRoot()
    {
        EntityRoot = new GameObject();
        EntityRoot.transform.position = Vector3.zero;
        EntityRoot.name = "EntityRoot";
        Object.DontDestroyOnLoad(EntityRoot);
    }

    public ActorEntity CreateActorEntity(ActorType actorType, GameObject gameObject,bool isMainPlayer = false)
    {
        ActorEntity entity = null;

        switch (actorType)
        {
            case ActorType.PlayerActor:
            {
                entity = EcsGameSys.Create<PlayerActor>();
                    break;
            }
            case ActorType.MonsterActor:
            {
                entity = EcsGameSys.Create<MonsterActor>();
                    break;
            }
        }

        if (entity == null)
        {
            return null;
        }

        if (isMainPlayer)
        {
            gameObject.tag = "Player";
        }

        entity.Bind(m_currentActorId, gameObject);
        gameObject.transform.SetParent(EntityRoot.transform);
        entity.AddComponent<ECSEventCmpt>();
        m_entityDic.Add(m_currentActorId, entity);
        m_currentActorId++;
#if UNITY_EDITOR
        entity.CheckDebugInfo(gameObject);
#endif
        return entity;
    }

    public void DestroyActorEntity(ActorEntity entity)
    {
        var elm = m_entityDic.GetEnumerator();
        while (elm.MoveNext())
        {
            if (elm.Current.Value == entity)
            {
                m_entityDic.Remove(elm.Current.Key);
                return;
            }
        }
        ECSObject.Destroy(entity);
    }

    public Entity Create()
    {
        var entity = EcsGameSys.Create<Entity>();
        m_entityDic.Add(m_currentActorId, entity);
        m_currentActorId++;
        return entity;
    }

    public void Destroy(Entity entity)
    {
        ECSObject.Destroy(entity);
        var elm = m_entityDic.GetEnumerator();
        while (elm.MoveNext())
        {
            if (elm.Current.Value == entity)
            {
                m_entityDic.Remove(elm.Current.Key);
                return;
            }
        }
    }

    public void Destroy(uint entityId)
    {
        if (m_entityDic.TryGetValue(entityId, out Entity entity))
        {
            ECSObject.Destroy(entity);
        }
    }
}