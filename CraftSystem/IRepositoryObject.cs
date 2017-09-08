namespace CraftSystem
{
    /// <summary>
    /// Интерфейс, который реализуют все элементы 
    /// репозиториев инвентаря и магазина
    /// </summary>
    public interface IRepositoryObject
    {
        /// <summary>
        /// Отключить выделение
        /// </summary>
        void HighlightingControl(bool flag);
    }
}
