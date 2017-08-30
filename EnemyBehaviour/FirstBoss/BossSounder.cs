
using AbstractBehaviour;
using UnityEngine;
using VotanLibraries;

namespace EnemyBehaviour
{
    /// <summary>
    /// Звуковой контроллер для босса
    /// </summary>
    public class BossSounder 
        : AbstractSoundStorage
    {
        #region Переменные
        protected static AudioClip[] audioHurt; // Звуки повреждений по телу
        protected static AudioClip[] audioDead; // Звуки смерти персонажа
        protected static AudioClip[] audioSteps; // Звуки шагов
        protected static AudioClip[] audioSpin; // Звуки вращения оружия
        protected static AudioClip[] audioRoarAttack; // Звуки вращения оружия
        protected static AudioClip[] audioBreath;
        protected static AudioClip[] audioIntroBoss;
        protected bool isMayToPlayWeaponAudio;
        #endregion

        #region Инициализация
        /// <summary>
        /// Общая инициализация
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
            InitialisationRoarAttackSounds();
            InitialisationBreathAudio();
            InitialisationIntroAudio();
        }

        /// <summary>
        /// Инициализация звуков интро
        /// </summary>
        private void InitialisationIntroAudio()
        {
            if (audioIntroBoss == null)
            {
                tempAudioList = Resources.LoadAll("Sounds/Mobs/FirstBoss/Intro");
                audioIntroBoss = new AudioClip[tempAudioList.Length];
                for (int i = 0; i < tempAudioList.Length; i++)
                {
                    audioIntroBoss[i] = (AudioClip)tempAudioList[i];
                }
            }
        }

