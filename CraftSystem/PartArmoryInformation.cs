using GameBehaviour;
using PlayerBehaviour;
using UnityEngine;

namespace CraftSystem
{
    /// <summary>
    /// Информация об префабе брони
    /// </summary>
    public class PartArmoryInformation
		: MonoBehaviour
	{
        #region Переменные
        [SerializeField, Tooltip("Стоимость брони в золоте")]
        private long moneyCost;
        [SerializeField, Tooltip("Стоимость брони в гемах")]
        private long gemsCost;
        [SerializeField, Tooltip("Изображение части брони")]
		private Sprite imageArmory;
		[SerializeField, Tooltip("Тип брони")]
		private ArmoryClass armoryType;
        [SerializeField, Range(1, 10000f), Tooltip("Базовое значение части брони")]
        private float armoryValue;
        [SerializeField, Range(1, 33), Tooltip("Вес")]
        private float weightArmory;
        [SerializeField, Tooltip("Название части брони")]
		private string armoryName;
        private PlayerComponentsControl playerComponentsControl;
        #endregion

        #region Свойства
        public float ArmoryValue
		{
			get
			{
				return armoryValue;
			}

			set
			{
				armoryValue = value;
			}
		}

		public Sprite ImageArmory
		{
			get
			{
				return imageArmory;
			}

			set
			{
				imageArmory = value;
			}
		}

		public string ArmoryName
		{
			get
			{
				return armoryName;
			}

			set
			{
				armoryName = value;
			}
		}

		public Sprite ImageArm
		{
			get
			{
				return imageArmory;
			}
		}

        public ArmoryClass ArmoryType
        {
            get
            {
                return armoryType;
            }

            set
            {
                armoryType = value;
            }
        }

        public float WeightArmory
        {
            get
            {
                return weightArmory;
            }

            set
            {
                weightArmory = value;
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
        #endregion

        /// <summary>
        /// Инициализация
        /// </summary>
        private void Awake()
        {
            playerComponentsControl = GameObject.FindWithTag("Player").
                GetComponent<PlayerComponentsControl>();
            InitToPlayerArmory();
            Destroy(gameObject, 3);
        }

        /// <summary>
        /// Инициализация частей брони
        /// </summary>
        private void InitToPlayerArmory()
        {
            switch (armoryType)
            {
                case ArmoryClass.Helmet:
                    playerComponentsControl.PlayerArmory.Helmet
                        = transform.GetChild(0).GetComponent<PartArmoryManager>();
                    break;
                case ArmoryClass.Cuirass:
                    IterationsForNonHelmet(true);
                    break;
                case ArmoryClass.Shield:
                    IterationsForNonHelmet(false);
                    break;
            }
        }

        /// <summary>
        /// Итерационная инициализация листа с кирасами, либо со щитом
        /// </summary>
        /// <param name="isKirasa"></param>
        private void IterationsForNonHelmet(bool isKirasa)
        {
            for (int i = 0;i<transform.childCount;i++)
            {
                if (isKirasa)
                {
                    playerComponentsControl.PlayerArmory.KirasaParts
                        .Add(transform.GetChild(i).GetComponent<PartArmoryManager>());
                }
                else
                {
                    playerComponentsControl.PlayerArmory.ShieldParts
                        .Add(transform.GetChild(i).GetComponent<PartArmoryManager>());
                }
            }
        }
    }
}
