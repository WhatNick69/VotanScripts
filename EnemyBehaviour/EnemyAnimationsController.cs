using AbstractBehaviour;
using VotanInterfaces;

namespace EnemyBehaviour
{
    /// <summary>
    /// Осуществляет контроль за анимациями врага
    /// </summary>
    public class EnemyAnimationsController
        : AbstactObjectAnimations, IEnemyAnimations
    {
        /// <summary>
        /// Инициализация
        /// </summary>
        public void Start()
        {
            StructStatesNames = new StructStatesNames("isRunning", "isFighting"
                , "isReAttack", "isDamage", "isDead");
        }

        /// <summary>
        /// Установить высокую скорость анимации врага
        /// </summary>
        public override void HighSpeedAnimation()
        {
            if (animatorOfObject.speed == 1) return;
            animatorOfObject.speed = 1f;
        }

        /// <summary>
        /// Установить низкую скорость анимации врага
        /// </summary>
        public override void LowSpeedAnimation()
        {
            animatorOfObject.speed = 0.2f;
        }

        /// <summary>
        /// Задаем значение состоянию анимации врагу-рыцарю.
        /// 0 - бег, 1 - битва, 2 - ущерб, 
        /// 3 - смерть
        /// </summary>
        /// <param name="state"></param>
        /// <param name="flag"></param>
        public override void SetState(byte state, bool flag)
        {
            base.SetState(state, flag);
        }
    }
}
