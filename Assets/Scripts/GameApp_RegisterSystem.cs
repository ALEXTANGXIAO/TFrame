using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed partial class GameApp
{
    public int TargetFrameRate = 60;

    public enum HostPoint
    {
        LinuxServer,
        WinServer,
        LocalHost,
    }

    public HostPoint hostPoint = HostPoint.LocalHost;
    public string Host
    {
        get
        {
            switch (hostPoint)
            {
                case (HostPoint.LocalHost):
                    return "127.0.0.1";
                case (HostPoint.LinuxServer):
                    return "1.12.241.46";
                case (HostPoint.WinServer):
                    return "1.14.132.143";
            }
            return "127.0.0.1";
        }
    }

    private void SetTargetFrameRate()
    {
        Application.targetFrameRate = TargetFrameRate;
    }


    #region 生命周期
    public void Start()
    {
        GameTime.StartFrame();
        var listLogic = m_listLogicMgr;
        var logicCnt = listLogic.Count;
        for (int i = 0; i < logicCnt; i++)
        {
            var logic = listLogic[i];
            logic.OnStart();
        }
    }

    public void Update()
    {
        GameTime.StartFrame();
        var listLogic = m_listLogicMgr;
        var logicCnt = listLogic.Count;
        for (int i = 0; i < logicCnt; i++)
        {
            var logic = listLogic[i];
            logic.OnUpdate();
        }
    }

    public void LateUpdate()
    {
        GameTime.StartFrame();
        var listLogic = m_listLogicMgr;
        var logicCnt = listLogic.Count;
        for (int i = 0; i < logicCnt; i++)
        {
            var logic = listLogic[i];
            logic.OnLateUpdate();
        }
    }

    public void OnPause()
    {
        GameTime.StartFrame();
        for (int i = 0; i < m_listLogicMgr.Count; i++)
        {
            var logicSys = m_listLogicMgr[i];
            logicSys.OnPause();
        }
    }

    public void OnResume()
    {
        GameTime.StartFrame();
        for (int i = 0; i < m_listLogicMgr.Count; i++)
        {
            var logicSys = m_listLogicMgr[i];
            logicSys.OnResume();
        }
    }

    public void OnDestroy()
    {
        GameTime.StartFrame();
        for (int i = 0; i < m_listLogicMgr.Count; i++)
        {
            var logicSys = m_listLogicMgr[i];
            logicSys.OnDestroy();
        }
    }

    #endregion

    #region 系统注册
    //-------------------------------------------------------系统注册--------------------------------------------------------//
    private List<ILogicSys> m_listLogicMgr = new List<ILogicSys>();

    private void InitLibImp()
    {

    }

    private void RegistAllSystem()
    {
        //EventCenter.OnInit();
        AddLogicSys(UISys.Instance);
        AddLogicSys(EcsFactory.Instance);
        AddLogicSys(DataCenterSys.Instance);
        GameClient.Instance.Init();
    }

    protected bool AddLogicSys(ILogicSys logicSys)
    {
        if (m_listLogicMgr.Contains(logicSys))
        {
            Debug.Log("Repeat add logic system: " + logicSys.GetType().Name);
            return false;
        }

        if (!logicSys.OnInit())
        {
            Debug.Log(" Init failed " + logicSys.GetType().Name);
            return false;
        }

        m_listLogicMgr.Add(logicSys);

        return true;
    }
    #endregion
}