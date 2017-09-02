using MovementEffects;
using System.Collections.Generic;
using UnityEngine;
using VotanInterfaces;

namespace EnemyBehaviour
{
    /// <summary>
    /// Класс для описания атаки арбалетчика
    /// </summary>
    public class CrossbowmanAttack
        : EnemyAttack
    {
        #region Переменные и ссылки
        [SerializeField,Tooltip("Оружие врага")]
        private CrossbowWeapon crossbowWeapon;
        private bool isReloaded;
        private bool isTrueAngle;
        #endregion

        #region Свойства
        public bool IsReloaded
        {
            get
            {
                return isReloaded;
            }

            set
            {
                isReloaded = value;
            }
        }

        public bool IsTrueAngle
        {
            get
            {
                return isTrueAngle;
            }
            set
            {
                isTrueAngle = value;
            }
        }

        public CrossbowWeapon CrossbowWeapon
        {
            get
            {
                return crossbowWeapon;
            }

            set
            {
                crossbowWeapon = value;
            }
        }
        #endregion

        /// <summary>
        /// Инициализация
        /// </summary>
        public override void Start()
        {
            base.Start();
            isReloaded = true;
        }

        /// <summary>
        /// Корутина для атаки
        /// </summary>
        /// <param name="isStop"></param>
        /// <returns></returns>
        public override IEnumerator<float> CoroutineForAttack(bool isStop)
        {
            isMayToPlayAttackAnimation = false;
            if (isStop)
                enemyAbstract.EnemyMove.DisableAgent();

            enemyAbstract.EnemyAnimationsController.SetState(0, false);
            enemyAbstract.EnemyAnimationsController.SetState(3, false);
            enemyAbstract.EnemyAnimationsController.SetSpeedAnimationByRunSpeed(0.5f);
            enemyAbstract.EnemyAnimationsController.SetState(1, true);
            yield return Timing.WaitForSeconds(attackLatency);
            isMayToPlayAttackAnimation = true;
            enemyAbstract.EnemyAnimationsController.SetState(1, false);
        }

        /// <summary>
        /// Перезарядка оружия
        /// </summary>
        public void ReloadWeapon()
        {
            if (crossbowWeapon.Reload())
            {
                isReloaded = true;
            }
            enemyAbstract.EnemyAnimationsController.DisableAllStates();
        }

        /// <summary>
        /// Стрельба из оружия
        /// </summary>
        public void FireWeapon()
        {
            if (isReloaded && crossbowWeapon.PlayerComponentsControl != null)
            {
                isReloaded = false;
                crossbowWeapon.Fire();
            }
        }

        /// <summary>
        /// Инициализация цели для арбалета врага
        /// </summary>
        /// <param name="iPlayerBehaviour"></param>
        public void InitialisationPlayerTarget(IPlayerBehaviour iPlayerBehaviour)
        {
            crossbowWeapon.PlayerComponentsControl = iPlayerBehaviour;
        }
    }
}
