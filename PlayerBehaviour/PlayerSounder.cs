using AbstractBehaviour;
using System.Collections.Generic;
using UnityEngine;
using VotanLibraries;
using MovementEffects;

namespace PlayerBehaviour
{
    /// <summary>
    /// Описывает звук для игрока
    /// </summary>
    public class PlayerSounder 
        : AbstractSoundStorage
    {
        #region Переменные
        protected static AudioClip[] audioHurt; // Звуки повреждений по телу
        protected static AudioClip[] audioDead; // Звуки смерти персонажа
        protected static AudioClip[] audioSteps; // Звуки шагов
        protected static AudioClip[] audioDestroyArmory; // Звуки ломающейся брони
        private static AudioClip[] audioSpin; // Звуки вращения оружия
        private static AudioClip[] audioRoar;

        private bool isMayToPlayWeaponAudio;
        [SerializeField]
        private PlayerComponentsControl playerComponentsControl;
        #endregion

        #region Инициализация
        /// <summary>
        /// Инициализация
        /// </summary>
        protected override void Start()
        {
            base.Start();
            isMayToPlayWeaponAudio = true;

            InitialisationHitToArmorySounds();
            InitialisationStepsSounds();
            InitialisationSpinSounds();
            InitialisationHurtSounds();
            InitialisationDeadSounds();
            InitialisationDestroyArmorySounds();
            InitialisationsRoarSounds();
        }

        /// <summary>
        /// Инициализация звуков рева игрока
        /// </summary>
        private void InitialisationsRoarSounds()
        {
            tempAudioList = Resources.LoadAll("Sounds/PlayerMale/Roar");
            audioRoar = new AudioClip[tempAudioList.Length];
            for (int i = 0; i < tempAudioList.Length; i++)
            {
                audioRoar[i] = (AudioClip)tempAudioList[i];
            }
        }

        /// <summary>
        /// Удар по броне
        /// </summary>
        private void InitialisationHitToArmorySounds()
        {
            tempAudioList = Resources.LoadAll("Sounds/Common/HurtArmory");
            audioHitArmory = new AudioClip[tempAudioList.Length];
            for (int i = 0; i < tempAudioList.Length; i++)
            {
                audioHitArmory[i] = (AudioClip)tempAudioList[i];
            }
        }

        /// <summary>
        /// Поломка брони
        /// </summary>
        private void InitialisationDestroyArmorySounds()
        {
            tempAudioList = Resources.LoadAll("Sounds/Common/DestroyArmory");
            audioDestroyArmory = new AudioClip[tempAudioList.Length];
            for (int i = 0; i < tempAudioList.Length; i++)
            {
                audioDestroyArmory[i] = (AudioClip)tempAudioList[i];
            }
        }

        /// <summary>
        /// Инициализация звуков кручения
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
        /// Инициализация звуков шагов
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
        /// Инициализация звуков получения повреждений
        /// </summary>
        protected override void InitialisationHurtSounds()
        {
            tempAudioList = Resources.LoadAll("Sounds/PlayerMale/Hurt");
            audioHurt = new AudioClip[tempAudioList.Length];
            for (int i = 0; i < tempAudioList.Length; i++)
            {
                audioHurt[i] = (AudioClip)tempAudioList[i];
            }
        }

        /// <summary>
        /// Инициализация звуков смерти
        /// </summary>
        protected override void InitialisationDeadSounds()
        {
            tempAudioList = Resources.LoadAll("Sounds/PlayerMale/Dead");
            audioDead = new AudioClip[tempAudioList.Length];
            for (int i = 0; i < tempAudioList.Length; i++)
            {
                audioDead[i] = (AudioClip)tempAudioList[i];
            }
        }
        #endregion

        /// <summary>
        /// Удар по броне, либо по крик (если по телу)
        /// </summary>
        public override void PlayGetDamageAudio(bool isArmory)
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

        /// <summary>
        /// Проиграть звук вращения оружием
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="isSuperAttack"></param>
        public override void PlaySpinAudio(float speed, bool isSuperAttack=false)
        {
            speed /= 25;
            speed = Mathf.Abs(speed);
            if (speed <= 0.1f) return;

            if (isSuperAttack)
            {
                audioSourceWeapon.clip = audioSpin[audioSpin.Length - 1];
            }
            else
            {
                audioSourceWeapon.clip =
                    audioSpin[UnityEngine.Random.Range(0, audioSpin.Length - 1)];
            }
            audioSourceWeapon.pitch = LibraryStaticFunctions.GetRangeValue(0.5f + speed/2,0.05f);
            audioSourceWeapon.Play();
        }

        /// <summary>
        /// Проиграть звук лязга оружия в зависимости от его типа
        /// </summary>
        /// <param name="weaponType"></param>
        public override void PlayWeaponHitAudio(int isCuttingWeapon)
        {
            if (isMayToPlayWeaponAudio)
            {
                if (isCuttingWeapon == 1)
                {
                    audioSourceWeapon.clip =
                       audioCutBody[UnityEngine.Random.Range(0, audioCutBody.Length)];
                    audioSourceWeapon.pitch = LibraryStaticFunctions.GetRangeValue(1, 0.1f);
                    audioSourceWeapon.Play();
                }
                else if (isCuttingWeapon == 0)
                { 
                    audioSourceWeapon.clip =
                        audioHitBody[UnityEngine.Random.Range(0, audioHitBody.Length)];
                    audioSourceWeapon.pitch = LibraryStaticFunctions.GetRangeValue(1, 0.1f);
                    audioSourceWeapon.Play();
                }
                Timing.RunCoroutine(CoroutineForMayToPlayWeaponSound());
            }
        }

        /// <summary>
        /// Корутина на возможность проигрывания звука вонзания в плоть врага
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForMayToPlayWeaponSound()
        {
            isMayToPlayWeaponAudio = false;
            yield return Timing.WaitForSeconds(0.25f);
            isMayToPlayWeaponAudio = true;
        }

        /// <summary>
        /// Проиграть звук ломающейся брони в зависимости от типа брони
        /// </summary>
        /// <param name="armoryPosition"></param>
        public void PlayAnyDestroyArmoryAudio(ArmoryPosition armoryPosition)
        {
            audioSourceObject.volume =
                LibraryStaticFunctions.GetRangeValue(volumeHutred, 0.1f);
            switch (armoryPosition)
            {                   
                case ArmoryPosition.Cuirass:
                    audioSourceObject.clip =
                        audioDestroyArmory[UnityEngine.Random.Range(0, 3)];
                    audioSourceObject.pitch = LibraryStaticFunctions.GetRangeValue(1, 0.1f);
                    audioSourceObject.Play();
                    break;
                case ArmoryPosition.Helmet:
                    audioSourceObject.clip =
                        audioDestroyArmory[UnityEngine.Random.Range(2, audioDestroyArmory.Length)];
                    audioSourceObject.pitch = LibraryStaticFunctions.GetRangeValue(1, 0.1f);
                    audioSourceObject.Play();
                    break;
                case ArmoryPosition.LeftBallast:
                    audioSourceObject.clip =
                        audioDestroyArmory[UnityEngine.Random.Range(2, audioDestroyArmory.Length)];
                    audioSourceObject.pitch = LibraryStaticFunctions.GetRangeValue(1, 0.1f);
                    audioSourceObject.Play();
                    break;
                case ArmoryPosition.LeftShoulder:
                    audioSourceObject.clip =
                        audioDestroyArmory[UnityEngine.Random.Range(2, audioDestroyArmory.Length)];
                    audioSourceObject.pitch = LibraryStaticFunctions.GetRangeValue(1, 0.1f);
                    audioSourceObject.Play();
                    break;
                case ArmoryPosition.RightBallast:
                    audioSourceObject.clip =
                        audioDestroyArmory[UnityEngine.Random.Range(2, audioDestroyArmory.Length)];
                    audioSourceObject.pitch = LibraryStaticFunctions.GetRangeValue(1, 0.1f);
                    audioSourceObject.Play();
                    break;
                case ArmoryPosition.RightShoulder:
                    audioSourceObject.clip =
                        audioDestroyArmory[UnityEngine.Random.Range(2, audioDestroyArmory.Length)];
                    audioSourceObject.pitch = LibraryStaticFunctions.GetRangeValue(1, 0.1f);
                    audioSourceObject.Play();
                    break;
                case ArmoryPosition.Shield:

                    audioSourceObject.clip =
                        audioDestroyArmory[UnityEngine.Random.Range(0, 3)];
                    audioSourceObject.pitch = LibraryStaticFunctions.GetRangeValue(1, 0.1f);
                    audioSourceObject.Play();
                    break;
            }
        }

        public override void PlaySpinAudio(float speed)
        {
            // Пустая реализация
        }

        /// <summary>
        /// Звук падения
        /// </summary>
        public override void FallObject()
        {
            if (!playerComponentsControl.PlayerConditions.IsFallingDead)
            {
                WorkWithSoundsBodyFall(audioSourceLegs);
            }
        }

        /// <summary>
        /// Звук атакующего рывка оружием
        /// </summary>
        public void PlayAttackDashAudio()
        {
            audioSourceWeapon.clip =
                audioSpin[audioSpin.Length-1];
            audioSourceWeapon.pitch = LibraryStaticFunctions.GetRangeValue(1, 0.05f);
            audioSourceWeapon.Play();
        }

        /// <summary>
        /// Рев персонажа
        /// </summary>
        public void PlayRoarAudio()
        {
            audioSourceObject.clip =
                audioRoar[UnityEngine.Random.Range(0, audioRoar.Length)];
            audioSourceObject.pitch = LibraryStaticFunctions.GetRangeValue(1, 0.05f);
            audioSourceObject.Play();
        }

        /// <summary>
        /// Проиграть звук ходьбы
        /// </summary>
        public override void PlayStepAudio()
        {
            audioSourceLegs.volume = volumeStep;
            audioSourceLegs.clip =
                audioSteps[UnityEngine.Random.Range(0, audioSteps.Length)];
            audioSourceLegs.pitch = LibraryStaticFunctions.GetRangeValue(1, 0.1f);
            audioSourceLegs.Play();
        }

        /// <summary>
        /// Проиграть звук смерти
        /// </summary>
        public override void PlayDeadAudio()
        {
            audioSourceObject.volume =
                LibraryStaticFunctions.GetRangeValue(volumeDead, 0.1f);
            audioSourceObject.clip =
               audioDead[UnityEngine.Random.Range(0, audioDead.Length)];
            audioSourceObject.pitch = LibraryStaticFunctions.GetRangeValue(1, 0.1f);
            audioSourceObject.Play();
        }
    }
}
