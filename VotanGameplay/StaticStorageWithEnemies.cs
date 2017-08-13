using AbstractBehaviour;
using UnityEngine;

namespace VotanGameplay
{
    /// <summary>
    /// Массив со всеми врагами.
    /// Реализован, как стэк
    /// </summary>
    public class StaticStorageWithEnemies 
    {
        #region Переменные
        /// <summary>
        /// Массив врагов. Является статическим объектом
        /// к которому возможен доступ через методы,
        /// реализованные в этом классе из любой точки
        /// кода.
        /// </summary>
        private static AbstractEnemy[] listEnemy;
        private static AbstractEnemy bossEnemy;
        private static int countOfEnemies;
        #endregion

        #region Свойства
        /// <summary>
        /// Лист врагов
        /// </summary>
        public static AbstractEnemy[] ListEnemy
        {
            get
            {
                return listEnemy;
            }

            set
            {
                listEnemy = value;
            }
        }

        /// <summary>
        /// Число врагов, готовых сражаться
        /// </summary>
        public static int CountOfEnemies
        {
            get
            {
                return countOfEnemies;
            }

            set
            {
                countOfEnemies = value;
                if (countOfEnemies < 0) countOfEnemies = 0;
            }
        }

        public static AbstractEnemy BossEnemy
        {
            get
            {
                return bossEnemy;
            }

            set
            {
                bossEnemy = value;
            }
        }
        #endregion

        /// <summary>
        /// Получить число врагов, которые готовы сражаться
        /// </summary>
        /// <returns></returns>
        public static int GetNumberOfEmptyEnemy()
        {
            for (int i = 0;i<countOfEnemies;i++)
            {
                if (!listEnemy[i].gameObject.activeSelf)
                    return i;
            }
            return countOfEnemies;
        }

        /// <summary>
        /// Получить размер листа врагов, которые не мертвы
        /// </summary>
        /// <returns></returns>
        public static int GetCountOfAliveEnemies()
        {
            int count = 0;
            for (int i = 0;i<listEnemy.Length;i++)
            {
                if (listEnemy[i].gameObject.activeSelf) count++;
            }
            return count;
        }

        /// <summary>
        /// Получить элемент из листа по индексу
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static AbstractEnemy GetFromListByIndex(int index)
        {
            return listEnemy[index];
        }

        /// <summary>
        /// Получить ближайшего врага в радиусе поиска.
        /// Враг должен быть жив и еще не шокирован.
        /// </summary>
        /// <param name="ourPosition"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static AbstractEnemy GetClosestNonShockedEnemy(Vector3 ourPosition,float radius)
        {
            float distance = float.MaxValue;
            float tempDistance = float.MaxValue;
            AbstractEnemy closestEnemy = null;

            for (int i = 0;i<listEnemy.Length;i++)
            {
                if (!listEnemy[i].gameObject.activeSelf 
                    || !listEnemy[i].EnemyConditions.IsAlive) continue;

                tempDistance = Vector3.Distance
                    (ourPosition, listEnemy[i].transform.position);
                if (tempDistance <= radius)
                {
                    if (tempDistance <= distance 
                        && listEnemy[i].EnemyConditions.IsAlive
                        && !listEnemy[i].EnemyConditions.IsShocked)
                    {
                        distance = tempDistance;
                        closestEnemy = listEnemy[i];
                    }
                }
            }
            return closestEnemy;
        }
    }
}
