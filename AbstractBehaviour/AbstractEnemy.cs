using System;
using EnemyBehaviour;
using UnityEngine;
using VotanInterfaces;

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

        private EnemyAnimationsController enemyAnimationsController;
        private EnemyAttack enemyAttack;
        private IEnemyConditions enemyConditions;
        private EnemyOpponentChoiser enemyOpponentChoiser;
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

        public IEnemyConditions EnemyConditions
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
