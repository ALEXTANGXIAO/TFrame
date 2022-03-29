class ActorCmptPool : BehaviourSingleton<ActorCmptPool>
{
    private ActorCmpt m_listCmptHead;
    private ActorCmpt m_listStartCmptHead;

    private GameTimerMgr m_timerMgr;

    public override void Awake()
    {
        m_timerMgr = GameTimerMgr.Instance;
    }

    public T AllocCmpt<T>() where T : ActorCmpt, new()
    {
        var cmpt = new T();
        AddToList(ref m_listStartCmptHead, cmpt);

        return cmpt;
    }

    public void FreeCmpt(ActorCmpt cmpt)
    {
        if (cmpt.m_destroy)
        {
            return;
        }

        cmpt.m_destroy = true;
        if (!cmpt.m_callStart)
        {
            RmvFromList(ref m_listStartCmptHead, cmpt);
        }
        else
        {
            RmvFromList(ref m_listCmptHead, cmpt);

            var timerMgr = m_timerMgr;
            timerMgr.DestroyTimer(ref cmpt.m_updateTimer);
            timerMgr.DestroyTimer(ref cmpt.m_lateUpdateTimer);
        }

        //DLogger.Assert(cmpt.m_next == null && cmpt.m_prev == null);
    }

    public override void Update()
    {
        var timerMgr = m_timerMgr;

        while (m_listStartCmptHead != null)
        {
            var start = m_listStartCmptHead;
            //DLogger.Assert(!start.m_destroy);
            start.CallStart();
            if (start.m_destroy)
            {
                //这个时候，肯定已经从链表里删除了
                //DLogger.Assert(m_listStartCmptHead != start);
                continue;
            }

            RmvFromList(ref m_listStartCmptHead, start);
            AddToList(ref m_listCmptHead, start);
            start.AfterOnStartAction(timerMgr);
        }
    }

    public void RequestOnceUpdate(ActorCmpt cmpt)
    {
        cmpt.m_updateTimer = m_timerMgr.CreateOnceFrameTimer(cmpt.GetType().FullName, cmpt.Update);
    }

    public void RequestLoopUpdate(ActorCmpt cmpt, string detail = null)
    {
        var timerName = cmpt.GetType().FullName;

#if DOD_PROFILER
            if (!string.IsNullOrEmpty(detail))
            {
                timerName += ("_" + detail);
            }
#endif

        m_timerMgr.CreateLoopFrameTimer(ref cmpt.m_updateTimer, timerName, cmpt.Update);
    }

    public void StopLoopUpdate(ActorCmpt cmpt)
    {
        m_timerMgr.DestroyTimer(ref cmpt.m_updateTimer);
    }

    public void RequestLoopLateUpdate(ActorCmpt cmpt)
    {
        //DLogger.Assert(GameTimer.IsNull(cmpt.m_lateUpdateTimer));
        m_timerMgr.CreateLoopFrameLateTimer(ref cmpt.m_lateUpdateTimer, cmpt.GetType().FullName, cmpt.LateUpdate);
    }

    public void StopLoopLateUpdate(ActorCmpt cmpt)
    {
        m_timerMgr.DestroyTimer(ref cmpt.m_lateUpdateTimer);
    }

    private void AddToList(ref ActorCmpt head, ActorCmpt cmpt)
    {
        if (head != null)
        {
            cmpt.m_next = head;
            head.m_prev = cmpt;
        }

        head = cmpt;
    }

    private void RmvFromList(ref ActorCmpt head, ActorCmpt cmpt)
    {
        var prev = cmpt.m_prev;
        var next = cmpt.m_next;

        if (prev != null)
        {
            prev.m_next = next;
        }

        if (next != null)
        {
            next.m_prev = prev;
        }

        cmpt.m_next = null;
        cmpt.m_prev = null;

        if (cmpt == head)
        {
            head = next;
        }
        else
        {
            //DLogger.Assert(prev != null);
        }
    }
}