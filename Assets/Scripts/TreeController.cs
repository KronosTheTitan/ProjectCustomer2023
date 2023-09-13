using UnityEngine;
using static Map.HexTile;

public class TreeController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject burningTreeModel;
    [SerializeField] private GameObject treeModel;

    private void Start()
    {
        // Initially, deactivate the burning tree model
        burningTreeModel.SetActive(false);
        treeModel.SetActive(true);
    }

    private void StartBurningAnimationWithDelay()
    {
        // Activate the burning tree model and play its animation
        burningTreeModel.SetActive(true);
        treeModel.SetActive(false);
        // Transition to the Burning state
        animator.SetTrigger("StartBurning");
    }

    /// <summary>
    /// This function is called when the tile state changes
    /// </summary>
    /// <param name="newState"></param>
    public void OnTileStateChanged(TileState newState)
    {
        Debug.Log(newState.ToString());
        switch (newState)
        {
            case TileState.Neutral:
                // Deactivate the burning tree model
                burningTreeModel.SetActive(false);
                treeModel.SetActive(true);
                // Transition to the Neutral state
                animator.SetTrigger("StartNeutral");
                break;
            case TileState.Burning:
                // Start the animation with a random delay
                float randomDelay = Random.Range(1.0f, 3.0f); // Adjust the range as needed
                Invoke("StartBurningAnimationWithDelay", randomDelay);
                break;
            case TileState.Recovering:
                // Deactivate the burning tree model
                burningTreeModel.SetActive(false);
                treeModel.SetActive(true);
                // Transition to the Recovering state
                animator.SetTrigger("StartRecovering");
                break;
            case TileState.Empty:
                // Handle the Empty state if needed
                break;
        }
    }
}
