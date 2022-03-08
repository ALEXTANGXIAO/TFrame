using System;
using System.Collections;
using System.Collections.Generic;
using ECS;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class GamePlayEcs : MonoBehaviour
{
    public EcsGameSystem EcsGameSystem = new EcsGameSystem();
    #region 输入
    public float Horizontal { private set; get; }
    public float Vertical { private set; get; }
    public bool Splash { private set; get; }
    public bool Jump { private set; get; }
    #endregion


    public GameObject Prefab;
    public List<GameObject> Objs;
    public Dictionary<ulong, GameObject> Players = new Dictionary<ulong, GameObject>();

    private uint NextEventFrame = 60;
    private List<GameObject> objs = new List<GameObject>();
    void Awake()
    {
        MainCameraTrans = Camera.main.transform;
        MonoManager.Instance.AddUpdateListener(EcsGameSystem.OnUpdate);
        MonoManager.Instance.AddFixedUpdateListener(EcsGameSystem.OnFixedUpdate);

        var data = Game.Instance.Logic.Data;
        foreach (var kv in data.Players)
        {
            var obj = Object.Instantiate(Prefab, Vector3.zero, Quaternion.identity);
            var entity = EcsFactory.Instance.CreateActorEntity(ActorType.PlayerActor, obj, true);
            if (kv.Key == data.MyID)
            {
                
            }
            entity.AddComponent<ECSMoveCmpt>();
            entity.AddComponent<ECSAnimatorCmpt>();
            kv.Value.ActorEntity = entity;
            Players[kv.Key] = obj;
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

    private float m_Speed = 3f;
    public Transform MainCameraTrans;
    private Vector3 m_CamForward;
    private Vector3 Move;

    void Update()
    {
        var d = Game.Instance.Logic.Data;

        Splash = Input.GetKey(KeyCode.LeftShift);

        if (Splash)
        {
            Horizontal = InputSys.GetAxis("Horizontal");
        }
        else
        {
            Horizontal = InputSys.GetAxis("Horizontal") / 2;
        }

        if (Splash)
        {
            Vertical = InputSys.GetAxis("Vertical");
        }
        else
        {
            Vertical = InputSys.GetAxis("Vertical") / 2;
        }

        var speed = Time.deltaTime * m_Speed;

        if (MainCameraTrans != null)
        {
            m_CamForward = Vector3.Scale(MainCameraTrans.forward, new Vector3(1, 0, 1)).normalized;

            Move = Vertical * m_CamForward * speed + Horizontal * MainCameraTrans.right * speed;
        }
        else
        {
            Move = new Vector3(Horizontal * speed, 0, Vertical * speed);
        }

        if (Move.magnitude > 1f)
        {
            Move.Normalize();
        }

        var msg = pb.C2S_InputMsg.CreateBuilder();
        msg.SetX(Move.x);
        msg.SetY(Move.y);
        msg.SetZ(Move.z);
        Network.Instance.Send(pb.ID.MSG_Input, msg.Build());


        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    var msg = pb.C2S_InputMsg.CreateBuilder();
        //    msg.SetSid(InputDefined.Forward);
        //    Network.Instance.Send(pb.ID.MSG_Input, msg.Build());
        //}
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    var msg = pb.C2S_InputMsg.CreateBuilder();
        //    msg.SetSid(InputDefined.Back);
        //    Network.Instance.Send(pb.ID.MSG_Input, msg.Build());
        //}
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    var msg = pb.C2S_InputMsg.CreateBuilder();
        //    msg.SetSid(InputDefined.Left);
        //    Network.Instance.Send(pb.ID.MSG_Input, msg.Build());
        //}
        //if (Input.GetKeyDown(KeyCode.D))
        //{
        //    var msg = pb.C2S_InputMsg.CreateBuilder();
        //    msg.SetSid(InputDefined.Right);
        //    Network.Instance.Send(pb.ID.MSG_Input, msg.Build());
        //}
    }


    void TickFrame(uint a, GameData b)
    {
        //if (a >= NextEventFrame)
        //{

        //    NextEventFrame = a + (uint)Random.Range(10, 40);
        //    var x = Random.Range(-8, 8);
        //    var z = Random.Range(-8, 8);

        //    var o = GameObject.Instantiate(Objs[Random.Range(0, Objs.Count - 1)], new Vector3(x, 0, z), Quaternion.identity);
        //    o.name = a.ToString();
        //    objs.Add(o);
        //}
        //for (int i = 0; i < objs.Count;)
        //{
        //    var n = UInt64.Parse(objs[i].name);

        //    if (a > n + 60)
        //    {
        //        GameObject.Destroy(objs[i]);
        //        objs.RemoveAt(i);
        //        continue;
        //    }
        //    i++;
        //}
    }
}
