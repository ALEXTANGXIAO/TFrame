using System;
using System.Collections;
using GameProtocol;
using UnityEngine;

public class LoginDataMgr : DataCenterModule<LoginDataMgr>
{
    public string m_userName;
    public override void Init()
    {
        GameClient.Instance.RegActionHandle((int)ActionCode.Login, LoginRes);
        GameClient.Instance.RegActionHandle((int)ActionCode.Register, RegisterRes);
    }

    private void LoginRes(MainPack mainPack)
    {
        if (Utils.CheckHaveError(mainPack))
        {
            return;
        }
        m_userName = mainPack.LoginPack.Username;

        UISys.Mgr.CloseWindow<LoginMainUI>();
        Debug.Log(mainPack);
        RoomDataMgr.Instance.JoinRoomReq("1");
        //UISys.Mgr.ShowWindow<RoomUI>();
        //ScenesManager.Instance.LoadSceneAsyn("08 - japan", () => { AudioMgr.Instance.PlayBackGroundAudio("Prologue"); });
    }

    private void RegisterRes(MainPack mainPack)
    {
        Debug.Log(mainPack);
        if (Utils.CheckHaveError(mainPack))
        {
            return;
        }
    }


    public void LoginReq(string user, string pass)
    {
        if (Connect(user, pass) == false)
        {
            return;
        }


        if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
        {
            return;
        }

        MainPack mainPack = ProtoUtil.BuildMainPack(RequestCode.User, ActionCode.Login);
        LoginPack loginPack = new LoginPack();
        loginPack.Username = user;
        loginPack.Password = pass;
        mainPack.LoginPack = loginPack;

        GameClient.Instance.SendCSMsg(mainPack);
    }

    public void ResigterReq(string user, string pass)
    {
        if (Connect(user, pass) == false)
        {
            return;
        }

        if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
        {
            return;
        }

        MainPack mainPack = ProtoUtil.BuildMainPack(RequestCode.User, ActionCode.Register);
        LoginPack loginPack = new LoginPack();
        loginPack.Username = user;
        loginPack.Password = pass;
        mainPack.LoginPack = loginPack;

        GameClient.Instance.SendCSMsg(mainPack);
    }

    public bool Connect(string user, string pass)
    {
        if (GameClient.Instance.Status != GameClientStatus.StatusConnect)
        {
            GameClient.Instance.Connect();
            GameClient.Instance.ConnectUdp();

            MonoManager.Instance.StartCoroutine(AsyncConnect(user, pass, (() => { GameClient.Instance.Connect(); })));

            return false;
        }

        return true;
    }

    IEnumerator AsyncConnect(string user, string pass, Action action)
    {
        yield return action;

        MainPack mainPack = ProtoUtil.BuildMainPack(RequestCode.User, ActionCode.Login);
        LoginPack loginPack = new LoginPack();
        loginPack.Username = user;
        loginPack.Password = pass;
        mainPack.LoginPack = loginPack;

        GameClient.Instance.SendCSMsg(mainPack);
    }

}
