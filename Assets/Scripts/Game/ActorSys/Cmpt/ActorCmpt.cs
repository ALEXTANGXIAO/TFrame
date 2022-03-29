using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

/// <summary>
/// 组件基类
/// </summary>
public abstract class ActorCmpt
{
    public bool m_destroy;
    public ActorCmpt m_prev;
    public ActorCmpt m_next;

    public GameTimer m_updateTimer;
    public GameTimer m_lateUpdateTimer;
    public bool m_needOnceUpdate = false;
    public bool m_needLoopUpdate = false;
    public bool m_needLoopLateUpdate = false;

    protected void RequestOnceUpdate()
    {
        if (!m_callStart)
        {
            m_needOnceUpdate = true;
            return;
        }

        if (GameTimer.IsNull(m_updateTimer))
        {
            ActorCmptPool.Instance.RequestOnceUpdate(this);
        }
    }

    protected void RequestLoopUpdate(string detail = null)
    {
        if (!m_callStart)
        {
            m_needLoopUpdate = true;
            return;
        }

        if (GameTimer.IsNull(m_updateTimer))
        {
            ActorCmptPool.Instance.RequestLoopUpdate(this, detail);
        }
    }

    protected void RequestLoopLateUpdate()
    {
        if (!m_callStart)
        {
            m_needLoopLateUpdate = true;
            return;
        }

        if (GameTimer.IsNull(m_lateUpdateTimer))
        {
            ActorCmptPool.Instance.RequestLoopLateUpdate(this);
        }
    }

    protected void StopLoopUpdate()
    {
        //如果是持久update的类型
        if (!m_callStart)
        {
            m_needLoopUpdate = false;
            return;
        }

        if (!GameTimer.IsNull(m_updateTimer))
        {
            ActorCmptPool.Instance.StopLoopUpdate(this);
        }
    }

    private class RegisterAttrChangeData
    {
        public int m_attrId;
        public Delegate m_handler;

        public RegisterAttrChangeData(int attrId, Delegate handler)
        {
            m_attrId = attrId;
            m_handler = handler;
        }
    }

    public bool m_callStart = false;
    private bool m_calledOnDestroy = false;
    protected GameActor m_actor;

    private List<RegisterAttrChangeData> m_registAttrChanged = new List<RegisterAttrChangeData>();


    public GameActor OwnActor
    {
        get
        {
            if (m_actor != null && m_actor.IsDestroyed)
            {
                m_actor = null;
                return null;
            }

            return m_actor;
        }
    }


    public Vector3 Position
    {
        get { return m_actor.Position; }
    }

    public string ActorName
    {
        get { return m_actor.Name; }
    }

    #region 操作接口

    /// <summary>
    /// 只有添加到对象上，才触发下面的初始化逻辑
    /// </summary>
    /// <param name="actor"></param>
    /// <returns></returns>
    public bool BeforeAddToActor(GameActor actor)
    {
        m_actor = actor;
        m_callStart = false;
        Awake();

        //添加调试
        AddDebug();
        return true;
    }

    public void BeforeDestroy()
    {
        if (m_calledOnDestroy)
        {
            return;
        }

        RmvDebug();

        m_calledOnDestroy = true;
        if (m_actor != null)
        {
            ClearAllEventChangeHandler();
            OnDestroy();
        }
    }

    public void CallStart()
    {
        //DLogger.Assert(!m_callStart);
        Start();
        m_callStart = true;
    }


    public void AfterOnStartAction(GameTimerMgr timerMgr)
    {
        if (m_needLoopUpdate)
        {
            //DLogger.Debug("add to loop update: {0}", GetType().FullName);
            //DLogger.Assert(GameTimer.IsNull(m_updateTimer));

            timerMgr.CreateLoopFrameTimer(ref m_updateTimer, GetType().FullName, Update);
        }
        else if (m_needOnceUpdate)
        {
            //DLogger.Debug("add to once update: {0}", GetType().FullName);
            //DLogger.Assert(GameTimer.IsNull(m_updateTimer));

            if (GameTimer.IsNull(m_updateTimer))
            {
                m_updateTimer = timerMgr.CreateOnceFrameTimer(GetType().FullName, Update);
            }
        }

        if (m_needLoopLateUpdate)
        {
            //DLogger.Assert(GameTimer.IsNull(m_lateUpdateTimer));
            timerMgr.CreateLoopFrameLateTimer(ref m_lateUpdateTimer, GetType().FullName, LateUpdate);
        }
    }


