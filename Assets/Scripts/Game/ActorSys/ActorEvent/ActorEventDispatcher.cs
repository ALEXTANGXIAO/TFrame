using System;
using System.Collections.Generic;
using UnityEngine;

class EventRegInfo
{
    public Delegate callback;
    public object owner;
    public bool deleted;

    public EventRegInfo(Delegate _callback, object _owner)
    {
        callback = _callback;
        owner = _owner;
        deleted = false;
    }
}

public class ActorEventDispatcher
{
    private Dictionary<int, List<EventRegInfo>> m_dictAllEventListener = new Dictionary<int, List<EventRegInfo>>();

    /// <summary>
    /// 用于标记一个事件是不是正在处理
    /// </summary>
    private List<int> m_processEevent = new List<int>();
    /// <summary>
    /// 用于标记一个事件是不是被移除
    /// </summary>
    private List<int> m_delayDeleteEvent = new List<int>();

    public void DestroyAllEventListener()
    {
        ///list暂时保留，不做清理,下次可能会继续使用
        var itr = m_dictAllEventListener.GetEnumerator();
        while (itr.MoveNext())
        {
            var kv = itr.Current;
            kv.Value.Clear();
        }

        m_processEevent.Clear();
        m_delayDeleteEvent.Clear();
    }

    private void AddDelayDelete(int eventId)
    {
        if (!m_delayDeleteEvent.Contains(eventId))
        {
            m_delayDeleteEvent.Add(eventId);
            Debug.LogFormat("delay delete eventId[{0}]", eventId);
        }
    }

    /// <summary>
    /// 如果找到eventId对应的监听，删除所有标记为delete的监听
    /// </summary>
    /// <param name="eventId"></param>
    private void CheckDelayDetete(int eventId)
    {
        if (m_delayDeleteEvent.Contains(eventId))
        {
            List<EventRegInfo> listListener;
            if (m_dictAllEventListener.TryGetValue(eventId, out listListener))
            {
                for (int i = 0; i < listListener.Count; i++)
                {
                    if (listListener[i].deleted)
                    {
                        Debug.LogFormat("remove delay delete eventId[{0}]", eventId);
                        listListener[i] = listListener[listListener.Count - 1];
                        listListener.RemoveAt(listListener.Count - 1);
                        i--;


                    }
                }
            }

            m_delayDeleteEvent.Remove(eventId);
        }
    }

    public void SendEvent(ActorEventType eEventId)
    {
        int eventId = (int)eEventId;
        List<EventRegInfo> listListener;
        if (m_dictAllEventListener.TryGetValue(eventId, out listListener))
        {

            m_processEevent.Add(eventId);
#if UNITY_EDITOR
            int iEventCnt = m_processEevent.Count;
#endif

            var count = listListener.Count;
            for (int i = 0; i < count; i++)
            {
                var node = listListener[i];
                if (node.deleted)
                {
                    continue;
                }

                Action callBack = listListener[i].callback as Action;
                if (callBack != null)
                {
                    callBack();
                }
                else
                {
                    Debug.LogErrorFormat("Invalid event data type: {0}", eventId);
                }
            }


//#if UNITY_EDITOR
//            DLogger.Assert(iEventCnt == m_processEevent.Count);
//            DLogger.Assert(eventId == m_processEevent[m_processEevent.Count - 1]);
//#endif
            m_processEevent.RemoveAt(m_processEevent.Count - 1);

            CheckDelayDetete(eventId);
        }
    }

