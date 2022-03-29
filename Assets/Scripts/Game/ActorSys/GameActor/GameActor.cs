using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract partial class GameActor
{
    public bool IsDestroyed { get; private set; }

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
}
