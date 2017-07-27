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
        /// Инициализация
        /// </summary>
        void Start();

        /// <summary>
        /// Зажечь ледяной эффект
        /// </summary>
        /// <param name="time"></param>
        void EventEffect(float damage,float time, IWeapon weapon);
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
        void EventEffect(float damage,IWeapon weapon);
    }

    /// <summary>
    /// Интерфейс для реализации земляного эффекта
    /// </summary>
    public interface IPhysicEffect
    {
        /// <summary>
        /// Зажечь земляной эффект
        /// </summary>
        /// <param name="time"></param>
        void EventEffect(IWeapon weapon);

        /// <summary>
        /// Земляной эффект на исключительное отталкивание врага 
        /// </summary>
        /// <param name="weapon"></param>
        void EventEffectWithoutDefenceBonus(IWeapon weapon);
    }

    /// <summary>
    /// Интерфейс для реализации эффекта начисления бонуса
    /// от врага к игроку
    /// </summary>
    public interface IScoreAddingEffect
    {
        /// <summary>
        /// Бонус за убийство противника
        /// </summary>
        int ScoreBonus { get; }

        /// <summary>
        /// Трэил-бонус
        /// </summary>
        Transform TrailScore { get; set; }

        /// <summary>
        /// Запустить эффект преследующих вспышек к игроку.
        /// Достигая игрока - вспышки исчезают, а игроку начисляется бонус
        /// </summary>
        /// <param name="weapon"></param>
        /// <param name="scoreBonus"></param>
        void EventEffect(IWeapon weapon);
    }
}
