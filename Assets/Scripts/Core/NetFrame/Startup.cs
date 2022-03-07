using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Startup : MonoBehaviour
{

    public GameObject Game;

    public static GameObject GameIns;

    void Start()
    {
        if (null == GameIns)
        {
            GameIns = GameObject.Instantiate(Game);
        }
        GameIns.GetComponent<Game>().Reset();
    }
}
