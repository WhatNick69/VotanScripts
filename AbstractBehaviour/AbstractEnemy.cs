using EnemyBehaviour;
using UnityEngine;
using VotanInterfaces;
using GameBehaviour;
using System;

namespace AbstractBehaviour
{
    /// <summary>
    /// Абстрактный класс для реализации любого врага
    /// 
    /// Хранит в себе ссылки на все компоненты
    /// </summary>
    public abstract class AbstractEnemy
      : MonoBehaviour, IEnemyBehaviour, IEnemyInStack
    {
        #region Переменные
        [SerializeField, Tooltip("Точка в центре врага")]
        private Transform centerOfEnemy;
        [SerializeField,Tooltip("Класс врага")]
        private EnemyType enemyType;
        [SerializeField,Tooltip("Номер противника в стеке")]
        private int enemyNumber;
        private AbstractSoundStorage abstractObjectSounder;
        protected EnemyAnimationsController enemyAnimationsController;
        protected EnemyAttack enemyAttack;
        protected EnemyConditions enemyConditions;
        protected EnemyOpponentChoiser enemyOpponentChoiser;
        protected DownInterfaceRotater downInterfaceRotater;
        protected IAIMoving enemyMove;

        protected IIceEffect iceEffectManager;
        protected IFireEffect fireEffectManager;
        protected IElectricEffect electricEffectManager;
        protected IPhysicEffect physicEffect;
        protected IScoreAddingEffect scoreAddingEffect;

        #endregion

        #region Свойства
        public EnemyAnimationsController EnemyAnimationsController
        {
            get
            {
                return enemyAnimationsController;
            }

            set
            {
                enemyAnimationsController = value;
            }
        }

        public EnemyAttack EnemyAttack
        {
            get
            {
                return enemyAttack;
            }

            set
            {
                enemyAttack = value;
            }
        }

        public EnemyConditions EnemyConditions
        {
            get
            {
                return enemyConditions;
            }

            set
            {
                enemyConditions = value;
            }
        }

        public EnemyOpponentChoiser EnemyOpponentChoiser
        {
            get
            {
                return enemyOpponentChoiser;
            }

            set
            {
                enemyOpponentChoiser = value;
            }
        }

        public IIceEffect IceEffect
        {
            get
            {
                return iceEffectManager;
            }

            set
            {
                iceEffectManager = value;
            }
        }

        public IFireEffect FireEffect
        {
            get
            {
                return fireEffectManager;
            }

            set
            {
                fireEffectManager = value;
            }
        }

        public IElectricEffect ElectricEffect
        {
            get
            {
                return electricEffectManager;
            }

            set
            {
                electricEffectManager = value;
            }
        }

        public IAIMoving EnemyMove
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

        public IScoreAddingEffect ScoreAddingEffect
        {
            get
            {
                return scoreAddingEffect;
            }

            set
            {
                scoreAddingEffect = value;
            }
        }

        public IPhysicEffect Physicffect
        {
            get
            {
                return physicEffect;
            }

            set
            {
                physicEffect = value;
            }
        }

        public AbstractSoundStorage AbstractObjectSounder
        {
            get
            {
               return abstractObjectSounder;
            }

            set
            {
                abstractObjectSounder = value;
            }
        }

        public int EnemyNumber
        {
            get
            {
                return enemyNumber;
            }

            set
            {
                enemyNumber = value;
            }
        }

        public DownInterfaceRotater DownInterfaceRotater
        {
            get
            {
                return downInterfaceRotater;
            }

            set
            {
                downInterfaceRotater = value;
            }
        }

        public EnemyType EnemyType
        {
            get
            {
                return enemyType;
            }
        }
        #endregion

        /// <summary>
        /// Возвращает положение врага или его точек в сцене
        /// </summary>
        /// <param name="child"></param>
        /// <returns></returns>
        public Vector3 ReturnEnemyPosition()
        {
            return centerOfEnemy.position; // Позиция врага
        }

        /// <summary>
        /// Рестартировать врага
        /// </summary>
        public abstract void RestartEnemy();
    }

    /// <summary>
    /// Перечислитель, задающий класс врага
    /// </summary>
    public enum EnemyType
    {
        Knight,
        Crazy,
        CrossbowMan,
        Halberdier,
        FirstBoss
    }
}
