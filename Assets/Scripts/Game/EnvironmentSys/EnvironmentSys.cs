#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor.Presets;
using UnityEngine;

/// <summary>
/// 环境类型
/// </summary>
public enum EnvironmentType
{
    Sky,
    Light,
    Clouds,
    Windy,
    Water
}

/// <summary>
/// 环境时间
/// </summary>
public enum EnvironmentTime
{
    Day,
    Night,
    OverCast,
    Cloudy,
    Storm,
    Sunset,
}

public class EnvironmentSys : SerUnitySingleton<EnvironmentSys>
{
    private EnvironmentTime m_environmentTime;

    [SerializeField]
    public EnvironmentTime MEnvironmentTime
    {
        set
        {
            m_environmentTime = value;
            if (EnvDic != null)
            {
                if (EnvDic.TryGetValue(m_environmentTime, out EnvironmentData elements))
                {
                    ChangeEnvironments(elements.m_envDic);
                    ChangeEnvironment(EnvironmentType.Sky, elements.SkyMaterial);
                }
            }
        }
        get
        {
            return m_environmentTime;
        }
    }

    public Dictionary<EnvironmentTime, EnvironmentData> EnvDic = new Dictionary<EnvironmentTime, EnvironmentData>();

    public EnvironmentManager EnvironmentMgr;
    public CloudsVolume CloudsVolumeMgr;
    public Light Light;
    public GameTimerTick TimerTick;

    /// <summary>
    /// 插值改变开放世界环境
    /// </summary>
    /// <param name="envValues"></param>
    public void ChangeEnvironments(Dictionary<EnvironmentType, Preset> envValues)
    {
        if (envValues == null)
        {
            return;
        }

        var elements = envValues.GetEnumerator();

        while (elements.MoveNext())
        {
            var key = elements.Current.Key;
            var element = elements.Current.Value;

            ChangeEnvironment(key, element);
        }

        elements.Dispose();
    }

    public void ChangeEnvironment(EnvironmentType environmentType, Preset element)
    {
        if (element == null)
        {
            Debug.LogErrorFormat("EnvironmentType {0} of Preset is null !!!",environmentType);
        }

        switch (environmentType)
        {
            case EnvironmentType.Sky:
            {
                element.ApplyTo(RenderSettings.skybox);
                break;
            }

            case EnvironmentType.Light:
            {
                element.ApplyTo(Light);
                break;
            }

            case EnvironmentType.Clouds:
            {
                element.ApplyTo(CloudsVolumeMgr);
                break;
            }

            case EnvironmentType.Windy:
            {
                element.ApplyTo(EnvironmentMgr);
                break;
            }

            case EnvironmentType.Water:
            {

                break;
            }
        }
    }

    public void ChangeEnvironment(EnvironmentType environmentType,Material material)
    {
        if (material == null)
        {
            Debug.LogErrorFormat("EnvironmentType {0} of Preset is null !!!", environmentType);
        }

        switch (environmentType)
        {
            case EnvironmentType.Sky:
            {
                RenderSettings.skybox = material;
                break;
            }
        }
    }

    [SerializeField]
    public class EnvironmentData
    {
        public Dictionary<EnvironmentType, Preset> m_envDic = new Dictionary<EnvironmentType, Preset>();
        public Material SkyMaterial;
    }
}
#endif
