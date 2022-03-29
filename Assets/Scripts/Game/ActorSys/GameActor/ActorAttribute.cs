using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorAttribute
{
    private class AttrChangeHandler
    {
        public Delegate m_handle;

        public void AddDelegate(Delegate handler)
        {
            m_handle = Delegate.Combine(m_handle, handler);
        }

        public void RmvDelegate(Delegate handler)
        {
            m_handle = Delegate.Remove(m_handle, handler);
        }
    }

    private Dictionary<int, object> m_dictAttr = new Dictionary<int, object>();
    private Dictionary<int, AttrChangeHandler> m_attrChangedListener = new Dictionary<int, AttrChangeHandler>();

    public void SetAttribute<T>(ActorAttrDef attrId, T val)
    {
        SetAttribute((int)attrId, val);
    }

    public void ClearAttribute(ActorAttrDef attrId)
    {
        m_dictAttr.Remove((int)attrId);
    }

    public void SetAttribute<T>(int attrId, T val)
    {
        bool changed = false;

        T existVal;
        object exist;
        if (m_dictAttr.TryGetValue(attrId, out exist))
        {
            existVal = (T)exist;

            if (!EqualityComparer<T>.Default.Equals(existVal, val))
            {
                changed = true;
            }
        }
        else
        {
            existVal = default(T);
            changed = true;
        }


        if (changed)
        {
            m_dictAttr[attrId] = val;

            AttrChangeHandler handler;
            if (m_attrChangedListener.TryGetValue(attrId, out handler))
            {
                Action<T, T> deleHandle = handler.m_handle as Action<T, T>;
                if (deleHandle != null)
                {
                    deleHandle(existVal, val);
                }
            }
        }
    }

    public T GetAttribute<T>(ActorAttrDef attrId)
    {
        T val;
        TryGetAttribute(attrId, out val);
        return val;
    }

    public bool IsHaveAttribute(ActorAttrDef attrId)
    {
        return m_dictAttr.ContainsKey((int)attrId);
    }

    public bool TryGetAttribute<T>(ActorAttrDef attrId, out T val)
    {
        return GetAttributeValue((int)attrId, out val);
    }

    public bool GetAttributeValue<T>(int attrId, out T val)
    {
        object objVal;
        var ret = m_dictAttr.TryGetValue(attrId, out objVal);
        if (ret)
        {
            val = (T)objVal;
        }
        else
        {
            val = default(T);
        }

        return ret;
    }

    public void RegAttrChangeEvent<T>(int attrId, Action<T, T> handler)
    {
        AttrChangeHandler handleNode;
        if (!m_attrChangedListener.TryGetValue(attrId, out handleNode))
        {
            handleNode = new AttrChangeHandler();
            m_attrChangedListener[attrId] = handleNode;
        }

        handleNode.AddDelegate(handler);
    }

    public void UnRegAttrChangeEvent<T>(int attrId, Action<T, T> handler)
    {
        UnRegAttrChangeEvent(attrId, handler);
    }

    public void UnRegAttrChangeEvent(int attrId, Delegate handler)
    {
        AttrChangeHandler handleNode;
        if (m_attrChangedListener.TryGetValue(attrId, out handleNode))
        {
            handleNode.RmvDelegate(handler);
        }
    }
}

public enum ActorAttrDef
{
    OWNER_ACTOR_ID,
    CURRENT_STATE_ID,
}
