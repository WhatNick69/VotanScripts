using AbstractBehaviour;
using System.Collections.Generic;
using UnityEngine;

namespace VotanGameplay
{
    /// <summary>
    /// Лист со всеми врагами.
    /// 
    /// Обеспечивает прямую работу с листом.
    /// </summary>
    public class StaticStorageWithEnemies 
    {
        /// <summary>
        /// Лист врагов. Является статическим объектом
        /// к которому возможен доступ через методы,
        /// реализованные в этом классе из любой точки
        /// кода.
        /// </summary>
        private static List<AbstractEnemy> listEnemy
            = new List<AbstractEnemy>();

        /// <summary>
        /// Получить размер листа
        /// </summary>
        /// <returns></returns>
        public static int GetCountListOfEnemies()
        {
            return listEnemy.Count;
        }

        /// <summary>
        /// Добавить в лист элемент
        /// </summary>
        /// <param name="absEnemy"></param>
        public static void AddToList(AbstractEnemy absEnemy)
        {
            listEnemy.Add(absEnemy);
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
        /// Это не пустой элемент?
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static bool IsNonNullElement(int index)
        {
            return listEnemy[index] ? true : false;
        }

        /// <summary>
        /// Удалить из листа элемент по его и ндексу
        /// </summary>
        /// <param name="index"></param>
        public static void RemoveFromListByIndex(int index)
        {
            listEnemy.RemoveAt(index);
        }

        /// <summary>
        /// Удалить из листа элемент
        /// </summary>
        /// <param name="enemyBehaviour"></param>
        public static void RemoveFromListByElement(AbstractEnemy enemyBehaviour)
        {
            listEnemy.Remove(enemyBehaviour);
        }

        /// <summary>
        /// Получить дистанцию, между игроком и врагом
        /// </summary>
        /// <param name="playerPos"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static float DistanceBetweenPlayerAndEnemy(Vector3 playerPos,int index)
        {
            return Vector3.Distance(playerPos, listEnemy[index].transform.position);
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

            for (int i = 0;i<listEnemy.Count;i++)
            {
                if (!listEnemy[i]) continue;

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
