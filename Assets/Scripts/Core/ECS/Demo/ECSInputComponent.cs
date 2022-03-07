using UnityEngine;

namespace ECS
{
    public class ECSInputComponent : ECSComponent, IUpdate
    {
        public float Horizontal { private set; get; }
        public float Vertical { private set; get; }
        public bool Splash { private set; get; }
        public bool Jump { private set; get; }
        public void Update()
        {
            Splash = Input.GetKey(KeyCode.LeftShift);

            if (Splash)
            {
                Horizontal = Input.GetAxis("Horizontal");
            }
            else
            {
                Horizontal = Input.GetAxis("Horizontal")/2;
            }

            if (Splash)
            {
                Vertical = Input.GetAxis("Vertical");
            }
            else
            {
                Vertical = Input.GetAxis("Vertical") / 2;
            }

            Jump = Input.GetKeyDown(KeyCode.Space);
        }
    }
}