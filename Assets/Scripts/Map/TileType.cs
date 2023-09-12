using UnityEngine;

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
        public bool IsBurnable;

        /// <summary>
        /// Indicates whether this tile is a water source.
        /// </summary>
        public bool IsWaterSource;

        /// <summary>
        /// Indicates whether this tile can recover from being burned.
        /// </summary>
        public bool CanRecover;

        /// <summary>
        /// The time it takes for this tile to burn.
        /// </summary>
        public int burnTime;

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
