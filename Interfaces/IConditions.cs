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
        /// Установить значение (новое) жизней моба/игрока
        /// </summary>
        void SetHealthParameter(float health);

        /// <summary>
        /// Жив ли объект
        /// </summary>
        bool IsAlive { get; set; }

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
        int GetDamage(float damageValue);

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
    /// Данный интерфейс позволяет поворачивать 
    /// нижний интерфейс объекты, когда тот поднимается по лестнице
    /// </summary>
    public interface IObjectFitBat
    {
        /// <summary>
        /// Булева переменная, что отвечает за изменение 
        /// трансформа нижнего интерфейса объекта
        /// </summary>
        bool IsDownInterfaceTransformHasBeenChanged { get; set; }

        /// <summary>
        /// Поворачивает нижний интерфейс врага
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="angle"></param>
        void RotateConditionBar(bool flag, float angle=0);

        /// <summary>
        /// Отключить/включить объект-интерфейс под объектом
        /// </summary>
        void ActiveDownInterface(bool flag);
    }

    /// <summary>
    /// Интерфейс для реализации контроля над состояниями врага
    /// </summary>
    public interface IEnemyConditions
    {
        /// <summary>
        /// Ресрат состояний врага
        /// </summary>
        void RestartEnemyConditions();

        /// <summary>
        /// Инициализация
        /// </summary>
        void Start();

        /// <summary>
        /// Заморожен ли враг
        /// </summary>
        bool IsFrozen { get; set; }

        /// <summary>
        /// Шокирован ли электричеством враг
        /// </summary>
        bool IsShocked { get; set; }

        /// <summary>
        /// Жарится ли от огня враг
        /// </summary>
        bool IsBurned { get; set; }

        /// <summary>
        /// Получить урон от персонажа по стихии
        /// </summary>
        /// <param name="dmg"></param>
        /// <param name="typeOfDamage"></param>
        /// <returns></returns>
        float GetDamageWithResistance(float dmg, float gemPower, 
            IWeapon weapon);

        /// <summary>
        /// Получить ледяной удар и стать медленнее
        /// </summary>
        void RunCoroutineForFrozenDamage(float damage,IWeapon weapon);

        /// <summary>
        /// Корутина на ледяной удар
        /// </summary>
        /// <returns></returns>
        IEnumerator<float> CoroutineForFrozenDamage(float damage,IWeapon weapon);

        /// <summary>
        /// Корутина на физический урон
        /// </summary>
        /// <param name="weapon"></param>
        /// <returns></returns>
        IEnumerator<float> CoroutineForPhysicDamage(IWeapon weapon);

        /// <summary>
        /// Запустить корутину физичесого эффекта
        /// </summary>
        /// <param name="weapon"></param>
        void RunCoroutineForPhysicDamage(IWeapon weapon);

        /// <summary>
        /// Поворачивать бар в сторону камеры
        /// </summary>
        IEnumerator<float> CoroutineBarBillboard();

        /// <summary>
        /// Получить урон от персонажа
        /// </summary>
        /// <param name="dmg"></param>
        /// <param name="dmgType"></param>
        /// <param name="weapon"></param>
        bool GetDamage(float dmg, float gemPower, IWeapon weapon,bool isSuperAttack);

        /// <summary>
        /// Получить урон в результате молнии
        /// </summary>
        /// <param name="dmg"></param>
        /// <param name="gemPower"></param>
        /// <param name="weapon"></param>
        void GetDamageElectricity(float dmg, float gemPower
            , IWeapon weapon);

        /// <summary>
        /// Корутина на получение урона от персонажа.
        /// Работает как для ближнего, так и для дальнего боя
        /// </summary>
        /// <param name="isLongAttack"></param>
        /// <returns></returns>
        IEnumerator<float> CoroutineForGetDamage(bool isLongAttack=false);

        /// <summary>
        /// Найти камеру вотчера
        /// </summary>
        void FindCameraInScene();

        /// <summary>
        /// Корутина электрического удара по врагу
        /// </summary>
        IEnumerator<float> CoroutineForElectricDamage(float damage, 
            float gemPower, IWeapon weapon);

        /// <summary>
        /// Запуск корутины для шокирования и самого эффекта следующему врагу
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="gemPower"></param>
        /// <param name="weapon"></param>
        void RunCoroutineForGetElectricDamage(float damage, float gemPower, IWeapon weapon);

        /// <summary>
        /// Запустить огненный эффект
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="weapon"></param>
        void RunFireDamage(float damage, IWeapon weapon);
    }
}
