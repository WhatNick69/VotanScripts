using PlayerBehaviour;
using UnityEngine;
using UnityEngine.UI;

namespace CraftSystem
{
    /// <summary>
    /// Наконечник оружия
    /// </summary>
    public class Head 
        : MonoBehaviour
    {
        #region Переменные
        [SerializeField, Tooltip("Изображение лезвия")]
        private Sprite imageHead;
        [SerializeField,Range(1,100f),Tooltip("Базовое значение урона оружием")]
        private float damageBase;
        [SerializeField, Range(0,100f), Tooltip("Шанс критического урона")]
        private float criticalChance;

        [SerializeField, Tooltip("Тип оружия")] 
        private WeaponType headType;
        [SerializeField,Tooltip("Название головы")]
        private string headName;
        [SerializeField, Tooltip("Трэил-лента оружия")]
        private TrailRenderer trailRenderer;
		private string prefabName;
		private string linkPrefab = "Prefabs/Weapon/Grip/";
		#endregion

		#region Свойства
		public string PrefabName
		{
			get
			{
				return prefabName + linkPrefab;
			}
		}
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

        public WeaponType HeadType
        {
            get
            {
                return headType;
            }

            set
            {
                headType = value;
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
        #endregion
    }
}
