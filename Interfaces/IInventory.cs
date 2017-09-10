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
		/// Сколько голд стоит итем
		/// </summary>
		int PriceGold { get; }

		/// <summary>
		/// Название итема
		/// </summary>
		string ItemName { get; }

		/// <summary>
		/// Описание итема
		/// </summary>
		string ItemTutorial { get; }

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
        /// Выполнение условий, приводящих к включению кнопки
        /// </summary>
        void EnableItem();

        /// <summary>
        /// Установить силу восстанавливаемого предмета в зависимости от его качества
        /// </summary>
        void SetItemStrenghtDependenceItemQuality();

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
        /// Время перезарядки умения
        /// </summary>
        int SecondsForTimer { get; set; }

        /// <summary>
        /// Стоимость умения в золоте
        /// </summary>
        long MoneyCost { get; set; }

        /// <summary>
        /// Стоимость умения в гемах
        /// </summary>
        long GemsCost { get; set; }

        /// <summary>
        /// Сколько опыта стоит скилл
        /// </summary>
        int PriceExp { get; }

		/// <summary>
		/// Название умения
		/// </summary>
		string SkillName { get; }

		/// <summary>
		/// Описание умения
		/// </summary>
		string SkillTutorial { get; }

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
        Lite,
        Medium,
        Strong
    }
}
