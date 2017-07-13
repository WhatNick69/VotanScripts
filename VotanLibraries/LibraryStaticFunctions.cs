using UnityEngine;

namespace VotanLibraries
{
    /// <summary>
    /// Статические функции
    /// </summary>
    public class LibraryStaticFunctions
        : MonoBehaviour
    {
        public static System.Random rnd = new System.Random();

        /// <summary>
        /// В указанном диапазоне возвращает значение
        /// </summary>
        /// <param name="dmg"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        public static float GetRangeValue(float dmg, float range=0)
        {
            if (range > 1) range = 1;
            else if (range < 0) range = 0;

            return dmg + (float)((rnd.NextDouble()
                * 2 - 1) * dmg * range); 
        }

        /// <summary>
        /// Вернуть значение + либо -
        /// </summary>
        /// <param name="valueMax"></param>
        /// <returns></returns>
        public static float GetPlusMinusValue(float valueMax)
        {
            return (float)rnd.NextDouble() * valueMax * 2 - valueMax;
        }

        /// <summary>
        /// Случайная позиция врага во время отдыха
        /// </summary>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static float GetRandomAxisOfEnemyRest(float radius)
        {
            return (float)(rnd.NextDouble()* (radius*2))-radius;
        }

        /// <summary>
        /// Вычисляет вероятность возникновения эффекта заморозки 
        /// и возвращает true в случае успеха
        /// </summary>
        /// <returns></returns>
        public static bool MayableToBeFreezy(float gemPower)
        {
            return rnd.NextDouble() <= gemPower / 150 ? true : false;
        }

        /// <summary>
        /// Получаем вес двух предметов. 
        /// Текущая реализация: сложение
        /// 
        /// Диапазон значений: 0 - 100 
        /// </summary>
        /// <param name="weightA"></param>
        /// <param name="weightB"></param>
        /// <returns></returns>
        public static float TotalWeight(float weightA, float weightB)
        {
            return weightA + weightB;
        }

        /// <summary>
        /// Получаем общую скорость вращения оружием
        /// Текущая реализация: 20 + (40-(WEIGHT)/2.5) + (A+B)/2.5
        /// 
        /// Диапазон значений: 20 - 100 
        /// </summary>
        /// <param name="bonusA"></param>
        /// <param name="bonusB"></param>
        /// <param name="totalWeight"></param>
        /// <returns></returns>
        public static float TotalSpinSpeed(float bonusA, float bonusB, float totalWeight)
        {
            return 20 + (40 - (totalWeight) / 2.5f) + (bonusA + bonusB) / 2.5f;
        }

        /// <summary>
        /// Получаем урон оружием.
        /// Текущая реализация: УРОН * (1+(ВЕС/10))
        /// 
        /// Диапазон значений: 1.2 - 1100
        /// </summary>
        /// <param name="damageBase"></param>
        /// <param name="totalWeight"></param>
        /// <returns></returns>
        public static float TotalDamage(float damageBase, float totalWeight)
        {
            return damageBase * (1+(totalWeight / 10));
        }

        /// <summary>
        /// Рассчет урона при атаке по врагу
        /// 
        /// Если это кручение: умножить урон на 0.5*(СКОРОСТЬ/ОБЩАЯСКОРОСТЬ)
        /// Если это рывок: умножить атаку на 2
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="spinSpeed"></param>
        /// <returns></returns>
        public static float AttackToEnemyDamage(float damage, 
            float spinSpeed,float spinSpeedoriginal,bool isSuperAttack=false)
        {
            return isSuperAttack ? damage*2 : damage * (0.5f + (spinSpeed / spinSpeedoriginal));
        }
    }
}
