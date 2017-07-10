using MovementEffects;
using System.Collections.Generic;
using UnityEngine;
using VotanInterfaces;
using VotanLibraries;

namespace GameBehaviour
{
    /// <summary>
    /// Менеджер ледяного эффекта
    /// </summary>
    public class IceEffectManager
        : MonoBehaviour, IEffect
    {
        #region Переменные
        [SerializeField, Tooltip("Лист ледяных мешей")]
        private List<Transform> listIceObjects; 
        [SerializeField, Tooltip("Лист трэилов")]
        private List<Transform> listTrailObjects;

        private List<Vector3> listPositionIceObjects;
        private List<Vector3> listPositionTrailObjects;

        private bool isRandomTrailPosition;
        private float timeToDisable;

        private bool isOneCoroutine;
        #endregion

        /// <summary>
        /// Зажечь событие для ледяного эффекта.
        /// Ледяные объекты возникают под игровым объектом из под земли
        /// поднимаясь снизу вверх. Кончики ледяных объектов блестят, при помощи
        /// трэилов. Затем, по истечению определенного времени, эти ледяные глыбы
        /// вместе с трэилами исчезают, посредством сведения к нулю их альфа-канала.
        /// </summary>
        public void FireEventEffect(float timeToDisable=0)
        {
            isOneCoroutine = false;
            this.timeToDisable = timeToDisable-0.5f;
            SetActiveForTrailObjects(true);
            SetActiveForIceObjects(true);

            listPositionIceObjects = new List<Vector3>();
            listPositionTrailObjects = new List<Vector3>();
            isRandomTrailPosition = true;

            RandomSetScaleAndPosition();
            Timing.RunCoroutine(CoroutineForFireDisableIceObjects());
            Timing.RunCoroutine(CoroutineForMoveIceObjects());
            Timing.RunCoroutine(CoroutineSetRandomPositionForTrailIce());
        }

        private IEnumerator<float> CorutineDebug(float value)
        {
            while (true)
            {
                this.timeToDisable = value;
                SetActiveForIceObjects(true);
                listPositionIceObjects = new List<Vector3>();
                listPositionTrailObjects = new List<Vector3>();
                isRandomTrailPosition = true;

                RandomSetScaleAndPosition();
                Timing.RunCoroutine(CoroutineForFireDisableIceObjects());
                Timing.RunCoroutine(CoroutineForMoveIceObjects());
                Timing.RunCoroutine(CoroutineSetRandomPositionForTrailIce());

                yield return Timing.WaitForSeconds(10);
            }
        }

        /// <summary>
        /// Сбросить изменения трансформа
        /// </summary>
        /// <param name="flag"></param>
        private void SetActiveForIceObjects(bool flag)
        {
            if (this == null) return;
            foreach (Transform iceObject in listIceObjects)
            {
                iceObject.localPosition = Vector3.zero;
                iceObject.localScale = new Vector3(1,1,1);
                iceObject.gameObject.SetActive(flag);
            }
        }

        private void SetActiveForTrailObjects(bool flag)
        {
            foreach (Transform trailObject in listTrailObjects)
                trailObject.gameObject.SetActive(flag);
        }

        /// <summary>
        /// Устанавливаем случайное положение ледяных объектов, 
        /// их размер, поворот.
        /// Затем кешируем позиции объектов
        /// </summary>
        private void RandomSetScaleAndPosition()
        {
            float scale = 0;
            for (int i = 0; i < listIceObjects.Count; i++)
            {
                // Позиция
                listIceObjects[i].position =
                    new Vector3(listIceObjects[i].position.x +
                        LibraryStaticFunctions.GetPlusMinusValue(0.5f), 
                    listIceObjects[i].position.y + 0,
                    listIceObjects[i].position.z + 
                        LibraryStaticFunctions.GetPlusMinusValue(0.5f));

                // Размер
                scale = LibraryStaticFunctions.GetRangeValue(1,0.2f);
                listIceObjects[i].localScale = new Vector3(scale,scale, scale);

                // Поворот
                listIceObjects[i].rotation = Quaternion.Euler
                    (0, LibraryStaticFunctions.rnd.Next(0,360), 0);

                // Кеширование позиции
                listPositionIceObjects.Add(new Vector3(listIceObjects[i].position.x,
                    listIceObjects[i].position.y + 
                    (float)LibraryStaticFunctions.rnd.NextDouble()/4+1.75f,
                    listIceObjects[i].position.z));
            }
        }

        /// <summary>
        /// Корутина для движения 
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForMoveIceObjects()
        {
            int i = 0;
            while (i < 25)
            {
                for (int j = 0; j < listIceObjects.Count; j++)
                {
                    if (this == null) yield break;

                    listIceObjects[j].position = Vector3.Lerp(listIceObjects[j].position,
                        listPositionIceObjects[j], 0.3f);
                }
                yield return Timing.WaitForSeconds(0.05f);
                i++;
            }       
        }

        /// <summary>
        /// Корутина для установления случайной позиции для трэила льда
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> CoroutineSetRandomPositionForTrailIce()
        {
            // Лист позиций с ледяными трэилами
            foreach (Transform iceTrailPos in listTrailObjects)
                listPositionTrailObjects.Add(iceTrailPos.localPosition);

            while (isRandomTrailPosition)
            {
                for (int i = 0; i < listTrailObjects.Count; i++)
                {
                    if (this == null) yield break;
                    listTrailObjects[i].localPosition =
                        new Vector3(LibraryStaticFunctions.
                            GetRangeValue(listPositionTrailObjects[i].x, 0.05f),
                        LibraryStaticFunctions.
                            GetRangeValue(listPositionTrailObjects[i].y, 0.05f),
                        LibraryStaticFunctions.
                            GetRangeValue(listPositionTrailObjects[i].z, 0.05f));
                }
                yield return Timing.WaitForSeconds(0.1f);
            }
        }

        /// <summary>
        /// Корутина для отключения ледяного эффекта
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForFireDisableIceObjects()
        {
            yield return Timing.WaitForSeconds(timeToDisable);
            isRandomTrailPosition = false;
            if (this == null) yield break;
            Timing.RunCoroutine(CoroutineDisableIceObjects());
        }

        /// <summary>
        /// Корутина на выключение эффекта
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> CoroutineDisableIceObjects()
        {
            isOneCoroutine = true;

            SetActiveForTrailObjects(false);
            Vector3 tempVector = new Vector3(0, 2, 0);
            for (int j = 0; j < listPositionIceObjects.Count; j++)
                listPositionIceObjects[j] -= tempVector;

            int i = 0;
            while (i < 25)
            {
                for (int j = 0; j < listIceObjects.Count; j++)
                {
                    if (this == null || !isOneCoroutine) yield break;

                    listIceObjects[j].position = 
                        Vector3.Lerp(listIceObjects[j].position,
                        listPositionIceObjects[j], 0.3f);
                    listIceObjects[j].localScale = 
                        Vector3.Lerp(listIceObjects[j].localScale, Vector3.zero, 0.3f);
                }
                yield return Timing.WaitForSeconds(0.05f);
                i++;
            }
            if (isOneCoroutine) SetActiveForIceObjects(false);
        }
    }
}