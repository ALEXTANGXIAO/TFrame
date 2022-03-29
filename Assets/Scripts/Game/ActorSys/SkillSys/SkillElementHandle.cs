using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 每个元素的状态
/// </summary>
public enum SkillElementStatus
{
    ELEM_STATUS_INIT,
    ELEM_STATUS_RUN,
    ELEM_STATUS_STOP
}

public class SkillElementHandle 
{
    protected SkillPlayData m_playData = null;
    protected SkillElementStatus m_status = SkillElementStatus.ELEM_STATUS_INIT;

    /// <summary>
    /// 获取技能播放数据
    /// </summary>
    public SkillPlayData PlayData
    {
        get { return m_playData; }
        set { m_playData = value; }
    }

    /// <summary>
    /// 获取技能ID
    /// </summary>
    public uint SkillId
    {
        get { return m_playData.m_skillId; }
    }

    public SkillElementStatus Status
    {
        get { return m_status; }
    }

    public GameActor CasterActor
    {
        get { return m_playData.CasterActor; }
    }

    /// <summary>
    /// 初始化接口
    /// </summary>
    public void Init()
    {
        m_status = SkillElementStatus.ELEM_STATUS_INIT;
        OnInit();
    }

    public void Destroy()
    {
        if (m_status != SkillElementStatus.ELEM_STATUS_STOP)
        {
            Stop();
        }

        m_status = SkillElementStatus.ELEM_STATUS_STOP;
        OnDestroy();
    }

    /// <summary>
    /// 触发element开始
    /// </summary>
    public void Start(SkillTriggleEvent eventType)
    {
        if (m_status != SkillElementStatus.ELEM_STATUS_INIT)
        {
            Debug.LogErrorFormat("invalid status skillId[{0}] element Type[{1}]", m_playData.m_skillId, GetType().Name);
            return;
        }

        m_status = SkillElementStatus.ELEM_STATUS_RUN;

        //如果是重复触发的机制，则不需要开始就触发
        OnStart(eventType, !IsRepeatTriggleType);
    }

    #region override function
    /// <summary>
    /// 检查配置是否正常
    /// </summary>
    /// <returns></returns>
    public virtual string CheckElementConfig(ref string errField)
    {
        return null;
    }

    /// <summary>
    /// 初始化一些数，在加入到技能列表的时候触发
    /// </summary>
    protected virtual void OnInit()
    {
    }

    protected virtual void OnDestroy()
    {
    }

    /// <summary>
    /// 触发开始的消息
    /// </summary>
    /// <param name="eventType">触发开始的消息类型</param>
    /// <param name="needTrigleAction">是否需要出发行为，比如释放子弹，播放特效, 当元素是由命中等重复触发来驱动的情况下，就为false，否则则为true</param>
    protected virtual void OnStart(SkillTriggleEvent eventType, bool needTrigleAction)
    {
    }

    /// <summary>
    /// 触发结束的消息
    /// </summary>
    protected virtual void OnStop()
    {
    }

    /// <summary>
    /// 命中敌人触发, 将触发的消息细分，可以方便后续的扩展
    /// 不然很容易出现已有的元素未处理的类型异常
    /// </summary>
    protected virtual void OnShootTarget(SkillTriggleShootData shootData)
    {
    }

    /// <summary>
    /// 移动结束后触发
    /// 主要考虑服务器计算的时候，时间点和shootpoint很难精确计算
    /// 所以增加一个特殊触发方式，当移动结束后触发，这样可以保证移动和shootpoint的时机比较切当
    /// </summary>
    /// <param name="transId">移动ID</param>
    protected virtual void OnTranslateFinish(int transId)
    {
    }

    /// <summary>
    /// 循环tick
    /// </summary>
    protected virtual void OnUpdate()
    {
    }

    public virtual void OnDrawGizmos()
    {
    }
    #endregion
}