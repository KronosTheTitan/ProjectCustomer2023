using UnityEngine;

[CreateAssetMenu(fileName = "New Difficulty", menuName = "ProjectCustomer/Difficulty")]
public class DifficultySetting : ScriptableObject
{
    [SerializeField] private int campsiteIncome;
    public int CampsiteIncome => campsiteIncome;
}