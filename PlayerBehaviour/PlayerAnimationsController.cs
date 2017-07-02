using AbstractBehaviour;
using UnityEngine;

namespace PlayerBehaviour
{
    /// <summary>
    /// Реализует контроль за анимацией персонажа
    /// </summary>
    public class PlayerAnimationsController 
        : AbstactObjectAnimations
    {
        private void Start()
        {

        }

        public void LowSpeedAnimation()
        {
            AnimatorOfObject.speed = 0.2f;
        }

        public void HighSpeedAnimation()
        {
            AnimatorOfObject.speed = 1f;
        }

        public void SetSpeedAnimationByRunSpeed(float speed)
        {
            Debug.Log(speed);
            AnimatorOfObject.speed = speed;
        }
    }
}
