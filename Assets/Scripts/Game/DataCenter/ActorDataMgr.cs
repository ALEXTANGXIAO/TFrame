using GameProtocol;
using UnityEngine;

public class ActorDataMgr : DataCenterModule<ActorDataMgr>
{
    private bool IsUDP = true;

    public override void Init()
    {
        GameClient.Instance.UdpRegActionHandle((int)ActionCode.UpPos, UpPosRes);
        CachePack();
    }


    private void UpPosRes(MainPack mainPack)
    {
        if (Utils.CheckHaveError(mainPack))
        {
            return;
        }
        ActorSys.Instance.UpPos(mainPack);
    }

    public void UpCachePosReq(Vector3 pos, float dir,Vector2 move,int animation = 0)
    {
        m_mainPack.Playerpack[0].PosPack.PosX = pos.x;
        m_mainPack.Playerpack[0].PosPack.PosY = pos.y;
        m_mainPack.Playerpack[0].PosPack.PosZ = pos.z;

        m_mainPack.Playerpack[0].PosPack.RotaX = move.x;
        m_mainPack.Playerpack[0].PosPack.RotaY = move.y;
        //m_mainPack.Playerpack[0].PosPack.RotaZ = rotation.z;

        m_mainPack.Playerpack[0].PosPack.Dirt = dir;

        m_mainPack.Playerpack[0].PosPack.Animation = animation;

        m_mainPack.Playerpack[0].Playername = LoginDataMgr.Instance.m_userName;
        m_mainPack.User = LoginDataMgr.Instance.m_userName;
        if (IsUDP)
        {
            GameClient.Instance.SendCSMsgUdp(m_mainPack);
        }
        else
        {
            GameClient.Instance.SendCSMsg(m_mainPack);
        }
    }

    private bool m_setName;
    private MainPack m_mainPack;
    private void CachePack()
    {
        m_mainPack = ProtoUtil.BuildMainPack(RequestCode.Game, ActionCode.UpPos);
        PosPack posPack = new PosPack();
        PlayerPack playerPack = new PlayerPack();
        playerPack.PosPack = posPack;
        m_mainPack.Playerpack.Add(playerPack);
    }
}