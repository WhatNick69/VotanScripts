﻿using MovementEffects;
using UnityEngine;
using VotanInterfaces;
using System.Collections.Generic;
using GameBehaviour;
using System;

namespace VotanGameplay
{
    /// <summary>
    /// Описывает простейшее поведение ящика на сцене.
    /// Вызов события позволяет включать физику у ящика, тем самым ломая его.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class DynamicGameobjectPhysicsBox
        : MonoBehaviour, IDynamicGameobject
    {
        #region Переменные и ссылки
        [SerializeField,Tooltip("Время отключения физики у всех дочерних объектов")]
        private float timerToDisable;
        [SerializeField, Tooltip("Время удаления объекта")]
        private float timerToDestroyObject;
        [SerializeField, Tooltip("Компонент-дроп")]
        private RandomInGameDrop randomComponent;
        private Rigidbody[] listRigidbody;
        private BoxCollider[] listBoxColliders;
        private Vector3[] positionsOfObjects;
        private AudioSource audioSource;
        #endregion

        /// <summary>
        /// Инициализация
        /// </summary>
        public void Start()
        {
            audioSource = GetComponent<AudioSource>();

            GameObject[] tempListObjects
                = new GameObject[transform.childCount];
            for (int i = 0; i < tempListObjects.Length; i++)
                tempListObjects[i] = transform.GetChild(i).gameObject;
            listRigidbody = new Rigidbody[tempListObjects.Length];
            listBoxColliders = new BoxCollider[tempListObjects.Length];

            for (int i = 0; i < listRigidbody.Length; i++)
            {
                listRigidbody[i] =
                    tempListObjects[i].GetComponent<Rigidbody>();
                listBoxColliders[i] =
                    tempListObjects[i].GetComponent<BoxCollider>();
            }

            RigidbodyEnable(false);
            SaveObjectsPositions();
        }

        /// <summary>
        /// Зажечь событие
        /// </summary>
        public void FireEvent()
        {
            Timing.RunCoroutine(CoroutineFreezyAll());
            Timing.RunCoroutine(CoroutineForDestroyObject());
        }

        /// <summary>
        /// Корутина на уничтожение объекта.
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForDestroyObject()
        {
            yield return Timing.WaitForSeconds(timerToDestroyObject-2);
            Timing.RunCoroutine(CoroutineForDownObject());
            yield return Timing.WaitForSeconds(2);
            Destroy(gameObject);
        }

        /// <summary>
        /// Корутина для опускания под землю предмета/умения.
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForDownObject()
        {
            Vector3 translateVector = new Vector3(0, 10, 0);
            Vector3 destPosition = transform.localPosition - translateVector;
            for (int i =0;i<20;i++)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, 
                    destPosition, Time.deltaTime * i);
                yield return Timing.WaitForSeconds(0.05f);
            }
        }

        /// <summary>
        /// Заморозить все объекты.
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> CoroutineFreezyAll()
        {
            PlayCrushSound();
            RigidbodyEnable(true);
            randomComponent.StartRendering();
            BoxColliderEnable(true);

            yield return Timing.WaitForSeconds(timerToDisable);

            randomComponent.StartCoroutines();
            RigidbodyEnable(false);
            BoxColliderEnable(false);
        }

        /// <summary>
        /// Проиграть звук ломающегося ящика
        /// </summary>
        private void PlayCrushSound()
        {
            audioSource.Play();
        }

        /// <summary>
        /// Включить, либо выключить физику у объекта
        /// </summary>
        /// <param name="isEnable"></param>
        private void RigidbodyEnable(bool isEnable)
        {
            foreach (Rigidbody rig in listRigidbody)
            {
                if (rig == null) continue;

                rig.useGravity = isEnable;
                rig.detectCollisions = isEnable;
                if (isEnable)
                    rig.constraints = RigidbodyConstraints.None;
                else
                    rig.constraints = RigidbodyConstraints.FreezeAll;
            }
        }

        /// <summary>
        /// Рестарт всего объекта
        /// </summary>
        public void RestartObject()
        {
            ResetObjectsPositions();
        }

        /// <summary>
        /// Сохранить позиции дочерних объектов родителя
        /// </summary>
        private void SaveObjectsPositions()
        {
            positionsOfObjects = new Vector3[listRigidbody.Length];
            for (int i = 0; i < listRigidbody.Length; i++)
                positionsOfObjects[i] = listRigidbody[i].transform.position;
        }

        /// <summary>
        /// Рестарт позиций дочерних объектов родителя
        /// </summary>
        private void ResetObjectsPositions()
        {
            RigidbodyEnable(false);
            BoxColliderEnable(false);
            for (int i = 0; i < listRigidbody.Length; i++)
                listRigidbody[i].transform.position = positionsOfObjects[i];
        }

        /// <summary>
        /// Включить/выключить коллизию дочерних объектов
        /// </summary>
        /// <param name="isEnable"></param>
        private void BoxColliderEnable(bool isEnable)
        {
            foreach (BoxCollider boxCollider in listBoxColliders)
            {
                if (boxCollider == null) continue;

                boxCollider.enabled = isEnable;
            }
        }
    }
}
