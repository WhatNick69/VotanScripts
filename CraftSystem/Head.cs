using PlayerBehaviour;
using UnityEngine;

namespace CraftSystem
{
    /// <summary>
    /// Наконечник оружия
    /// </summary>
    public class Head 
        : MonoBehaviour
    {
        #region Переменные
        [SerializeField, Range(1, 75f),Tooltip("Вес головной части")]
        private float headWeight;
        [SerializeField,Range(1,100f),Tooltip("Базовое значение урона оружием")]
        private int damageBase;
        [SerializeField, Range(0, 25f), Tooltip("Бонус к скорости вращения за счет головной части")]
        private float bonusSpinSpeedFromHead;

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

        public float BonusSpinSpeedFromHead
        {
            get
            {
                return bonusSpinSpeedFromHead;
            }

            set
            {
                bonusSpinSpeedFromHead = value;
            }
        }

        public float HeadWeight
        {
            get
            {
                return headWeight;
            }

            set
            {
                headWeight = value;
            }
        }

        public int DamageBase
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
        #endregion
    }
}
