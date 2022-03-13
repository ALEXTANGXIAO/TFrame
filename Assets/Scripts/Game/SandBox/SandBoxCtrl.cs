using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandBoxCtrl : MonoBehaviour
{
    public bool SandboxMode;

    // 储存的检测到地形(由于地形可能由多种不同的数据类型构成,比如游戏对象gameobject或者地形对象terrain,这两种对象在Unity中是有区别的,所以我们使用var来声明变量,这样它可以储存不同类型对象）
    public Terrain Terrain;

    public GameObject HitGameObject;

    // 所选择建筑物的索引,可以用用户输入等方式获取
    public int SelectedBuilding;

    public List<GameObject> BuildingList = new List<GameObject>();

    public List<string> NotBuildableTerrainMarks = new List<string>();

    public float Distance = 10f;

    void Start()
    {
        
    }

    void Update()
    {
        //如果没有进入沙盒系统则退出

        if (!SandboxMode)
        {
            return;
        }

        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }

        Ray r = Camera.main.ScreenPointToRay(new Vector2(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2));

        //储存我们所需要的碰撞信息

        RaycastHit info;

        if (Physics.Raycast(r,out info, Distance))
        {
            HitGameObject = info.collider.gameObject;

            Terrain = info.collider.gameObject.GetComponent<Terrain>();

            if (Terrain == null)
            {
                return;
            }

            Vector3 buildpos = new Vector3(info.point.x, info.point.y + 0.5f, info.point.z); 

            //notBuildaleTerrainMarks是sandbox base中的一个string列表,储存着所有不可以把建筑物建造在上面的地形类型的tag,通过terrain与其的循环比对,检测出该地形是不是可以建造建筑物

            foreach (string notag in NotBuildableTerrainMarks)
            {

                if (Terrain.tag == notag)
                {
                    Debug.Log("can't build here!");

                    return;
                }
            }

            GameObject buildingToBuild = BuildingList[SelectedBuilding];

            //如果按下鼠标左键就完成建造,但是这里的建造要多费点心,我们要去观察一下建筑物游戏对象的轴点在哪里,如果在其正中央,在生成的时候就要把buildpos.y再加上一个建筑物游戏对象高度的一半(因为生成的时候Unity是按照建筑物对象的轴点来生成的).

            if (Input.GetMouseButtonDown(0))
            {
                Instantiate(buildingToBuild, buildpos, transform.rotation);
            }
        }
    }
}
