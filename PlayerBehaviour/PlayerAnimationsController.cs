using UnityEngine;
using VotanObjectBehaviour;

namespace PlayerBehaviour
{
    /// <summary>
    /// Реализует контроль за анимацией персонажа
    /// </summary>
    public class PlayerAnimationsController 
        : AbstactObjectAnimations
    {
        /// <summary>
        /// Инициализация
        /// </summary>
        private void Start()
        {
            InitialisationAnimationClipName();
            PlayAnimation(9);
        }

        public void PlayAnimation(int animationState)
        {
            int beginAnim = (int)animationsStateList[animationState].x;
            int finishAnim = (int)animationsStateList[animationState].y;
            //Debug.Log(beginAnim + " and " + finishAnim);

            animationOfPlayer.Play("AttackSuper");
        }
    }
}
