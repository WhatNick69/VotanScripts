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
        private PlayerAttack playerTarget;
        private IEnemyBehaviour enemyAbstract;
        private bool inFightMode;
        private bool isMayToPlayAttackAnimation = true;
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
        public override void Start()
        {
            base.Start();
            enemyAbstract = GetComponent<AbstractEnemy>();
            isMayToDamage = false;
        }

        private void DrawerLiner()
        {
            Debug.DrawLine(enemyFinishGunPoint.position, 
                playerTarget.GetPlayerPoint(0),Color.red,1);
            Debug.DrawLine(enemyFinishGunPoint.position, 
                playerTarget.GetPlayerPoint(1), Color.red, 1);
            Debug.DrawLine(enemyFinishGunPoint.position, 
                playerTarget.GetPlayerPoint(2), Color.red, 1);
            Debug.DrawLine(enemyFinishGunPoint.position, 
                playerTarget.GetPlayerPoint(3), Color.red, 1);
            Debug.Log("0: " + Vector3.Distance
                (enemyFinishGunPoint.position, playerTarget.GetPlayerPoint(0))+
                "___1: " + Vector3.Distance
                (enemyFinishGunPoint.position, playerTarget.GetPlayerPoint(1))+
                "___2: " + Vector3.Distance
                (enemyFinishGunPoint.position, playerTarget.GetPlayerPoint(2))+
                "___3: " + Vector3.Distance
                (enemyFinishGunPoint.position, playerTarget.GetPlayerPoint(3)));
        }

        public bool IsHited()
        {
            i++;
            if (Vector3.Distance(enemyFinishGunPoint.position, 
                playerTarget.GetPlayerPoint(0)) < 1
                || Vector3.Distance(enemyFinishGunPoint.position, 
                playerTarget.GetPlayerPoint(1)) < 1
                || Vector3.Distance(enemyFinishGunPoint.position, 
                playerTarget.GetPlayerPoint(2)) < 1
                || Vector3.Distance(enemyFinishGunPoint.position, 
                playerTarget.GetPlayerPoint(3)) < 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Атакуем персонажа
        /// </summary>
        /// <returns></returns>
        public bool AttackToPlayer()
        {
            //if (isMayToDamage) DrawerLiner();

            if (isMayToDamage && (LibraryPhysics.BushInLine
                (enemyStartGunPoint.position, enemyFinishGunPoint.position,
                 playerTarget.GetPlayerPoint(0),
                 playerTarget.GetPlayerPoint(1)) ||
                 LibraryPhysics.BushInLine
                    (enemyStartGunPoint.position, enemyFinishGunPoint.position,
                 playerTarget.GetPlayerPoint(2),
                 playerTarget.GetPlayerPoint(3))))
            {
                Timing.RunCoroutine(CoroutineMayDoDamage());
				return true;
            }
            else
            {
                return false;
            }
        }
        private int i = 0;

        /// <summary>
        /// Для класса врага
        /// </summary>
        public void EventStartAttackAnimation()
        {
            if (!isMayToPlayAttackAnimation) return;

            isMayToPlayAttackAnimation = false;

            enemyAbstract.EnemyAnimationsController.SetState(0, false);
            enemyAbstract.EnemyAnimationsController.SetState(1, true);
            enemyAbstract.EnemyAnimationsController.SetSpeedAnimationByRunSpeed(0.5f);
        }

        /// <summary>
        /// Для анимации
        /// </summary>
        public void EventEndAttackAnimation()
        {
            Timing.RunCoroutine(CoroutineForAttackAnimation());
        }

        /// <summary>
        /// Корутина для воспроизведения анимации атаки
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForAttackAnimation()
        {
            isMayToDamage = false;
            enemyAbstract.EnemyAnimationsController.SetState(1, false);
            yield return Timing.WaitForSeconds(attackLatency + 0.5f);

            isMayToPlayAttackAnimation = true;
        }

        /// <summary>
        /// Корутина для нанесения урона по персонажу
        /// </summary>
        /// <returns></returns>
        public IEnumerator<float> CoroutineMayDoDamage()
        {
            isMayToDamage = false;
            enemyAbstract.EnemyAnimationsController.SetState(1, false);
            yield return Timing.WaitForSeconds(attackLatency + 0.5f);
            enemyAbstract.EnemyAnimationsController.SetState(1, true);
            //isMayToDamage = true;
        }

        public void MayToCalculateHiting()
        {
            isMayToDamage = true;
        }
    }
}
