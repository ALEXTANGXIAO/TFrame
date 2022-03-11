using UnityEngine;

public class UnitySingleton<T> : MonoBehaviour where T :MonoBehaviour
{
    private static T m_Instance;

    public static T Instance
    {
        get
        {
            if (m_Instance == null)
            {
                GameObject obj = new GameObject();
                m_Instance = obj.AddComponent<T>();
                obj.hideFlags = HideFlags.DontSave;
#if UNITY_EDITOR
                obj.name = string.Format("Core:{0}", typeof(T).Name);
#else
                
#endif
            }
            return m_Instance;
        }
    }

    public virtual void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (m_Instance == null)
        {
            m_Instance = this as T;
        }
        else
        {
            Object.Destroy(this.gameObject);
        }
    }

    public static bool Init
    {
        get
        {
            if (m_Instance == null)
            {
                GameObject obj = new GameObject();
                m_Instance = obj.AddComponent<T>();
                obj.hideFlags = HideFlags.DontSave;
#if UNITY_EDITOR
                obj.name = string.Format("Core:{0}", typeof(T).Name);
#else
                
#endif
            }
            return true;
        }
    }
}