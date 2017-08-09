using UnityEngine;

namespace PlayerBehaviour
{
    /// <summary>
    /// Хранит звуки HUD для игркоа
    /// </summary>
    class PlayerHUDAudioStorage
        : MonoBehaviour
    {
        #region Переменные
        [SerializeField,Tooltip("Звуковой компонент для интерфейса")]
        private AudioSource audioSource;

        private static AudioClip[] audioSwipeInventory;
        private static AudioClip[] audioButtons;
        private static AudioClip[] audioItemsPick;
        private static AudioClip[] audioSkillsPick;
        private static object[] tempAudioList;
        #endregion

        #region Инициализация
        /// <summary>
        /// Общая инициализация
        /// </summary>
        private void Start()
        {
            InitialisationSwipeAudio();
            InitialisationButtonAudio();
            InitialisationItemsAudio();
            InitialisationSkillsAudio();
        }

        /// <summary>
        /// Инициализация звуков вызова предметов
        /// </summary>
        private void InitialisationItemsAudio()
        {
            tempAudioList = Resources.LoadAll("Sounds/HUD/Items");
            audioItemsPick = new AudioClip[tempAudioList.Length];
            for (int i = 0; i < tempAudioList.Length; i++)
            {
                audioItemsPick[i] = (AudioClip)tempAudioList[i];
            }
        }

        /// <summary>
        /// Инициализация звуков вызова умений
        /// </summary>
        private void InitialisationSkillsAudio()
        {
            tempAudioList = Resources.LoadAll("Sounds/HUD/Skills");
            audioSkillsPick = new AudioClip[tempAudioList.Length];
            for (int i = 0; i < tempAudioList.Length; i++)
            {
                audioSkillsPick[i] = (AudioClip)tempAudioList[i];
            }
        }

        /// <summary>
        /// Инициализация звуков нажатия на кнопку
        /// </summary>
        private void InitialisationButtonAudio()
        {
            tempAudioList = Resources.LoadAll("Sounds/HUD/Buttons");
            audioButtons = new AudioClip[tempAudioList.Length];
            for (int i = 0; i < tempAudioList.Length; i++)
            {
                audioButtons[i] = (AudioClip)tempAudioList[i];
            }
        }

        /// <summary>
        /// Инициализация звуков свайпа
        /// </summary>
        private void InitialisationSwipeAudio()
        {
            tempAudioList = Resources.LoadAll("Sounds/HUD/Inventory");
            audioSwipeInventory = new AudioClip[tempAudioList.Length];
            for (int i = 0; i < tempAudioList.Length; i++)
            {
                audioSwipeInventory[i] = (AudioClip)tempAudioList[i];
            }
        }
        #endregion

        /// <summary>
        /// Проиграть звук инвентаря
        /// </summary>
        /// <param name="isOpen"></param>
        public void PlaySoundSwipeInventory(bool isOpen)
        {
            if (isOpen)
                audioSource.clip = audioSwipeInventory[0];
            else
                audioSource.clip = audioSwipeInventory[1];

            audioSource.Play();
        }

        /// <summary>
        /// Проиграть звук по нажатию на кнопку
        /// </summary>
        public void PlaySoundButtonClick()
        {
            audioSource.clip = 
                audioButtons[UnityEngine.Random.Range(0, audioButtons.Length)];
            audioSource.Play();
        }

        /// <summary>
        /// Проиграть звук предмета
        /// </summary>
        public void PlaySoundItemClick()
        {
            audioSource.clip =
                audioItemsPick[UnityEngine.Random.Range(0, audioItemsPick.Length)];
            audioSource.Play();
        }

        /// <summary>
        /// Проиграть звук умения
        /// </summary>
        public void PlaySoundSkillClick()
        {
            audioSource.clip =
                audioSkillsPick[UnityEngine.Random.Range(0, audioSkillsPick.Length)];
            audioSource.Play();
        }
    }
}
