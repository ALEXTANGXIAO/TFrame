using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GamePlay : MonoBehaviour
{
    public GameObject Prefab;
    public List<GameObject> Objs;
    public Dictionary<ulong, GameObject> Players = new Dictionary<ulong, GameObject>();

    private uint NextEventFrame = 60;
    private List<GameObject> objs = new List<GameObject>();
    void Awake()
    {
        var d = Game.Instance.Logic.Data;
        foreach (var kv in d.Players)
        {
            Players[kv.Key] = GameObject.Instantiate(Prefab, Vector3.zero, Quaternion.identity);
            Players[kv.Key].name = kv.Key.ToString();
        }
    }

    void Start()
    {
        Game.Instance.Callback += TickFrame;
    }

    void OnDestroy()
    {
        Game.Instance.Callback -= TickFrame;
    }

    void Update()
    {
        var d = Game.Instance.Logic.Data;
        foreach (var playerData in d.Players)
        {
            GameObject o = null;
            if (Players.TryGetValue(playerData.Key, out o))
            {
                o.transform.localPosition = Vector3.Lerp(o.transform.localPosition, playerData.Value.Pos, Time.deltaTime * 20);
                //o.transform.localPosition = playerData.Value.Pos;
            }
        }

        if (false)
        {
            var msg = pb.C2S_InputMsg.CreateBuilder();
            msg.SetX(1f);
            msg.SetY(1f);
            msg.SetZ(1f);
            Network.Instance.Send(pb.ID.MSG_Input, msg.Build());
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            var msg = pb.C2S_InputMsg.CreateBuilder();
            msg.SetSid(InputDefined.Forward);
            Network.Instance.Send(pb.ID.MSG_Input, msg.Build());
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            var msg = pb.C2S_InputMsg.CreateBuilder();
            msg.SetSid(InputDefined.Back);
            Network.Instance.Send(pb.ID.MSG_Input, msg.Build());
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            var msg = pb.C2S_InputMsg.CreateBuilder();
            msg.SetSid(InputDefined.Left);
            Network.Instance.Send(pb.ID.MSG_Input, msg.Build());
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            var msg = pb.C2S_InputMsg.CreateBuilder();
            msg.SetSid(InputDefined.Right);
            Network.Instance.Send(pb.ID.MSG_Input, msg.Build());
        }
    }


    void TickFrame(uint a, GameData b)
    {
        if (a >= NextEventFrame)
        {

            NextEventFrame = a + (uint)Random.Range(10, 40);
            var x = Random.Range(-8, 8);
            var z = Random.Range(-8, 8);

            var o = GameObject.Instantiate(Objs[Random.Range(0, Objs.Count - 1)], new Vector3(x, 0, z), Quaternion.identity);
            o.name = a.ToString();
            objs.Add(o);
        }
        for (int i = 0; i < objs.Count;)
        {
            var n = UInt64.Parse(objs[i].name);

            if (a > n + 60)
            {
                GameObject.Destroy(objs[i]);
                objs.RemoveAt(i);
                continue;
            }
            i++;
        }
    }
}
