﻿using UnityEngine;
using VotanLibraries;

namespace AbstractBehaviour
{
    /// <summary>
    /// Общее звуковое хранилищ.
    /// Объекты могут его наследовать 
    /// и реализовывать абстрактные методы.
    /// </summary>
    public abstract class AbstractSoundStorage 
        : MonoBehaviour
    {
        #region Переменные и ссылки
        protected static AudioClip[] audioCutBody; // Звуки лязга колющего оружия
        protected static AudioClip[] audioHitBody; // Звуки лязка дробящего оружия
        protected static AudioClip[] audioHitArrow; // Звук попадания стрелы по объекту
        protected static AudioClip[] audioCollisionMetalThings;
        protected static AudioClip[] audioCollisionWoodenThings;
        protected static AudioClip[] audioIce;
        protected static AudioClip[] audioBodyFall;
        protected static AudioClip[] audioGameMusic;
        protected static AudioClip[] audioDeadMusic;
        protected static AudioClip[] audioWinMusic;
        protected static AudioClip[] audioToBurn;
        protected static AudioClip[] audioBurning;
        protected static AudioClip[] audioEnvironmentLighting;
        protected AudioClip[] audioHitArmory; // Звуки лязка дробящего оружия

        [SerializeField, Tooltip("Звуковой компонент на ногах")]
        protected AudioSource audioSourceLegs;
        [SerializeField, Tooltip("Звуковой компонент на оружии")]
        protected AudioSource audioSourceWeapon;
        [SerializeField, Tooltip("Звуковой компонент на игроке")]
        protected AudioSource audioSourceObject;

        [SerializeField, Tooltip("Громкость шагов")]
        protected float volumeStep;
        [SerializeField, Tooltip("Громкость падения")]
        protected float volumeFall;
        [SerializeField, Tooltip("Громкость страданий")]
        protected float volumeHutred;
        [SerializeField, Tooltip("Громкость смерти")]
        protected float volumeDead;

        protected static object[] tempAudioList;
        #endregion

        #region Инициализация
        /// <summary>
        /// Инициализация первостепенных данных
        /// </summary>
        protected virtual void Start()
        {
            InitialisationCutToBodySounds();
            InitialisationHitToBodySounds();
            InitialisationArrowHitSounds();
        }

        /// <summary>
        /// Инициализация звуков попадания стрелы по объекту
        /// </summary>
        private void InitialisationArrowHitSounds()
        {
            if (audioHitArrow == null)
            {
                tempAudioList = Resources.LoadAll("Sounds/Common/Weapon/ShootingHits");
                audioHitArrow = new AudioClip[tempAudioList.Length];
                for (int i = 0; i < tempAudioList.Length; i++)
                {
                    audioHitArrow[i] = (AudioClip)tempAudioList[i];
                }
            }
        }

        /// <summary>
        /// Статическая инициализация
        /// </summary>
        public static void LoadAllStaticSounds()
        {
            InitialisationDeadMusic();
            InitialisationGameMusic();
            InitialisationWinMusic();
            InitialisationSoundsForMetalThings();
            InitialisationSoundsForWoodenThings();
            InitialisationSoundsIce();
            InitialisationSoundsBodyFall();
            InitialisationSoundsBurnAndBurning();
            InitialisationEnvironmentLighting();
        }

        /// <summary>
        /// Инициализация музыки
        /// </summary>
        private static void InitialisationGameMusic()
        {
            tempAudioList = Resources.LoadAll("Sounds/Music/Gameplay");
            audioGameMusic = new AudioClip[tempAudioList.Length];
            for (int i = 0; i < tempAudioList.Length; i++)
            {
                audioGameMusic[i] = (AudioClip)tempAudioList[i];
            }
        }

        /// <summary>
        /// Инициализация звуков грозы
        /// </summary>
        private static void InitialisationEnvironmentLighting()
        {
            tempAudioList = Resources.LoadAll("Sounds/Environment/Lighting");
            audioEnvironmentLighting = new AudioClip[tempAudioList.Length];
            for (int i = 0; i < tempAudioList.Length; i++)
            {
                audioEnvironmentLighting[i] = (AudioClip)tempAudioList[i];
            }
        }

        /// <summary>
        /// Инициализация музыки проигрыша
        /// </summary>
        private static void InitialisationDeadMusic()
        {
            tempAudioList = Resources.LoadAll("Sounds/Music/Dead");
            audioDeadMusic = new AudioClip[tempAudioList.Length];
            for (int i = 0; i < tempAudioList.Length; i++)
            {
                audioDeadMusic[i] = (AudioClip)tempAudioList[i];
            }
        }

        /// <summary>
        /// Инициализация музыки победы
        /// </summary>
        private static void InitialisationWinMusic()
        {
            tempAudioList = Resources.LoadAll("Sounds/Music/Win");
            audioWinMusic = new AudioClip[tempAudioList.Length];
            for (int i = 0; i < tempAudioList.Length; i++)
            {
                audioWinMusic[i] = (AudioClip)tempAudioList[i];
            }
        }

        /// <summary>
        /// Инициализация деревянных звуков
        /// </summary>
        private static void InitialisationSoundsForWoodenThings()
        {
            tempAudioList = Resources.LoadAll("Sounds/Common/WoodenThings");
            audioCollisionWoodenThings = new AudioClip[tempAudioList.Length];
            for (int i = 0; i < tempAudioList.Length; i++)
            {
                audioCollisionWoodenThings[i] = (AudioClip)tempAudioList[i];
            }
        }

        /// <summary>
        /// Инициализация звуков горения
        /// </summary>
        private static void InitialisationSoundsBurnAndBurning()
        {
            tempAudioList = Resources.LoadAll("Sounds/Effects/Fire/ToBurn");
            audioToBurn = new AudioClip[tempAudioList.Length];
            for (int i = 0; i < tempAudioList.Length; i++)
            {
                audioToBurn[i] = (AudioClip)tempAudioList[i];
            }

            tempAudioList = Resources.LoadAll("Sounds/Effects/Fire/Burning");
            audioBurning = new AudioClip[tempAudioList.Length];
            for (int i = 0; i < tempAudioList.Length; i++)
            {
                audioBurning[i] = (AudioClip)tempAudioList[i];
            }
        }

        /// <summary>
        /// Инициализация звуков падающего тела
        /// </summary>
        private static void InitialisationSoundsBodyFall()
        {
            tempAudioList = Resources.LoadAll("Sounds/Common/BodyFall");
            audioBodyFall = new AudioClip[tempAudioList.Length];
            for (int i = 0; i < tempAudioList.Length; i++)
            {
                audioBodyFall[i] = (AudioClip)tempAudioList[i];
            }
        }

        /// <summary>
        /// Инициализация звуков металлических деталей
        /// </summary>
        private static void InitialisationSoundsForMetalThings()
        {
            tempAudioList = Resources.LoadAll("Sounds/Common/MetalThings");
            audioCollisionMetalThings = new AudioClip[tempAudioList.Length];
            for (int i = 0; i < tempAudioList.Length; i++)
            {
                audioCollisionMetalThings[i] = (AudioClip)tempAudioList[i];
            }
        }

        /// <summary>
        /// Инициализация звукову льда
        /// </summary>
        private static void InitialisationSoundsIce()
        {
            tempAudioList = Resources.LoadAll("Sounds/Effects/Ice");
            audioIce = new AudioClip[tempAudioList.Length];
            for (int i = 0; i < tempAudioList.Length; i++)
            {
                audioIce[i] = (AudioClip)tempAudioList[i];
            }
        }

        /// <summary>
        /// Инициализация лязга оружия режущего типа по мясу
        /// </summary>
        private void InitialisationCutToBodySounds()
        {
            if (audioCutBody == null)
            {
                tempAudioList = Resources.LoadAll("Sounds/Common/Weapon/CuttingMeat");
                audioCutBody = new AudioClip[tempAudioList.Length];
                for (int i = 0; i < tempAudioList.Length; i++)
                {
                    audioCutBody[i] = (AudioClip)tempAudioList[i];
                }
            }
        }

        /// <summary>
        /// Инициализация лязга оружия дробящего типа
        /// </summary>
        private void InitialisationHitToBodySounds()
        {
            if (audioHitBody == null)
            {
                tempAudioList = Resources.LoadAll("Sounds/Common/Weapon/Crushing");
                audioHitBody = new AudioClip[tempAudioList.Length];
                for (int i = 0; i < tempAudioList.Length; i++)
                {
                    audioHitBody[i] = (AudioClip)tempAudioList[i];
                }
            }
        }

        /// <summary>
        /// Инициализация звуков шагов
        /// </summary>
        protected abstract void InitialisationStepsSounds();

        /// <summary>
        ///  Инициализация звуков получения урона
        /// </summary>
        protected abstract void InitialisationHurtSounds();

        /// <summary>
        /// Инициализация звуков смерти
        /// </summary>
        protected abstract void InitialisationDeadSounds();
        #endregion

        #region Открытые методы
        /// <summary>
        /// Звук шагов
        /// </summary>
        public abstract void PlayStepAudio();

        /// <summary>
        /// Работа со звуком ледяного эффекта
        /// </summary>
        /// <param name="auSo"></param>
        public static void PlayIceAudio(bool isStart, float volume, AudioSource auSo)
        {
            auSo.volume =
                LibraryStaticFunctions.GetRangeValue(volume, 0.1f);
            if (isStart)
            {
                auSo.clip = audioIce[0];
            }
            else
            {
                auSo.clip =
                    audioIce[Random.Range(1, audioIce.Length)];
            }
            auSo.pitch = LibraryStaticFunctions.GetRangeValue(1, 0.1f);
            auSo.Play();
        }

        /// <summary>
        /// Работа с воспроизведением звука у металлической вещи
        /// </summary>
        /// <param name="auSo"></param>
        public static void PlayMetalAudio(AudioSource auSo)
        {
            auSo.clip =
                audioCollisionMetalThings[Random.Range
                (0, audioCollisionMetalThings.Length)];
            auSo.pitch = LibraryStaticFunctions.GetRangeValue(1, 0.1f);
            auSo.Play();
        }

        /// <summary>
        /// Работа с воспроизведением звука у деревянной вещи
        /// </summary>
        /// <param name="auSo"></param>
        public static void PlayWoodenAudio(AudioSource auSo)
        {
            auSo.clip =
                audioCollisionWoodenThings[Random.Range
                (0, audioCollisionWoodenThings.Length)];
            auSo.pitch = LibraryStaticFunctions.GetRangeValue(1, 0.1f);
            auSo.Play();
        }

        /// <summary>
        /// Проиграть звук молнии
        /// </summary>
        /// <param name="auSo"></param>
        public static void PlayLightingAudio(AudioSource auSo)
        {
            auSo.clip =
                audioEnvironmentLighting[Random.Range
                (0, audioEnvironmentLighting.Length)];
            auSo.pitch = LibraryStaticFunctions.GetRangeValue(1, 0.1f);
            auSo.Play();
        }

        /// <summary>
        /// Работа со звуком воспламенения
        /// </summary>
        /// <param name="auSo"></param>
        public static void PlayBurnAudio(AudioSource auSo)
        {
            auSo.clip =
                audioToBurn[Random.Range
                (0, audioToBurn.Length)];
            auSo.pitch = LibraryStaticFunctions.GetRangeValue(1, 0.1f);
            auSo.Play();
        }

        /// <summary>
        /// Работа со звуком длительного горения
        /// </summary>
        /// <param name="auSo"></param>
        public static void PlayBurningAudio(AudioSource auSo)
        {
            auSo.clip =
                audioBurning[Random.Range
                (0, audioBurning.Length)];
            auSo.pitch = LibraryStaticFunctions.GetRangeValue(1, 0.1f);
            auSo.Play();
        }

        /// <summary>
        /// Музыка игрового процесса
        /// </summary>
        /// <param name="auSo"></param>
        public static void GameplayMusic(AudioSource auSo)
        {
            auSo.clip =
                audioGameMusic[Random.Range
                (0, audioGameMusic.Length)];
            auSo.Play();
        }

        /// <summary>
        /// Музыка окончания игры
        /// </summary>
        /// <param name="auSo"></param>
        public static void GameOverMusic(AudioSource auSo)
        {
            auSo.clip =
                audioDeadMusic[Random.Range
                (0, audioDeadMusic.Length)];
            auSo.Play();
        }

        /// <summary>
        /// Звук проигрыша
        /// </summary>
        /// <param name="auSo"></param>
        public static void WinOverMusic(AudioSource auSo)
        {
            auSo.clip =
                audioWinMusic[Random.Range
                (0, audioWinMusic.Length)];
            auSo.Play();
            Debug.Log("PLAYING: " + auSo.clip);
        }

        /// <summary>
        /// Работа со звуком падения тела
        /// </summary>
        /// <param name="audioSource"></param>
        public void PlayBodyFallAudio(AudioSource audioSource)
        {
            audioSource.clip =
               audioBodyFall[Random.Range
               (0, audioBodyFall.Length)];
            audioSource.pitch = LibraryStaticFunctions.GetRangeValue(1, 0.1f);
            audioSource.volume = LibraryStaticFunctions.GetRangeValue(volumeFall, 0.1f);
            audioSource.Play();
        }

        /// <summary>
        /// Проиграть звук попадания стрелы по объекту
        /// </summary>
        /// <param name="auSo"></param>
        public static void PlayArrowHitAudio(AudioSource auSo)
        {
            auSo.clip =
               audioHitArrow[Random.Range(0, audioHitArrow.Length)];
            auSo.pitch = LibraryStaticFunctions.GetRangeValue(1, 0.1f);
            auSo.Play();
        }

        /// <summary>
        /// Звук смерти
        /// </summary>
        public abstract void PlayDeadAudio();

        /// <summary>
        /// Уронить объект
        /// </summary>
        public abstract void FallObject();

        /// <summary>
        /// Звук получения урона
        /// </summary>
        /// <param name="isArmory"></param>
        public abstract void PlayGetDamageAudio(bool isArmory=false);

        /// <summary>
        /// Звук удара оружия
        /// </summary>
        /// <param name="value"></param>
        public abstract void PlayWeaponHitAudio(int value);

        /// <summary>
        /// Звук кручения оружия
        /// </summary>
        /// <param name="speed"></param>
        public abstract void PlaySpinAudio(float speed);

        /// <summary>
        /// Звук кручения оружия. Перегрузка
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="value"></param>
        public abstract void PlaySpinAudio(float speed, bool value=false);
        #endregion
    }
}
