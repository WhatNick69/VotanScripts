using PlayerBehaviour;
using System;
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
        /// В указанном диапазоне возвращает значение.
        /// 
        /// Текущая реализация: если dmg=10, а range=0.1, то
        /// 0.9 - 1.1
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
        /// Получить коэффициент тряски при получении урона от врага
        /// </summary>
        /// <param name="coefficient"></param>
        /// <returns></returns>
        public static float GetCoefficientForGetDamageNoize(float coefficient)
        {
            if (coefficient > 1) coefficient = 1;
            return coefficient /= 4;
        }

        /// <summary>
        /// Вернуть значение +valueMax либо -valueMax.
        /// 
        /// Текущая реализация: если valueMax=10, то
        /// -10.0 - +10.0
        /// </summary>
        /// <param name="valueMax"></param>
        /// <returns></returns>
        public static float GetPlusMinusValue(float valueMax)
        {
            return (float)rnd.NextDouble() * valueMax * 2 - valueMax;
        }

        /// <summary>
        /// Вычисляет вероятность возникновения эффекта заморозки 
        /// и возвращает true в случае успеха.
        /// 
        /// Текущая реализация: если gemPower=25, то
        /// зерно МЕНЬШЕ ИЛИ РАВНо 0.16
        /// </summary>
        /// <returns></returns>
        public static bool MayableToBeFreezy(float gemPower)
        {
            return rnd.NextDouble() <= (gemPower / 220)+0.05f ? true : false;
        }

        /// <summary>
        /// Время горения.
        /// 
        /// Текущая реализация: 1 + (СИЛАГ_ГЕМА/20) +- 10%
        /// 0.945 - 6.6 секунды
        /// </summary>
        /// <param name="weapon"></param>
        /// <returns></returns>
        public static float TimeToBurning(IWeapon weapon)
        {
            return GetRangeValue(1 + (weapon.GemPower / 20), 0.1f);
        }

        /// <summary>
        /// Получить урон от огненного оружия
        /// 
        /// Текущая реализация: УРОН/10 * (1+СИЛА_ГЕМА/200).
        /// Значение: 0.126 - 150
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="weapon"></param>
        /// <returns></returns>
        public static float FireDamagePerPeriod(float damage, IWeapon weapon)
        {
            return (damage / 10) * (1 + (weapon.GemPower / 200));
        }

        /// <summary>
        /// Получить урон от ледяного оружия
        /// 
        /// Текущая реализация: УРОН/10 * (1+СИЛА_ГЕМА/200)
        /// Значение: 0.01 - 16.5
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="weapon"></param>
        /// <returns></returns>
        public static float IceDamagePerPeriod(float damage, IWeapon weapon)
        {
            return (damage / 100) * (1 + (weapon.GemPower / 200));
        }

        /// <summary>
        /// Получить число партиклов для эффекта горения
        /// </summary>
        /// <param name="damage"></param>
        /// <returns></returns>
        public static int GetCountOfParticleSystemElements(float damage)
        {
            if (damage > 50)
                damage = 50;
            return 5 + Convert.ToInt32(damage / 1.666f);
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
        /// Сила отталкивания
        /// </summary>
        /// <param name="weapon"></param>
        /// <returns></returns>
        public static float StrenghtOfNockback(IWeapon weapon,bool isSuperAttack=false)
        {
            return isSuperAttack ? 3 + (weapon.Weight / 100) : 2 +  (weapon.GemPower / 100);
        }

        /// <summary>
        /// Как долго ждать после очередной физической атаки?
        /// </summary>
        /// <param name="gemPower"></param>
        /// <returns></returns>
        public static float HowMuchWaitForPhysicAttack(float gemPower)
        {
            return 1 - (gemPower / 100);
        }

        /// <summary>
        /// Урон, после прохождения через временную защиту 
        /// камня земли
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="weapon"></param>
        /// <returns></returns>
        public static float DamageFromTempPhysicDefence(float damage, IWeapon weapon)
        {
            float a = 0;
            if (weapon.TempPhysicDefence <= 0.01f) return damage;
            else return damage * (1 - (weapon.TempPhysicDefence / 100));
        }

        /// <summary>
        /// Получаем вес двух предметов. 
        /// Текущая реализация: сложение.
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
        /// Зависимость скорости передвижения персонажа от веса оружия.
        /// </summary>
        /// <param name="weight"></param>
        /// <returns></returns>
        public static float DependenceMoveSpeedAndWeaponWeight(float weight)
        {
            return 1.5f + (0.5f-(weight *0.005f));
        }

        /// <summary>
        /// Зависимость скорости поворота персонажа от веса оружия.
        /// </summary>
        /// <param name="weight"></param>
        /// <returns></returns>
        public static float DependenceRotateSpeedAndWeaponWeight(float weight)
        {
            return 3.5f + (2.5f-(weight * 0.025f));
        }

        /// <summary>
        /// Получаем общую скорость вращения оружием.
        /// Текущая реализация: 20 + (40-(WEIGHT)/2.5) + (A+B)/2.5.
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
        /// Текущая реализация: УРОН * (1+(ВЕС/10)).
        /// 
        /// Диапазон значений: 1.2 - 1100
        /// </summary>
        /// <param name="damageBase"></param>
        /// <param name="totalWeight"></param>
        /// <returns></returns>
        public static float TotalDamage(float damageBase, float totalWeight)
        {
            return (float)Math.Round(damageBase * (1+(totalWeight / 10)),1);
        }

        /// <summary>
        /// Рассчет урона при атаке по врагу.
        /// 
        /// Текущая реализация:
        /// Умножить урон на (СКОРОСТЬ/ОБЩАЯСКОРОСТЬ).
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="spinSpeed"></param>
        /// <returns></returns>
        public static float AttackToEnemyDamage(float damage, 
            float spinSpeed,float spinSpeedoriginal)
        {
            return damage * (spinSpeed / spinSpeedoriginal);
        }

        /// <summary>
        /// Рассчет урона при атаке по врагу атакующим рывком
        /// 
        /// Текущая реализация:
        /// Умножить урон на 0.5+(ВЕС/200)
        /// </summary>
        /// <param name="weapon"></param>
        /// <returns></returns>
        public static float AttackToEnemyDamageLongAttack(IWeapon weapon)
        {
            return weapon.Damage * (weapon.Weight / 200);
        }

        /// <summary>
        /// Установить длину позиции для атакующего рывка
        /// 
        /// Текущая реализация:
        /// Длина по оси Z=8-(ВЕС/50)
        /// </summary>
        /// <param name="weapon"></param>
        /// <param name="attackPosition"></param>
        public static void SetAttackTransformPosition(IWeapon weapon,Transform attackPosition)
        {
            attackPosition.localPosition = new Vector3(0,0.5f, 8-(weapon.Weight/50));
        }

        /// <summary>
        /// Хватаит ли мощности оружия, чтобы сгенерить эффект молний?
        /// </summary>
        /// <param name="weapon"></param>
        /// <returns></returns>
        public static bool MayableToBeElectricity(IWeapon weapon)
        {
            return weapon.SpinSpeed / weapon.OriginalSpinSpeed >= 0.25f 
                ? true : false;
        }

        /// <summary>
        /// Можно ли отталкнуть врага?
        /// </summary>
        /// <param name="weapon"></param>
        /// <returns></returns>
        public static bool MayToNockbackEnemy(IWeapon weapon)
        {
            return weapon.SpinSpeed / weapon.OriginalSpinSpeed >= 0.95f ? true : false;
        }

        /// <summary>
        /// Возвращает значение, которое лежит в границах от и до.
        /// 
        /// Текущая реализация: ЛЕВ + ((ПРАВ-ЛЕВ)*ЧАСТНОЕ).
        /// Если: a=1, b=3, frequent=0.5.
        /// Тогда: return 2
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="frequent"></param>
        /// <returns></returns>
        public static float GetValueByFrequent(float a, float b, float frequent)
        {
            return a + ((b - a) * frequent);
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

        /// <summary>
        /// Функция, вычисляющая угол, между двумя векторами. 
        /// Используется в классе PlayerController, когда необходимо вычислять
        /// угол, между игроком и точкой назначения, а затем блокировать эту точку,
        /// в случае, если она приведет к коллизии.
        /// </summary>
        /// <param name="to">Пункт назначения</param>
        /// <param name="from">Точка отсчета</param>
        /// <returns></returns>
        public static float CalculateAngleBetweenPointAndVector(Vector3 to, Vector3 from)
        {
            return Quaternion.FromToRotation(Vector3.forward,
                to - from).eulerAngles.y;
        }
    }
}
