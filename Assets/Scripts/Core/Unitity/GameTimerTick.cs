using UnityEngine;

/// <summary>
/// 定时触发模块
/// </summary>
public class GameTimerTick
{
    public delegate void OnTick();

    protected OnTick m_handle;
    protected float m_lastTime = 0;
    protected float m_interval = 0;
    protected bool m_resetInterval = true;

    /// <summary>
    /// 默认 immeditate 为 true立刻触发
    /// 默认 resetInterval 为 true，每次update的间隔都一样
    /// </summary>
    /// <param name="interval">间隔时间，单位 秒</param>
    /// <param name="tickHandle"></param>
    public GameTimerTick(float interval, OnTick tickHandle)
    {
        Init(interval, true, true, tickHandle);
    }

    /// <summary>
    /// 默认 resetInterval 为 true，每次update的间隔都一样
    /// </summary>
    /// <param name="interval">间隔时间，单位 秒</param>
    /// <param name="immeditate">是否立刻触发</param>
    /// <param name="tickHandle"></param>
    public GameTimerTick(float interval, bool immeditate, OnTick tickHandle)
    {
        Init(interval, immeditate, true, tickHandle);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="interval">间隔时间，单位 秒</param>
    /// <param name="immeditate">是否立刻触发</param>
    /// <param name="resetInterval">是否重置间隔 true 为等间隔触发 false </param>
    /// <param name="tickHandle"></param>
    public GameTimerTick(float interval, bool immeditate, bool resetInterval, OnTick tickHandle)
    {
        Init(interval, immeditate, resetInterval, tickHandle);
    }
    private void Init(float interval, bool immeditate, bool resetInterval, OnTick tickHandle)
    {
        m_interval = interval;
        m_handle = tickHandle;
        m_resetInterval = resetInterval;
        if (!immeditate)
        {
            ResetTime();
        }
    }
    public void Init(float interval, OnTick tickHandle)
    {
        m_lastTime = 0;
        Init(interval, true, true, tickHandle);
    }

    public void ResetTime()
    {
        m_lastTime = GameTime.time + m_interval;
    }

    public void ChangeInterval(float interval)
    {
        m_interval = interval;
        m_resetInterval = true;
        ResetTime();
    }

    public void OnUpdate()
    {
        float now = GameTime.time + m_interval;
        if (m_lastTime + m_interval < now)
        {
            if (m_resetInterval)
            {
                m_lastTime = now;
            }
            else
            {
                if (m_lastTime == 0f)
                {
                    m_lastTime = now;
                }
                else
                {
                    m_lastTime = m_lastTime + m_interval;
                }
            }
            m_handle();
        }
    }
}


public static class GameTime
{
    public static void StartFrame()
    {
        time = Time.time;
        deltaTime = Time.deltaTime;
        quickRealTime = Time.realtimeSinceStartup;
        frameCount = Time.frameCount;
        unscaledTime = Time.unscaledTime;
    }

    public static float time;
    public static float deltaTime;
    public static int frameCount;
    public static float unscaledTime;

    public static float realtimeSinceStartup
    {
        get { return Time.realtimeSinceStartup; }
    }

    public static float quickRealTime;
}