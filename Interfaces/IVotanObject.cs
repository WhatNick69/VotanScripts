using EnemyBehaviour;
using PlayerBehaviour;
using UnityEngine;

/*
 * Интерфейсы данного скрипта реализуют 
 * общую логику поведения
 * таких объектов, как общий объект, что 
 * является родителей для
 * таких потомков, как игрок, либо враг.
 */
namespace VotanInterfaces
{
    /// <summary>
    /// Интерфейс для реализации общего 
    /// поведения объекта
    /// </summary>
    public interface IVotanObjectBehaviour
    {

    }

    /// <summary>
    /// Интерфейс для реализации поведения 
    /// игрока
    /// </summary>
    public interface IPlayerBehaviour
    {
         PlayerController PlayerController { get; set; }

        PlayerConditions PlayerConditions { get; set; }

        PlayerFight PlayerFight { get; set; }

        PlayerAttack PlayerAttack { get; set; }

        PlayerCollision PlayerCollision { get; set; }

        GameObject PlayerCamera { get; set; }

        PlayerArmory PlayerArmory { get; set; }

        PlayerWeapon PlayerWeapon { get; set; }

        PlayerCameraSmooth PlayerCameraSmooth { get; set; }

        PlayerBloodInterfaceEffect PlayerBloodInterfaceEffect { get; set; }

        PlayerScore PlayerScore { get; set; }

        PlayerAnimationsController PlayerAnimationsController { get; set; }

        Transform PlayerParent { get; set; }

        Transform PlayerObject { get; set; }

        Transform PlayerModel { get; set; }
    }

    /// <summary>
    /// Интерфейс для реализации поведения 
    /// врага
    /// </summary>
    public interface IEnemyBehaviour
    {
        /// <summary>
        /// Свойства для компоненты "Атака"
        /// </summary>
        EnemyAttack EnemyAttack { get; set; }

        /// <summary>
        /// Свойство для компоненты "Аниматора"
        /// </summary>
        EnemyAnimationsController EnemyAnimationsController { get; set; }

        /// <summary>
        /// Свойство для компоненты "Кондиция"
        /// </summary>
        EnemyConditions EnemyConditions { get; set; }

        /// <summary>
        /// Свойство для компоненты "Выбор игрока"
        /// </summary>
        EnemyOpponentChoiser EnemyOpponentChoiser { get; set; }

        /// <summary>
        /// Свойство для компоненты "Движение врага"
        /// </summary>
        IAIMoving EnemyMove { get; set; }

        /// <summary>
        /// Ледяной эффект
        /// </summary>
        IIceEffect IceEffect { get; set; }

        /// <summary>
        /// Огненный эффект
        /// </summary>
        IFireEffect FireEffect { get; set; }

        /// <summary>
        /// Эектрический эффект
        /// </summary>
        IElectricEffect ElectricEffect { get; set; }

        /// <summary>
        /// Земляной эффект
        /// </summary>
        IPhysicEffect Physicffect { get; set; }

        /// <summary>
        /// Эффект начисления бонуса за убийство игроку
        /// </summary>
        IScoreAddingEffect ScoreAddingEffect { get; set; }

        /// <summary>
        /// Вернуть позицию
        /// </summary>
        /// <param name="child"></param>
        /// <returns></returns>
        Vector3 ReturnPosition(int child);
    }
}

