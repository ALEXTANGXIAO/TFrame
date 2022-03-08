using ECS;


public class EcsGameSystem : ECSSystem
{
    public EcsGameSystem()
    {

    }

    public void OnUpdate()
    {
        Update();
    }

    public void OnFixedUpdate()
    {
        FixedUpdate();
    }
}
