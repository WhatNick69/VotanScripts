using PlayerBehaviour;
using UnityEngine;

namespace VotanInterfaces
{
    /// <summary>
    /// Интерфейс для реализации ледяного эффекта
    /// </summary>
    public interface IIceEffect
    {
        /// <summary>
        /// Зажечь ледяной эффект
        /// </summary>
        /// <param name="time"></param>
        void EventEffect(float time);
    }

    /// <summary>
    /// Интерфейс для реализации электрического эффекта
    /// </summary>
    public interface IElectricEffect
    {
        /// <summary>
        /// Зажечь электрический эффект
        /// </summary>
        /// <param name="time"></param>
        void EventEffect(float damage, float gemPower, IWeapon weapon);
    }

    /// <summary>
    /// Интерфейс для реализации огненного эффекта
    /// </summary>
    public interface IFireEffect
    {
        /// <summary>
        /// Зажечь огненный эффект
        /// </summary>
        /// <param name="time"></param>
        void EventEffect(float time, Transform position);
    }
}
