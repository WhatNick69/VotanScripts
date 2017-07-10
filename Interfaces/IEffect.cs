namespace VotanInterfaces
{
    /// <summary>
    /// Интерфейс для реализации эффекта
    /// </summary>
    public interface IEffect
    {
        /// <summary>
        /// Зажечь эффект
        /// </summary>
        /// <param name="time"></param>
        void FireEventEffect(float time);
    }
}
