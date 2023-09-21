using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    /// <summary>
    /// Manages the game logic and contains references to other manager classes.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        #region Singleton
        private static GameManager _instance;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            if (_instance != null)
                Destroy(this.gameObject); // Destroy this object if another instance exists.
            else
                _instance = this; // Set this as the singleton instance.
            SceneManager.sceneUnloaded += MemoryLeakPrevention;
        }

        /// <summary>
        /// Gets the singleton instance of the GameManager.
        /// </summary>
        /// <returns>The GameManager instance.</returns>
        public static GameManager GetInstance()
        {
            return _instance;
        }
        #endregion

        [SerializeField] private EconomyManager economyManager;
        [SerializeField] private TileManager tileManager;
        [SerializeField] private BuildingManager buildingManager;
        [SerializeField] private UIManager uiManager;
        [SerializeField] private int turnNumber = 1;
        [SerializeField] private int gracePeriod;
        [SerializeField] private int randomFireChance;
        [SerializeField] private int firePenalty;

        /// <summary>
        /// Gets the EconomyManager component.
        /// </summary>
        public EconomyManager EconomyManager => economyManager;

        /// <summary>
        /// Gets the FireManager component.
        /// </summary>
        public TileManager TileManager => tileManager;

        /// <summary>
        /// Gets the BuildingManager component.
        /// </summary>
        public BuildingManager BuildingManager => buildingManager;

        /// <summary>
        /// Gets the UIManager component.
        /// </summary>
        public UIManager UIManager => uiManager;

        /// <summary>
        /// Gets the current turn number.
        /// </summary>
        public int TurnNumber => turnNumber;

        public int GracePeriod => gracePeriod;
        public int RandomFireChance => randomFireChance;
        public int FirePenalty => firePenalty;

        /// <summary>
        /// Delegate for the next turn event.
        /// </summary>
        public delegate void GameManagerDelegate();

        /// <summary>
        /// Event that is triggered when the game advances to the next turn.
        /// </summary>
        public event GameManagerDelegate OnNextTurn;
        public event GameManagerDelegate OnGameOver;

        /// <summary>
        /// Advances the game to the next turn, handling income, fire spread, and turn count.
        /// </summary>
        public async void NextTurn()
        {
            economyManager.ReceiveIncome(); // Calculate and receive income.
            OnNextTurn?.Invoke(); // Trigger the next turn event.

            // Attempt to spread fire, and start random fire if spreading fails.
            if(!tileManager.SpreadFire())
                tileManager.StartRandomFire();
            
            turnNumber++; // Increment the turn count.
        }

        public void GameOver()
        {
            OnGameOver?.Invoke();
        }

        private void MemoryLeakPrevention(Scene scene)
        {
            _instance = null;
        }
    }
}