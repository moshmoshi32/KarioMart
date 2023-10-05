using UnityEngine;

namespace Pickups
{
    [CreateAssetMenu(fileName = "New Item Effect", menuName = "Item/New Item Effect Data", order = 1)]
    public class ItemEffectSO : ScriptableObject
    {
        public ItemEffect itemEffect;
        public float speedBoost;
        public float timerAmount;
        
            
        public void ActivateEffect(CarHandler player)
        {
            switch (itemEffect)
            {
                case ItemEffect.SpeedBoost:
                    ActivateSpeedBoost(player);
                    break;
                default:
                    Debug.Log("Effect doesn't exist");
                    break;
            }
        }

        private void ActivateSpeedBoost(CarHandler car)
        {
            car.ChangeMaxSpeed(speedBoost);
            GameManager.Instance.TimerManager.CreateTimer(timerAmount,
                speedBoost,
                ResetSpeedBoost,
                car);
        }

        private void ResetSpeedBoost(float negateSpeedBoost, CarHandler car)
        {
            car.ChangeMaxSpeed(-negateSpeedBoost);
            Debug.Log($"Car Number{car.PlayerInputManager.GetPlayerIndex()} lost its boost");
        }
    }


    public enum ItemEffect
    {
        NONE,
        SpeedBoost,
    }
}