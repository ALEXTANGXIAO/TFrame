using System.Collections;
using System.Collections.Generic;
using ECS;
using UnityEngine;

public static class ECSEventHelper 
{
    public static void Send(Entity entity, int eventId)
    {
        entity.Event?.Send(eventId);
    }

    public static void Send<T>(Entity entity, int eventId,T info)
    {
        entity.Event?.Send<T>(eventId,info);
    }

    public static void Send<T,U>(Entity entity, int eventId, T info1, U info2)
    {
        entity.Event?.Send<T,U>(eventId,info1,info2);
    }

    public static void Send<T,U,V>(Entity entity, int eventId, T info1, U info2,V info3)
    {
        entity.Event?.Send<T,U,V>(eventId,info1,info2,info3);
    }
}
