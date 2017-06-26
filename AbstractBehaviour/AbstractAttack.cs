using System.Collections.Generic;
using UnityEngine;
using PlayerBehaviour;
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
		
		protected List<AbstractEnemy> listEnemy;
		protected List<AbstractEnemy> attackList;
		[SerializeField]
		Transform playerPoint, playerRightPoint,
		playerLeftPoint, playerFacePoint, playerBackPoint; // точки персонажа
		[SerializeField]
		protected Transform playerStartGunPoint, playerFinishGunPoint,
			enemyStartGunPoint, enemyFinishGunPoint; // точки оружия персонажа и врага
		
		private float a;
		private float b;
		private float c;
		private float ta;
		private float tb;

		public void SetPlayerPoint(int index, Transform tr)
		{
			if (index == 0) playerRightPoint = tr;
			if (index == 1) playerLeftPoint = tr;
			if (index == 2) playerFacePoint = tr;
			if (index == 3) playerBackPoint = tr;
		}


		public Transform PlayerPosition(int index)
		{
			if (index == 0) return playerRightPoint;
			if (index == 1) return playerLeftPoint;
			if (index == 2) return playerFacePoint;
			if (index == 3) return playerBackPoint;
			else return playerPoint;
		}

		public void AddEnemyToList(AbstractEnemy x)// Добавить врага в лист врагов
		{
			listEnemy.Add(x);
		}

		public List<AbstractEnemy> ReturnList() // Вернуть лит врагов
		{
			return listEnemy;
		}

		public float AttackRange(Vector3 Player, Vector3 Enemy) // Расстояние между персонажем игрока и врагом
		{
			return Vector3.Distance(Player, Enemy);
		}

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

		public Vector3 AttackPoint(Transform X, Transform Y)
		{
			float A = X.position.x + ta * (Y.position.x - X.position.x);
			float B = X.position.z + ta * (Y.position.z - X.position.z);

			return new Vector3(A, 2, B);
		}

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


		public bool EnemyAttack()// для врага
		{
			if (Bush(enemyStartGunPoint.position, enemyFinishGunPoint.position, LibraryPlayerPosition.GetPlayerPoint(0), LibraryPlayerPosition.GetPlayerPoint(1)) ||
				Bush(enemyStartGunPoint.position, enemyFinishGunPoint.position, LibraryPlayerPosition.GetPlayerPoint(2), LibraryPlayerPosition.GetPlayerPoint(3)))
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
	/// Типы атаки
	/// </summary>
	public enum DamageType
	{
		Frozen, Fire, Powerful, Electric
	}
}



