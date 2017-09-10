using PlayerBehaviour;
using UnityEngine;

namespace CraftSystem
{
    /// <summary>
    /// Наконечник оружия
    /// </summary>
    public class Weapon
        : MonoBehaviour
    {
        #region Переменные
        [SerializeField, Tooltip("Стоимость брони в золоте")]
        private long moneyCost;
        [SerializeField, Tooltip("Стоимость брони в гемах")]
        private long gemsCost;
        [SerializeField, Tooltip("Изображение оружия")]
        private Sprite imageHead;
        [SerializeField, Range(1, 1000f), Tooltip("Базовое значение урона оружием")]
        private float damageBase;
        [SerializeField, Range(100, 999), Tooltip("Величина критического умножение урона в процентах")]
        private float criticalChance;
        [SerializeField, Tooltip("Название оружия")]
        private string headName;
        [SerializeField, Tooltip("Трэил-лента оружия")]
        private TrailRenderer trailRenderer;
        [SerializeField, Tooltip("Тип атаки камня")]
        private GemType damageTypeGem;
        [SerializeField, Range(0, 100f), Tooltip("Сила камня")]
        private float gemPower;
		[SerializeField]
		Transform attackPoint;
        private string prefabName;
        #endregion

        #region Свойства
       
        public string HeadName
        {
            get
            {
                return headName;
            }

            set
            {
                headName = value;
            }
        }

        public float DamageBase
        {
            get
            {
                return damageBase;
            }

            set
            {
                damageBase = value;
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

        public Sprite ItemImage
        {
            get
            {
                return imageHead;
            }
        }

        public float CriticalChance
        {
            get
            {
                return criticalChance;
            }

            set
            {
                criticalChance = value;
            }
        }

        public GemType DamageTypeGem
        {
            get
            {
                return damageTypeGem;
            }

            set
            {
                damageTypeGem = value;
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

		public Transform AttackPoint
		{
			get
			{
				return attackPoint;
			}

			set
			{
				attackPoint = value;
			}
		}

        public long GemsCost
        {
            get
            {
                return gemsCost;
            }

            set
            {
                gemsCost = value;
            }
        }

        public long MoneyCost
        {
            get
            {
                return moneyCost;
            }

            set
            {
                moneyCost = value;
            }
        }
        #endregion
    }
}