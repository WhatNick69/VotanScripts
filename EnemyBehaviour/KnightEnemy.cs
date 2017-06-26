using AbstractBehaviour;
using UnityEngine;
using VotanLibraries;
using MovementEffects;
using System.Collections.Generic;


namespace EnemyBehaviour
{ 


    /// <summary>
    /// Класс противника "Рыцарь"
    /// </summary>
    class KnightEnemy
        : AbstractEnemy
    {
		IEnumerator<float> CoroutineAttack()
		{
			while (true)
			{
				if (EnemyAttack())
				{
					
					LibraryPlayerPosition.playerConditions.GetDamage(1);
				}
				yield return Timing.WaitForSeconds(0.07f);
			}
		}
		private void Start()
		{
			StartCoroutine(CoroutineAttack());
		}

	}
}
