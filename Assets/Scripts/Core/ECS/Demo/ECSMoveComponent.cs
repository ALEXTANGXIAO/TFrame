using UnityEngine;

namespace ECS
{
    public class ECSMoveComponent : ECSComponent, IUpdate
    {
        public ECSInputComponent EcsInputComponent;
        private Vector3 m_CamForward;
        public ECSActor EcsActor;
        public Transform MainCameraTrans;

        public float Speed = 2f;
        public override void Awake()
        {
            base.Awake();

            EcsInputComponent = Entity.GetComponent<ECSInputComponent>();
            
            EcsActor = Entity.GetComponent<ECSActor>();

            MainCameraTrans = Camera.main.transform;
        }

        private Vector3 Move;
        float m_TurnAmount;
        float m_ForwardAmount;
        [SerializeField] float m_MovingTurnSpeed = 360;
        [SerializeField] float m_StationaryTurnSpeed = 180;
        public void Update()
        {
            if (EcsInputComponent == null || EcsActor == null)
            {
                return;
            }


            var element = Time.deltaTime * Speed;
            if (MainCameraTrans != null)
            {
                m_CamForward = Vector3.Scale(MainCameraTrans.forward, new Vector3(1, 0, 1)).normalized;
                Move = EcsInputComponent.Vertical * m_CamForward * element + EcsInputComponent.Horizontal * MainCameraTrans.right * element;
            }
            else
            {
                Move = new Vector3(EcsInputComponent.Horizontal * element, 0, EcsInputComponent.Vertical * element);
            }


            if (Move.magnitude > 1f)
            {
                Move.Normalize();
            }
            Move = EcsActor.transform.InverseTransformDirection(Move);
            Move = Vector3.ProjectOnPlane(Move, Vector3.up);

            m_TurnAmount = Mathf.Atan2(Move.x, Move.z);
            m_ForwardAmount = Move.z;

            EcsActor.transform.Translate(Move);
            ApplyExtraTurnRotation();
        }

        void ApplyExtraTurnRotation()
        {
            float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
            EcsActor.transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
        }
    }
}
