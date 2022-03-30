using UnityEngine;

public abstract partial class GameActor
{
    public uint ActorID;

    public bool IsDestroyed { get; private set; }

    //protected SkillCaster m_skillCaster;

    private ActorEventDispatcher m_event = new ActorEventDispatcher();

    public ActorEventDispatcher Event
    {
        get { return m_event; }
    }
    public string Name
    {
        get { return GetActorName(); }
    }

    public Vector3 Position
    {
        get { return m_modelTrans.position; }
        set
        {
            m_modelTrans.position = value;
        }
    }

    public virtual string GetActorName()
    {
        return "UN-NAMED";
    }

    public abstract ActorType GetActorType();


    public void Destroy(bool forceNoPool = false)
    {
        //var skillCaster = GetSkillCaster();
        //if (skillCaster != null)
        //{
        //    skillCaster.ClearSkill();
        //}
        m_isDestroyAlling = true;

        //调用所有的destroy
        BeforeDestroyAllCmpt();

        //最后再清理内存
        DestroyAllCmpt();

        if (m_goModel != null)
        {
            if (forceNoPool)
            {
                //DodUtil.SafeDestroyForceNoPool(m_goModel);
                Object.Destroy(m_goModel);
            }
            else
            {
                Object.Destroy(m_goModel);
            }

            m_goModel = null;
            m_modelTrans = null;
        }

        IsDestroyed = true;
        m_isDestroyAlling = false;
    }

    //public SkillCaster GetSkillCaster()
    //{
    //    return m_skillCaster;
    //}
}
