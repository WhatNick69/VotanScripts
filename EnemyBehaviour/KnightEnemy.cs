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

        /// <summary>
        /// Инициализация
        /// </summary>
        private void Start()
        {
            enemyAttack = GetComponent<EnemyAttack>();
        }

		/// <summary>
        /// Таймовое обновление
        /// </summary>
        private void FixedUpdate()
		{
			if (enemyAttack.AttackToPlayer())
			{
                LibraryPlayerPosition.PlayerConditions.GetDamage(enemyAttack.DmgEnemy);
			}
		}
	}
}
