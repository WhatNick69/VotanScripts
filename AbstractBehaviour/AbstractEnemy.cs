using System;
using EnemyBehaviour;
using UnityEngine;
using VotanInterfaces;
using GameBehaviour;

namespace AbstractBehaviour
{
    /// <summary>
    /// Абстрактный класс для реализации любого врага
    /// 
    /// Хранит в себе ссылки на все компоненты
    /// </summary>
    public abstract class AbstractEnemy
      : MonoBehaviour, IEnemyBehaviour
    {
        #region Переменные
        [SerializeField] // точки противника
        protected Transform rightShoulderPoint, leftShoulderPoint, 
			facePoint, backPoint;

        protected EnemyAnimationsController enemyAnimationsController;
        protected EnemyAttack enemyAttack;
        protected EnemyConditions enemyConditions;
        protected EnemyOpponentChoiser enemyOpponentChoiser;
        protected EnemyMove enemyMove;

        protected IEffect iceEffectManager;
        protected IEffect fireEffectManager;
        protected IEffect electricEffectManager;
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

        public abstract EnemyMove EnemyMove { get; set; }

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

        public IEffect IceEffect
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

        public IEffect FireEffect
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

        public IEffect ElectricEffect
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
        #endregion

        /// <summary>
        /// Возвращает положение врага или его точек в сцене
        /// </summary>
        /// <param name="child"></param>
        /// <returns></returns>
        public Vector3 ReturnPosition(int child)
        {
            switch (child)
            {
                case 0:
                    return rightShoulderPoint.position; //Right
                case 1:
                    return leftShoulderPoint.position; //Left
                case 2:
                    return facePoint.position; //Face
                case 3:
                    return backPoint.position; //Back
                case 4:
                    return transform.position; // Позиция врага
                case 5:
                    return EnemyAttack.PlayerStartGunPoint.position;
                case 6:
                    return EnemyAttack.PlayerFinishGunPoint.position;
            }  
			return Vector3.zero;
        }
    }
}
