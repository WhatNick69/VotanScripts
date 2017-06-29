using System.Collections.Generic;
using UnityEngine;
using VotanLibraries;

namespace AbstractBehaviour
{
    /// <summary>
    /// Абстрактный компонент-атака.
    /// Является родителем для компонентов-атаки 
    /// персонажей и противников
    /// </summary>
    public abstract class AbstractAttack
		: MonoBehaviour
	{
        // Лист врагов
		protected List<AbstractEnemy> listEnemy;
        // Лист атаки
		protected List<AbstractEnemy> attackList;
		[SerializeField]
		Transform playerPoint, playerRightPoint,
		playerLeftPoint, playerFacePoint, playerBackPoint; // точки персонажа
        [SerializeField]
        private Transform enemyStartGunPoint;
        [SerializeField]
        private Transform enemyFinishGunPoint; // точки оружия персонажа и врага
        [SerializeField]
        private Transform playerStartGunPoint;
        [SerializeField]
        private Transform playerFinishGunPoint;

        private float a;
		private float b;
		private float c;
		private float ta;
		private float tb;

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
        /// Добавить врага в лист
        /// </summary>
        /// <param name="x"></param>
        public void AddEnemyToList(AbstractEnemy x)
		{
			listEnemy.Add(x);
		}

		/// <summary>
        /// Вернуть лист врагов
        /// </summary>
        /// <returns></returns>
        public List<AbstractEnemy> ReturnList()
		{
			return listEnemy;
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
        /// Просчет столкновений
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="w"></param>
        /// <returns></returns>
        public bool Bush(Vector3 x, Vector3 y, Vector3 z, Vector3 w)
		{
			Vector3 PV1 = x;
			Vector3 PV2 = y;
			Vector3 EV3 = z;
			Vector3 EV4 = w;

			a = (PV1.x - PV2.x) * (EV4.z - EV3.z) - (PV1.z - PV2.z) * (EV4.x - EV3.x);
			b = (PV1.x - EV3.x) * (EV4.z - EV3.z) - (PV1.z - EV3.z) * (EV4.x - EV3.x);
			c = (PV1.x - PV2.x) * (PV1.z - EV3.z) - (PV1.z - PV2.z) * (PV1.x - EV3.x);

			ta = b / a;
			tb = c / a;
			//Debug.Log("a: " + a + ", b: " + b + ", c: " + c + ", ta: " + ta + ", tb: " + tb);
			return (ta >= 0 && ta <= 1 && tb >= 0 && tb <= 1);
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

		/// <summary>
        /// Атака по врагу
        /// </summary>
        /// <param name="Damage"></param>
        /// <param name="DamTyp"></param>
        public void EnemyAttack(int Damage, DamageType DamTyp)// для игрока
		{
			for (int i = 0; i < attackList.Count; i++)
			{
				if (attackList[i])
				{
					if (Bush(playerStartGunPoint.position,
						playerFinishGunPoint.position, attackList[i].ReturnPosition(0),
						attackList[i].ReturnPosition(1)) ||
						Bush(playerStartGunPoint.position,
						playerFinishGunPoint.position, attackList[i].ReturnPosition(2)
						, attackList[i].ReturnPosition(3)))
					{
						attackList[i].GetDamage(1);
						if (attackList[i].ReturnHealth() <= 0 || !attackList[i])
						{
							attackList.RemoveAt(i);
						}
					}
				}
			}
		}

		/// <summary>
        /// Атака по персонажу
        /// </summary>
        /// <returns></returns>
        public bool EnemyAttack()// для врага
		{
			if (Bush(enemyStartGunPoint.position, enemyFinishGunPoint.position, 
                LibraryPlayerPosition.GetPlayerPoint(0), LibraryPlayerPosition.GetPlayerPoint(1)) ||
				Bush(enemyStartGunPoint.position, enemyFinishGunPoint.position, 
                LibraryPlayerPosition.GetPlayerPoint(2), LibraryPlayerPosition.GetPlayerPoint(3)))
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}

	/// <summary>
	/// Типы атаки. Перечисление
	/// </summary>
	public enum DamageType
	{
		Frozen, Fire, Powerful, Electric
	}
}



