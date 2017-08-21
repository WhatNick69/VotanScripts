using PlayerBehaviour;
using UnityEngine.UI;
using System.Collections.Generic;

namespace VotanInterfaces
{
    /// <summary>
    /// Интерфейс для описания логики предмета
    /// </summary>
    public interface IItem
    {
        /// <summary>
        /// Тип предмета (здоровье, сила, скорость)
        /// </summary>
        ItemType ItemType { get; }

        /// <summary>
        /// Качество предмета
        /// </summary>
        ItemQuality ItemQuality { get; }

        /// <summary>
        /// Время перезарядки предмета
        /// </summary>
        int SecondsForTimer { get; set; }

        /// <summary>
        /// Величина силы предмета
        /// </summary>
        float ItemStrenght { get; set; }

        /// <summary>
        /// Выполнение условий, приводящих к включению кнопки
        /// </summary>
        void EnableItem();

        /// <summary>
        /// Нажать по предмету
        /// </summary>
        void OnClickFireItem();

        /// <summary>
        /// Номер позиции предмета в инвентаре
        /// </summary>
        int ItemNumberPosition { get; set; }

        /// <summary>
        /// Экземпляр хранителя компонентов игрока
        /// </summary>
        PlayerComponentsControl PlayerComponentsControlInstance { get; set; }

        /// <summary>
        /// Инициализация предмета
        /// </summary>
        /// <param name="itemImage">Изображение предмета</param>
        /// <param name="bonusValue">Бонусная величина</param>
        /// <param name="itemCount">Количество предметов</param>
        /// <param name="secondsForTimer">Время перезарядки</param>
        void InitialisationItem(Image itemImage, float bonusValue, int itemCount, int secondsForTimer);

        /// <summary>
        /// Инициализация
        /// </summary>
        void Starter(int number);

        /// <summary>
        /// Если ли доступный предмет данного класса в экземпляре
        /// </summary>
        bool IsContainsItem { get; }

        /// <summary>
        /// Количество предметов данного класса
        /// </summary>
        int ItemCount { get; set; }

        /// <summary>
        /// Изображение предмета
        /// </summary>
        Image ItemImage { get; }

        /// <summary>
        /// Выполнить событие, которое реализует класс
        /// </summary>
        /// <param name="playerComponentsControlInstance"></param>
        void FireEventItem();

        /// <summary>
        /// Таймер на перезарядку
        /// </summary>
        /// <returns></returns>
        IEnumerator<float> CoroutineTimer();
    }

    /// <summary>
    /// Интерфейс для описания логики умения
    /// </summary>
    public interface ISkill
    {
        /// <summary>
        /// Можно ли запустить умение
        /// </summary>
        bool IsMayToFire { get; set; }

        /// <summary>
        /// Нажать по предмету
        /// </summary>
        void OnClickFireSkill();

        /// <summary>
        /// Время перезарядки умения
        /// </summary>
        /// <returns></returns>
        IEnumerator<float> CoroutineTimer();

        /// <summary>
        /// Зажечь умение
        /// </summary>
        void FireEventSkill();

        /// <summary>
        /// Номер позиции умения в инвентаре
        /// </summary>
        int ItemNumberPosition { get; set; }

        /// <summary>
        /// Экземпляр хранителя компонентов игрока
        /// </summary>
        PlayerComponentsControl PlayerComponentsControlInstance { get; set; }

        /// <summary>
        /// Инициализация умения
        /// </summary>
        /// <param name="skillImage">Изображение умения</param>
        /// <param name="secondsForTimer">Время перезарядки</param>
        void InitialisationSkill(Image skillImage,int secondsForTimer);

        /// <summary>
        /// Инициализация. Запуск.
        /// </summary>
        void Starter(int number);

        /// <summary>
        /// Изображение умения
        /// </summary>
        Image SkillImage { get; }
    }

    /// <summary>
    /// Перечислитель возможных предметов для улучшения
    /// </summary>
    public enum ItemType
    {
        HealthItem,
        SpeedItem,
        PowerItem
    }

    /// <summary>
    /// Качество предмета
    /// </summary>
    public enum ItemQuality
    {
        VeryLite = 0,
        Lite = 1,
        Medium = 2,
        UpMedium = 3,
        Strong = 4,
        UpStrong = 5,
        Ultimate = 6
    }
}
