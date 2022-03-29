using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ActorCmptDebugKeyInfo
{
    //[HideInInspector]
    public string m_name;
    public string val;
}

[Serializable]
public class ActorCmptDebugInfo
{
    //[HideInInspector]
    public string m_name;
    public List<ActorCmptDebugKeyInfo> m_info = new List<ActorCmptDebugKeyInfo>();
}

/// <summary>
/// 用来调试查看actor信息
/// </summary>
public class ActorDebugerBehaviour : MonoBehaviour
{
    public List<ActorCmptDebugInfo> m_cmptInfo = new List<ActorCmptDebugInfo>();

    //[HideInInspector]
    public event Action m_onGizmosSelect;
    //[HideInInspector]
    public event Action m_onGizmos;

    public ActorCmptDebugInfo AddDebugCmpt(string cmptName)
    {
        var cmptInfo = m_cmptInfo.Find((item) => { return item.m_name == cmptName; });
        if (cmptInfo == null)
        {
            cmptInfo = new ActorCmptDebugInfo();
            cmptInfo.m_name = cmptName;
            m_cmptInfo.Add(cmptInfo); ;
        }

        return cmptInfo;
    }

    public void RmvDebugCmpt(string cmptName)
    {
        m_cmptInfo.RemoveAll((item) => { return item.m_name == cmptName; });
    }

    public void SetDebugInfo(string cmptName, string key, string val)
    {
        var cmptInfo = AddDebugCmpt(cmptName);
        var entry = cmptInfo.m_info.Find((t) => { return t.m_name == key; });
        if (entry == null)
        {
            entry = new ActorCmptDebugKeyInfo();
            entry.m_name = key;
            cmptInfo.m_info.Add(entry);
        }

        entry.val = val;
    }

    public void RemoveAllDebugInfo(string cmpName)
    {
        var cmpInfo = m_cmptInfo.Find((item) => { return item.m_name == cmpName; });
        if (cmpInfo != null)
        {
            cmpInfo.m_info.Clear();
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (m_onGizmosSelect != null)
        {
            m_onGizmosSelect();
        }
    }

    void OnDrawGizmos()
    {
        if (m_onGizmos != null)
        {
            m_onGizmos();
        }
    }
#endif
}
