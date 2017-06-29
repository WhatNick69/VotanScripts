using AbstractBehaviour;
using System.Collections.Generic;

namespace EnemyBehaviour
{
    /// <summary>
    /// Компонент-атака для врага
    /// </summary>
    class EnemyAttack 
        : AbstractAttack
    {
        /// <summary>
        /// Инициализация
        /// </summary>
        void Start()
        {
            listEnemy = new List<AbstractEnemy>();
            attackList = new List<AbstractEnemy>();
        }


    }
}
