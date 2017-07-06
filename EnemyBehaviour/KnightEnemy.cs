using AbstractBehaviour;
using PlayerBehaviour;
using UnityEngine;
using UnityEngine.AI;
using VotanInterfaces;
using VotanLibraries;

namespace EnemyBehaviour
{
    /// <summary>
    /// Класс противника "Рыцарь"
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    class KnightEnemy
        : AbstractEnemy
    {
        private EnemyMove enemyMove; // игрок может двигаться

        public EnemyMove EnemyMove
        {
            get
            {
                return enemyMove;
            }

            set
            {
                enemyMove = value;
            }
        }

        /// <summary>
        /// Инициализация
        /// </summary>
        public void Awake()
        {
            EnemyOpponentChoiser =
                GetComponent<EnemyOpponentChoiser>();
            EnemyAnimationsController = 
                GetComponent<EnemyAnimationsController>();
            EnemyAttack = 
                GetComponent<EnemyAttack>();
            EnemyConditions = 
                GetComponent<EnemyConditions>();
            EnemyMove = 
                GetComponent<EnemyMove>();
        }

		/// <summary>
        /// Таймовое обновление
        /// 
        /// 
        /// </summary>
        private void FixedUpdate()
		{
            if (EnemyMove.IsStopped)
            {
                if (EnemyAttack.AttackToPlayer())
                {
                    EnemyOpponentChoiser.PlayerConditionsTarget.GetDamage(EnemyAttack.DmgEnemy);
                    EnemyAnimationsController.SetState(0, false);
                    EnemyAnimationsController.SetState(1, true);
                    EnemyAnimationsController.SetSpeedAnimationByRunSpeed(0.5f);
                }
            }
            else
            {
                EnemyAnimationsController.SetState(0, true);
                EnemyAnimationsController.SetState(1, false);
                EnemyAnimationsController.HighSpeedAnimation();
            }
		}
    }
}
