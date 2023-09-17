using UnityEngine;
using UnityEngine.Serialization;

namespace Map
{
    /// <summary>
    /// Represents a type of tile in the game world.
    /// </summary>
    [CreateAssetMenu(fileName = "New TileType", menuName = "ProjectCustomer/TileType")]
    public class TileType : ScriptableObject
    {
        /// <summary>
        /// Indicates whether this tile can be burned.
        /// </summary>
        [FormerlySerializedAs("IsBurnable")] public bool isBurnable;

        /// <summary>
        /// Indicates whether this tile is a water source.
        /// </summary>
        [FormerlySerializedAs("IsWaterSource")] public bool isWaterSource;

        /// <summary>
        /// Indicates whether this tile can recover from being burned.
        /// </summary>
        [FormerlySerializedAs("CanRecover")] public bool canRecover;

        /// <summary>
        /// The time it takes for this tile to burn.
        /// </summary>
        public int burnTime;

        /// <summary>
        /// 
        /// </summary>
        public int revenue;

        /// <summary>
        /// The time it takes for this tile to recover from being burned.
        /// </summary>
        public int recoveryTime;

        /// <summary>
        /// The GameObject representing the neutral state of this tile type.
        /// </summary>
        public GameObject gfxNeutral;

        /// <summary>
        /// The GameObject representing the burning state of this tile type.
        /// </summary>
        public GameObject gfxBurning;

        /// <summary>
        /// The GameObject representing the recovering state of this tile type.
        /// </summary>
        public GameObject gfxRecovering;
    }
}
