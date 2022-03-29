using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorMgr:Singleton<ActorMgr>
{
    private GameActor m_mainPlayer = null;
    private Dictionary<uint, GameActor> m_actorPool = new Dictionary<uint, GameActor>();
    private Dictionary<int, GameActor> m_goModelHash2Actor = new Dictionary<int, GameActor>();
    private List<GameActor> m_listActor = new List<GameActor>();

    public GameActor CreateGameActor(ActorType actorType, uint actorID, bool isMainPlayer)
    {
        GameActor ret = null;

        // 规则：真实视野优先级高
        GameActor actorSave;
        if (m_actorPool.TryGetValue(actorID, out actorSave))
        {
            var oldActor = m_actorPool[actorID];
            var oldActorType = oldActor.GetActorType();
            Debug.LogFormat("duplicate actor gid {0} {1} {2}", actorID, actorType, oldActorType);
            if (oldActorType != actorType)
            {
                DestroyActor(actorID);
                ret = CreateGameActor(actorType, actorID);
            }
            else
            {
                ret = m_actorPool[actorID];
            }
        }
        else
        {
            ret = CreateGameActor(actorType, actorID);
        }

        if (ret == null)
        {
            Debug.LogErrorFormat("create actor failed, type is {0}, id is {1}", actorType, actorID);
            return null;
        }

        if (isMainPlayer)
        {
            SetMainPlayer(ret);
        }

        return ret;
    }

    private GameActor CreateGameActor(ActorType actorType, uint actorID)
    {
        GameActor newActor = null;

        switch (actorType)
        {
            case ActorType.PlayerActor:
            {
                //newActor = new GamePlayer();
                break;
            }
            case ActorType.MonsterActor:
            {
                //newActor = new MonsterActor();
                break;
            }
            case ActorType.PetActor:
            {
                //newActor = new NamePlayer();
                break;
            }
            default:
            {
                Debug.LogErrorFormat("unknown actor type:{0}", actorType);
                break;
            }
        }

        if (newActor != null)
        {
            newActor.ActorID = actorID;
            m_actorPool.Add(actorID, newActor);
            m_listActor.Add(newActor);
        }

        return newActor;
    }


    #region DestroyActor
    public bool DestroyActor(GameActor actor)
    {
        return DestroyActor(actor.ActorID);
    }

    public bool DestroyActor(uint actorID)
    {
        return DestroyActor(actorID, ActorType.ActorNone);
    }

    private bool DestroyActor(uint actorID, ActorType actorTyep)
    {
        Debug.LogFormat("on destroy actor {0}", actorID);
        GameActor actor = null;
        if (m_actorPool.TryGetValue(actorID, out actor))
        {
            if (actorTyep != ActorType.ActorNone && actor.GetActorType() != actorTyep)
            {
                return false;
            }

            if (actor == m_mainPlayer)
            {
                SetMainPlayer(null);
            }

            if (actor.GoModel != null)
            {
                m_goModelHash2Actor.Remove(actor.GoModel.GetHashCode());
            }

            actor.Destroy();
            m_actorPool.Remove(actorID);
            m_listActor.Remove(actor);
            return true;
        }

        return false;
    }

    #endregion

    #region GetGameActor

    public GameActor GetActor(uint actorGID)
    {
        GameActor findActor = null;
        m_actorPool.TryGetValue(actorGID, out findActor);
        return findActor;
    }

    public GameActor GetActor(GameObject go)
    {
        GameActor actor;
        if (m_goModelHash2Actor.TryGetValue(go.GetHashCode(), out actor))
        {
            return actor;
        }

        return null;
    }

    public GameActor GetActorFromRoleId(ulong roleId)
    {
        var enumerator = m_actorPool.GetEnumerator();
        while (enumerator.MoveNext())
        {
            var actor = enumerator.Current.Value;
            if (actor.GetActorType() == ActorType.PlayerActor)
            {
                //if (actor.GetRoleID() == roleId)
                //{
                //    return actor;
                //}
            }
        }
        return null;
    }

    public Dictionary<uint, GameActor>.Enumerator GetActorEnumerator()
    {
        return m_actorPool.GetEnumerator();
    }
    #endregion

    private void SetMainPlayer(GameActor actor)
    {
        m_mainPlayer = actor;
        //var currCamera = SceneSys.Instance.CameraMgr.CurrGameCamera;
        //var camera = currCamera != null ? currCamera.OwnCamera : null;
        //DScene.UpdateMainCameraTransform(actor != null ? actor.ModelTrans : null, 1, camera, false);
    }
}

public static class ActorHelper
{
    public static SkillCaster GetSkillCaster(GameActor actor)
    {
        if (actor != null)
        {
            return actor.GetSkillCaster();
        }

        return null;
    }
}
