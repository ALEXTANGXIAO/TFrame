using System;
using System.Collections.Generic;
using UnityEngine;

public enum SkillPlayStatus
{
    /// <summary>
    /// 初始状态
    /// </summary>
    PlayInit,

    /// <summary>
    /// 播放技能阶段，该阶段同时只能有一个技能播放
    /// </summary>
    PlayingFront,

    /// <summary>
    /// 后台播放阶段，前台播放完后，可能还有一些元素要继续生效，这个时候转为后台播放阶段
    /// 同时玩家可以释放新的技能
    /// </summary>
    PlayingBack,

    /// <summary>
    /// 等待释放内存
    /// </summary>
    PlayingFree
}


public class SkillPlayData : IMemPoolObject
{
    /// <summary>
    /// 技能内存Id,代表该玩家当前的唯一技能Id
    /// </summary>
    public int m_skillGID = 0;

    /// <summary>
    /// 技能请求序列，因为m_skillGID是服务器来分配，所以要先预留个标识
    /// 等服务器返回后绑定起来
    /// </summary>
    public UInt16 m_skillReqSeq;

    /// <summary>
    /// 技能的配置Id
    /// </summary>
    public uint m_skillId;

    /// <summary>
    /// 技能等级
    /// </summary>
    public uint m_skillLevel;

    /// <summary>
    /// 目标对象ID
    /// </summary>
    public uint m_targetActorID = 0;

    private SkillPlayStatus m_status = SkillPlayStatus.PlayInit;

    /// <summary>
    /// 播放状态
    /// </summary>
    public SkillPlayStatus Status
    {
        set
        {
            m_status = value;
            if (m_status == SkillPlayStatus.PlayingBack)
            {
                m_startBackTime = GameTime.time;
            }
        }
        get { return m_status; }
    }

    /// <summary>
    /// 技能表现配置文件
    /// </summary>
    public SkillDispData m_dispData;

    /// <summary>
    /// 开始时间
    /// </summary>
    public float m_startTime;

    /// <summary>
    /// 开始技能进入后台的时间
    /// </summary>
    public float m_startBackTime;

    /// <summary>
    /// 加速比例
    /// </summary>
    public float m_accelRatio;

    /// <summary>
    /// 技能元素列表
    /// </summary>
    public List<SkillElementHandle> m_handleList = new List<SkillElementHandle>();

    /// <summary>
    /// 击杀目标数量
    /// </summary>
    public int m_killNum = 0;

    private GameActor m_casterActor = null;
    private SkillCaster m_skillCaster = null;
    private bool m_haveLockTargetPos = false;
    private Vector3 m_lockTargetPos = Vector3.zero;
    private uint m_casterId = 0;

    public uint CasterId
    {
        get { return m_casterId; }
    }

    public GameActor CasterActor
    {
        get
        {
            if (m_casterActor != null)
            {
                return m_casterActor;
            }

            if (m_casterId > 0)
            {
                m_casterActor = ActorMgr.Instance.GetActor(m_casterId);
            }

            return m_casterActor;
        }

        set
        {
            m_casterActor = value;
            if (m_casterActor != null)
            {
                m_skillCaster = ActorHelper.GetSkillCaster(m_casterActor);
                m_casterId = m_casterActor.ActorID;
            }
            else
            {
                m_skillCaster = null;
            }
        }
    }

    /// <summary>
    /// 获取技能播放模块
    /// </summary>
    public SkillCaster SkillCaster
    {
        get { return m_skillCaster; }
    }

    public bool AddElementHandle(SkillElementHandle handle)
    {
        string errField = null;
        string checkResult = handle.CheckElementConfig(ref errField);
        if (!string.IsNullOrEmpty(checkResult))
        {
            Debug.LogErrorFormat("skill elemnet config[{0}] error: {1}, SkillID[{2}]", handle.GetType().ToString(), checkResult,
                m_skillId);
            return false;
        }

        handle.PlayData = this;
        m_handleList.Add(handle);
        return true;
    }

    /// <summary>
    /// 获取服务器同步过来的首选目标的坐标,同步播放效果的方位
    /// </summary>
    /// <returns></returns>
    public bool TryGetLockTargetPos(out Vector3 targetPos)
    {
        if (m_haveLockTargetPos)
        {
            targetPos = m_lockTargetPos;
            return true;
        }

        targetPos = Vector3.zero;
        return false;
    }

    public void SetLockTargetPos(Vector3 targetPos)
    {
        m_haveLockTargetPos = true;
        m_lockTargetPos = targetPos;
    }


    public void Destroy()
    {
        //销毁所有的elementhandle
        for (int i = 0; i < m_handleList.Count; i++)
        {
            SkillElementHandle elemHandle = m_handleList[i];
            if (elemHandle != null)
            {
                elemHandle.Destroy();
                elemHandle.PlayData = null;
            }
        }

        m_handleList.Clear();

        m_skillCaster = null;
        m_casterActor = null;
    }

    public void Init()
    {
        m_skillGID = 0;
        m_skillId = 0;
        m_skillLevel = 1;
        m_dispData = null;
        Status = SkillPlayStatus.PlayInit;
        m_startTime = 0f;
        m_startBackTime = 0f;
        m_accelRatio = 1f;
        m_handleList.Clear();
        m_casterActor = null;
        m_casterId = 0;
        m_skillCaster = null;
        m_killNum = 0;
        m_haveLockTargetPos = false;
        m_lockTargetPos = Vector3.zero;
    }
}
