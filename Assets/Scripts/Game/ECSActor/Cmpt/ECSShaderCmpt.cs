using System.Collections;
using System.Collections.Generic;
using ECS;
using UnityEngine;

public enum EntityBody
{
    Body_A = 1,
    Face_A = 2,
    Head_A = 3,
    Horn_A_part1 = 4,
    Horn_A_part2 = 5,
    Tail_A = 6,
    Teeth_A = 7,
}

public class ECSShaderCmpt: ECSComponent
{
    /// <summary>
    /// 各种类型的模型
    /// key => 模型类型
    /// val => 模型实例
    /// </summary>
    private readonly Dictionary<EntityBody, Renderer> m_dicRender = new Dictionary<EntityBody, Renderer>();

    public override void Awake()
    {
        var actorEntity = Entity as ActorEntity;

        if (actorEntity == null || actorEntity.gameObject == null)
        {
            return;
        }

        Renderer[] renderers = actorEntity.gameObject.GetComponentsInChildren<Renderer>(true);

        if (renderers != null && renderers.Length > 0)
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                var render = renderers[i];
                if (render == null)
                {
                    continue;
                }
                m_dicRender.Add((EntityBody)(i+1),render);
            }
        }

        Entity.Event.AddEventListener<EntityBody, Material>(ActorEventDefine.SetMaterial, SetMaterial);
        Entity.Event.AddEventListener<Material>(ActorEventDefine.SetBodyMaterial, SetBodyMaterial);
        Entity.Event.AddEventListener<Material>(ActorEventDefine.SetFaceMaterial, SetFaceMaterial);
    }

    public override void OnDestroy()
    {
        m_dicRender.Clear();

        Entity.Event.RemoveEventListener<EntityBody, Material>(ActorEventDefine.SetMaterial, SetMaterial);
        Entity.Event.RemoveEventListener<Material>(ActorEventDefine.SetBodyMaterial, SetBodyMaterial);
        Entity.Event.RemoveEventListener<Material>(ActorEventDefine.SetFaceMaterial, SetFaceMaterial);
    }

    public void SetMaterial(EntityBody element, Material material)
    {
        if (m_dicRender.TryGetValue(element,out Renderer renderer))
        {
            renderer.sharedMaterial = material;
        }
    }

    public void SetBodyMaterial(Material material)
    {
        var elm = m_dicRender.GetEnumerator();
        while (elm.MoveNext())
        {
            var type = elm.Current.Key;
            if (type == EntityBody.Face_A)
            {
                continue;
            }
            var renderer = elm.Current.Value;
            renderer.sharedMaterial = material;
        }
    }

    public void SetFaceMaterial(Material material)
    {
        var elm = m_dicRender.GetEnumerator();
        while (elm.MoveNext())
        {
            var type = elm.Current.Key;
            if (type != EntityBody.Face_A)
            {
                continue;
            }
            var renderer = elm.Current.Value;
            renderer.sharedMaterial = material;
        }
    }
}
