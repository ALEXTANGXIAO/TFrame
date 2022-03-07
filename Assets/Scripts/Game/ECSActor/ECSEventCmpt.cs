using System;
using System.Collections.Generic;
using ECS;

public class ECSEventCmpt : ECSComponent
{
    private Dictionary<int, IEventInfo> m_eventDic = new Dictionary<int, IEventInfo>();

    #region AddEventListener
    public void AddEventListener<T>(int eventid, Action<T> action)
    {
        if (m_eventDic.ContainsKey(eventid))
        {
            (m_eventDic[eventid] as EventInfo<T>).actions += action;
        }
        else
        {
            m_eventDic.Add(eventid, new EventInfo<T>(action));
        }
    }

    public void AddEventListener<T, U>(int eventid, Action<T, U> action)
    {
        if (m_eventDic.ContainsKey(eventid))
        {
            (m_eventDic[eventid] as EventInfo<T, U>).actions += action;
        }
        else
        {
            m_eventDic.Add(eventid, new EventInfo<T, U>(action));
        }
    }

    public void AddEventListener<T, U, W>(int eventid, Action<T, U, W> action)
    {
        if (m_eventDic.ContainsKey(eventid))
        {
            (m_eventDic[eventid] as EventInfo<T, U, W>).actions += action;
        }
        else
        {
            m_eventDic.Add(eventid, new EventInfo<T, U, W>(action));
        }
    }

    public void AddEventListener(int eventid, Action action)
    {
        if (m_eventDic.ContainsKey(eventid))
        {
            (m_eventDic[eventid] as EventInfo).actions += action;
        }
        else
        {
            m_eventDic.Add(eventid, new EventInfo(action));
        }
    }
    #endregion

    #region RemoveEventListener
    public void RemoveEventListener<T>(int eventid, Action<T> action)
    {
        if (action == null)
        {
            return;
        }

        if (m_eventDic.ContainsKey(eventid))
        {
            (m_eventDic[eventid] as EventInfo<T>).actions -= action;
        }
    }

    public void RemoveEventListener<T, U>(int eventid, Action<T, U> action)
    {
        if (action == null)
        {
            return;
        }

        if (m_eventDic.ContainsKey(eventid))
        {
            (m_eventDic[eventid] as EventInfo<T, U>).actions -= action;
        }
    }

    public void RemoveEventListener<T, U, W>(int eventid, Action<T, U, W> action)
    {
        if (action == null)
        {
            return;
        }

        if (m_eventDic.ContainsKey(eventid))
        {
            (m_eventDic[eventid] as EventInfo<T, U, W>).actions -= action;
        }
    }

    public void RemoveEventListener(int eventid, Action action)
    {
        if (action == null)
        {
            return;
        }

        if (m_eventDic.ContainsKey(eventid))
        {
            (m_eventDic[eventid] as EventInfo).actions -= action;
        }
    }
    #endregion

    #region Send
    public void Send<T>(int eventid, T info)
    {
        if (m_eventDic.ContainsKey(eventid))
        {
            var eventInfo = (m_eventDic[eventid] as EventInfo<T>);
            if (eventInfo != null)
            {
                eventInfo.actions.Invoke(info);
            }
        }
    }

    public void Send<T, U>(int eventid, T info, U info2)
    {
        if (m_eventDic.ContainsKey(eventid))
        {
            var eventInfo = (m_eventDic[eventid] as EventInfo<T, U>);
            if (eventInfo != null)
            {
                eventInfo.actions.Invoke(info, info2);
            }
        }
    }

    public void Send<T, U, W>(int eventid, T info, U info2, W info3)
    {
        if (m_eventDic.ContainsKey(eventid))
        {
            var eventInfo = (m_eventDic[eventid] as EventInfo<T, U, W>);
            if (eventInfo != null)
            {
                eventInfo.actions.Invoke(info, info2, info3);
            }
        }
    }

    /// <summary>
    /// 事件触发 无参
    /// </summary>
    /// <param name="name"></param>
    public void Send(int eventid)
    {
        if (m_eventDic.ContainsKey(eventid))
        {
            var eventInfo = (m_eventDic[eventid] as EventInfo);
            if (eventInfo != null)
            {
                eventInfo.actions.Invoke();
            }
        }
    }
    #endregion

    #region Clear
    public void Clear()
    {
        m_eventDic.Clear();
    }
    #endregion

    public override void OnDestroy()
    {
        base.OnDestroy();
        Clear();
    }

    public override void Awake()
    {
        base.Awake();
        Entity.Event = this;
    }
}

#region EventInfo
internal interface IEventInfo
{

}

public class EventInfo : IEventInfo
{
    public Action actions;

    public EventInfo(Action action)
    {
        actions += action;
    }
}


public class EventInfo<T> : IEventInfo
{
    public Action<T> actions;

    public EventInfo(Action<T> action)
    {
        actions += action;
    }
}

public class EventInfo<T, U> : IEventInfo
{
    public Action<T, U> actions;

    public EventInfo(Action<T, U> action)
    {
        actions += action;
    }
}

public class EventInfo<T, U, W> : IEventInfo
{
    public Action<T, U, W> actions;

    public EventInfo(Action<T, U, W> action)
    {
        actions += action;
    }
}
#endregion