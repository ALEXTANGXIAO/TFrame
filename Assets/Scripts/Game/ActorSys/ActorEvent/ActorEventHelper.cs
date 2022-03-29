using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ActorEventHelper
{
    public static void SendOnCtrlMoveTo(GameActor actor, Vector3 pos)
    {
        actor.Event.SendEvent(ActorEventType.OnCtrlMoveTo, pos);
    }

    public static void SendMoveChange(GameActor actor, bool moving)
    {
        actor.Event.SendEvent(ActorEventType.ActorMoveChangeEvent, moving);
    }

    public static void SendDoSkill(GameActor actor, uint skillId)
    {
        actor.Event.SendEvent(ActorEventType.DoSkill, skillId);
    }

    public static void SendSvrMoveSetPos(GameActor actor, Vector3 pos, Vector3 forward)
    {
        actor.Event.SendEvent(ActorEventType.SVR_MOVE_SET_CURR_POS, pos, forward);
    }

    public static void SendSvrStartMovePath(GameActor actor, List<Vector3> movePath, float elapseTime)
    {
        actor.Event.SendEvent(ActorEventType.SVR_MOVE_START_MOVE, movePath, elapseTime);
    }

    public static void SendResetDisplayInfo(GameActor actor)
    {
        actor.Event.SendEvent(ActorEventType.ResetDisplayInfo);
    }
}
