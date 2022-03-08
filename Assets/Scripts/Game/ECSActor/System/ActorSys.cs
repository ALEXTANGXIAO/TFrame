﻿using System.Collections.Generic;
using ECS;
using GameProtocol;
using UnityEngine;

public class ActorSys : BaseLogicSys<ActorSys>
{
    private Dictionary<string,ActorEntity> playerActors = new Dictionary<string, ActorEntity>();
    private Dictionary<string, ActorOnlineCmpt> onlineActors = new Dictionary<string, ActorOnlineCmpt>();
    public GameObject PlayerPerfab;

    public ActorSys()
    {
        PlayerPerfab = Resources.Load("Perfab/ActorEntity") as GameObject;
    }

    public void AddPlayerEntity(MainPack mainPack)
    {
        if (mainPack == null)
        {
            return;
        }

        foreach (var pack in mainPack.Playerpack)
        {
            if (playerActors.ContainsKey(pack.Playername))
            {
                continue;
            }

            var gameObject = Object.Instantiate(PlayerPerfab);
            var entity = EcsFactory.Instance.CreateActorEntity(ActorType.PlayerActor, gameObject);
            if (pack.Playername.Equals(LoginDataMgr.Instance.m_userName))
            {
                entity.AddComponent<ECSInputCmpt>();
                entity.AddComponent<ActorPosCmpt>();
                gameObject.tag = "Player";
            }
            else
            {
                onlineActors.Add(pack.Playername, entity.AddComponent<ActorOnlineCmpt>());
            }
            entity.AddComponent<ECSMoveCmpt>();
            entity.AddComponent<ECSAnimatorCmpt>();

            playerActors.Add(pack.Playername,entity);
        }
    }

    public void UpPos(MainPack mainPack)
    {
        if (mainPack == null)
        {
            return;
        }

        PosPack posPack = mainPack.Playerpack[0].PosPack;

        if (onlineActors.TryGetValue(mainPack.Playerpack[0].Playername, out ActorOnlineCmpt cmpt))
        {
            if (cmpt == null)
            {
                return;
            }
            cmpt.UpdatePosPack(posPack);
        }
    }
}
