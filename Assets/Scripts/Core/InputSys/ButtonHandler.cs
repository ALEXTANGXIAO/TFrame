using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandler : MonoBehaviour
{

    public string Name;

    void OnEnable()
    {

    }

    public void SetDownState()
    {
        InputSys.SetButtonDown(Name);
    }


    public void SetUpState()
    {
        InputSys.SetButtonUp(Name);
    }


    public void SetAxisPositiveState()
    {
        InputSys.SetAxisPositive(Name);
    }


    public void SetAxisNeutralState()
    {
        InputSys.SetAxisZero(Name);
    }


    public void SetAxisNegativeState()
    {
        InputSys.SetAxisNegative(Name);
    }

    public void Update()
    {

    }
}
