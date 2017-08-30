using AbstractBehaviour;
using MovementEffects;
using System.Collections.Generic;
using UnityEngine;
using VotanLibraries;

namespace VotanGameplay
{
    /// <summary>
    /// Гроза. 
    /// Используется в игре, на заднем плане.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class GameLighting 
        : MonoBehaviour
    {
        [SerializeField, Tooltip("Частота молний"), Range(1, 30)]
        private float frequencyOfLighting;
        private AudioSource audioSource;
        private Transform[] lightingList;
        private TrailRenderer[] trailRendererList;
        bool[] isLookAtDestinationList;
        private Vector3 startPosition;
        private Vector3 newPosition;
        private bool isLightingsActive;
        private bool isLight;
        private byte iterations;

        /// <summary>
        /// Инициализация
        /// </summary>
        private void Start()
        {
            lightingList = new Transform[transform.childCount];
            trailRendererList = new TrailRenderer[lightingList.Length];

            startPosition = transform.position;
            audioSource = GetComponent<AudioSource>();

            GetAllChilds();
            Timing.RunCoroutine(CoroutineForLighting());
        }

        /// <summary>
        /// Получить все элементы грозы
        /// </summary>
        private void GetAllChilds()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                lightingList[i] = transform.GetChild(i);
                trailRendererList[i] = transform.GetChild(i).GetComponent<TrailRenderer>();
            }
                
            isLookAtDestinationList = new bool[lightingList.Length];
        }

        /// <summary>
        /// Корутина на обработку грозы
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForLighting()
        {
            while (true)
            {
                // Промежуток, между грозой
                yield return Timing.WaitForSeconds
                    (5+(LibraryStaticFunctions.GetRangeValue
                    (frequencyOfLighting, 0.9f)));
                if (!isLight)
                {
                    FireLighting();
                }
            }
        }

        /// <summary>
        /// Зажечь молнию
        /// </summary>
        public void FireLighting()
        {
            RandomPositionOfLighting();
            NullPositions();
            RandomerScalerAndWidther();

            Timing.RunCoroutine(CoroutineForMoveTrails());
            Timing.RunCoroutine(CoroutineForSound());
        }

        /// <summary>
        /// Корутина для воспроизведения звука для грозы с задержкой
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForSound()
        {
            yield return Timing.WaitForSeconds(LibraryStaticFunctions.GetRangeValue(2, 0.5f));
            AbstractSoundStorage.PlayLightingAudio(audioSource);
        }

        /// <summary>
        /// Случайная позиция для грозы
        /// </summary>
        private void RandomPositionOfLighting()
        {
            newPosition = new Vector3
                (startPosition.x + (LibraryStaticFunctions.
                GetPlusMinusValue(Mathf.Abs(startPosition.x)/2)),
                startPosition.y-LibraryStaticFunctions.GetRangeValue(60,0.1f),
                startPosition.z + (LibraryStaticFunctions.
                GetPlusMinusValue(Mathf.Abs(startPosition.z)/2)));
        }

        /// <summary>
        /// Обнулить позицию
        /// </summary>
        private void NullPositions()
        {
            transform.position = 
                new Vector3(newPosition.x, startPosition.y, newPosition.z);
            foreach (Transform childTransform in lightingList)
                childTransform.localPosition = Vector3.zero;
        }

        /// <summary>
        /// Меняем значение буля. 
        /// false - трэил движется в рандомном направлении.
        /// true - трэил движется к точке назначения.
        /// </summary>
        /// <param name="i"></param>
        private bool RandomBoolValue(int i)
        {
            return Random.Range(0,2) == 1 ? true : false;
        }

        /// <summary>
        /// Задать случайный размер для грозы
        /// </summary>
        private void RandomerScalerAndWidther()
        {
            foreach (TrailRenderer trailChild in trailRendererList)
            {
                trailChild.startWidth = 0.5f + 
                    LibraryStaticFunctions.GetPlusMinusValue(0.2f);
                trailChild.endWidth = trailChild.startWidth - 
                    LibraryStaticFunctions.GetRangeValue(trailChild.startWidth/2);
            }
        }

        /// <summary>
        /// Отключить либо включить объекты грозы
        /// </summary>
        private void DisableObjects()
        {
            if (isLightingsActive) isLightingsActive = false;
            else isLightingsActive = true;

            foreach (Transform lighting in lightingList)
                lighting.gameObject.SetActive(isLightingsActive);
        }

        /// <summary>
        /// Корутина для зажигания электрического эффекта
        /// Безопасный цикл с ограничением в 100 итераций
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForMoveTrails()
        {
            AllPlayerManager.FireLightingEffectInAllPlayers();
            isLight = true;
            DisableObjects();
            iterations = 0;

            Vector3 tempPosition = new Vector3
                (newPosition.x + LibraryStaticFunctions.
                GetPlusMinusValue(1),
                newPosition.y,
                newPosition.z + LibraryStaticFunctions.
                GetPlusMinusValue(1));

            while (iterations < 50)
            {
                for (int i = 0; i < lightingList.Length; i++)
                {
                    if (isLookAtDestinationList[i])
                    {
                        lightingList[i].LookAt(tempPosition);
                        lightingList[i].Translate(lightingList[i].forward
                            * (2 + Random.Range(0,1f)), Space.World);
                    }
                    else
                    {
                        lightingList[i].rotation = Quaternion.Euler
                            (Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
                        lightingList[i].Translate(lightingList[i].forward
                            * (1 + Random.Range(0,1f)));
                    }

                    if (Vector3.Distance(lightingList[i].
                        position, tempPosition) <= 1)
                    {
                        DisableObjects();
                        isLight = false;
                        yield break;
                    }

                    isLookAtDestinationList[i] = RandomBoolValue(i);
                }
                yield return Timing.WaitForSeconds(0.01f);
                iterations++;
            }
            isLight = false;
            DisableObjects();
        }
    }
}
