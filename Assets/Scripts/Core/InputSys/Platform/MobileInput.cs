using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileInput : VirtualInput
{
    private void AddButton(string name)
    {
        InputSys.RegisterVirtualButton(new InputSys.VirtualButton(name));
    }


    private void AddAxes(string name)
    {
        InputSys.RegisterVirtualAxis(new InputSys.VirtualAxis(name));
    }


    public override float GetAxis(string name, bool raw)
    {
        if (!m_VirtualAxes.ContainsKey(name))
        {
            AddAxes(name);
        }
        return m_VirtualAxes[name].GetValue;
    }


    public override void SetButtonDown(string name)
    {
        if (!m_VirtualButtons.ContainsKey(name))
        {
            AddButton(name);
        }
        m_VirtualButtons[name].Pressed();
    }


    public override void SetButtonUp(string name)
    {
        if (!m_VirtualButtons.ContainsKey(name))
        {
            AddButton(name);
        }
        m_VirtualButtons[name].Released();
    }


    public override void SetAxisPositive(string name)
    {
        if (!m_VirtualAxes.ContainsKey(name))
        {
            AddAxes(name);
        }
        m_VirtualAxes[name].Update(1f);
    }


    public override void SetAxisNegative(string name)
    {
        if (!m_VirtualAxes.ContainsKey(name))
        {
            AddAxes(name);
        }
        m_VirtualAxes[name].Update(-1f);
    }


    public override void SetAxisZero(string name)
    {
        if (!m_VirtualAxes.ContainsKey(name))
        {
            AddAxes(name);
        }
        m_VirtualAxes[name].Update(0f);
    }


    public override void SetAxis(string name, float value)
    {
        if (!m_VirtualAxes.ContainsKey(name))
        {
            AddAxes(name);
        }
        m_VirtualAxes[name].Update(value);
    }


    public override bool GetButtonDown(string name)
    {
        if (m_VirtualButtons.ContainsKey(name))
        {
            //Debug.Log(name+ "  m_VirtualButtons[name].GetButtonDown: "+ m_VirtualButtons[name].GetButtonDown);
            return m_VirtualButtons[name].GetButtonDown;
        }

        AddButton(name);
        return m_VirtualButtons[name].GetButtonDown;
    }


    public override bool GetButtonUp(string name)
    {
        if (m_VirtualButtons.ContainsKey(name))
        {
            return m_VirtualButtons[name].GetButtonUp;
        }

        AddButton(name);
        return m_VirtualButtons[name].GetButtonUp;
    }


    public override bool GetButton(string name)
    {
        if (m_VirtualButtons.ContainsKey(name))
        {
            return m_VirtualButtons[name].GetButton;
        }

        AddButton(name);
        return m_VirtualButtons[name].GetButton;
    }


    public override Vector3 MousePosition()
    {
        return virtualMousePosition;
    }
}
