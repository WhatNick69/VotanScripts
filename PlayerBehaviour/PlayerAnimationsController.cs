using AbstractBehaviour;
using UnityEngine;

namespace PlayerBehaviour
{
    /// <summary>
    /// Реализует контроль за анимацией персонажа
    /// </summary>
    public class PlayerAnimationsController 
        : AbstactObjectAnimations
    {
        private PlayerFight playerFight;
        /// <summary>
        /// Инициализация
        /// </summary>
        private void Start()
        {
            structStatesNames = new StructStatesNames("isRunning",
                "isFighting","isDefensing","isLongAttack","isDamage",
                "isDead");
        }

        /// <summary>
        /// Задаем значение состоянию анимации персонажа.
        /// 0 - бег, 1 - кручение, 2 - защита, 3 - рывок, 4 - урон, 5 - смерть
        /// </summary>
        /// <param name="state"></param>
        /// <param name="flag"></param>
        public override void SetState(byte state,bool flag)
        {
            base.SetState(state, flag);
        }

        /// <summary>
        /// Установить низкую скорость анимации
        /// </summary>
        public override void LowSpeedAnimation()
        {
            if (animatorOfObject.speed == 0.2f) return;
            else if ((!PlayerFight.IsFighting 
                || PlayerFight.IsDefensing) && !PlayerFight.IsSpining)
            {
                Debug.Log("НАС ВЫЗЫВАЮТ!");
                animatorOfObject.speed = 0.2f;
            }
        }

        /// <summary>
        /// Установить высокую скорость анимации
        /// </summary>
        public override void HighSpeedAnimation()
        {
            if (animatorOfObject.speed == 1) return;
            animatorOfObject.speed = 1f;
        }
    }
}
