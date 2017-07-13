using System;
using System.Collections.Generic;
using UnityEngine;
using VotanInterfaces;

namespace AbstractBehaviour
{
    /// <summary>
    /// Абстрактный компонент-атака.
    /// Является родителем для компонентов-атаки 
    /// персонажей и противников
    /// </summary>
    public abstract class AbstractAttack
		: MonoBehaviour, IVotanObjectAttack
    {
        #region Переменные
        [SerializeField]
		protected List<AbstractEnemy> attackList;
		[SerializeField] // точки персонажа
        protected Transform playerPoint, playerRightPoint,
		playerLeftPoint, playerFacePoint, playerBackPoint,
        playerStartGunPoint, playerFinishGunPoint; 
        [SerializeField] // точки оружия врага
        protected Transform enemyStartGunPoint, enemyFinishGunPoint; 
        [SerializeField, Tooltip("Как часто объект может бить/стрелять")]
        protected float attackLatency;
		
		protected Vector3 oldFinishGunPoint; // сохраняю коардинаты оружия из прошлого кадра
		protected bool onLevelOne = true;
		protected bool onLevelTwo = false;

		private float a;
		private float b;
		private float c;
		private float ta;
		private float tb;
        public bool isMayToDamage = true;
        #endregion

        #region Свойства
        public Transform PlayerStartGunPoint
        {
            get
            {
                return playerStartGunPoint;
            }

            set
            {
                playerStartGunPoint = value;
            }
        }

        public Transform PlayerFinishGunPoint
        {
            get
            {
                return playerFinishGunPoint;
            }

            set
            {
                playerFinishGunPoint = value;
            }
        }

        public Transform EnemyStartGunPoint
        {
            get
            {
                return enemyStartGunPoint;
            }

            set
            {
                enemyStartGunPoint = value;
            }
        }

        public Transform EnemyFinishGunPoint
        {
            get
            {
                return enemyFinishGunPoint;
            }

            set
            {
                enemyFinishGunPoint = value;
            }
        }
        #endregion

        /// <summary>
        /// Инициализация
        /// </summary>
        public virtual void Start()
        {      
            attackList = new List<AbstractEnemy>();
        }

        /// <summary>
        /// Установить позицию персонажа
        /// </summary>
        /// <param name="index"></param>
        /// <param name="tr"></param>
        public void SetPlayerPoint(int index, Transform tr)
		{
            switch (index)
            {
                case 0:
                    playerRightPoint = tr;
                    break;
                case 1:
                    playerLeftPoint = tr;
                    break;
                case 2:
                    playerFacePoint = tr;
                    break;
                case 3:
                    playerBackPoint = tr;
                    break;
            }
		}

		/// <summary>
		/// Задать новую длинну оружия 
		/// </summary>
		/// <param name="newPoint"></param>
		public void SetPlayerGunLocalPoint(Vector3 newPoint)
		{
			playerFinishGunPoint.localPosition = newPoint;
		}
	
		/// <summary>
		/// Возвращает позиции персонажа
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public Transform PlayerPosition(int index)
		{
            switch (index)
            {
                case 0:
                    return playerRightPoint;
                case 1:
                    return playerLeftPoint;
                case 2:
                    return playerFacePoint;
                case 3:
                    return playerBackPoint;
            }
			return playerPoint;
		}

        /// <summary>
        /// Радиус атаки
        /// Расстояние между персонажем игрока и врагом
        /// </summary>
        /// <param name="Player"></param>
        /// <param name="Enemy"></param>
        /// <returns></returns>
        public float AttackRange(Vector3 Player, Vector3 Enemy) 
		{
			return Vector3.Distance(Player, Enemy);
		}

		/// <summary>
		/// Точка атаки
		/// </summary>
		/// <param name="X"></param>
		/// <param name="Y"></param>
		/// <returns></returns>
		public Vector3 AttackPoint(Transform X, Transform Y)
		{
			float A = X.position.x + ta * (Y.position.x - X.position.x);
			float B = X.position.z + ta * (Y.position.z - X.position.z);

			return new Vector3(A, 2, B);
		}
    }
}



