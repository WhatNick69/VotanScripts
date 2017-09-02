using AbstractBehaviour;
using UnityEngine;
using VotanLibraries;

namespace EnemyBehaviour
{
    /// <summary>
    /// Звуковой менеджер для арбалетчика
    /// </summary>
    public class CrossbowmanSounder
        : AbstractSoundStorage
    {
        #region Переменные, ссылки и массивы
        protected static AudioClip[] audioHurt; // Звуки повреждений по телу
        protected static AudioClip[] audioDead; // Звуки смерти персонажа
        protected static AudioClip[] audioSteps; // Звуки шагов
        protected static AudioClip[] audioShooting; // Звуки стрельбы
        protected static AudioClip[] audioReloading;
        protected bool isMayToPlayWeaponAudio;
        #endregion

        #region Инициализация
        /// <summary>
        /// Инициализация
        /// </summary>
        protected override void Start()
        {
            base.Start();
            isMayToPlayWeaponAudio = true;
            InitialisationStepsSounds();
            InitialisationHurtSounds();
            InitialisationDeadSounds();

            InitialisationShootingSounds();
            InitialisationReloadingSounds();
        }

        /// <summary>
        /// Инициализация звуков стрельбы из арбалета
        /// </summary>
        private void InitialisationShootingSounds()
        {
            if (audioShooting == null)
            {
                tempAudioList = Resources.LoadAll("Sounds/Common/Weapon/CrossbowShoot");
                audioShooting = new AudioClip[tempAudioList.Length];
                for (int i = 0; i < tempAudioList.Length; i++)
                {
                    audioShooting[i] = (AudioClip)tempAudioList[i];
                }
            }
        }

        /// <summary>
        /// Инициализация звуков перезарядки арбалета
        /// </summary>
        private void InitialisationReloadingSounds()
        {
            if (audioReloading == null)
            {
                tempAudioList = Resources.LoadAll("Sounds/Common/Weapon/CrossbowReloading");
                audioReloading = new AudioClip[tempAudioList.Length];
                for (int i = 0; i < tempAudioList.Length; i++)
                {
                    audioReloading[i] = (AudioClip)tempAudioList[i];
                }
            }
        }

        /// <summary>
        /// Инициализация звуков смерти
        /// </summary>
        protected override void InitialisationDeadSounds()
        {
            if (audioDead == null)
            {
                tempAudioList = Resources.LoadAll("Sounds/Mobs/LiveEnemies/Dead");
                audioDead = new AudioClip[tempAudioList.Length];
                for (int i = 0; i < tempAudioList.Length; i++)
                {
                    audioDead[i] = (AudioClip)tempAudioList[i];
                }
            }
        }

        /// <summary>
        /// Инициализация звуков боли
        /// </summary>
        protected override void InitialisationHurtSounds()
        {
            if (audioHurt == null)
            {
                tempAudioList = Resources.LoadAll("Sounds/Mobs/LiveEnemies/GetDamage");
                audioHurt = new AudioClip[tempAudioList.Length];
                for (int i = 0; i < tempAudioList.Length; i++)
                {
                    audioHurt[i] = (AudioClip)tempAudioList[i];
                }
            }
        }

        /// <summary>
        /// Инициализация звуков ходьбы
        /// </summary>
        protected override void InitialisationStepsSounds()
        {
            if (audioSteps == null)
            {
                tempAudioList = Resources.LoadAll("Sounds/PlayerMale/Steps");
                audioSteps = new AudioClip[tempAudioList.Length];
                for (int i = 0; i < tempAudioList.Length; i++)
                {
                    audioSteps[i] = (AudioClip)tempAudioList[i];
                }
            }
        }
        #endregion

        /// <summary>
        /// Проиграть звук падения
        /// </summary>
        public override void FallObject()
        {
            PlayBodyFallAudio(audioSourceLegs);
        }

        /// <summary>
        /// Проиграть звук смерти
        /// </summary>
        public override void PlayDeadAudio()
        {
            audioSourceObject.volume =
              LibraryStaticFunctions.GetRangeValue(volumeDead, 0.1f);
            audioSourceObject.clip =
               audioDead[Random.Range(0, audioDead.Length)];
            audioSourceObject.pitch = 1 +
                Random.Range(0, 1f) / 2;
            audioSourceObject.Play();
        }

        /// <summary>
        /// Проиграть звук получения урона
        /// </summary>
        /// <param name="isArmory"></param>
        public override void PlayGetDamageAudio(bool isArmory = false)
        {
            audioSourceObject.volume =
               LibraryStaticFunctions.GetRangeValue(volumeHutred, 0.1f);
            audioSourceObject.clip =
                 audioHurt[Random.Range(0, audioHurt.Length)];
            audioSourceObject.pitch = LibraryStaticFunctions.GetRangeValue(1, 0.1f);
            audioSourceObject.Play();
        }

        /// <summary>
        /// Проиграть звук стрельбы из арбалета
        /// </summary>
        /// <param name="speed"></param>
        public override void PlaySpinAudio(float speed)
        {
            audioSourceWeapon.clip =
                audioShooting[Random.Range(0, audioShooting.Length)];
            audioSourceWeapon.pitch =
                LibraryStaticFunctions.GetRangeValue(1, 0.1f);
            audioSourceWeapon.Play();
        }

        /// <summary>
        /// Проиграть звук ходьбы
        /// </summary>
        public override void PlayStepAudio()
        {
            audioSourceLegs.clip =
                audioSteps[Random.Range(0, audioSteps.Length)];
            audioSourceLegs.volume = volumeStep;
            audioSourceLegs.pitch = LibraryStaticFunctions.GetRangeValue(1, 0.1f);
            audioSourceLegs.Play();
        }

        /// <summary>
        /// Проиграть звук перезарядки арбалета
        /// </summary>
        public void PlayReloadAudio()
        {
            audioSourceWeapon.clip =
                audioReloading[Random.Range(0, audioReloading.Length - 1)];
            audioSourceWeapon.pitch =
                LibraryStaticFunctions.GetRangeValue(1, 0.1f);
            audioSourceWeapon.Play();
        }

        /// <summary>
        /// Пустая реализация
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="value"></param>
        public override void PlaySpinAudio(float speed, bool value = false)
        {
            // пустая реализация
        }

        public override void PlayWeaponHitAudio(int value)
        {
            // пустая реализация
        }
    }
}
