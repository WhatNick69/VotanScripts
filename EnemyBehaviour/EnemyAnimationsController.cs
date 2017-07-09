using System;
using System.Collections.Generic;
using AbstractBehaviour;
using VotanInterfaces;
using MovementEffects;
using UnityEngine;

namespace EnemyBehaviour
{
    /// <summary>
    /// Осуществляет контроль за анимациями врага
    /// </summary>
    public class EnemyAnimationsController
        : AbstactObjectAnimations, IEnemyAnimations
    {
        /// <summary>
        /// Инициализация
        /// </summary>
        public void Start()
        {
            StructStatesNames = new StructStatesNames("isRunning", "isFighting"
                , "isDamage", "isDead");
        }

        /// <summary>
        /// Установить высокую скорость анимации врага
        /// </summary>
        public override void HighSpeedAnimation()
        {
            if (animatorOfObject.speed == 1) return;
            animatorOfObject.speed = 1f;
        }

        /// <summary>
        /// Установить низкую скорость анимации врага
        /// </summary>
        public override void LowSpeedAnimation()
        {
            if (IsFalseAllStates())
                animatorOfObject.speed = 0.05f;
        }
      
        /// <summary>
        /// Задаем значение состоянию анимации врагу-рыцарю.
        /// 0 - бег, 1 - битва, 2 - ущерб, 
        /// 3 - смерть
        /// </summary>
        /// <param name="state"></param>
        /// <param name="flag"></param>
        public override void SetState(byte state, bool flag)
        {
            base.SetState(state, flag);
        }

        public override void PlayDeadNormalizeCoroutine()
        {
            Timing.RunCoroutine(CoroutineDeadYNormalized());
        }

        public override IEnumerator<float> CoroutineDeadYNormalized()
        {
            int i = 0;
            yield return Timing.WaitForSeconds(0.5f);

            Vector3 newPosition =
            new Vector3(transformForDeadYNormalizing.position.x, 
                transformForDeadYNormalizing.position.y - 0.8f,
                transformForDeadYNormalizing.position.z);

            while (i < 10)
            {
                i++;
                transformForDeadYNormalizing.position =
                    Vector3.Lerp(transformForDeadYNormalizing.position,
                    newPosition, Time.deltaTime*2);
                yield return Timing.WaitForSeconds(0.01f);
            }
            yield return Timing.WaitForSeconds(0.01f);
        }
    }
}
