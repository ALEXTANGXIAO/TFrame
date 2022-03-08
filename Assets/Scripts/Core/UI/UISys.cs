using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISys : BaseLogicSys<UISys>
{
    public static int DesginWidth
    {
        get
        {
            //if (PcPlatformMgr.Instance.ShowPcMode)
            //{
            //    return 1600;
            //}
            return 1138;
        }
    }

    public static int DesginHeight
    {
        get
        {
            //if (PcPlatformMgr.Instance.ShowPcMode)
            //{
            //    return 900;
            //}
            return 640;
        }
    }

    public static int ScreenWidth;
    public static int ScreenHeight;
    public bool IsLandScape { private set; get; }
    private List<IUIController> m_listController = new List<IUIController>();

    public static UIManager Mgr
    {
        get { return UIManager.Instance; }
    }

    public override void OnUpdate()
    {
        UIManager.Instance.Update();
    }

    public override bool OnInit()
    {
        base.OnInit();

        ScreenWidth = Screen.width;

        ScreenHeight = Screen.height;

        IsLandScape = ScreenWidth > ScreenHeight;

        RegistAllController();

        return true;
    }

    private void RegistAllController()
    {
        //AddController<LoadingUIController>();
    }

    private void AddController<T>() where T : IUIController, new()
    {
        for (int i = 0; i < m_listController.Count; i++)
        {
            var type = m_listController[i].GetType();

            if (type == typeof(T))
            {
                Debug.LogError(string.Format("repeat controller type: {0}", typeof(T).Name));

                return;
            }
        }

        var controller = new T();

        m_listController.Add(controller);

        controller.ResigterUIEvent();
    }

    public static void ShowTipMsg(string str)
    {

    }
}
