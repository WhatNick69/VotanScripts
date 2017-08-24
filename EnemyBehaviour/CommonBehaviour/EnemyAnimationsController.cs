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
        #region Переменные и  ссылки
        [SerializeField,Tooltip("Скорость аниматора")]
        private float lowSpeedAnimator;
        [SerializeField,Tooltip("Необходимо ли после смерти нормализовывать по Y?")]
        private bool needToNormalizeYAfterDead;

        private IEnemyConditions enemyConditions;
        private IEnemyBehaviour enemyBehaviour;
        #endregion

        /// <summary>
        /// Инициализация
        /// </summary>
        private void Start()
        {
            enemyBehaviour = GetComponent<IEnemyBehaviour>();
            enemyConditions = enemyBehaviour.EnemyConditions;
            StructStatesNames = new StructStatesNames(animatorOfObject);
        }

        /// <summary>
        /// Рестарт анимации врага
        /// </summary>
        public void RestartEnemyAnimationsController()
        {
            IsDowner = false;
            animatorOfObject.enabled = true;
            TransformForDeadYNormalizing.localPosition = Vector3.zero;
            animatorOfObject.Rebind();
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
            {
                animatorOfObject.speed = value;
            }
        }

        /// <summary>
        /// Установить низкую скорость анимации врага
        /// </summary>
        public override void LowSpeedAnimation()
        {
            if (IsFalseAllStates())
                animatorOfObject.speed = lowSpeedAnimator;
        }

        /// <summary>
        /// Установить низкую скорость анимации врага 
        /// без проверки на состояния
        /// </summary>
        public override void NonCheckLowSpeedAnimation()
        {
            animatorOfObject.speed = lowSpeedAnimator;
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

            if (needToNormalizeYAfterDead)
            {
                while (i < 17)
                {
                    i++;
                    TransformForDeadYNormalizing.Translate(0, -0.035f, 0);
                    yield return Timing.WaitForSeconds(0.01f);
                }
            }
            yield return Timing.WaitForSeconds(3);

            i = 0;
            while (i < 100)
            {
                i++;
                TransformForDeadYNormalizing.Translate(0, -0.05f, 0);
                yield return Timing.WaitForSeconds(0.01f);
            }
            IsDowner = true;
        }
    }
}
