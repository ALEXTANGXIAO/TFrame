using UnityEngine;

namespace ECS
{
    public class ECSInputComponent : ECSComponent, IUpdate
    {
        public float Horizontal { private set; get; }
        public float Vertical { private set; get; }
        public bool Splash { private set; get; }
        public void Update()
        {
            Splash = Input.GetKeyDown(KeyCode.LeftShift);

            if (Splash)
            {
                Horizontal = Input.GetAxis("Horizontal");
            }
            else
            {
                Horizontal = Input.GetAxis("Horizontal")/2;
            }

            Vertical = Input.GetAxis("Vertical");
        }
    }
}