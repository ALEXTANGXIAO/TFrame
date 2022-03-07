using System.Collections.Generic;
using ECS;
using UnityEngine;


public class EcsFactory : Singleton<EcsFactory>
{
    public EcsGameSystem EcsGameSys;
    private uint m_currentActorId = 1000;
    private Dictionary<uint, Entity> m_entityDic = new Dictionary<uint, Entity>();
    public GameObject EntityRoot;
    public EcsFactory()
    {
        EcsGameSys = GameApp.Instance.EcsGameSystem;
        EntityRoot = new GameObject();
        EntityRoot.transform.position = Vector3.zero;
        EntityRoot.name = "EntityRoot";
        Object.DontDestroyOnLoad(EntityRoot);
    }

    public Entity Create(GameObject gameObject)
    {
        var entity = EcsGameSys.Create<Entity>();
        entity.AddComponent<ECSGameObjectCmpt>().BindCmpt(gameObject, m_currentActorId);
        gameObject.transform.SetParent(EntityRoot.transform);
#if UNITY_EDITOR
        entity.CheckDebugInfo(gameObject);
#endif
        m_entityDic.Add(m_currentActorId, entity);
        m_currentActorId++;
        return entity;
    }

    public Entity Create()
    {
        return EcsGameSys.Create<Entity>();
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