    #endregion

    #region 扩展接口

    protected virtual void Awake()
    {
    }

    protected virtual void Start()
    {
    }

    public virtual void LateUpdate()
    {
    }

    public virtual void Update()
    {
    }

    protected virtual void OnDestroy()
    {
    }

    /// <summary>
    /// 不显示的时候是否需要update
    /// </summary>
    /// <returns></returns>
    public virtual bool IsInvisibleNeedUpdate()
    {
        return true;
    }
    #endregion

    [Conditional("DOD_DEBUG")]
    protected void AddDebug()
    {
#if UNITY_EDITOR
        var debugData = UnityUtil.AddMonoBehaviour<ActorDebugerBehaviour>(OwnActor.GoModel);
        debugData.AddDebugCmpt(GetType().Name);
#endif
    }

    [Conditional("DOD_DEBUG")]
    protected void RmvDebug()
    {
#if UNITY_EDITOR
        var debugData = UnityUtil.AddMonoBehaviour<ActorDebugerBehaviour>(OwnActor.GoModel);
        debugData.RmvDebugCmpt(GetType().Name);
#endif
    }

    [Conditional("DOD_DEBUG")]
    protected void SetDebugInfo(string key, string val)
    {
#if UNITY_EDITOR
        var debugData = UnityUtil.AddMonoBehaviour<ActorDebugerBehaviour>(OwnActor.GoModel);
        debugData.SetDebugInfo(GetType().Name, key, val);
#endif
    }

    [Conditional("DOD_DEBUG")]
    protected void RemoveAllDebugInfo()
    {
#if UNITY_EDITOR
        var debugData = UnityUtil.AddMonoBehaviour<ActorDebugerBehaviour>(OwnActor.GoModel);
        debugData.RemoveAllDebugInfo(GetType().Name);
#endif
    }

    #region Event操作函数
    public void AddEventListener(ActorEventType eventType, Action eventCallback)
    {
        m_actor.Event.AddEventListener(eventType, eventCallback, this);
    }

    //回调带参数
    public void AddEventListener<T>(ActorEventType eventType, Action<T> eventCallback)
    {
        m_actor.Event.AddEventListener(eventType, eventCallback, this);
    }

    //回调带参数
    public void AddEventListener<T, U>(ActorEventType eventType, Action<T, U> eventCallback)
    {
        m_actor.Event.AddEventListener(eventType, eventCallback, this);
    }

    public void AddEventListener<T, U, V>(ActorEventType eventType, Action<T, U, V> eventCallback)
    {
        m_actor.Event.AddEventListener(eventType, eventCallback, this);
    }

    public void AddEventListener<T, U, V, S>(ActorEventType eventType, Action<T, U, V, S> eventCallback)
    {
        m_actor.Event.AddEventListener(eventType, eventCallback, this);
    }

    //public void RegAttrChangeEvent<T>(ActorAttrDef attrId, Action<T, T> handler)
    //{
    //    m_registAttrChanged.Add(new RegisterAttrChangeData((int)attrId, handler));
    //    OwnActor.Attr.RegAttrChangeEvent((int)attrId, handler);
    //}

    private void ClearAllEventChangeHandler()
    {
        var attr = OwnActor.Attr;
        for (int i = 0; i < m_registAttrChanged.Count; i++)
        {
            var data = m_registAttrChanged[i];
            attr.UnRegAttrChangeEvent(data.m_attrId, data.m_handler);
        }
    }

    #endregion
}
