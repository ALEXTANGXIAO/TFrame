using GameProtocol;

public class ActorSys : BaseLogicSys<ActorSys>
{

    public void UpPos(MainPack pack)
    {
        if (pack == null)
        {
            return;
        }

        PosPack posPack = pack.Playerpack[0].PosPack;

        //if (onlineActors.TryGetValue(pack.Playerpack[0].Playername, out ActorOnlineCmpt cmpt))
        //{
        //    if (cmpt == null)
        //    {
        //        return;
        //    }
        //    cmpt.UpdatePosPack(posPack);
        //}
    }
}
