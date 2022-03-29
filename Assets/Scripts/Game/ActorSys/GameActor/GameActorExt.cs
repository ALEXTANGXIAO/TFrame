
using System;
using System.Collections.Generic;
using UnityEngine;


abstract partial class GameActor
{
    public ActorAttribute Attr = new ActorAttribute();

    /// <summary>
    /// 组件封装
    /// </summary>
    private List<ActorCmpt> m_listCmpt = new List<ActorCmpt>();
    private Dictionary<string, ActorCmpt> m_mapCmpt = new Dictionary<string, ActorCmpt>();
    private bool m_isDestroyAlling = false;

    protected void InitExt()
    {
        m_isDestroyAlling = false;
    }

    #region component oper function
    public T AddCmpt<T>(bool respawn = true) where T : ActorCmpt, new()
    {
        //当前正在销毁，不能添加新的组件了
        if (IsDestroyed || m_isDestroyAlling)
        {
            Debug.LogErrorFormat("Actor is destoryed, cant add component: {0}, IsDestrying[{1}]",
                GetClassName(typeof(T)), m_isDestroyAlling);
            return null;
        }

        T cmpt = GetCmpt<T>();
        if (cmpt != null)
        {
            return cmpt;
        }

        //如果不存在，则创建
        //cmpt = new T();
        cmpt = ActorCmptPool.Instance.AllocCmpt<T>();
        if (!AddCmpt_Imp(cmpt))
        {
            Debug.LogErrorFormat("AddComponent failed, Component name: {0}", GetClassName(typeof(T)));
            cmpt.BeforeDestroy();
            ActorCmptPool.Instance.FreeCmpt(cmpt);
            return null;
        }

        return cmpt;
    }

    public T GetCmpt<T>() where T : ActorCmpt
    {
        ActorCmpt cmpt;
        if (m_mapCmpt.TryGetValue(GetClassName(typeof(T)), out cmpt))
        {
            return cmpt as T;
        }

        return null;
    }

    public void RemoveCmpt<T>() where T : ActorCmpt
    {
        if (m_isDestroyAlling)
        {
            Debug.LogErrorFormat("GameActor[{0}] is destroying, no need destroy cmpt anyway", Name);
            return;
        }

        string className = GetClassName(typeof(T));
        ActorCmpt cmpt;
        if (m_mapCmpt.TryGetValue(className, out cmpt))
        {
            cmpt.BeforeDestroy();

            Event.RemoveAllListenerByOwner(cmpt);
            m_mapCmpt.Remove(className);
            m_listCmpt.Remove(cmpt);

            ActorCmptPool.Instance.FreeCmpt(cmpt);
        }
    }

    private bool AddCmpt_Imp<T>(T cmpt) where T : ActorCmpt
    {
        //判断是否已经存在
        if (!cmpt.BeforeAddToActor(this))
        {
            return false;
        }

        m_listCmpt.Add(cmpt);
        m_mapCmpt[GetClassName(typeof(T))] = cmpt;
        return true;
    }

    private string GetClassName(Type type)
    {
        return type.FullName;
    }

    private void BeforeDestroyAllCmpt()
    {
        var listCmpt = m_listCmpt;
        for (int i = listCmpt.Count - 1; i >= 0; i--)
        {
            //然后释放内存，触发OnDestroy
            listCmpt[i].BeforeDestroy();
        }
    }

    private void DestroyAllCmpt()
    {
        var cmptPool = ActorCmptPool.Instance;

        var listCmpt = m_listCmpt;
        for (int i = listCmpt.Count - 1; i >= 0; i--)
        {
            //然后释放内存，触发OnDestroy
            cmptPool.FreeCmpt(listCmpt[i]);
        }

        m_listCmpt.Clear();
        m_mapCmpt.Clear();
    }

    #endregion
}
