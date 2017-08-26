using System;
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
        protected static AudioClip[] audioHurt; // Звуки повреждений по телу
        protected static AudioClip[] audioDead; // Звуки смерти персонажа
        protected static AudioClip[] audioSteps; // Звуки шагов
        private static AudioClip[] audioSpin; // Звуки вращения оружия
        private static AudioClip[] audioRoarAttack; // Звуки вращения оружия
        private static AudioClip[] audioBreath;
        private bool isMayToPlayWeaponAudio;

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
        }

        private void InitialisationBreathAudio()
        {
            tempAudioList = Resources.LoadAll("Sounds/Mobs/FirstBoss/Breath");
            audioBreath = new AudioClip[tempAudioList.Length];
            for (int i = 0; i < tempAudioList.Length; i++)
            {
                audioBreath[i] = (AudioClip)tempAudioList[i];
            }
        }

        protected override void InitialisationStepsSounds()
        {
            tempAudioList = Resources.LoadAll("Sounds/PlayerMale/Steps");
            audioSteps = new AudioClip[tempAudioList.Length];
            for (int i = 0; i < tempAudioList.Length; i++)
            {
                audioSteps[i] = (AudioClip)tempAudioList[i];
            }
        }

        protected void InitialisationRoarAttackSounds()
        {
            tempAudioList = Resources.LoadAll("Sounds/Mobs/FirstBoss/Attack");
            audioRoarAttack = new AudioClip[tempAudioList.Length];
            for (int i = 0; i < tempAudioList.Length; i++)
            {
                audioRoarAttack[i] = (AudioClip)tempAudioList[i];
            }
        }

        /// <summary>
        /// Взмах оружия
        /// </summary>
        private void InitialisationSpinSounds()
        {
            tempAudioList = Resources.LoadAll("Sounds/PlayerMale/Attack");
            audioSpin = new AudioClip[tempAudioList.Length];
            for (int i = 0; i < tempAudioList.Length; i++)
            {
                audioSpin[i] = (AudioClip)tempAudioList[i];
            }
        }

        /// <summary>
        /// Удар по броне
        /// </summary>
        private void InitialisationHitToArmorySounds()
        {
            tempAudioList = Resources.LoadAll("Sounds/Common/Weapon/Cutting");
            audioHitArmory = new AudioClip[tempAudioList.Length];
            for (int i = 0; i < tempAudioList.Length; i++)
            {
                audioHitArmory[i] = (AudioClip)tempAudioList[i];
            }
        }

        protected override void InitialisationHurtSounds()
        {
            tempAudioList = Resources.LoadAll("Sounds/Mobs/FirstBoss/GetDamage");
            audioHurt = new AudioClip[tempAudioList.Length];
            for (int i = 0; i < tempAudioList.Length; i++)
            {
                audioHurt[i] = (AudioClip)tempAudioList[i];
            }
        }

        protected override void InitialisationDeadSounds()
        {
            tempAudioList = Resources.LoadAll("Sounds/Mobs/FirstBoss/Dead");
            audioDead = new AudioClip[tempAudioList.Length];
            for (int i = 0; i < tempAudioList.Length; i++)
            {
                audioDead[i] = (AudioClip)tempAudioList[i];
            }
        }

        public override void PlayStepAudio()
        {
            audioSourceLegs.clip =
                audioSteps[UnityEngine.Random.Range(0, audioSteps.Length)];
            audioSourceLegs.pitch = LibraryStaticFunctions.GetRangeValue(1, 0.1f);
            audioSourceLegs.volume = volumeStep;
            audioSourceLegs.Play();
        }

        public override void PlayDeadAudio()
        {
            audioSourceObject.volume =
                LibraryStaticFunctions.GetRangeValue(volumeDead, 0.1f);
            audioSourceObject.clip =
               audioDead[UnityEngine.Random.Range(0, audioDead.Length)];
            audioSourceObject.pitch = 1 +
                UnityEngine.Random.Range(0,1f) / 2;
            audioSourceObject.Play();
        }

        public void PlayRoarAttackAudio()
        {
            audioSourceObject.volume =
                LibraryStaticFunctions.GetRangeValue(volumeHutred, 0.1f);
            audioSourceObject.clip =
               audioRoarAttack[UnityEngine.Random.Range(0, audioRoarAttack.Length)];
            audioSourceObject.pitch = 1 +
                UnityEngine.Random.Range(0,1f) / 2;
            audioSourceObject.Play();
        }

        public override void FallObject()
        {
            PlayBodyFallAudio(audioSourceLegs);
        }

        public override void PlayGetDamageAudio(bool isArmory = false)
        {
            audioSourceObject.volume =
               LibraryStaticFunctions.GetRangeValue(volumeHutred, 0.1f);
            if (isArmory)
            {
                audioSourceObject.clip =
                   audioHitArmory[UnityEngine.Random.Range(0, audioHitArmory.Length)];
                audioSourceObject.pitch = LibraryStaticFunctions.GetRangeValue(1, 0.1f);
                audioSourceObject.Play();
            }
            else
            {
                audioSourceObject.clip =
                     audioHurt[UnityEngine.Random.Range(0, audioHurt.Length)];
                audioSourceObject.pitch = LibraryStaticFunctions.GetRangeValue(1, 0.1f);
                audioSourceObject.Play();
            }
        }

        public override void PlayWeaponHitAudio(int value)
        {
            if (value == 2)
            {
                audioSourceWeapon.clip =
                    audioHitArmory[UnityEngine.Random.Range(0, audioHitArmory.Length - 1)];
                audioSourceWeapon.pitch =
                    LibraryStaticFunctions.GetRangeValue(1, 0.1f);
                audioSourceWeapon.Play();
            }
            else if (value == 1)
            {
                audioSourceWeapon.clip =
                    audioCutBody[UnityEngine.Random.Range(0, audioCutBody.Length - 1)];
                audioSourceWeapon.pitch =
                    LibraryStaticFunctions.GetRangeValue(1, 0.1f);
                audioSourceWeapon.Play();
            }
        }

        public void PlayBreathAudio()
        {
            audioSourceObject.clip =
                audioBreath[UnityEngine.Random.Range(0, audioBreath.Length)];
            audioSourceObject.pitch = LibraryStaticFunctions.GetRangeValue(1, 0.1f);
            audioSourceObject.loop = true;
            audioSourceObject.Play();
        }

        public override void PlaySpinAudio(float value)
        {
            audioSourceWeapon.clip =
                audioSpin[audioSpin.Length - 1];
            audioSourceWeapon.pitch =
                LibraryStaticFunctions.GetRangeValue(1, 0.1f);
            audioSourceWeapon.Play();
        }

        public override void PlaySpinAudio(float speed, bool value = false)
        {
            // Пустая реализация
        }
    }
}
