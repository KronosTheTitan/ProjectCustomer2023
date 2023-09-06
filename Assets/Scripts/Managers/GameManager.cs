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

        public EconomyManager EconomyManager => economyManager;
        public FireManager FireManager => fireManager;
        public BuildingManager BuildingManager => buildingManager;
        public UIManager UIManager => uiManager;
        
        public DifficultySetting Difficulty => difficulty;

        public void NextTurn()
        {
            economyManager.ReceiveIncome();
            fireManager.SpreadFire();
        }
    }
}
