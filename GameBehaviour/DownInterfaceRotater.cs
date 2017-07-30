using MovementEffects;
using System.Collections.Generic;
using UnityEngine;
using VotanGameplay;
using VotanInterfaces;

namespace GameBehaviour
{
    /// <summary>
    /// Компонента для поворота
    /// down-интерфейса при подъеме по лестнице
    /// </summary>
    [RequireComponent(typeof(IVotanObjectConditions))]
    public class DownInterfaceRotater
        : MonoBehaviour
    {
        #region Переменные
        [SerializeField, Tooltip
            ("Частота обновления (0 - каждый кадр"), Range(0, 1f)]
        private float refreshFrequency;
        [SerializeField, Tooltip("Объект, интерфейс которого нужно поворачивать")]
        private Transform objectTransform;
        private IVotanObjectConditions votanConditions;
        public IObjectFitBat downInterface;
        private TriaglesRender triangleRender;
        private float angle = 0;
        #endregion

        /// <summary>
        /// Инициализация
        /// </summary>
        private void Start()
        {
            votanConditions = GetComponent<IVotanObjectConditions>();
            downInterface = GetComponent<IObjectFitBat>();
            triangleRender = GameObject.Find("Environment")
                .transform.GetComponentInChildren<TriaglesRender>();
        }

        public virtual void RestartDownInterfaceRotater()
        {
            votanConditions = GetComponent<IVotanObjectConditions>();
            downInterface = GetComponent<IObjectFitBat>();
            triangleRender = GameObject.Find("Environment")
                .transform.GetComponentInChildren<TriaglesRender>();

            if (refreshFrequency == 0)
                Timing.RunCoroutine(CoroutineForRotateDownInterfacePerFrame());
            else
                Timing.RunCoroutine(CoroutineForRotateDownInterfaceWithFrequency());
        }

        /// <summary>
        /// Корутина для работы по кадрам
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForRotateDownInterfaceWithFrequency()
        {
            while (votanConditions.IsAlive)
            {
                BodyOfCoroutine();
                yield return Timing.WaitForSeconds(refreshFrequency);
            }
            downInterface.ActiveDownInterface(false);
        }

        /// <summary>
        /// Корутина для работы по частоте
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForRotateDownInterfacePerFrame()
        {
            while (votanConditions.IsAlive)
            {
                BodyOfCoroutine();
                yield return Timing.WaitForOneFrame;
            }
            downInterface.ActiveDownInterface(false);
        }

        /// <summary>
        /// Тело корутины
        /// </summary>
        private void BodyOfCoroutine()
        {
            if (triangleRender.IsOnTheStairs(objectTransform.position, ref angle))
                downInterface.RotateConditionBar(true, angle);
            else
                downInterface.RotateConditionBar(false, 90);
        }
    }
}
