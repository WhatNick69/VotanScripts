using AbstractBehaviour;
using UnityEngine;
using VotanLibraries;

namespace EnemyBehaviour
{
    /// <summary>
    /// Звуковой менеджер для рыцаря
    /// </summary>
    public class KnightSounder 
        : AbstractSoundStorage
    {
        #region Переменные, ссылки и массивы
        protected static AudioClip[] audioHurt; // Звуки повреждений по телу
        protected static AudioClip[] audioDead; // Звуки смерти персонажа
        protected static AudioClip[] audioSteps; // Звуки шагов
        protected static AudioClip[] audioSpin; // Звуки вращения оружия
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
            InitialisationSpinSounds();
            InitialisationHurtSounds();
            InitialisationDeadSounds();
            InitialisationHitToArmorySounds();
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
        /// Инициализация звуков получения урона
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
        /// Инициализация звуков шагов
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

        /// <summary>
        /// Инициализация звуков удара по броне
        /// </summary>
        private void InitialisationHitToArmorySounds()
        {
            if (audioHitArmory == null)
            {
                tempAudioList = Resources.LoadAll("Sounds/Common/Weapon/Cutting");
                audioHitArmory = new AudioClip[tempAudioList.Length];
                for (int i = 0; i < tempAudioList.Length; i++)
                {
                    audioHitArmory[i] = (AudioClip)tempAudioList[i];
                }
            }
        }

        /// <summary>
        /// Инициализация звуков взмаха оружием
        /// </summary>
        private void InitialisationSpinSounds()
        {
            if (audioSpin == null)
            {
                tempAudioList = Resources.LoadAll("Sounds/Mobs/Common/Swipe");
                audioSpin = new AudioClip[tempAudioList.Length];
                for (int i = 0; i < tempAudioList.Length; i++)
                {
                    audioSpin[i] = (AudioClip)tempAudioList[i];
                }
            }
        }
        #endregion

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
        /// Проиграть звук смерти
        /// </summary>
        public override void PlayDeadAudio()
        {
            audioSourceObject.volume =
                LibraryStaticFunctions.GetRangeValue(volumeDead, 0.1f);
            audioSourceObject.clip =
               audioDead[Random.Range(0, audioDead.Length)];
            audioSourceObject.pitch = 1 +
                Random.Range(0,1f) / 2 ;
            audioSourceObject.Play();
        }

        /// <summary>
        /// Звук вращения клинком
        /// </summary>
        public override void PlaySpinAudio(float value)
        {
            audioSourceWeapon.clip =
                audioSpin[Random.Range(0, audioSpin.Length - 1)];
            audioSourceWeapon.pitch =
                LibraryStaticFunctions.GetRangeValue(0.5f, 0.1f);
            audioSourceWeapon.Play();
        }

        /// <summary>
        /// Проиграть звук удара
        /// </summary>
        /// <param name="value"></param>
        public override void PlayWeaponHitAudio(int value)
        {
            if (value == 2)
            {
                audioSourceWeapon.clip =
                    audioHitArmory[Random.Range(0, audioHitArmory.Length - 1)];
                audioSourceWeapon.pitch =
                    LibraryStaticFunctions.GetRangeValue(1, 0.1f);
                audioSourceWeapon.Play();
            }
            else if (value == 1)
            {
                audioSourceWeapon.clip =
                    audioCutBody[Random.Range(0, audioCutBody.Length - 1)];
                audioSourceWeapon.pitch =
                    LibraryStaticFunctions.GetRangeValue(1, 0.1f);
                audioSourceWeapon.Play();
            }
        }

        /// <summary>
        /// Проиграть звук падения тела на землю полсе смерти
        /// </summary>
        public override void FallObject()
        {
            PlayBodyFallAudio(audioSourceLegs);
        }

        /// <summary>
        /// Проиграть звук шагов
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
        /// Пустая реализация звука вращения оружием
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="value"></param>
        public override void PlaySpinAudio(float speed, bool value = false)
        {
            // Пустая реализация
        }
    }
}
