using UnityEngine;
using PlayerBehaviour;
using VotanLibraries;

namespace CraftSystem
{
    /// <summary>
    /// Конструктор для оружия
    /// </summary>
    public class PlayerWeaponConstructor 
        : MonoBehaviour
	{
        #region Переменные
        [SerializeField] // точка, привязанная к правой руке (кости), отождествляет оружие
		private GameObject weapon;
		private PlayerComponentsControl plComponents;
		private PlayerWeapon plWeapon;

		private float weaponWeight;
		private float weaponDamage;
		private float weaponDefence;
		private DamageType weaponDamType;
		private float weaponSpinSpeed;

		private Grip gripClass;
		private Head headClass;
		private Gem gemClass;
		private GameObject grip;
		private GameObject head;
		private GameObject gem;

		private Vector3 attackPoint;
		private Vector3 headPosition;
		private Vector3 headRotate = new Vector3(35, 0, 0);
		private Vector3 gripPoint;

		private Vector3 shortGripPosition = new Vector3(0, 0, 0);
		private Vector3 shortGripRotate = new Vector3(0, 0, 0);

		private Vector3 midleGripPosition = new Vector3(2, 0, 0);
		private Vector3 midleGripRotate = new Vector3(0, 0, 0);

		private Vector3 longGripPosition = new Vector3(4, 0, 0);
		private Vector3 longGripRotate = new Vector3(0, 0, 0);

		private int gripType;
        #endregion

        /// <summary>
        /// Настройки оружия
        /// </summary>
        private void WeaponSettings()
		{
			switch (gripClass.GripType)
			{
				case LenghtGrip.Short:
					gripPoint = shortGripPosition;
					headPosition = new Vector3(-62,0,0);
					attackPoint = new Vector3(-70, 0, 0);
					break;
				case LenghtGrip.Middle:
					gripPoint = midleGripPosition;
					headPosition = new Vector3(-67, 0, 0);
					attackPoint = new Vector3(-72, 0, 0);
					break;
				case LenghtGrip.Long:
					gripPoint = longGripPosition;
					headPosition = new Vector3(-72,0,0);
					attackPoint = new Vector3(-82, 0, 0);
					break;
				default:
					gripPoint = Vector3.zero;
                    break;
			}
		}

		/// <summary>
        /// Установить статы для оружия
        /// </summary>
        private void SetWeaponStats()
		{
            // получаем вес оружия
			weaponWeight = 
                LibraryStaticFunctions.TotalWeight(gripClass.GripWeight,headClass.HeadWeight);
            // получаем вращения оружием
            weaponSpinSpeed = 
                LibraryStaticFunctions.TotalSpinSpeed
                (gripClass.BonusSpinSpeedFromGrip, headClass.BonusSpinSpeedFromHead, weaponWeight);
            // получаем урон оружием
            weaponDamage = 
                LibraryStaticFunctions.TotalDamage(headClass.DamageBase,weaponWeight);

            // защита от ручки
            weaponDefence = gripClass.GripDefence;

			weaponDamType = gemClass.DamageTypeGem;

			plComponents.PlayerWeapon.SetWeaponParameters(weaponDamage, weaponDefence, 
                weaponDamType, headClass.TrailRenderer,weaponSpinSpeed, weaponWeight, gemClass.GemPower);
		}

		/// <summary>
        /// Инициализация перед запуском первого кадра
        /// </summary>
        private void Awake()
		{
			plComponents = GameObject.FindWithTag("Player").GetComponent<PlayerComponentsControl>();
			grip = Instantiate(GameObject.Find("GetWeaponPrefabs").GetComponent<WeaponPrefabs>().Grip);
			head = Instantiate(GameObject.Find("GetWeaponPrefabs").GetComponent<WeaponPrefabs>().Head);
			gem = Instantiate(GameObject.Find("GetWeaponPrefabs").GetComponent<WeaponPrefabs>().Gem);

			gripClass = grip.GetComponent<Grip>();
			headClass = head.GetComponent<Head>();
			gemClass = gem.GetComponent<Gem>();

			WeaponSettings();

			grip.transform.parent = weapon.transform;
			grip.transform.localPosition = gripPoint;
			grip.transform.localEulerAngles = midleGripRotate;
			grip.transform.localScale = new Vector3(1,1,1);

			head.transform.parent = weapon.transform;
			head.transform.localPosition = headPosition;
			head.transform.localEulerAngles = headRotate;
			head.transform.localScale = new Vector3(1, 1, 1);

		    gem.transform.parent = weapon.transform;
		    gem.transform.localPosition = headPosition;
			gem.transform.localEulerAngles = headRotate;
	        gem.transform.localScale = new Vector3(1, 1, 1);

			plComponents.PlayerAttack.SetPlayerGunLocalPoint(attackPoint);
			SetWeaponStats();
		}
	}	
}
