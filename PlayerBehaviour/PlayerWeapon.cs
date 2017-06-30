using UnityEngine;
using AbstractBehaviour;
using System;

namespace PlayerBehaviour
{
    /// <summary>
    /// Интерфейс для описания оружия
    /// </summary>
    interface IWeapon
	{
        string typeName1 { get; set; }
		string typeName2 { get; set; }
		string weaponName { get; set; }
		string gripName { get; set; }

        void Start();
        void SetWeapon(float damage, float defenceValue, DamageType attackType, float spinSpeed);
        void SetWeapon(float damage, DamageType attackType, float spinSpeed);
		void SetWeapon(float damage, DamageType attackType);
		void SetWeapon(float damage);
	}

	/// <summary>
    /// Класс, описывающий поведение оружия
    /// </summary>
    public class PlayerWeapon 
        : MonoBehaviour, IWeapon
	{
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
        [SerializeField, Tooltip("Скорость вращения"), Range(40, 70)]
        private float spinSpeed;
        [SerializeField]
		public GameObject player { get; set; }

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

		/// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="attackType"></param>
        /// <param name="spinSpeed"></param>
        public PlayerWeapon(float damage, float defenceValue, DamageType attackType, float spinSpeed) 
		{
            this.defenceValue = defenceValue;
			this.damage = damage;
			this.attackType = attackType;
			this.spinSpeed = spinSpeed;
		}

        /// <summary>
        /// Задать характеристики оружия через урон, 
        /// мощность блока, тип урона и скорость
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="attackType"></param>
        /// <param name="spinSpeed"></param>
        public void SetWeapon(float damage, float defenceValue,DamageType attackType, float spinSpeed)
        {
            this.defenceValue = defenceValue;
            this.damage = damage;
            this.attackType = attackType;
            this.spinSpeed = spinSpeed;
        }

        /// <summary>
        /// Задать характеристики оружия через урон
        /// , тип урона и скорость
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="attackType"></param>
        /// <param name="spinSpeed"></param>
        public void SetWeapon(float damage, DamageType attackType, float spinSpeed)
		{
			this.damage = damage;
			this.attackType = attackType;
			this.spinSpeed = spinSpeed;
		}

		/// <summary>
        /// Задать характеристики оружия через урон и тип урона
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="attackType"></param>
        public void SetWeapon(float damage, DamageType attackType)
		{
			this.damage = damage;
			this.attackType = attackType;
		}

		/// <summary>
        /// Задать характеристики оружия через урон
        /// </summary>
        /// <param name="damage"></param>
        public void SetWeapon(float damage)
		{
			this.damage = damage;
		}

        /// <summary>
        /// Инициализация
        /// </summary>
        public void Start()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<PlayerFight>().MyWeapon = this;
        }
    }
}
