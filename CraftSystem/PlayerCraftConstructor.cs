using PlayerBehaviour;
using UnityEngine;
using VotanLibraries;
using VotanInterfaces;

namespace CraftSystem
{
    /// <summary>
    /// Крафт игрока (оружие+броня)
    /// </summary>
    class PlayerCraftConstructor
        : MonoBehaviour
    {
        #region Переменные
        [SerializeField] // точка, привязанная к правой руке (кости), отождествляет оружие
        private GameObject weapon;
        private PlayerComponentsControl plComponents;
        private PlayerWeapon plWeapon;

        private float weaponWeight;
        private float weaponDamage;
		private float weaponCrit;
		private float weaponGemPower;
        private GemType weaponGemType;
        private float weaponSpinSpeed;

        private Weapon weaponClass;
        private GameObject weaponObj;

        private PartArmoryInformation shieldArmoryInformation;
        private PartArmoryInformation cuirassArmoryInformation;
        private PartArmoryInformation helmetArmoryInformation;
        #endregion

        /// <summary>
        /// Установить статы для оружия
        /// </summary>
        private void SetWeaponStats()
        {

			weaponGemType = weaponClass.DamageTypeGem;

			plComponents.PlayerWeapon.SetWeaponParameters(weaponClass.DamageBase, 
                weaponClass.CriticalChance, weaponGemType,
				weaponClass.TrailRenderer, weaponClass.GemPower);

			plComponents.PlayerWeapon.SetSpinSpeed
				(LibraryStaticFunctions.TotalSpinSpeed(plComponents.PlayerArmory.ArmoryWeight));
		}

        /// <summary>
        /// Общая инициализация
        /// </summary>
        private void Awake()
        {
            plComponents = GetComponent<PlayerComponentsControl>();

            ArmoryInitialisation();
            WeaponInitialisation();
            InventoryInitialisation();
			OtherPlayerInitialisation();
		}

        /// <summary>
        /// Инициализация оружия
        /// </summary>
        private void WeaponInitialisation()
        {
			weaponObj = Instantiate(GameObject.Find("GetPrefabs").GetComponent<WeaponPrefabs>().Weapon);

			weaponClass = weaponObj.GetComponent<Weapon>();

			weaponObj.transform.parent = weapon.transform;
			weaponObj.transform.localPosition = Vector3.zero;
			weaponObj.transform.localScale = new Vector3(1, 1, 1);
			weaponObj.transform.localEulerAngles = new Vector3(50, 180, 0);
			plComponents.PlayerAttack.SetPlayerGunLocalPoint(-weaponClass.AttackPoint.localPosition);
            SetWeaponStats();
		}

        /// <summary>
        /// Инициализация инвентаря игрока: его предметов и умений
        /// </summary>
        private void InventoryInitialisation()
        {
            ItemSkillPrefabs itemSkillPrefabs = 
                GameObject.Find("GetPrefabs").GetComponent<ItemSkillPrefabs>();
            GameObject gO;

            #region Инициализация предметов
            if (itemSkillPrefabs.FirstItem != null)
            {
                gO = Instantiate(itemSkillPrefabs.FirstItem);
                ProcedureForPlayerItem(gO);
            }
            if (itemSkillPrefabs.SecondItem != null)
            {
                gO = Instantiate(itemSkillPrefabs.SecondItem);
                ProcedureForPlayerItem(gO);
            }
            if (itemSkillPrefabs.ThirdItem != null)
            {
                gO = Instantiate(itemSkillPrefabs.ThirdItem);
                ProcedureForPlayerItem(gO);
            }
            #endregion

            #region Инициализация умений
            if (itemSkillPrefabs.FirstSkill != null)
            {
                gO = Instantiate(itemSkillPrefabs.FirstSkill);
                ProcedureForPlayerSkill(gO);
            }
            if (itemSkillPrefabs.SecondSkill != null)
            {
                gO = Instantiate(itemSkillPrefabs.SecondSkill);
                ProcedureForPlayerSkill(gO);
            }
            if (itemSkillPrefabs.ThirdSkill != null)
            {
                gO = Instantiate(itemSkillPrefabs.ThirdSkill);
                ProcedureForPlayerSkill(gO);
            }
            #endregion
        }

        /// <summary>
        /// Процедура для инициализации предмета персонажа
        /// </summary>
        /// <param name="gO"></param>
        private void ProcedureForPlayerItem(GameObject gO)
        {
            IItem iItem = gO.GetComponent<IItem>(); // получаем интерфейс
            iItem.PlayerComponentsControlInstance = plComponents; // инициализируем компонент-ссылку
            gO.transform.SetParent(plComponents.PlayerHUDManager.ItemsParent); // даем родителя
            gO.transform.localPosition = Vector3.zero;  // обнуляем позицию
        }

        /// <summary>
        /// Процедура для инициализации умения персонажа
        /// </summary>
        /// <param name="gO"></param>
        private void ProcedureForPlayerSkill(GameObject gO)
        {
            ISkill iSkill = gO.GetComponent<ISkill>(); // получаем интерфейс
            iSkill.PlayerComponentsControlInstance = plComponents; // инициализируем компонент-ссылку
            gO.transform.SetParent(plComponents.PlayerHUDManager.SkillsParent); // даем родителя
            gO.transform.localPosition = Vector3.zero; // обнуляем позицию
        }

        /// <summary>
        /// Инициализация брони
        /// </summary>
        private void ArmoryInitialisation()
        {
			// На месте 0 в GetShield(0) должен стоять уровень щита
			shieldArmoryInformation = Instantiate(GameObject.Find("GetPrefabs").
				GetComponent<ArmorPrefabs>().Shield.GetComponent<PartArmoryInformation>()); 
			cuirassArmoryInformation =  Instantiate(GameObject.Find("GetPrefabs").
                GetComponent<ArmorPrefabs>().Cuirass.GetComponent<PartArmoryInformation>());
            helmetArmoryInformation = Instantiate(GameObject.Find("GetPrefabs").
                GetComponent<ArmorPrefabs>().Helmet.GetComponent<PartArmoryInformation>());

            SendArmoryParametersToPlayer();
        }

        /// <summary>
        /// Отправить игроку значения брони и веса брони
        /// </summary>
        private void SendArmoryParametersToPlayer()
        {
			plComponents.PlayerArmory.HealthValue += shieldArmoryInformation.ArmoryValue;
			plComponents.PlayerArmory.HealthValue += helmetArmoryInformation.ArmoryValue;
			plComponents.PlayerArmory.HealthValue += cuirassArmoryInformation.ArmoryValue;

			// Передаем вес брони в компоненту-броню на персонаже
			plComponents.PlayerArmory.SetArmoryWeight(helmetArmoryInformation.WeightArmory,
				shieldArmoryInformation.WeightArmory,
				cuirassArmoryInformation.WeightArmory);
		}

		private void OtherPlayerInitialisation()
		{
			plComponents.PlayerController.OriginalSpinSpeed =
				plComponents.PlayerWeapon.SpinSpeed;
			plComponents.PlayerController.MaxSpinSpeed =
				plComponents.PlayerWeapon.SpinSpeed;
		}
	}
}
