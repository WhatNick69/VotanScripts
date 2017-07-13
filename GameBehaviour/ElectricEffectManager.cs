﻿using AbstractBehaviour;
using MovementEffects;
using PlayerBehaviour;
using System;
using System.Collections.Generic;
using UnityEngine;
using VotanInterfaces;
using VotanLibraries;

namespace GameBehaviour
{
    /// <summary>
    /// Зажечь электрический эффект
    /// </summary>
    class ElectricEffectManager
        : MonoBehaviour, IElectricEffect
    {
        #region Переменные
        [SerializeField, Tooltip("Лист электрических трэилов")]
        private List<Transform> listTrailObjects;
        private float radiusSearch;
        private float gemPower;
        private float damage;
        private IWeapon weapon;
        private int childrenCount;
        private AbstractEnemy abstractEnemyTarget; // цель для атаки
        #endregion

        /// <summary>
        /// Получить допустимое количество детей.
        /// </summary>
        private void GetAllChild(int count)
        {
            if (count >= 6) childrenCount = 5;
            else if (count <= 0) childrenCount = 1;
            else childrenCount = count;

            for (int i = 0; i < childrenCount; i++)
            {
                listTrailObjects.Add(transform.GetChild(i));
            }
        }

        /// <summary>
        /// Установить видимость объектам
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="count"></param>
        private void VisibleOfElements(bool flag)
        {
            if (this == null) return;
            for (int i = 0; i < childrenCount; i++)
            {
                listTrailObjects[i].gameObject.SetActive(flag);
            }
        }

        /// <summary>
        /// Зажечь электрический эффект
        /// </summary>
        /// <param name="time"></param>
        public void EventEffect(float damage, float gemPower, IWeapon weapon)
        {
            this.gemPower = gemPower;
            this.damage = damage;
            this.weapon = weapon;

            if (gemPower <= 0) return;
            //Debug.Log("Запускаем, со значениями DMG: " + damage + ", GP: " + gemPower);
            listTrailObjects.Clear();
            GetAllChild(1 + (int)(gemPower / 20)); // находим объекты


            // запустить корутину, найдя ближайшего врага, как точку назначения для молний
            ContinueElectricDamage(transform.position);
        }

        /// <summary>
        /// Проверка на правильность
        /// </summary>
        private void ChangeValues()
        {
            if (gemPower <= 19)
                damage = damage * (gemPower / 20);
            gemPower -= 20;
        }

        /// <summary>
        /// Продолжаем наносить электрический урон
        /// </summary>
        /// <param name="position"></param>
        /// <param name="isFirst"></param>
        private void ContinueElectricDamage(Vector3 position)
        {
            if (listTrailObjects.Count != 0)
            {
                abstractEnemyTarget = 
                    StaticStorageWithEnemies.GetClosestNonShockedEnemy(position, 5);
                if (abstractEnemyTarget == null) return;

                VisibleOfElements(true); // включаем объекты
                Vector3 destination = abstractEnemyTarget.ReturnPosition(2);
                Timing.RunCoroutine(CoroutineForMoveTrails(destination));
            }
        }

        /// <summary>
        /// Корутина для зажигания электрического эффекта
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForMoveTrails(Vector3 destination)
        {
            bool flag = false;
            int needNumber = -1;
            List<Transform> tempListTrailobjects = new List<Transform>(listTrailObjects);
            bool[] isLookAtDestinationList = new bool[tempListTrailobjects.Count];

            while (true)
            {
                if (tempListTrailobjects.Count == 0) break;
                for (int i = 0;i< tempListTrailobjects.Count;i++)
                {
                    if (this == null) yield break;
                    if (isLookAtDestinationList[i])
                    {
                        tempListTrailobjects[i].LookAt(destination);
                        tempListTrailobjects[i].Translate
                            (tempListTrailobjects[i].forward * 2, Space.World);
                    }
                    else
                    {
                        tempListTrailobjects[i].rotation = Quaternion.Euler
                            (LibraryStaticFunctions.rnd.Next(0, 360),
                            LibraryStaticFunctions.rnd.Next(0, 360),
                            LibraryStaticFunctions.rnd.Next(0, 360));
                        tempListTrailobjects[i].Translate(tempListTrailobjects[i].forward * 2);
                    }

                    if (Vector3.Distance(tempListTrailobjects[i].
                        position, destination) <= 0.5f)
                    {
                        // встретили цель. ударили её. передали эстафету.
                        if (!flag)
                        {
                            flag = true;
                            needNumber = i;
                            if (gemPower >= 1)
                            {
                                ChangeValues();
                                abstractEnemyTarget.EnemyConditions.
                                GetDamageLongDistance(damage*0.75f, gemPower, DamageType.Electric, weapon);
                            }
                        }
                        tempListTrailobjects[i].gameObject.SetActive(false);
                        tempListTrailobjects.Remove(tempListTrailobjects[i]);
                        break;                    
                    }

                    isLookAtDestinationList[i] = RandomBoolValue(i);
                }
                yield return Timing.WaitForSeconds(0.01f);
            }
            NullPositions();
        }

        /// <summary>
        /// Вернуть молнии на свои позиции
        /// </summary>
        private void NullPositions()
        {
            foreach (Transform transformOfTrail in listTrailObjects)
                transformOfTrail.localPosition = Vector3.zero;
        }

        /// <summary>
        /// Меняем значение буля. 
        /// false - трэил движется в рандомном направлении.
        /// true - трэил движется к точке назначения.
        /// </summary>
        /// <param name="i"></param>
        private bool RandomBoolValue(int i)
        {
            return LibraryStaticFunctions.rnd.Next(0, 2) == 1 
                ? true : false;
        }
    }
}