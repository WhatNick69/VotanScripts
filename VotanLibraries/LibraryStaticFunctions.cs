using PlayerBehaviour;
using System;
using UnityEngine;
using VotanInterfaces;

namespace VotanLibraries
{
	/// <summary> 
	/// Статические функции 
	/// </summary> 
	public class LibraryStaticFunctions
	: MonoBehaviour
	{
		/// <summary> 
		/// В указанном диапазоне возвращает значение. 
		/// 
		/// Текущая реализация: если dmg=10, а range=0.1, то 
		/// 0.9 - 1.1 
		/// </summary> 
		/// <param name="dmg"></param> 
		/// <param name="range"></param> 
		/// <returns></returns> 
		public static float GetRangeValue(float dmg, float range = 0)
		{
			if (range > 1) range = 1;
			else if (range < 0) range = 0;

			return dmg + (UnityEngine.Random.Range(0, 1f)
			* 2 - 1) * dmg * range;
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
			return UnityEngine.Random.Range(0, 1f) * valueMax * 2 - valueMax;
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
			return UnityEngine.Random.Range(0, 1f) <= (gemPower / 220) + 0.05f ? true : false;
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
		///</summary> 
		/// <param name="weapon"></param> 
		/// <returns></returns> 
		public static float StrenghtOfNockback(IWeapon weapon, bool isSuperAttack = false)
		{
			return isSuperAttack ? 3 + (weapon.GetPlayer.PlayerArmory.ArmoryWeight / 100) : 2 + (weapon.GemPower / 100);
		}

        /// <summary> 
        /// Рассчитать силу замедления вращения оружия при попадании по врагу. 
        /// 
        /// Чем меньше множитель - тем меньше замедление
        /// Текущая реализация: ОРИГ._СКОР._ВРАЩЕНИЯ * (0.4 - (ВЕС_БРОНИ/400)). 
        /// Диапазон значений: ~0 - 25. 
        /// </summary> 
        /// <param name="originalSpinSpeed"></param> 
        /// <param name="armoryWeight"></param> 
        /// <returns></returns> 
        public static float CalculateSpinSpeedSlowMotionValue(float originalSpinSpeed, float armoryWeight)
		{
			return originalSpinSpeed * (0.2f - (armoryWeight / 500));
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
        /// Возвращает случайное качество предмета
        /// </summary>
        /// <returns></returns>
        public static ItemQuality RandomDropItemQuality()
        {
            float randomValue = UnityEngine.Random.RandomRange(0, 1f);
            if (randomValue >= 0.9f)
            {
                return ItemQuality.Strong;
            }
            else if (randomValue >= 0.6f)
            {
                return ItemQuality.Medium;
            }
            else 
            {
                return ItemQuality.Lite;
            }
        }

		/// <summary> 
		/// Может ли босс воспроизвести анимацию получения урона?
		/// 
		/// Текущая реализация: если получаемый урон больше либо равен 
		/// 0.05 прочности брони врага, то воспроизводим анимацию 
		/// </summary> 
		/// <param name="healthValue"></param> 
		/// <param name="dmg"></param> 
		/// <returns></returns> 
		public static bool BossMayPlayGetDamageAnimation(float healthValue, float dmg)
		{
			return dmg / healthValue >= 0.05f ? true : false;
		}

		/// <summary> 
		/// Получаем вес всех элеметов брони 
		/// Текущая реализация: сложение. 
		/// 
		/// Диапазон значений: 0 - 100 
		/// </summary> 
		/// <param name="weightA"></param> 
		/// <param name="weightB"></param> 
		/// <returns></returns> 
		public static float TotalWeight(float weightA, float weightB, float weightC)
		{
			return weightA + weightB + weightC;
		}

		/// <summary> 
		/// Зависимость скорости передвижения персонажа от веса оружия. 
		/// </summary> 
		/// <param name="weight"></param> 
		/// <returns></returns> 
		public static float DependenceMoveSpeedAndArmoryWeight(float weight)
		{
			return 1.5f + (0.7f - (weight * 0.007f));
		}

		/// <summary> 
		/// Зависимость скорости поворота персонажа от веса оружия. 
		/// </summary> 
		/// <param name="weight"></param> 
		/// <returns></returns> 
		public static float DependenceRotateSpeedAndArmoryWeight(float weight)
		{
			return 3.5f + (3.5f - (weight * 0.035f));
		}

		/// <summary> 
		/// Получаем общую скорость вращения оружием. 
		/// Текущая реализация: 20 + (80-(WEIGHT)/1.25). 
		/// 
		/// Диапазон значений: 20 - 100 
		/// </summary> 
		/// <param name="totalWeight"></param> 
		/// <returns></returns> 
		public static float TotalSpinSpeed(float totalWeight)
		{
			return 20 + (80 - (totalWeight) / 1.25f);
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
			return (float)Math.Round(damageBase * (1 + (totalWeight / 10)), 1);
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
		public static float AttackToEnemyDamage(IWeapon weapon)
		{
			return weapon.Damage *
			(weapon.SpinSpeed / weapon.OriginalSpinSpeed);
		}

		/// <summary> 
		/// Вызван ли критический удар? 
		/// </summary> 
		/// <returns></returns> 
		public static bool IsCritHit()
		{
			return UnityEngine.Random.Range(0, 1f) <= 0.1f ? true : false;
		}

		/// <summary> 
		/// Критический удар 
		/// </summary> 
		/// <returns></returns> 
		public static float DamageWithCrit(float damage, float critPercentages)
		{
			return damage * (critPercentages / 100);
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
			return weapon.Damage * (weapon.GetPlayer.PlayerArmory.ArmoryWeight / 150);
		}

		/// <summary> 
		/// Установить длину позиции для атакующего рывка 
		/// 
		/// Текущая реализация: 
		/// Длина по оси Z=8-(ВЕС/50) 
		/// </summary> 
		/// <param name="weapon"></param> 
		/// <param name="attackPosition"></param> 
		public static void SetAttackTransformPosition(IWeapon weapon, Transform attackPosition)
		{
			attackPosition.localPosition = new Vector3(0, 0.5f, 8 - (weapon.GetPlayer.PlayerArmory.ArmoryWeight / 50));
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
		public static Color GetColorFromGemPower(float gemPower, GemType damageType)
		{
			switch (damageType)
			{
				case GemType.Electric:
					return new Color((gemPower * 2.55f) * 0.003921f, 0, 1);

				case GemType.Fire:
					float g = gemPower / 100;
					return g >= 0.647f ?
					    new Color(1 - ((gemPower / 100) - 0.647f), 0, 0) :
					    new Color(1, 0.647f - g, 0);

				case GemType.Frozen:
					return new Color(1 - (0.21569f + (gemPower * 2) * 0.0039f), 1, 1);

				case GemType.Powerful:
					return new Color(0.803f - gemPower * 0.002f,
					0.5215f + gemPower * 0.0028f, 0.247f - gemPower * 0.0005f);

				default:
					return Color.white;
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


        /// <summary>
        /// Конвертатор очков в ресурсы (железо, дерево и гемы)
        /// </summary>
        /// <param name="playerResources"></param>
        public static void ConvertScoreToResources(PlayerResources playerResources)
        {
            long playerScore = playerResources.ScoreValue;
            playerScore /= 100;
            playerResources.SteelResource = (long)(playerScore * 0.3f);
            playerResources.WoodResource = (long)(playerScore * 0.7f);
            playerResources.Gems = GetRandomGemRes();
        }

        /// <summary>
        /// Вернуть случайное количество гемов (от 0 до 3)
        /// </summary>
        /// <returns></returns>
        private static long GetRandomGemRes()
        {
            float gems = UnityEngine.Random.RandomRange(0, 1f);
            if (gems >= 0.99f)
            {
                return 3;
            }
            else if (gems >= 0.93f)
            {
                return 2;
            }
            else if (gems >= 0.9f)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}