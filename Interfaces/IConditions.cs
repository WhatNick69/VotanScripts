using PlayerBehaviour;
using System.Collections.Generic;
using UnityEngine;

/*
 * Интерфейсы данного скрипта реализуют логику контроля над
 * такими видами состояний, как здоровье, мана и ярость таких
 * объектов, как игрок и враг. 
 */
namespace VotanInterfaces
{
    /// <summary>
    /// Интерфейс для реализации общего контроля над состояниями объекта
    /// </summary>
    public interface IVotanObjectConditions
    {
        /// <summary>
        /// Состояние смерти объекта
        /// </summary>
        /// <returns></returns>
        IEnumerator<float> DieState();

        /// <summary>
        /// Уничтожить объект
        /// </summary>
        void DestroyObject();

        /// <summary>
        /// Обновить бар здоровья объекта
        /// </summary>
        void RefreshHealthCircle();

        /// <summary>
        /// Реалтайм-игровой интерфейс объекта
        /// </summary>
        RectTransform MainBarCanvas { get; set; }
    }

    /// <summary>
    /// Интерфейс для реализации контроля над состояниями игрока
    /// </summary>
    public interface IPlayerConditions
    {
        /// <summary>
        /// Инициализация
        /// </summary>
        void Start();

        /// <summary>
        /// Обновить бар ярости персонажа
        /// </summary>
        void RefreshRingRage();

        /// <summary>
        /// Запуск корутины на реген бара ярости персонажа
        /// </summary>
        void StartRageCoroutineRegen();

        /// <summary>
        /// Запуск анимации бара ярости персонажа
        /// </summary>
        void StartRingbarAnimation();

        /// <summary>
        /// Корутина на реген ярости персонажа
        /// </summary>
        /// <returns></returns>
        IEnumerator<float> CoroutineForRage();

        /// <summary>
        /// Получить урон от врага
        /// </summary>
        /// <param name="damageValue"></param>
        void GetDamage(float damageValue);

        /// <summary>
        /// запуск корутины на возможность 
        /// получение урона от врага
        /// </summary>
        void CoroutineForIsMayGetDamage();

        /// <summary>
        /// Корутина на возможность получения урона от врага
        /// </summary>
        /// <returns></returns>
        IEnumerator<float> MayToGetDamage();
    }

    /// <summary>
    /// Интерфейс для реализации контроля над состояниями врага
    /// </summary>
    public interface IEnemyConditions
    {
        /// <summary>
        /// Жив ли объект
        /// </summary>
        bool IsAlive { get; set; }

        /// <summary>
        /// Заморожен ли игрок
        /// </summary>
        bool IsFrozen { get; set; }

        /// <summary>
        /// Получить урон от персонажа по стихии
        /// </summary>
        /// <param name="dmg"></param>
        /// <param name="typeOfDamage"></param>
        /// <returns></returns>
        float GetDamageWithResistance(float dmg, DamageType typeOfDamage);

        /// <summary>
        /// Получать огненный урон в течении некоторого времени
        /// </summary>
        /// <param name="damage"></param>
        void RunCoroutineForGetFireDamage(float damage);

        /// <summary>
        /// Получить ледяной удар и стать медленнее
        /// </summary>
        void RunCoroutineForFrozenDamage();

        /// <summary>
        /// Корутина на огненный удар
        /// </summary>
        /// <param name="damage"></param>
        /// <returns></returns>
        IEnumerator<float> CoroutineForFireDamage(float damage);

        /// <summary>
        /// Корутина на ледяной удар
        /// </summary>
        /// <returns></returns>
        IEnumerator<float> CoroutineForFrozenDamage();

        /// <summary>
        /// Частотное обновление
        /// </summary>
        void FixedUpdate();

        /// <summary>
        /// Поворачивать бар в сторону камеры
        /// </summary>
        void BarBillboard();

        /// <summary>
        /// Вернуть здоровье объекта
        /// </summary>
        /// <returns></returns>
        float ReturnHealth();

        /// <summary>
        /// Получить урон от персонажа
        /// </summary>
        /// <param name="dmg"></param>
        /// <param name="dmgType"></param>
        /// <param name="weapon"></param>
        void GetDamage(float dmg, DamageType dmgType, PlayerWeapon weapon);

        /// <summary>
        /// Корутина на полечение урона от персонажа
        /// </summary>
        /// <returns></returns>
        IEnumerator<float> CoroutineForGetDamage();

        /// <summary>
        /// Найти камеру вотчера
        /// </summary>
        void FindCameraInScene();
    }
}
