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

        [SerializeField] // точки персонажа
        private Transform playerPoint;
        [SerializeField] // точки оружия врага
        protected Transform startGunPoint, finishGunPoint;
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
        public Transform StartGunPoint
        {
            get
            {
                return startGunPoint;
            }

            set
            {
                startGunPoint = value;
            }
        }

        public Transform FinishGunPoint
        {
            get
            {
                return finishGunPoint;
            }

            set
            {
                finishGunPoint = value;
            }
        }

        public bool IsMayToDamage
        {
            get
            {
                return isMayToDamage;
            }

            set
            {
                isMayToDamage = value;
            }
        }

        public Transform PlayerPoint
        {
            get
            {
                return playerPoint;
            }

            set
            {
                playerPoint = value;
            }
        }

        public float AttackLatency
        {
            get
            {
                return attackLatency;
            }

            set
            {
                attackLatency = value;
            }
        }
        #endregion

        /// <summary>
        /// Задать новую длинну оружия 
        /// </summary>
        /// <param name="newPoint"></param>
        public void SetPlayerGunLocalPoint(Vector3 newPoint)
		{
			finishGunPoint.localPosition = newPoint;
		}
	
		/// <summary>
		/// Возвращает позиции персонажа
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public Transform ObjectPosition()
		{
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



