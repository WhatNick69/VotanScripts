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
        private EnemyAttack enemyAttack;

        private void Start()
        {
            enemyAttack = GetComponent<EnemyAttack>();
        }


		private void FixedUpdate()
		{
			//if (enAnim.GetBool("isEnemyAttack"))
			//{
			if (AbsAttack.EnemyAttack())
			{
			    LibraryPlayerPosition.PlayerConditions.GetDamage(enemyAttack.DmgEnemy);
			}
			//}
		}
	}
}
