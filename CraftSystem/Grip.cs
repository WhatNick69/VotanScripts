using PlayerBehaviour;
using UnityEngine;
using UnityEngine.UI;

namespace CraftSystem
{
	/// <summary>
	/// Ручка оружия
	/// </summary>
	public class Grip
		: MonoBehaviour
	{
        #region Переменные
        [SerializeField, Tooltip("Изображение рукояти")]
        private Sprite imageGrip;
        [SerializeField, Range(1, 25f), Tooltip("Вес ручки")]
		private float gripWeight;
		[SerializeField, Range(1, 100f), Tooltip("Защита")]
		private float gripDefence;
		[SerializeField, Range(0, 75f), Tooltip("Бонус к скорости вращения за счет ручки")]
		private float bonusSpinSpeedFromGrip;

		[SerializeField, Tooltip("Тип рукояти")]
		private LenghtGrip gripType;
		[SerializeField, Tooltip("Название рукояти")]
		private string gripName;
		[SerializeField]
		private string prefabName; // Задать ,только, имя префаба

		private string linkPrefab = "Prefabs/Weapon/Grip/";
		#endregion

		#region Свойства 
		/// <summary>
		/// Возвращает ссылку на префаб обьекта
		/// </summary>
		public string PrefabName
		{
			get
			{
				return linkPrefab + prefabName;
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

        public LenghtGrip GripType
        {
            get
            {
                return gripType;
            }

            set
            {
                gripType = value;
            }
        }

        public float BonusSpinSpeedFromGrip
        {
            get
            {
                return bonusSpinSpeedFromGrip;
            }

            set
            {
                bonusSpinSpeedFromGrip = value;
            }
        }

        public float GripWeight
        {
            get
            {
                return gripWeight;
            }

            set
            {
                gripWeight = value;
            }
        }

        public float GripDefence
        {
            get
            {
                return gripDefence;
            }

            set
            {
                gripDefence = value;
            }
        }

		public Sprite ItemImage
		{
			get
			{
				return imageGrip;
			}
		}
		#endregion
	}
}
