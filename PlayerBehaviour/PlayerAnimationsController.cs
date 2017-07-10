using System.Collections.Generic;
using AbstractBehaviour;
using Playerbehaviour;
using UnityEngine;
using MovementEffects;

namespace PlayerBehaviour
{
    /// <summary>
    /// Реализует контроль за анимацией персонажа
    /// </summary>
    public class PlayerAnimationsController 
        : AbstactObjectAnimations
    {
        [SerializeField,Tooltip("Хранитель компонентов")]
        private PlayerComponentsControl playerComponentsControl;

        /// <summary>
        /// Конструктор
        /// </summary>
        PlayerAnimationsController()
        {
            StructStatesNames = new StructStatesNames("isRunning",
                "isFighting","isDefensing","isLongAttack","isDamage",
                "isDead");
        }

        /// <summary>
        /// Задаем значение состоянию анимации персонажа.
        /// 0 - бег, 1 - кручение, 2 - защита, 
        /// 3 - рывок, 4 - урон, 5 - смерть
        /// </summary>
        /// <param name="state"></param>
        /// <param name="flag"></param>
        public override void SetState(byte state,bool flag)
        {
            base.SetState(state, flag);
        }

        /// <summary>
        /// Установить низкую скорость анимации
        /// </summary>
        public override void LowSpeedAnimation()
        {
            if (animatorOfObject.speed == 0.2f) return;
            else if ((!playerComponentsControl.PlayerFight.IsFighting 
                || playerComponentsControl.PlayerFight.IsDefensing) && !playerComponentsControl.PlayerFight.IsSpining
                && playerComponentsControl.PlayerConditions.IsAlive)
            {
                animatorOfObject.speed = 0.2f;
            }
        }

        /// <summary>
        /// Установить высокую скорость анимации
        /// </summary>
        public override void HighSpeedAnimation()
        {
            if (animatorOfObject.speed == 1) return;

            animatorOfObject.speed = 1f;
        }

        public override void PlayDeadNormalizeCoroutine()
        {
            Timing.RunCoroutine(CoroutineDeadYNormalized());
        }

        public override IEnumerator<float> CoroutineDeadYNormalized()
        {
            int i = 0;
            yield return Timing.WaitForSeconds(1f);

            Vector3 newPosition =
            new Vector3(transformForDeadYNormalizing.position.x,
                transformForDeadYNormalizing.position.y - 0.2f,
                transformForDeadYNormalizing.position.z);

            while (i < 10)
            {
                i++;
                transformForDeadYNormalizing.position =
                    Vector3.Lerp(transformForDeadYNormalizing.position,
                    newPosition, 1);
                yield return Timing.WaitForSeconds(0.01f);
            }
            yield return Timing.WaitForSeconds(0.01f);
        }
    }
}
