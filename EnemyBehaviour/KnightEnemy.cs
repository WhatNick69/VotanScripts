using AbstractBehaviour;
using VotanLibraries;

namespace EnemyBehaviour
{
    /// <summary>
    /// Класс противника "Рыцарь"
    /// </summary>
    class KnightEnemy
        : AbstractEnemy
    {
		private void FixedUpdate()
		{
			//if (enAnim.GetBool("isEnemyAttack"))
			//{
				if (AbsAttack.EnemyAttack())
				{
			    LibraryPlayerPosition.PlayerConditions.GetDamage(1);
				}
			//}
		}
	}
}
