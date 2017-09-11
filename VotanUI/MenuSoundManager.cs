using UnityEngine;

namespace VotanUI
{
    /// <summary>
    /// Хранит звуки для интерфейса
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class MenuSoundManager
        : MonoBehaviour
    {
        private static AudioClip[] audioUI;
        private static AudioClip[] pickUpsBottles;
        private static object[] tempAudioList;
        private static AudioSource audioSource;

        /// <summary>
        /// Инициализация звуков
        /// </summary>
        private void Start()
        {
            audioSource = GetComponent<AudioSource>();

            tempAudioList = Resources.LoadAll("Sounds/UI");
            audioUI = new AudioClip[tempAudioList.Length];
            for (int i = 0; i < tempAudioList.Length; i++)
            {
                audioUI[i] = (AudioClip)tempAudioList[i];
            }
        }

        private void InitialisationSoundsCommon()
        {
            audioSource = GetComponent<AudioSource>();

            tempAudioList = Resources.LoadAll("Sounds/UI");
            audioUI = new AudioClip[tempAudioList.Length];
            for (int i = 0; i < tempAudioList.Length; i++)
            {
                audioUI[i] = (AudioClip)tempAudioList[i];
            }
        }

        private void InitialisationSoundsPickUps()
        {
            audioSource = GetComponent<AudioSource>();

            tempAudioList = Resources.LoadAll("Sounds/Common/PickUps/Bottles");
            audioUI = new AudioClip[tempAudioList.Length];
            for (int i = 0; i < tempAudioList.Length; i++)
            {
                audioUI[i] = (AudioClip)tempAudioList[i];
            }
        }

        /// <summary>
        /// Проиграть статичное аудио (?)
        /// </summary>
        /// <param name="state"></param>
        public static void PlaySoundStatic(int state = 0)
        {
            if (state == -1)
                audioSource.clip = audioUI[audioUI.Length-1];
            else
                audioSource.clip = audioUI[state];
            audioSource.Play();
        }

        /// <summary>
        /// Проиграть звук
        /// </summary>
        /// <param name=""></param>
        public void PlaySound(int state)
        {
            audioSource.clip = audioUI[state];
            audioSource.Play();
        }
    }
}
