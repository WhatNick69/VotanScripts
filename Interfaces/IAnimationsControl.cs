using AbstractBehaviour;

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
        StructStatesNames StructStatesNames { get; set; }

        void SetState(byte state, bool flag);

        void LowSpeedAnimation();

        void HighSpeedAnimation();

        float GetAnimatorSpeed();

        void SetSpeedAnimationByRunSpeed(float value);

        void DisableAllStates();
    }

    /// <summary>
    /// Интерфейс для реализации частного 
    /// управления анимациями игрока
    /// </summary>
    public interface IPlayerAnimations
    {

        void Start();
    }

    /// <summary>
    /// Интерфейс для реализации частного 
    /// управления анимациями врага
    /// </summary>
    public interface IEnemyAnimations
    {
        void Start();
    }
}
