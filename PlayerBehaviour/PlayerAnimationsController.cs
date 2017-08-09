using System.Collections.Generic;
using AbstractBehaviour;
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
        private PlayerFight playerFight;
        private PlayerConditions playerConditions;

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
        /// Инициализация
        /// </summary>
        private void Awake()
        {
            playerFight = playerComponentsControl.PlayerFight;
            playerConditions = playerComponentsControl.PlayerConditions;
            StructStatesNames = new StructStatesNames(animatorOfObject);
        }

        /// <summary>
        /// Установить скорость для анимации
        /// </summary>
        /// <param name="value"></param>
        public override void SetSpeedAnimationByRunSpeed(float value)
        {
            if (playerFight.IsMayToLongAttack 
                && playerConditions.IsAlive
                    && !playerFight.IsDefensing)
            {
                animatorOfObject.speed = value;
            }
        }

        /// <summary>
        /// Установить низкую скорость анимации
        /// </summary>
        public override void LowSpeedAnimation()
        {
            if (animatorOfObject.speed == 0.2f) return;
            else if (!playerFight.IsFighting 
                && !playerFight.IsDefensing
                && !playerFight.IsSpining
                && playerConditions.IsAlive 
                && playerFight.IsMayToLongAttack)
            {
                animatorOfObject.speed = 0.2f;
            }
        }

        /// <summary>
        /// Установить низкую скорость анимации без проверки на состояние
        /// </summary>
        public override void NonCheckLowSpeedAnimation()
        {
           // НЕ ЮЗАЕТСЯ
        }

        /// <summary>
        /// Установить высокую скорость анимации
        /// </summary>
        public override void HighSpeedAnimation()
        {
            if (animatorOfObject.speed == 1) return;

            animatorOfObject.speed = 1f;
        }

        /// <summary>
        /// Проиграть корутину-нормализатор позиции по Y
        /// </summary>
        public override void PlayDeadNormalizeCoroutine()
        {
            Timing.RunCoroutine(CoroutineDeadYNormalized());
        }

        /// <summary>
        /// Нормализация позиции по Y после смерти.
        /// Ненавижу Саню.
        /// </summary>
        /// <returns></returns>
        public override IEnumerator<float> CoroutineDeadYNormalized()
        {
            int i = 0;
            yield return Timing.WaitForSeconds(0.5f);

            while (i < 10)
            {
                i++;
                TransformForDeadYNormalizing.Translate(0, -0.02f, 0); 
                yield return Timing.WaitForSeconds(0.01f);
            }
            yield return Timing.WaitForSeconds(0.01f);
        }
    }
}
