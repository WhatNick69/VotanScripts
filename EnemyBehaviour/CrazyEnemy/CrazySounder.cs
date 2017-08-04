using AbstractBehaviour;
using UnityEngine;
using VotanLibraries;

namespace EnemyBehaviour
{
    /// <summary>
    /// Саундер для бешеного рыцаря
    /// </summary>
    public class CrazySounder
        : AbstractSoundStorage
    {
        protected static AudioClip[] audioHurt; // Звуки повреждений по телу
        protected static AudioClip[] audioDead; // Звуки смерти персонажа
        protected static AudioClip[] audioSteps; // Звуки шагов
        private static AudioClip[] audioSpin; // Звуки вращения оружия
        private float spinRotating;
        private bool isMayToPlayWeaponAudio;

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
        /// Загрузить звуки смерти
        /// </summary>
        protected override void InitialisationDeadSounds()
        {
            tempAudioList = Resources.LoadAll("Sounds/Mobs/LiveEnemies/Dead");
            audioDead = new AudioClip[tempAudioList.Length];
            for (int i = 0; i < tempAudioList.Length; i++)
            {
                audioDead[i] = (AudioClip)tempAudioList[i];
            }
        }

        /// <summary>
        /// Загрузить стоны
        /// </summary>
        protected override void InitialisationHurtSounds()
        {
            tempAudioList = Resources.LoadAll("Sounds/Mobs/LiveEnemies/GetDamage");
            audioHurt = new AudioClip[tempAudioList.Length];
            for (int i = 0; i < tempAudioList.Length; i++)
            {
                audioHurt[i] = (AudioClip)tempAudioList[i];
            }
        }

        /// <summary>
        /// Загрузить шаги
        /// </summary>
        protected override void InitialisationStepsSounds()
        {
            tempAudioList = Resources.LoadAll("Sounds/PlayerMale/Steps");
            audioSteps = new AudioClip[tempAudioList.Length];
            for (int i = 0; i < tempAudioList.Length; i++)
            {
                audioSteps[i] = (AudioClip)tempAudioList[i];
            }
        }

        /// <summary>
        /// Получение урона
        /// </summary>
        /// <param name="isArmory"></param>
        public override void PlayGetDamageAudio(bool isArmory = false)
        {
            audioSourceObject.volume =
                LibraryStaticFunctions.GetRangeValue(volumeHutred, 0.1f);
            audioSourceObject.clip =
                 audioHurt[LibraryStaticFunctions.rnd.
                 Next(0, audioHurt.Length)];
            audioSourceObject.pitch = LibraryStaticFunctions.GetRangeValue(1, 0.1f);
            audioSourceObject.Play();
        }

        /// <summary>
        /// играть звук смерти
        /// </summary>
        public override void PlayDeadAudio()
        {
            audioSourceObject.volume =
                LibraryStaticFunctions.GetRangeValue(volumeDead, 0.1f);
            audioSourceObject.clip =
               audioDead[LibraryStaticFunctions.rnd.
               Next(0, audioDead.Length)];
            audioSourceObject.pitch = 1 +
                (float)LibraryStaticFunctions.rnd.NextDouble() / 2;
            audioSourceObject.Play();
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

        /// <summary>
        /// Взмах оружия
        /// </summary>
        private void InitialisationSpinSounds()
        {
            tempAudioList = Resources.LoadAll("Sounds/Mobs/Common/Swipe");
            audioSpin = new AudioClip[tempAudioList.Length];
            for (int i = 0; i < tempAudioList.Length; i++)
            {
                audioSpin[i] = (AudioClip)tempAudioList[i];
            }
        }

        /// <summary>
        /// Звук вращения клинком
        /// </summary>
        public override void PlaySpinAudio(float value)
        {
            value /= 1000;
            value = Mathf.Abs(value);
            if (value <= 0.1f) return;

            audioSourceWeapon.clip =
                audioSpin[LibraryStaticFunctions.rnd.
                Next(0, audioSpin.Length - 1)];
            audioSourceWeapon.pitch = 1 + value / 2;
            audioSourceWeapon.Play();
        }

        public override void PlaySpinAudio(float speed, bool value = false)
        {
            // Пустая реализация
        }

        public override void PlayWeaponHitAudio(int value)
        {
            if (value == 2)
            {

                audioSourceWeapon.clip =
                    audioHitArmory[LibraryStaticFunctions.rnd.
                    Next(0, audioHitArmory.Length - 1)];
                audioSourceWeapon.pitch =
                    LibraryStaticFunctions.GetRangeValue(1, 0.1f);
                audioSourceWeapon.Play();
            }
            else if (value == 1)
            {
                audioSourceWeapon.clip =
                    audioCutBody[LibraryStaticFunctions.rnd.
                    Next(0, audioCutBody.Length - 1)];
                audioSourceWeapon.pitch =
                    LibraryStaticFunctions.GetRangeValue(1, 0.1f);
                audioSourceWeapon.Play();
            }
        }

        public override void FallObject()
        {
            WorkWithSoundsBodyFall(audioSourceLegs);
        }

        public override void PlayStepAudio()
        {
            audioSourceLegs.clip =
                audioSteps[LibraryStaticFunctions.rnd.
                Next(0, audioSteps.Length)];
            audioSourceLegs.pitch = LibraryStaticFunctions.GetRangeValue(1, 0.1f);
            audioSourceLegs.volume = volumeStep;
            audioSourceLegs.Play();
        }
    }
}