    public void SendEvent<T>(ActorEventType eEventId, T data)
    {
        int eventId = (int)eEventId;
        List<EventRegInfo> listListener;
        if (m_dictAllEventListener.TryGetValue(eventId, out listListener))
        {
            m_processEevent.Add(eventId);
#if UNITY_EDITOR
            int iEventCnt = m_processEevent.Count;
#endif

            var count = listListener.Count;
            for (int i = 0; i < count; i++)
            {
                var node = listListener[i];
                if (node.deleted)
                {
                    continue;
                }

                Action<T> callBack = listListener[i].callback as Action<T>;
                if (callBack != null)
                {
                    callBack(data);
                }
                else
                {
                    Debug.LogErrorFormat("Invalid event data type: {0}", eventId);
                }
            }


//#if UNITY_EDITOR
//            DLogger.Assert(iEventCnt == m_processEevent.Count);
//            DLogger.Assert(eventId == m_processEevent[m_processEevent.Count - 1]);
//#endif

            m_processEevent.RemoveAt(m_processEevent.Count - 1);

            CheckDelayDetete(eventId);
        }
    }
    public void SendEvent<T, U>(ActorEventType eEventId, T dataT, U dataU)
    {
        int eventId = (int)eEventId;
        List<EventRegInfo> listListener;
        if (m_dictAllEventListener.TryGetValue(eventId, out listListener))
        {
            m_processEevent.Add(eventId);
#if UNITY_EDITOR
            int iEventCnt = m_processEevent.Count;
#endif

            var count = listListener.Count;
            for (int i = 0; i < count; i++)
            {
                var node = listListener[i];
                if (node.deleted)
                {
                    continue;
                }

                Action<T, U> callBack = listListener[i].callback as Action<T, U>;
                if (callBack != null)
                {
                    callBack(dataT, dataU);
                }
                else
                {
                    Debug.LogErrorFormat("Invalid event data type: {0}", eventId);
                }
            }


//#if UNITY_EDITOR
//            DLogger.Assert(iEventCnt == m_processEevent.Count);
//            DLogger.Assert(eventId == m_processEevent[m_processEevent.Count - 1]);
//#endif
            m_processEevent.RemoveAt(m_processEevent.Count - 1);

            CheckDelayDetete(eventId);
        }
    }

    public void SendEvent<T, U, V>(ActorEventType eEventId, T dataT, U dataU, V dataV)
    {
        int eventId = (int)eEventId;
        List<EventRegInfo> listListener;
        if (m_dictAllEventListener.TryGetValue(eventId, out listListener))
        {
            m_processEevent.Add(eventId);
#if UNITY_EDITOR
            int iEventCnt = m_processEevent.Count;
#endif

            var count = listListener.Count;
            for (int i = 0; i < count; i++)
            {
                var node = listListener[i];
                if (node.deleted)
                {
                    continue;
                }

                Action<T, U, V> callBack = node.callback as Action<T, U, V>;
                if (callBack != null)
                {
                    callBack(dataT, dataU, dataV);
                }
                else
                {
                    Debug.LogErrorFormat("Invalid event data type: {0}", eventId);
                }
            }


//#if UNITY_EDITOR
//            DLogger.Assert(iEventCnt == m_processEevent.Count);
//            DLogger.Assert(eventId == m_processEevent[m_processEevent.Count - 1]);
//#endif
            m_processEevent.RemoveAt(m_processEevent.Count - 1);

            CheckDelayDetete(eventId);
        }
    }

    public void SendEvent<T, U, V, S>(ActorEventType eEventId, T dataT, U dataU, V dataV, S dataS)
    {
        int eventId = (int)eEventId;

        List<EventRegInfo> listListener;
        if (m_dictAllEventListener.TryGetValue(eventId, out listListener))
        {
            m_processEevent.Add(eventId);
#if UNITY_EDITOR
            int iEventCnt = m_processEevent.Count;
#endif


            var count = listListener.Count;
            for (int i = 0; i < count; i++)
            {
                var node = listListener[i];
                if (node.deleted)
                {
                    continue;
                }

                Action<T, U, V, S> callBack = listListener[i].callback as Action<T, U, V, S>;
                if (callBack != null)
                {
                    callBack(dataT, dataU, dataV, dataS);
                }
                else
                {
                    Debug.LogErrorFormat("Invalid event data type: {0}", eventId);
                }
            }


//#if UNITY_EDITOR
//            DLogger.Assert(iEventCnt == m_processEevent.Count);
//            DLogger.Assert(eventId == m_processEevent[m_processEevent.Count - 1]);
//#endif
            m_processEevent.RemoveAt(m_processEevent.Count - 1);

            CheckDelayDetete(eventId);
        }
    }

    public void AddEventListener(ActorEventType eventHashId, Action eventCallback, object owner)
    {
        AddEventListenerImp(eventHashId, eventCallback, owner);
    }

    //回调带参数
    public void AddEventListener<T>(ActorEventType eventHashId, Action<T> eventCallback, object owner)
    {
        AddEventListenerImp(eventHashId, eventCallback, owner);
    }

