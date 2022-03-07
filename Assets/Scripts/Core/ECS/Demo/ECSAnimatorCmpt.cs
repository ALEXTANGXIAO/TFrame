using UnityEngine;

namespace ECS
{
    public class ECSAnimatorCmpt : ECSComponent, IUpdate
    {
        public Animator Animator;

        public ECSInputComponent EcsInputComponent;
        public override void Awake()
        {
            base.Awake();
            Animator = Entity.GetComponent<ECSActor>().gameObject?.GetComponentInChildren<Animator>();

            EcsInputComponent = Entity.GetComponent<ECSInputComponent>();
        }

        public void Move(Vector2 vector2)
        {
            Animator.SetFloat("horizontal", vector2.x);
            Animator.SetFloat("vertical", vector2.y);
        }

        public void Update()
        {
            if (EcsInputComponent == null)
            {
                return;
            }
            Move(new Vector2(EcsInputComponent.Horizontal,EcsInputComponent.Vertical));

            if (EcsInputComponent.Jump)
            {
                Animator.SetTrigger("Jump");
            }
        }
    }
}
