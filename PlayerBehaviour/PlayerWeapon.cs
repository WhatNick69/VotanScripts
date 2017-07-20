using UnityEngine;
using System.Collections.Generic;
using MovementEffects;
using VotanInterfaces;
using System;
using VotanLibraries;

namespace PlayerBehaviour
{
    /// <summary>
    /// Интерфейс для описания оружия
    /// 
    /// 1) проверка на дистанцию... если меньше указанной - враг попадает в торнадо... нет, скорость врага будет увеличиваться.
    /// 2) землетрясение и расхождение атакующей волны в определенном радиусе. спиралька ^^
    /// 
    /// </summary>
    public interface IWeapon
	{
        #region Переменные
        /// <summary>
        /// Тип оружия 1
        /// </summary>
        string TypeName1 { get; set; }

		/// <summary>
        /// Тип оружия 2
        /// </summary>
        string TypeName2 { get; set; }

		/// <summary>
        /// Название оружия
        /// </summary>
        string WeaponName { get; set; }

		/// <summary>
        /// Название рукояти
        /// </summary>
        string GripName { get; set; }

        /// <summary>
        /// Получить игрока
        /// </summary>
        IPlayerBehaviour GetPlayer { get; }

        /// <summary>
        /// Значения защиты
        /// </summary>
        float DefenceValue { get; set; }

        /// <summary>
        /// Значения урона оружием
        /// </summary>
        float Damage { get; set; }

        /// <summary>
        /// Тип атаки
        /// </summary>
        DamageType AttackType { get; set; }

        /// <summary>
        /// Скорость вращения оружием
        /// </summary>
        float SpinSpeed { get; set; }

        /// <summary>
        /// Сила камня
        /// </summary>
        float GemPower { get; set; }

        /// <summary>
        /// Вес оружия
        /// </summary>
        float Weight { get; set; }

        /// <summary>
        /// Оригинальная скорость оружия
        /// </summary>
        float OriginalSpinSpeed { get; set; }

        /// <summary>
        /// Трэил-лента оружия
        /// </summary>
        TrailRenderer TrailRenderer { get; set; }
        #endregion

        /// <summary>
        /// Инициализация
        /// </summary>
        void Start();

        /// <summary>
        /// Задаем урон оружию, величину защиты, тип атаки, 
        /// скорость вращения и весь оружия.
        /// Урон оружия влияет на урон, который оно наносит.
        /// Величина защиты влияет на величину урона, которую
        /// оружие может поглатить.
        /// Тип атаки оружия: физика, электричество, огонь или мороз.
        /// Скорость вращения влияет на скорость вращения оружием.
        /// Вес оружия влияет на скорость его замедления при ударе врага.
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="defenceValue"></param>
        /// <param name="attackType"></param>
        /// <param name="spinSpeed"></param>
        /// <param name="weight"></param>
        /// <param name="gemPower"></param>
        void SetWeaponParameters(float damage, float defenceValue, DamageType attackType,
            TrailRenderer trailRenderer, float spinSpeed,float weight,float gemPower);

        /// <summary>
        /// Снижаем скорость вращения при попадании по врагу
        /// </summary>
        void WhileTime();

        /// <summary>
        /// Корутина, для осуществления замедления оружия игрока
        /// </summary>
        /// <returns></returns>
        IEnumerator<float> CoroutineDoSlowMotionSpinSpeed();

        /// <summary>
        /// Установить цвет трэил-ленты оружия
        /// </summary>
        void SetColorTrailWeapon();
    }

