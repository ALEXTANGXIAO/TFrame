using System.Collections;
using System.Collections.Generic;
using UnityEngine;

partial class GameActor
{
    protected GameObject m_goModel;
    protected Transform m_modelTrans;

    public GameObject GoModel
    {
        get { return m_goModel; }
    }

    public Transform ModelTrans
    {
        get { return m_modelTrans; }
    }
}