    //回调带参数
    public void AddEventListener<T, U>(ActorEventType eventHashId, Action<T, U> eventCallback, object owner)
    {
        AddEventListenerImp(eventHashId, eventCallback, owner);
    }

    public void AddEventListener<T, U, V>(ActorEventType eventHashId, Action<T, U, V> eventCallback, object owner)
    {
        AddEventListenerImp(eventHashId, eventCallback, owner);
    }

    public void AddEventListener<T, U, V, S>(ActorEventType eventHashId, Action<T, U, V, S> eventCallback, object owner)
    {
        AddEventListenerImp(eventHashId, eventCallback, owner);
    }

    private void AddEventListenerImp(ActorEventType eventHashId, Delegate listener, object owner)
    {
        List<EventRegInfo> listListener;
        if (!m_dictAllEventListener.TryGetValue((int)eventHashId, out listListener))
        {
            listListener = new List<EventRegInfo>();
            m_dictAllEventListener.Add((int)eventHashId, listListener);
        }

        var existNode = listListener.Find((node) => { return node.callback == listener; });
        if (existNode != null)
        {
            //如果被删除了，又重新给加回来
            if (existNode.deleted)
            {
                existNode.deleted = false;
                Debug.LogWarningFormat("AddEvent hashId deleted, repeat add: {0}", eventHashId);
                return;
            }

            Debug.LogErrorFormat("AddEvent hashId repeated: {0}", eventHashId);
            return;
        }

        listListener.Add(new EventRegInfo(listener, owner));
    }

    public void RemoveAllListenerByOwner(object owner)
    {
        var itr = m_dictAllEventListener.GetEnumerator();
        while (itr.MoveNext())
        {
            var kv = itr.Current;
            var list = kv.Value;

            int eventId = kv.Key;
            bool isProcessing = m_processEevent.Contains(eventId);
            bool delayDeleted = false;

            for (int i = 0; i < list.Count; i++)
            {
                var regInfo = list[i];
                if (regInfo.owner == owner)
                {
                    if (isProcessing)
                    {
                        regInfo.deleted = true;
                        delayDeleted = true;
                    }
                    else
                    {
                        list[i] = list[list.Count - 1];
                        list.RemoveAt(list.Count - 1);
                        i--;
                    }
                }
            }

            if (delayDeleted)
            {
                AddDelayDelete(eventId);
            }
        }
        itr.Dispose();
    }

    public void RemoveEventListener(int eventHashId, Action eventCallback)
    {
        RemoveEventListenerImp(eventHashId, eventCallback);
    }

    //回调带参数
    public void RemoveEventListener<T>(int eventHashId, Action<T> eventCallback)
    {
        RemoveEventListenerImp(eventHashId, eventCallback);
    }

    //回调带参数
    public void RemoveEventListener<T, U>(int eventHashId, Action<T, U> eventCallback)
    {
        RemoveEventListenerImp(eventHashId, eventCallback);
    }
    //回调带参数
    public void RemoveEventListener<T, U, V>(int eventHashId, Action<T, U, V> eventCallback)
    {
        RemoveEventListenerImp(eventHashId, eventCallback);
    }
    //回调带参数
    public void RemoveEventListener<T, U, V, S>(int eventHashId, Action<T, U, V, S> eventCallback)
    {
        RemoveEventListenerImp(eventHashId, eventCallback);
    }

    /// <summary>
    /// 删除监听，如果是正在处理的监听则标记为删除
    /// </summary>
    /// <param name="eventHashId"></param>
    /// <param name="listener"></param>
    protected void RemoveEventListenerImp(int eventHashId, Delegate listener)
    {
        List<EventRegInfo> listListener;
        if (m_dictAllEventListener.TryGetValue(eventHashId, out listListener))
        {
            bool isProcessing = m_processEevent.Contains(eventHashId);
            if (!isProcessing)
            {
                listListener.RemoveAll((node) => { return node.callback == listener; });
            }
            else
            {
                int listenCnt = listListener.Count;
                for (int i = 0; i < listenCnt; i++)
                {
                    var node = listListener[i];
                    if (node.callback == listener)
                    {
                        node.deleted = true;
                        AddDelayDelete(eventHashId);
                        break;
                    }
                }
            }
        }
    }

}