	/// <summary>
    /// Класс, описывающий поведение оружия
    /// </summary>
    public class PlayerWeapon 
        : MonoBehaviour, IWeapon
	{
        #region Переменные
        [SerializeField, Tooltip("Название оружия 1")]
        private string typeName1;
        [SerializeField, Tooltip("Название оружия 1")]
        private string typeName2;
        [SerializeField, Tooltip("Название оружия")]
        private string weaponName;
        [SerializeField, Tooltip("Название рукояти")]
        private string gripName;
        private TrailRenderer trailRenderer;

        [SerializeField, Tooltip("Урон от оружия"), Range(1, 2200f)]
		private float damage;
		[SerializeField, Tooltip("Защита"), Range(0, 99f)]
		private float defenceValue;
		[SerializeField]
		private DamageType damageType;
		[SerializeField, Tooltip("Скорость вращения"), Range(10, 100f)]
		private float spinSpeed;
        [SerializeField, Tooltip("Сила камня"), Range(1, 100f)]
        private float gemPower;
        private float originalSpinSpeed;

		[SerializeField, 
            Tooltip("Вес оружия. Чем больше - тем меньше замедление при попадании по противнику")
                , Range(1f, 100f)]
		private float weight;
		[SerializeField, Tooltip("Задержка перед возвращением скорости"), Range(0.1f, 3)]
		private float speedReturnLatency;

		[SerializeField, Tooltip("Хранитель компонентов")]
		private PlayerComponentsControl playerComponentsControl;
        #endregion

        #region Свойства
        public float Damage
        {
            get
            {
                return damage;
            }

            set
            {
                damage = value;
            }
        }

        public float DefenceValue
        {
            get
            {
                return defenceValue;
            }

            set
            {
                if (value >= 100)
                    value = 99;
                defenceValue = value;
            }
        }

        public DamageType AttackType
        {
            get
            {
                return damageType;
            }

            set
            {
                damageType = value;
            }
        }

        public float SpinSpeed
        {
            get
            {
                return spinSpeed;
            }

            set
            {
                spinSpeed = value;
            }
        }

        public float Weight
        {
            get
            {
                return weight;
            }

            set
            {
                weight = value;
            }
        }

        public float GemPower
        {
            get
            {
                return gemPower;
            }

            set
            {
                gemPower = value;
            }
        }

        public float OriginalSpinSpeed
        {
            get
            {
                return originalSpinSpeed;
            }

            set
            {
                originalSpinSpeed = value;
            }
        }

        public IPlayerBehaviour GetPlayer
        {
            get
            {
                return playerComponentsControl;
            }
        }

        public string TypeName1
        {
            get
            {
                return typeName1;
            }

            set
            {
                typeName1 = value;
            }
        }

        public string TypeName2
        {
            get
            {
                return typeName2;
            }

            set
            {
                typeName2 = value;
            }
        }

        public string WeaponName
        {
            get
            {
                return weaponName;
            }

            set
            {
                weaponName = value;
            }
        }

        public string GripName
        {
            get
            {
                return gripName;
            }

            set
            {
                gripName = value;
            }
        }

        public TrailRenderer TrailRenderer
        {
            get
            {
                return trailRenderer;
            }

            set
            {
                trailRenderer = value;
            }
        }
        #endregion

        /// <summary>
        /// Задать характеристики оружия через урон, 
        /// мощность блока, тип урона, скорость вращения и вес   
        /// </summary>
        /// <param name="damage">Величина урона от оружия.</param>
        /// <param name="defenceValue">Величина защита от оружия.</param>
        /// <param name="attackType">Тип атаки оружия от камня</param>
        /// <param name="trailRenderer">Трэил-лента оружия</param>
        /// <param name="spinSpeed">Скорость вращения оружием</param>
        /// <param name="weight">Вес оружия</param>
        /// <param name="gemPower">Сила камня</param>
        public void SetWeaponParameters(float damage, float defenceValue,
            DamageType damageType, TrailRenderer trailRenderer ,float spinSpeed,float weight,float gemPower)
        {
            DefenceValue = defenceValue;
            this.damage = damage;
            this.damageType = damageType;
            this.spinSpeed = spinSpeed;
            this.weight = weight;
            this.gemPower = gemPower;
            this.trailRenderer = trailRenderer;

            SetColorTrailWeapon();
            originalSpinSpeed = spinSpeed;
        }
        
        /// <summary>
        /// Инициализация
        /// </summary>
        public void Start()
        {
            playerComponentsControl.PlayerWeapon = this;
        }

        /// <summary>
        /// Временно снижаем скорость вращения 
        /// оружием при попадании по врагу
        /// </summary>
        public void WhileTime()
        {
            Timing.RunCoroutine(CoroutineDoSlowMotionSpinSpeed());
        }

        /// <summary>
        /// Корутин на замедление вращения
        /// </summary>
        /// <returns></returns>
        public IEnumerator<float> CoroutineDoSlowMotionSpinSpeed()
        {

			float spSpeed = spinSpeed * (0.5f + Weight / 200);

            if (spinSpeed / originalSpinSpeed >= 0.25f)
                playerComponentsControl.PlayerCameraSmooth.DoNoize(spinSpeed / originalSpinSpeed);

            spinSpeed -= spSpeed;
         
            yield return Timing.WaitForSeconds(speedReturnLatency);

            for (int i = 0;i<10;i++)
            {
                spinSpeed += spSpeed / 10;
                if (spinSpeed > originalSpinSpeed)
                {
                    spinSpeed = originalSpinSpeed;
				}
                yield return Timing.WaitForSeconds(0.1f);
            }
        }

        /// <summary>
        /// Установить цвет трэил-ленты,
        /// время жизни трэила (1 - 2 секунды)
        /// и ширину ленты (0.2 - 0.3),
        /// в зависимости от силы камня
        /// </summary>
        public void SetColorTrailWeapon()
        {
            Color color = LibraryStaticFunctions.GetColorFromGemPower(GemPower, damageType);
            if (color == Color.black)
            {
                TrailRenderer.enabled = false;
                return;
            }
            //TrailRenderer.time = 1 + GemPower / 300; // время жизни молнии. от 1 до 1.33 секунды
            TrailRenderer.startWidth = 0.2f + GemPower / 2000; // размер молнии. от 0.2 до 0.25
            TrailRenderer.startColor = color;
            TrailRenderer.endColor = color;
        }
    }

    /// <summary>
    /// Типы атаки. Перечисление
    /// 
    /// Frozen - замораживающая атака
    /// Fire - огненная атака
    /// Powerful - физическая атака
    /// Electric - электрическая атака
    /// </summary>
    public enum DamageType
    {
        Frozen, Fire, Powerful, Electric
    }

    /// <summary>
    /// Тип оружия. Перечисление.
    /// 
    /// Crushing - дробящее оружие.
    /// Cutting - режущее оружие
    /// </summary>
    public enum WeaponType
    {
        Crushing, Cutting
    }

    /// <summary>
    /// Длина рукояти. Перечисление
    /// </summary>
    public enum LenghtGrip
    {
        Long, Middle, Short
    }
}
