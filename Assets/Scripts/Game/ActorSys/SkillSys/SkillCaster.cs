using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCaster : ActorCmpt
{
    /// <summary>
    /// 当前技能数据
    /// </summary>
    private SkillPlayData m_currSkillData = null;

    /// <summary>
    /// 获取当前的技能ID
    /// </summary>
    public uint CurrSkillId
    {
        get
        {
            if (m_currSkillData != null)
            {
                return m_currSkillData.m_skillId;
            }

            return 0;
        }
    }
}
