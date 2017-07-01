using AbstractBehaviour;

namespace VotanInterfaces
{
    /// <summary>
    /// Интерфейс для реализации атаки персонажем
    /// </summary>
    interface IPlayerAttack
    {
        void AttackToEnemy(float damage, DamageType dmgType);
    }

    /// <summary>
    /// Интерфейс для реализации атаки врагом
    /// </summary>
    interface IEnemyAttack
    {
        bool AttackToPlayer();
    }
}
