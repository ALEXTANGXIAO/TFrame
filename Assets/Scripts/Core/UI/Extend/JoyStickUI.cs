using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum JoyStickType
{
    Normal,

    HideMove,
}
public class JoyStickUI : UIWindow
{
    public float maxl = 50;
    public JoyStickType type = JoyStickType.HideMove;
    private Image touchRect;
    private Image Img_bg;
    private Image Img_btn;

    private Image moveRect;
    public string horizontalAxisName = "Horizontal";
    public string verticalAxisName = "Vertical";
    InputSys.VirtualAxis m_HorizontalVirtualAxis;
    InputSys.VirtualAxis m_VerticalVirtualAxis;
    protected override void ScriptGenerator()
    {
        touchRect = FindChildComponent<Image>("touchRect");
        Img_bg = FindChildComponent<Image>("touchRect/Img_bg");
        Img_btn = FindChildComponent<Image>("touchRect/Img_bg/Img_btn");
        moveRect = FindChildComponent<Image>("moveRect");

        InputSys.SwitchActiveInputMethod(InputSys.ActiveInputMethod.Touch);
        if (type != JoyStickType.Normal)
        {
            Img_bg.gameObject.SetActive(false);
        }
    }

    protected override void RegisterEvent()
    {
        UIManager.AddCustomEventListener(touchRect, EventTriggerType.PointerDown, PointerDown);
        UIManager.AddCustomEventListener(touchRect, EventTriggerType.PointerUp, PointerUp);
        UIManager.AddCustomEventListener(touchRect, EventTriggerType.Drag, Drag);
        UIManager.AddCustomEventListener(moveRect, EventTriggerType.PointerDown, MovePointerDown);
        UIManager.AddCustomEventListener(moveRect, EventTriggerType.PointerUp, MovePointerUp);
        UIManager.AddCustomEventListener(moveRect, EventTriggerType.Drag, MoveDrag);
        CreateVirtualAxes();
    }
    void UpdateVirtualAxes(Vector3 value)
    {
        m_HorizontalVirtualAxis.Update(value.x);
        m_VerticalVirtualAxis.Update(value.y);
    }

    void CreateVirtualAxes()
    {
        m_HorizontalVirtualAxis = new InputSys.VirtualAxis(horizontalAxisName);
        InputSys.RegisterVirtualAxis(m_HorizontalVirtualAxis);
        m_VerticalVirtualAxis = new InputSys.VirtualAxis(verticalAxisName);
        InputSys.RegisterVirtualAxis(m_VerticalVirtualAxis);
    }
    private void MoveDrag(BaseEventData data)
    {
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            Img_bg.rectTransform,                                       //父对象
            (data as PointerEventData).position,               //当前屏幕鼠标位置
            (data as PointerEventData).pressEventCamera,                //UI摄像机
            out localPos);                                              //相对坐标

        localPos = localPos.normalized;

        InputSys.SetAxis("Mouse X", localPos.x * 2);
        InputSys.SetAxis("Mouse Y", localPos.y/3);
    }

    private void MovePointerUp(BaseEventData data)
    {
        InputSys.SetAxis("Mouse X", 0);
        InputSys.SetAxis("Mouse Y", 0);
        //EventCenter.Instance.EventTrigger<Vector2>(InputEvent.JoyScreenMove, Vector2.zero);
    }

    private void MovePointerDown(BaseEventData data)
    {

    }

    #region touchRect
    private void PointerDown(BaseEventData data)
    {
        Img_bg.gameObject.SetActive(true);

        if (type != JoyStickType.Normal)
        {
            Vector2 localPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                touchRect.rectTransform,                                       //父对象
                (data as PointerEventData).position,               //当前屏幕鼠标位置
                (data as PointerEventData).pressEventCamera,                //UI摄像机
                out localPos);
            Img_bg.transform.localPosition = localPos; //相对坐标
        }
    }

    private void PointerUp(BaseEventData data)
    {
        Img_btn.transform.localPosition = Vector3.zero;
        //EventCenter.Instance.EventTrigger<Vector2>(InputEvent.JoyStickMove, Vector2.zero);
        if (type != JoyStickType.Normal)
        {
            Img_bg.gameObject.SetActive(false);
        }
        UpdateVirtualAxes(Vector3.zero);
    }

    private void Drag(BaseEventData data)
    {
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            Img_bg.rectTransform,                                       //父对象
            (data as PointerEventData).position,               //当前屏幕鼠标位置
            (data as PointerEventData).pressEventCamera,                //UI摄像机
            out localPos);                                              //相对坐标

        Img_btn.transform.localPosition = localPos;

        if (localPos.magnitude > maxl)
        {
            Img_btn.transform.localPosition = localPos.normalized * maxl;
        }
        //EventCenter.Instance.EventTrigger<Vector2>(InputEvent.JoyStickMove, localPos.normalized);

        UpdateVirtualAxes(localPos.normalized);
    }
    #endregion
}
