using GameBehaviour;
using PlayerBehaviour;
using UnityEngine;
using UnityEngine.UI;

namespace CraftSystem
{
	/// <summary>
	/// Информация об префабе брони
	/// </summary>
	public class PartArmoryInformation
		: MonoBehaviour
	{
		#region Переменные
		[SerializeField, Tooltip("Изображение части брони")]
		private Sprite imageArmory;
		[SerializeField, Tooltip("Тип брони")]
		private ArmoryClass armoryType;
		[SerializeField, Range(1, 1000f), Tooltip("Базовое значение части брони")]
		private float armoryValue;
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
			
        #endregion

        /// <summary>
        /// Инициализация
        /// </summary>
        private void Awake()
        {
            playerComponentsControl = GameObject.FindWithTag("Player").
                GetComponent<PlayerComponentsControl>();
            playerComponentsControl.PlayerArmory.HealthValue += armoryValue;
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
