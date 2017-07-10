using AbstractBehaviour;
using System.Collections.Generic;

/*
 * Интерфейсы данного скрипта реализуют 
 * общую логику управления
 * анимациями общего объекта и таких
 * частных объектов, как враг и игрок
 */
namespace VotanInterfaces
{
    /// <summary>
    /// Интерфейс для реализации общего 
    /// управления анимациями
    /// </summary>
    public interface IVotanObjectAnimations
    {
        /// <summary>
        /// Структура для быстрого доступа к анимациям
        /// </summary>
        StructStatesNames StructStatesNames { get; set; }

        /// <summary>
        /// Установить значение для состояния
        /// </summary>
        /// <param name="state"></param>
        /// <param name="flag"></param>
        void SetState(byte state, bool flag);

        /// <summary>
        /// Установить минимальную скорость анимации
        /// </summary>
        void LowSpeedAnimation();

        /// <summary>
        /// Установить максимальную скорость анимации
        /// </summary>
        void HighSpeedAnimation();

        /// <summary>
        /// Получить скорость анимации
        /// </summary>
        /// <returns></returns>
        float GetAnimatorSpeed();

        /// <summary>
        /// Проверить, все ли состояния отключены
        /// </summary>
        /// <returns></returns>
        bool IsFalseAllStates();

        /// <summary>
        /// Задать скорость анимации
        /// </summary>
        /// <param name="value"></param>
        void SetSpeedAnimationByRunSpeed(float value);

        /// <summary>
        /// Отключить все состояния анимации
        /// </summary>
        void DisableAllStates();

        /// <summary>
        /// Нормализация Y координаты после смерти
        /// </summary>
        void PlayDeadNormalizeCoroutine();

        /// <summary>
        /// Корутина для нормализации позиции Y после смерти
        /// </summary>
        /// <returns></returns>
        IEnumerator<float> CoroutineDeadYNormalized();

    }

    /// <summary>
    /// Интерфейс для реализации частного 
    /// управления анимациями игрока
    /// </summary>
    public interface IPlayerAnimations
    {

    }

    /// <summary>
    /// Интерфейс для реализации частного 
    /// управления анимациями врага
    /// </summary>
    public interface IEnemyAnimations
    {

    }
}
