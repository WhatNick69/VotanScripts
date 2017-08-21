using AbstractBehaviour;
using MovementEffects;
using PlayerBehaviour;
using System.Collections.Generic;
using UnityEngine;
using VotanInterfaces;
using VotanLibraries;

namespace EnemyBehaviour
{
    /// <summary>
    /// Компонент-атака для врага
    /// </summary>
    public class EnemyAttack 
        : AbstractAttack, IEnemyAttack
    {
        #region Переменные
        [SerializeField, Tooltip("Урон от удара врага")]
        private float dmgEnemy;
        protected PlayerAttack playerTarget;
        protected IEnemyBehaviour enemyAbstract;
        private bool inFightMode;
        [SerializeField]
        protected bool isMayToPlayAttackAnimation = true;
        #endregion

        #region Свойства
        public float DmgEnemy
        {
            get
            {
                return dmgEnemy;
            }

            set
            {
                dmgEnemy = value;
            }
        }

        public PlayerAttack PlayerTarget
        {
            get
            {
                return playerTarget;
            }

            set
            {
                playerTarget = value;
            }
        }

        public bool InFightMode
        {
            get
            {
                return inFightMode;
            }

            set
            {
                inFightMode = value;
            }
        }

        public bool IsMayToPlayAttackAnimation
        {
            get
            {
                return isMayToPlayAttackAnimation;
            }

            set
            {
                isMayToPlayAttackAnimation = value;
            }
        }
        #endregion

        /// <summary>
        /// Инициализация
        /// </summary>
        public void Start()
        {
            enemyAbstract = GetComponent<AbstractEnemy>();       
        }

        public virtual void RestartEnemyAttack()
        {
            IsMayToDamage = false;
        }

        /// <summary>
        /// Атакуем персонажа
        /// </summary>
        /// <returns></returns>
        public virtual bool AttackToPlayer()
        {
            if (IsMayToDamage && (LibraryPhysics.IsAttackEnemy(startGunPoint.position, 
				finishGunPoint.position, playerTarget.GetPlayerPoint())))
            {
                IsMayToDamage = false;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Для класса врага
        /// Переменная isStop говорит, необходимо ли останавливаться
        /// во время атаки, либо нет.
        /// </summary>
        /// <param name="isStop">Неолбходимо останавливаться?</param>
        public void EventStartAttackAnimation(bool isStop)
        {
            if (!isMayToPlayAttackAnimation) return;
            Timing.RunCoroutine(CoroutineForAttack(isStop));
        }

        /// <summary>
        /// Корутина для атаки
        /// </summary>
        /// <param name="isStop"></param>
        /// <returns></returns>
        public virtual IEnumerator<float> CoroutineForAttack(bool isStop)
        {
            isMayToPlayAttackAnimation = false;
            if (isStop)
                enemyAbstract.EnemyMove.DisableAgent();

            enemyAbstract.EnemyAnimationsController.SetState(0, false);
            enemyAbstract.EnemyAnimationsController.SetSpeedAnimationByRunSpeed(0.5f);
            enemyAbstract.EnemyAnimationsController.SetState(1, true);
            yield return Timing.WaitForSeconds(attackLatency);
            isMayToPlayAttackAnimation = true;
        }

        /// <summary>
        /// Выключили просчет
        /// </summary>
        public void EventEndAttackAnimation()
        {
            IsMayToDamage = false;
            enemyAbstract.EnemyAnimationsController.DisableAllStates();
            enemyAbstract.EnemyMove.EnableAgent(); // анимация закончилась. можем двигаться
        }

        /// <summary>
        /// Корутина для нанесения урона по персонажу
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator<float> CoroutineMayDoDamage()
        {
            IsMayToDamage = false;          
            yield return Timing.WaitForSeconds(attackLatency);
        }

        /// <summary>
        /// Включили просчет
        /// </summary>
        public void MayToCalculateHiting()
        {
            IsMayToDamage = true;
        }
    }
}
