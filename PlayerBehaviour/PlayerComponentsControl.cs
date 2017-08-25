using GameBehaviour;
using UnityEngine;
using VotanGameplay;
using VotanInterfaces;

namespace PlayerBehaviour
{
    /// <summary>
    /// Хранитель компонентов.
    /// Содержит в себе ссылки и доступы на все компоненты игрока
    /// </summary>
    public class PlayerComponentsControl
        : MonoBehaviour, IPlayerBehaviour
    {
        #region Ссылки на компоненты
        [SerializeField, Tooltip("Управление игроком")]
        private PlayerController playerController;
        [SerializeField, Tooltip("Состояние игрока")]
        private PlayerConditions playerConditions;
        [SerializeField, Tooltip("Возможность ведения боя")]
        private PlayerFight playerFight;
        [SerializeField, Tooltip("Логика атаки")]
        private PlayerAttack playerAttack;
        [SerializeField, Tooltip("Физика игрока")]
        private PlayerCollision playerCollision;
        [SerializeField, Tooltip("Камера игрока")]
        private GameObject playerCamera;
        [SerializeField, Tooltip("Броня игрока")]
        private PlayerArmory playerArmory;
        [SerializeField, Tooltip("Оружие игрока")]
        private PlayerWeapon playerWeapon;
        [SerializeField, Tooltip("Плавный поворот камеры")]
        private PlayerCameraSmooth playerCameraSmooth;
        [SerializeField, Tooltip("Менеджер ресурсов игрока")]
        private PlayerResources playerResources;
        [SerializeField, Tooltip("Аниматор игрока")]
        private PlayerAnimationsController playerAnimationsController;
        [SerializeField, Tooltip("Звуковой компонент игрока")]
        private PlayerSounder playerSounder;
        [SerializeField, Tooltip("Эффект грозы у игрока")]
        private PlayerVisualEffects playerVisualEffects;
        [SerializeField, Tooltip("Ротатор интерфейса игрока")]
        private DownInterfaceRotater downInterfaceRotater;
        [SerializeField, Tooltip("Родитель игрока")]
        private Transform playerParent;
        [SerializeField, Tooltip("Объект игрока")]
        private Transform playerObject;
        [SerializeField, Tooltip("Моделька игрока")]
        private Transform playerModel;
        [SerializeField, Tooltip("Интерфейс монитора игрока")]
        private PlayerUI playerUI;
        [SerializeField, Tooltip("Обработчик звуковых событий в HUD")]
        private PlayerHUDAudioStorage playerHUDAudioStorage;
        [SerializeField, Tooltip("HUD компонент игрока")]
        private PlayerHUDManager playerHUDManager;
        #endregion

        #region Доступы к компонентам
        public PlayerCameraSmooth PlayerCameraSmooth
        {
            get
            {
                return playerCameraSmooth;
            }

            set
            {
                playerCameraSmooth = value;
            }
        }

        public PlayerArmory PlayerArmory
        {
            get
            {
                return playerArmory;
            }

            set
            {
                playerArmory = value;
            }
        }

        public PlayerAttack PlayerAttack
        {
            get
            {
                return playerAttack;
            }

            set
            {
                playerAttack = value;
            }
        }

        public PlayerFight PlayerFight
        {
            get
            {
                return playerFight;
            }

            set
            {
                playerFight = value;
            }
        }

        public PlayerConditions PlayerConditions
        {
            get
            {
                return playerConditions;
            }

            set
            {
                playerConditions = value;
            }
        }

        public PlayerController PlayerController
        {
            get
            {
                return playerController;
            }

            set
            {
                playerController = value;
            }
        }

        public PlayerAnimationsController PlayerAnimationsController
        {
            get
            {
                return playerAnimationsController;
            }

            set
            {
                playerAnimationsController = value;
            }
        }

        public Transform PlayerParent
        {
            get
            {
                return playerParent;
            }

            set
            {
                playerParent = value;
            }
        }

        public Transform PlayerObject
        {
            get
            {
                return playerObject;
            }

            set
            {
                playerObject = value;
            }
        }

        public Transform PlayerModel
        {
            get
            {
                return playerModel;
            }

            set
            {
                playerModel = value;
            }
        }

        public PlayerWeapon PlayerWeapon
        {
            get
            {
                return playerWeapon;
            }

            set
            {
                playerWeapon = value;
            }
        }

        public GameObject PlayerCamera
        {
            get
            {
                return playerCamera;
            }

            set
            {
                playerCamera = value;
            }
        }

        public PlayerCollision PlayerCollision
        {
            get
            {
                return playerCollision;
            }

            set
            {
                playerCollision = value;
            }
        }

        public PlayerSounder PlayerSounder
        {
            get
            {
                return playerSounder;
            }

            set
            {
                playerSounder = value;
            }
        }

        public PlayerUI PlayerUI
        {
            get
            {
                return playerUI;
            }

            set
            {
                playerUI = value;
            }
        }

        public PlayerVisualEffects PlayerVisualEffects
        {
            get
            {
                return playerVisualEffects;
            }

            set
            {
                playerVisualEffects = value;
            }
        }

        public DownInterfaceRotater DownInterfaceRotater
        {
            get
            {
                return downInterfaceRotater;
            }

            set
            {
                downInterfaceRotater = value;
            }
        }

        public PlayerHUDAudioStorage PlayerHUDAudioStorage
        {
            get
            {
                return playerHUDAudioStorage;
            }

            set
            {
                playerHUDAudioStorage = value;
            }
        }

        public PlayerHUDManager PlayerHUDManager
        {
            get
            {
                return playerHUDManager;
            }

            set
            {
                playerHUDManager = value;
            }
        }

        public PlayerResources PlayerResources
        {
            get
            {
                return playerResources;
            }

            set
            {
                playerResources = value;
            }
        }
        #endregion

        /// <summary>
        /// Инициализация. Добавляем игрока в лист игроков.
        /// </summary>
        private void Start()
        {
            AllPlayerManager.AddPlayerToPlayerList(gameObject);
            DownInterfaceRotater.RestartDownInterfaceRotater();
        }
    }
}
