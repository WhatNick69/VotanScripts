using System.Collections.Generic;
using UnityEngine;

namespace VotanInterfaces
{
    /// <summary>
    /// Интерфейс, который описывает поведение боевой единицы из амуниции врага
    /// , будь то стрела, болт, пуля и многое другое
    /// </summary>
    public interface IAmmunation
    {
        /// <summary>
        /// Включить/выключить трэил снаряда
        /// </summary>
        /// <param name="flag"></param>
        void ActiveForTrailRender(bool flag);

        /// <summary>
        /// Готов ли снаряд к повторному использованию
        /// </summary>
        bool IsRestarted { get; set;}

        /// <summary>
        /// Достиг ли объект цели?
        /// </summary>
        bool IsDestinationed { get; set; }

        /// <summary>
        /// Цель для объекта
        /// </summary>
        Transform PlayerModel { get; set; }

        /// <summary>
        /// Начальный родитель
        /// </summary>
        Transform StartParent { get; set; }

        /// <summary>
        /// Инициализация
        /// </summary>
        void InitialisationAmmunationElement();

        /// <summary>
        /// Запустить движение объекта
        /// </summary>
        /// <param name="PlayerComponentsControl"></param>
        /// <param name="moveSpeed"></param>
        /// <param name="timeToRestart"></param>
        void FireAmmoObject(IPlayerBehaviour PlayerComponentsControl,
            float dmgValue, float moveSpeed, float timeToRestart);

        /// <summary>
        /// Рестарт единицы аммуниции
        /// </summary>
        void RestartAmmo();

        /// <summary>
        /// Корутина для движения объекта
        /// </summary>
        /// <returns></returns>
        IEnumerator<float> CoroutineForMovingAmmoObject();

        /// <summary>
        /// Корутина для проверки на достижение цели объектом
        /// </summary>
        /// <returns></returns>
        IEnumerator<float> CoroutineForCheckInPlayerPenetration();

        /// <summary>
        /// Корутина на перезапуск снаряда
        /// </summary>
        /// <returns></returns>
        IEnumerator<float> CoroutineForRestartTimer();
    }
}
