using PlayerBehaviour;
using UnityEngine;
using VotanLibraries;

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
        private PartArmoryInformation shieldArmoryInformation;
        private PartArmoryInformation cuirassArmoryInformation;
        private PartArmoryInformation helmetArmoryInformation;
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
                    headPosition = new Vector3(-62, 0, 0);
                    attackPoint = new Vector3(-70, 0, 0);
                    break;
                case LenghtGrip.Middle:
                    gripPoint = midleGripPosition;
                    headPosition = new Vector3(-67, 0, 0);
                    attackPoint = new Vector3(-72, 0, 0);
                    break;
                case LenghtGrip.Long:
                    gripPoint = longGripPosition;
                    headPosition = new Vector3(-72, 0, 0);
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
            // получаем вращения оружием
            weaponSpinSpeed =
                LibraryStaticFunctions.TotalSpinSpeed
                (gripClass.BonusSpinSpeedFromGrip, weaponWeight);
            // получаем урон оружием
            weaponDamage =
                LibraryStaticFunctions.TotalDamage(headClass.DamageBase, weaponWeight);

            // защита от ручки
            weaponDefence = gripClass.GripDefence;

            weaponDamType = gemClass.DamageTypeGem;

            plComponents.PlayerWeapon.SetWeaponParameters(weaponDamage, weaponDefence,
                weaponDamType, headClass.TrailRenderer, weaponSpinSpeed, weaponWeight, gemClass.GemPower);
        }

        /// <summary>
        /// Общая инициализация
        /// </summary>
        private void Awake()
        {
            plComponents = GetComponent<PlayerComponentsControl>();

            ArmoryInitialisation();
            WeaponInitialisation();
        }

        /// <summary>
        /// Инициализация оружия
        /// </summary>
        private void WeaponInitialisation()
        {
            grip = Instantiate(GameObject.Find("GetPrefabs").GetComponent<WeaponPrefabs>().Grip);
            head = Instantiate(GameObject.Find("GetPrefabs").GetComponent<WeaponPrefabs>().Head);
            gem = Instantiate(GameObject.Find("GetPrefabs").GetComponent<WeaponPrefabs>().Gem);

            gripClass = grip.GetComponent<Grip>();
            headClass = head.GetComponent<Head>();
            gemClass = gem.GetComponent<Gem>();

            WeaponSettings();

            grip.transform.parent = weapon.transform;
            grip.transform.localPosition = gripPoint;
            grip.transform.localEulerAngles = midleGripRotate;
            grip.transform.localScale = new Vector3(1, 1, 1);

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

        /// <summary>
        /// Инициализация брони
        /// </summary>
        private void ArmoryInitialisation()
        {
            shieldArmoryInformation =  Instantiate(GameObject.Find("GetPrefabs").
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

            weaponWeight = LibraryStaticFunctions.TotalWeight(shieldArmoryInformation.WeightArmory
                , helmetArmoryInformation.WeightArmory
                , cuirassArmoryInformation.WeightArmory);
        }
    }
}
