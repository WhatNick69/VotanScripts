using PlayerBehaviour;
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
            return rnd.NextDouble() <= (gemPower / 220)+0.05f ? true : false;
        }

        /// <summary>
        /// Время заморозки.
        /// 
        /// Текущая реализация: 1 + (СИЛА_ГЕМА/100)*3
        /// 0.9 - 4.4 секунды
        /// </summary>
        /// <param name="gemPower"></param>
        /// <returns></returns>
        public static float TimeToFreezy(float gemPower)
        {
            return GetRangeValue(1 + (gemPower / 100) * 3, 0.1f);
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

        /// <summary>
        /// Получить цвет трэил-ленты, на основе силы камня
        /// </summary>
        /// <param name="gemPower"></param>
        /// <param name="weaponType"></param>
        /// <returns></returns>
        public static Color GetColorFromGemPower(float gemPower, DamageType damageType)
        {
            switch (damageType)
            {
                case DamageType.Electric:
                    return new Color((gemPower * 2.55f) * 0.003921f, 0, 1);
                case DamageType.Fire:
                    float g = gemPower /100;
                    return g >= 0.647f ?
                        new Color(1 - ((gemPower / 100) - 0.647f), 0, 0) :
                        new Color(1, 0.647f - g, 0);
                case DamageType.Frozen:
                    return new Color(1 - (0.21569f + (gemPower * 2) * 0.0039f), 1, 1);
                case DamageType.Powerful:
                    return new Color(0.803f - gemPower*0.002f, 
                        0.5215f+gemPower*0.0028f, 0.247f-gemPower*0.0005f);
                default:
                    return Color.black;
            }
        }
    }
}