        /// <summary>
        /// Инициализация звуков рычания
        /// </summary>
        private void InitialisationBreathAudio()
        {
            if (audioBreath == null)
            {
                tempAudioList = Resources.LoadAll("Sounds/Mobs/FirstBoss/Breath");
                audioBreath = new AudioClip[tempAudioList.Length];
                for (int i = 0; i < tempAudioList.Length; i++)
                {
                    audioBreath[i] = (AudioClip)tempAudioList[i];
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

        /// <summary>
        /// Инициализация звуков рычания
        /// </summary>
        protected void InitialisationRoarAttackSounds()
        {
            if (audioRoarAttack == null)
            {
                tempAudioList = Resources.LoadAll("Sounds/Mobs/FirstBoss/Attack");
                audioRoarAttack = new AudioClip[tempAudioList.Length];
                for (int i = 0; i < tempAudioList.Length; i++)
                {
                    audioRoarAttack[i] = (AudioClip)tempAudioList[i];
                }
            }
        }

        /// <summary>
        /// Взмах оружия
        /// </summary>
        private void InitialisationSpinSounds()
        {
            if (audioSpin == null)
            {
                tempAudioList = Resources.LoadAll("Sounds/PlayerMale/Attack");
                audioSpin = new AudioClip[tempAudioList.Length];
                for (int i = 0; i < tempAudioList.Length; i++)
                {
                    audioSpin[i] = (AudioClip)tempAudioList[i];
                }
            }
        }

        /// <summary>
        /// Удар по броне
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
        /// Инициализация звуков смерти
        /// </summary>
        protected override void InitialisationHurtSounds()
        {
            if (audioHurt == null)
            {
                tempAudioList = Resources.LoadAll("Sounds/Mobs/FirstBoss/GetDamage");
                audioHurt = new AudioClip[tempAudioList.Length];
                for (int i = 0; i < tempAudioList.Length; i++)
                {
                    audioHurt[i] = (AudioClip)tempAudioList[i];
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
                tempAudioList = Resources.LoadAll("Sounds/Mobs/FirstBoss/Dead");
                audioDead = new AudioClip[tempAudioList.Length];
                for (int i = 0; i < tempAudioList.Length; i++)
                {
                    audioDead[i] = (AudioClip)tempAudioList[i];
                }
            }
        }
        #endregion

        /// <summary>
        /// Проиграть звук ходьбы
        /// </summary>
        public override void PlayStepAudio()
        {
            audioSourceLegs.clip =
                audioSteps[Random.Range(0, audioSteps.Length)];
            audioSourceLegs.pitch = LibraryStaticFunctions.GetRangeValue(1, 0.1f);
            audioSourceLegs.volume = volumeStep;
            audioSourceLegs.Play();
        }

        /// <summary>
        /// Проиграть звук смерти врага
        /// </summary>
        public override void PlayDeadAudio()
        {
            audioSourceObject.volume =
                LibraryStaticFunctions.GetRangeValue(volumeDead, 0.1f);
            audioSourceObject.clip =
               audioDead[Random.Range(0, audioDead.Length)];
            audioSourceObject.pitch = 0.5f +
                Random.Range(0,1f) / 4;
            audioSourceObject.loop = false;
            audioSourceObject.Play();
        }

        /// <summary>
        /// Проиграть звук рычания
        /// </summary>
        public void PlayRoarAttackAudio()
        {
            audioSourceObject.volume =
                LibraryStaticFunctions.GetRangeValue(volumeHutred, 0.1f);
            audioSourceObject.clip =
               audioRoarAttack[Random.Range(0, audioRoarAttack.Length)];
            audioSourceObject.pitch = 1 +
                Random.Range(0,1f) / 2;
            audioSourceObject.Play();
        }

        /// <summary>
        /// Проиграть звук падения
        /// </summary>
        public override void FallObject()
        {
            PlayBodyFallAudio(audioSourceLegs);
        }

        /// <summary>
        /// Проиграть звук получения урона
        /// </summary>
        /// <param name="isArmory"></param>
        public override void PlayGetDamageAudio(bool isArmory = false)
        {
            audioSourceObject.volume =
               LibraryStaticFunctions.GetRangeValue(volumeHutred, 0.1f);
            if (isArmory)
            {
                audioSourceObject.clip =
                   audioHitArmory[Random.Range(0, audioHitArmory.Length)];
                audioSourceObject.pitch = LibraryStaticFunctions.GetRangeValue(1, 0.1f);
                audioSourceObject.Play();
            }
            else
            {
                audioSourceObject.clip =
                     audioHurt[Random.Range(0, audioHurt.Length)];
                audioSourceObject.pitch = LibraryStaticFunctions.GetRangeValue(1, 0.1f);
                audioSourceObject.Play();
            }
        }

        /// <summary>
        /// Проиграть звук удара оружием
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
        /// Проиграть звук дыхания
        /// </summary>
        public void PlayBreathAudio()
        {
            audioSourceObject.clip =
                audioBreath[Random.Range(0, audioBreath.Length)];
            audioSourceObject.pitch = LibraryStaticFunctions.GetRangeValue(1, 0.1f);
            audioSourceObject.loop = true;
            audioSourceObject.Play();
        }

        /// <summary>
        /// Проиграть звук вращения оружием
        /// </summary>
        /// <param name="value"></param>
        public override void PlaySpinAudio(float value)
        {
            audioSourceWeapon.clip =
                audioSpin[audioSpin.Length - 1];
            audioSourceWeapon.pitch =
                LibraryStaticFunctions.GetRangeValue(1, 0.1f);
            audioSourceWeapon.Play();
        }

        /// <summary>
        /// Проиграть звук интро босса
        /// </summary>
        public virtual void PlayIntroSound()
        {
            audioSourceObject.clip =
                audioIntroBoss[Random.Range(0, audioBreath.Length)];
            audioSourceObject.pitch =
                LibraryStaticFunctions.GetRangeValue(1, 0.1f);
            audioSourceObject.Play();
        }

        /// <summary>
        /// Проиграть звук вращения оружием (пустая реализация)
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="value"></param>
        public override void PlaySpinAudio(float speed, bool value = false)
        {
            // Пустая реализация
        }
    }
}
