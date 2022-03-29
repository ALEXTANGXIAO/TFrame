using System.Collections.Generic;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public interface IMemPoolObject
{
    void Init();
    void Destroy();
}

public interface MemPoolBase
{
    string GetName();
    int GetPoolItemCount();
    void ClearPool();
}

public class MemPool 
{
    
}


public class MemPoolMgr : Singleton<MemPoolMgr>
{
    List<MemPoolBase> m_listPool = new List<MemPoolBase>();

    [Conditional("DOD_DEBUG")]
    public void ShowCount()
    {
        int totalCnt = 0;
        for (int i = 0; i < m_listPool.Count; i++)
        {
            var pool = m_listPool[i];
            totalCnt += pool.GetPoolItemCount();
            Debug.LogFormat("[pool][{0}] [{1}]", pool.GetName(), pool.GetPoolItemCount());
        }
        Debug.LogFormat("-------------------------memory pool count: {0}", totalCnt);
    }

    public void RegMemPool(MemPoolBase pool)
    {
        //BLogger.Assert(!m_listPool.Contains(pool));
        m_listPool.Add(pool);
    }

    public void ClearAllPool()
    {
        for (int i = 0; i < m_listPool.Count; i++)
        {
            var pool = m_listPool[i];
            pool.ClearPool();
        }
    }
}

public class GameMemPool<T> : Singleton<GameMemPool<T>>, MemPoolBase where T : IMemPoolObject, new()
{
    private List<T> m_objPool = new List<T>();

    public static T Alloc()
    {
        return GameMemPool<T>.Instance.DoAlloc();
    }

    public static void Free(T obj)
    {
        GameMemPool<T>.Instance.DoFree(obj);
    }

    public GameMemPool()
    {
        MemPoolMgr.Instance.RegMemPool(this);
    }

    private T DoAlloc()
    {
        T newObj;
        if (m_objPool.Count > 0)
        {
            var lastIndex = m_objPool.Count - 1;
            newObj = m_objPool[lastIndex];
            m_objPool.RemoveAt(lastIndex);
        }
        else
        {
            newObj = new T();
        }

        newObj.Init();
        return newObj;
    }

    private void DoFree(T obj)
    {
        if (obj == null)
        {
            return;
        }

        obj.Destroy();
        m_objPool.Add(obj);
    }

    public void ClearPool()
    {
#if UNITY_EDITOR
        Debug.LogFormat("clear memory[{0}] count[{1}]", GetName(), m_objPool.Count);
#endif
        m_objPool.Clear();
    }

    public string GetName()
    {
        return typeof(T).FullName;
    }

    public int GetPoolItemCount()
    {
        return m_objPool.Count;
    }
}