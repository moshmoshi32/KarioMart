using UnityEngine;

namespace Pickups
{
    [CreateAssetMenu(fileName = "New Item Effect", menuName = "Item/New Item Effect Data", order = 1)]
    public class ItemEffectSO : ScriptableObject
    {
        public ItemEffect itemEffect;
        public float speedBoost;
        public float timerAmount;
        
            
        public void ActivateEffect()
        {
            
        }
    }


    public enum ItemEffect
    {
        NONE,
        SpeedBoost,
    }
}