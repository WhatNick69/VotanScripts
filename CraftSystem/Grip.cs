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

		[SerializeField, Tooltip("Тип рукояти")]
		private LenghtGrip gripType;
		[SerializeField, Tooltip("Название рукояти")]
		private string gripName;
		[SerializeField]
		private string prefabName; // Задать только имя префаба

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
