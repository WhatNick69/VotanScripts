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

        /// <summary>
        /// Проиграть статичное аудио (?)
        /// </summary>
        /// <param name="state"></param>
        public static void PlaySoundStatic(int state = 0)
        {
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
