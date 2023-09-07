using UnityEngine;

namespace Map
{
    [CreateAssetMenu(fileName = "New TileType", menuName = "ProjectCustomer/TileType")]
    public class TileType : ScriptableObject
    {
        public bool IsBurnable;
        public bool IsWaterSource;
        public bool CanRecover;
        
        public int burnTime;
        public int recoveryTime;

        public GameObject gfxNeutral;
        public GameObject gfxBurning;
        public GameObject gfxRecovering;
    }
}