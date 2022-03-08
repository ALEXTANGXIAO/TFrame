using System;
using GameProtocol;
using UnityEngine;

public class GameDataMgr : DataCenterModule<GameDataMgr>
{
    public override void Init()
    {
        GameClient.Instance.RegActionHandle((int)ActionCode.StartGame, StartGameRes);
        GameClient.Instance.RegActionHandle((int)ActionCode.Starting, StartingRes);
    }

    private void StartingRes(MainPack mainPack)
    {
        Debug.Log(mainPack);
        if (Utils.CheckHaveError(mainPack))
        {
            return;
        }

        ActorSys.Instance.AddPlayerEntity(mainPack);
    }

    private void StartGameRes(MainPack mainPack)
    {
        Debug.Log(mainPack);
        if (Utils.CheckHaveError(mainPack))
        {
            return;
        }
    }

    public void StartGameReq()
    {
        MainPack mainPack = ProtoUtil.BuildMainPack(RequestCode.Room, ActionCode.StartGame);
        GameClient.Instance.SendCSMsg(mainPack);
    }
}