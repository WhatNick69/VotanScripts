using System;
using System.Collections.Generic;
using AbstractBehaviour;
using MovementEffects;
using UnityEngine;
using VotanInterfaces;

namespace EnemyBehaviour
{
    /// <summary>
    /// Осуществляет контроль за анимациями врага
    /// </summary>
    public class EnemyAnimationsController
        : AbstactObjectAnimations
    {
        private IEnemyConditions enemyConditions;
        private IEnemyBehaviour enemyBehaviour;

        /// <summary>
        /// Конструктор
        /// </summary>
        EnemyAnimationsController()
        {
            StructStatesNames = new StructStatesNames("isRunning", "isFighting"
                , "isDamage", "isDead");      
        }

        /// <summary>
        /// Инициализация
        /// </summary>
        private void Start()
        {
            enemyBehaviour = GetComponent<IEnemyBehaviour>();
            enemyConditions = enemyBehaviour.EnemyConditions;
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
        /// Задать скорость аниматору
        /// </summary>
        /// <param name="value"></param>
        public override void SetSpeedAnimationByRunSpeed(float value)
        {
            if (enemyConditions != null && !enemyConditions.IsFrozen)
                animatorOfObject.speed = value;
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

        /// <summary>
        /// Проиграть корутину номрализации у врага после его смерти
        /// </summary>
        public override void PlayDeadNormalizeCoroutine()
        {
            Timing.RunCoroutine(CoroutineDeadYNormalized());
        }

        /// <summary>
        /// Нормализация координаты Y у врага после смерти
        /// </summary>
        /// <returns></returns>
        public override IEnumerator<float> CoroutineDeadYNormalized()
        {
            if (enemyConditions.IsFrozen) yield break;

            int i = 0;       
            yield return Timing.WaitForSeconds(0.5f);
            if (this == null) yield break;

            Vector3 newPosition =
                new Vector3(transformForDeadYNormalizing.position.x, 
                transformForDeadYNormalizing.position.y - 0.8f,
                transformForDeadYNormalizing.position.z);

            while (i < 15)
            {
                i++;
                if (this == null) yield break;

                transformForDeadYNormalizing.position =
                Vector3.Lerp(transformForDeadYNormalizing.position,
                newPosition, Time.deltaTime*2);
                yield return Timing.WaitForSeconds(0.01f);
            }
        }
    }
}
