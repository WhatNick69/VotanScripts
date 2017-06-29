using AbstractBehaviour;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerBehaviour
{
    /// <summary>
    /// Компонент-атака для персонажа
    /// </summary>
    public class PlayerAttack
        : AbstractAttack 
    {
		IWeapon playerKillStik;
		PlayerWeapon pW;
		Animator playerAnim;

		public void GetWeapon(int damage, int damageType, float spinSpeed)
		{
			pW = new PlayerWeapon(damage, (DamageType)damageType, spinSpeed);
			playerKillStik = pW;
		}

		/// <summary>
		/// Инициализация
		/// </summary>
		void Start()
        {
            listEnemy = new List<AbstractEnemy>();
            attackList = new List<AbstractEnemy>();
        }

        /// <summary>
        /// Обновление с заданной частотой
        /// </summary>
        private void FixedUpdate()
        {
			//if (playerAnim.GetBool("isAttack"))
			//{
				for (int i = 0; i < listEnemy.Count; i++)
					if (listEnemy[i])
					{
						if (AttackRange(transform.position, listEnemy[i].transform.position) < 3)
						{
							if (!attackList.Contains(listEnemy[i])) attackList.Add(listEnemy[i]);
						}
					}
				EnemyAttack(1, DamageType.Powerful);//EnemyAttack(playerKillStik.Damage);
			//}
		}
    }
}

