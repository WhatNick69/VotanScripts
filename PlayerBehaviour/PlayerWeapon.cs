using UnityEngine;
using System.Collections.Generic;
using MovementEffects;
using Playerbehaviour;

namespace PlayerBehaviour
{
    /// <summary>
    /// Интерфейс для описания оружия
    /// 
    /// 1) проверка на дистанцию... если меньше указанной - враг попадает в торнадо... нет, скорость врага будет увеличиваться.
    /// 2) землетрясение и расхождение атакующей волны в определенном радиусе. спиралька ^^
    /// 
    /// </summary>
    interface IWeapon
	{
        string typeName1 { get; set; }
		string typeName2 { get; set; }
		string weaponName { get; set; }
		string gripName { get; set; }

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
        void SetWeaponParameters(float damage, float defenceValue, DamageType attackType, float spinSpeed,float weight);

        /// <summary>
        /// Снижаем скорость вращения при попадании по врагу
        /// </summary>
        void WhileTime();
    }

	/// <summary>
    /// Класс, описывающий поведение оружия
    /// </summary>
    public class PlayerWeapon 
        : MonoBehaviour, IWeapon
	{
        #region Переменные
        public string typeName1 { get; set; }
		public string typeName2 { get; set; }
		public string weaponName { get; set; }
		public string gripName { get; set; }

        [SerializeField, Tooltip("Урон от оружия")]
        private float damage;
        [SerializeField, Tooltip("Защита"), Range(0, 0.9f)]
        private float defenceValue;
        [SerializeField]
        private DamageType attackType;
        [SerializeField, Tooltip("Скорость вращения"), Range(40, 100)]
        private float spinSpeed;
        private float originalSpinSpeed;

        [SerializeField, Tooltip("Величина замедления при попадании по врагу. Вес оружия"), Range(0.1f, 0.5f)]
        private float weight;
        [SerializeField, Tooltip("Задержка перед возвращением скорости"),Range(0.5f,3)]

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
                defenceValue = value;
            }
        }

        public DamageType AttackType
        {
            get
            {
                return attackType;
            }

            set
            {
                attackType = value;
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
        #endregion

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="defenceValue"></param>
        /// <param name="attackType"></param>
        /// <param name="spinSpeed"></param>
        /// <param name="weight"></param>
        public PlayerWeapon(float damage, float defenceValue, DamageType attackType, float spinSpeed,float weight) 
		{
            this.defenceValue = defenceValue;
			this.damage = damage;
			this.attackType = attackType;
			this.spinSpeed = spinSpeed;
            this.weight = weight;
		}

        /// <summary>
        /// Задать характеристики оружия через урон, 
        /// мощность блока, тип урона, скорость вращения и вес
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="defenceValue"></param>
        /// <param name="attackType"></param>
        /// <param name="spinSpeed"></param>
        /// <param name="weight"></param>
        public void SetWeaponParameters(float damage, float defenceValue,DamageType attackType, float spinSpeed,float weight)
        {
            this.defenceValue = defenceValue;
            this.damage = damage;
            this.attackType = attackType;
            this.spinSpeed = spinSpeed;
            this.weight = weight;
        }
        
        /// <summary>
        /// Инициализация
        /// </summary>
        public void Start()
        {
            originalSpinSpeed = spinSpeed;
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
        private IEnumerator<float> CoroutineDoSlowMotionSpinSpeed()
        {
            float spSpeed = spinSpeed* Weight;

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
    }

    /// <summary>
    /// Типы атаки. Перечисление
    /// </summary>
    public enum DamageType
    {
        Frozen, Fire, Powerful, Electric
    }
}
