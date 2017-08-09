using AbstractBehaviour;
using MovementEffects;
using PlayerBehaviour;
using System.Collections.Generic;
using UnityEngine;
using VotanGameplay;
using VotanInterfaces;
using VotanLibraries;

namespace GameBehaviour
{
    /// <summary>
    /// Менеджер электрического эффекта
    /// </summary>
    class ElectricEffectManager
        : MonoBehaviour, IElectricEffect
    {
        #region Переменные
        private Transform[] listTrailObjects;
        private float radiusSearch;
        private float gemPower;
        private float damage;
        private IWeapon weapon;
        private int childrenCount;
        private AbstractEnemy abstractEnemyTarget; // цель для атаки
        #endregion

        /// <summary>
        /// Инициализация ссылки на рэндом
        /// </summary>
        private void Start()
        {
            listTrailObjects = new Transform[transform.childCount];

            for (int i = 0; i < transform.childCount; i++)
            {
                listTrailObjects[i] = transform.GetChild(i);
                listTrailObjects[i].gameObject.SetActive(false);
            }
        }

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
                listTrailObjects[i].GetComponent<TrailRenderer>().startColor =
                    weapon.TrailRenderer.startColor;
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
            float value = 0;
            for (int i = 0; i < childrenCount; i++)
            {
                listTrailObjects[i].gameObject.SetActive(flag);
                listTrailObjects[i].transform.localPosition = Vector3.zero;
                value = LibraryStaticFunctions.GetRangeValue(0.075f, 0.1f);
                listTrailObjects[i].GetComponent<TrailRenderer>().startWidth
                    = value;
                listTrailObjects[i].GetComponent<TrailRenderer>().endWidth
                    = LibraryStaticFunctions.GetRangeValue(value, 0.25f);
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

            if (gemPower < 0) return;

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
            if (childrenCount != 0)
            {
                abstractEnemyTarget = 
                    StaticStorageWithEnemies.GetClosestNonShockedEnemy(position, 10);
                if (abstractEnemyTarget == null) return;

                VisibleOfElements(true); // включаем объекты
                Timing.RunCoroutine(CoroutineForMoveTrails
                    (abstractEnemyTarget.transform));
            }
        }

        /// <summary>
        /// Вернуть молнии на свои позиции
        /// </summary>
        private void NullPositions()
        {
            foreach (Transform transformOfTrail in listTrailObjects)
            {
                transformOfTrail.gameObject.SetActive(false);
                transformOfTrail.localPosition = Vector3.zero;
            }
        }

        /// <summary>
        /// Меняем значение буля. 
        /// false - трэил движется в рандомном направлении.
        /// true - трэил движется к точке назначения.
        /// </summary>
        /// <param name="i"></param>
        private bool RandomBoolValue(int i)
        {
            return Random.Range(0, 2) == 1 ? true : false;
        }

        /// <summary>
        /// Корутина для зажигания электрического эффекта
        /// Безопасный цикл с ограничением в 100 итераций
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForMoveTrails(Transform destination)
        {
            int iterations = 0;
            bool flag = false;
            int needNumber = -1;
            List<Transform> tempListTrailobjects = new List<Transform>(listTrailObjects);
            bool[] isLookAtDestinationList = new bool[tempListTrailobjects.Count];
            while (iterations < 100)
            {
                if (tempListTrailobjects.Count == 0) break;
                for (int i = 0; i < tempListTrailobjects.Count; i++)
                {
                    if (this == null) yield break;
                    if (isLookAtDestinationList[i])
                    {
                        tempListTrailobjects[i].LookAt(destination.position);
                        tempListTrailobjects[i].Translate(tempListTrailobjects[i].forward
                            * (0.5f + Random.Range(0,1f)), Space.World);
                    }
                    else
                    {
                        tempListTrailobjects[i].rotation = Quaternion.Euler
                            (Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
                        tempListTrailobjects[i].Translate(tempListTrailobjects[i].forward
                            * (0.5f + Random.Range(0,1f)));
                    }

                    if (Vector3.Distance(tempListTrailobjects[i].
                        position, destination.position) <= 0.5f)
                    {
                        // встретили цель. ударили её. передали эстафету.
                        if (!flag)
                        {
                            flag = true;
                            needNumber = i;
                            if (gemPower >= 1)
                            {
                                ChangeValues();
                                if (abstractEnemyTarget != null)
                                    abstractEnemyTarget.EnemyConditions.
                                    GetDamageElectricity(damage * 0.75f, gemPower,weapon);
                            }
                        }
                        tempListTrailobjects[i].gameObject.SetActive(false);
                        tempListTrailobjects.Remove(tempListTrailobjects[i]);
                        break;
                    }

                    isLookAtDestinationList[i] = RandomBoolValue(i);
                }
                yield return Timing.WaitForSeconds(0.01f);
                iterations++;
            }
            //Debug.Log(iterations);
            NullPositions();
        }
    }
}
