using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        #region Singleton
        private static GameManager _instance;
        private void Awake() 
        { 
            if (_instance != null) 
                Destroy(this);
            else 
                _instance = this; 
        }
    
        public static GameManager GetInstance() 
        { 
            return _instance; 
        }
        #endregion

        [SerializeField] private EconomyManager economyManager;
        [SerializeField] private FireManager fireManager;
        [SerializeField] private BuildingManager buildingManager;
        [SerializeField] private UIManager uiManager;
        [SerializeField] private DifficultySetting difficulty;
        [SerializeField] private int turnNumber = 1;

        public EconomyManager EconomyManager => economyManager;
        public FireManager FireManager => fireManager;
        public BuildingManager BuildingManager => buildingManager;
        public UIManager UIManager => uiManager;
        public int TurnNumber => turnNumber;
        public DifficultySetting Difficulty => difficulty;

        public delegate void NextTurnDelegate();
        public event NextTurnDelegate OnNextTurn;

        public void NextTurn()
        {
            economyManager.ReceiveIncome();
            OnNextTurn?.Invoke();
            if(!fireManager.SpreadFire())
                fireManager.StartRandomFire();
            turnNumber++;
        }
    }
}
