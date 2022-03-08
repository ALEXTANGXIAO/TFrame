using UnityEngine;

namespace ECS
{
    public class ECSAnimatorCmpt : ECSComponent, IUpdate
    {
        public Animator Animator;

        public ECSInputCmpt EcsInputComponent;

        public override void Awake()
        {
            base.Awake();

            var actorEntity = Entity as ActorEntity;

            if (actorEntity == null)
            {
                return;
            }

            Animator = actorEntity.gameObject?.GetComponentInChildren<Animator>();

            EcsInputComponent = Entity.GetComponent<ECSInputCmpt>();
        }

        public void Move(Vector2 vector2)
        {
            Animator.SetFloat("horizontal", vector2.x);

            Animator.SetFloat("vertical", vector2.y);
        }

        public void Jump(bool isJump)
        {
            if (isJump)
            {
                Animator.SetTrigger("Jump");
            }
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
                Jump(EcsInputComponent.Jump);
            }
        }

        public override void OnDestroy()
        {
            this.Animator = null;
            this.EcsInputComponent = null;
        }
    }
}
