using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    /// <summary>
    /// Controls the behavior of a tree object, including animations and states.
    /// </summary>
    public class TreeController : MonoBehaviour
    {
        [SerializeField] private Animator burningAnimator;
        [SerializeField] private Animator neutralAnimator;
        [SerializeField] private Animator recoveringAnimator;

        [SerializeField] private GameObject burningTreeModel;
        [SerializeField] private GameObject neutralTreeModel;
        [SerializeField] private GameObject recoveringTreeModel;

        [SerializeField] private float minimumAnimationDelay = 0.1f;
        [SerializeField] private float maximumAnimationDelay = 3.0f;

        private Dictionary<TileState, (GameObject model, Animator animator)> _stateToModelAndAnimator;
        private TileState _oldState;
        private float _previousSpeed;

        private void Start()
        {
            _stateToModelAndAnimator = new Dictionary<TileState, (GameObject, Animator)>
            {
                { TileState.Neutral, (neutralTreeModel, neutralAnimator) },
                { TileState.Burning, (burningTreeModel, burningAnimator) },
                { TileState.Recovering, (recoveringTreeModel, recoveringAnimator) }
            };

            SwitchActiveModel(TileState.Neutral);
        }

        private void ResetAnimation(TileState state)
        {
            _previousSpeed = _stateToModelAndAnimator[state].animator.speed;
            _stateToModelAndAnimator[state].animator.Play($"{state}", -1, 0);
            _stateToModelAndAnimator[state].animator.speed = 0;
        }

        private void StartAnimation(TileState state)
        {
            _stateToModelAndAnimator[state].animator.speed = 1;
        }

        private void StartBurning() => StartAnimation(TileState.Burning);
        private void StartNeutral() => StartAnimation(TileState.Neutral);
        private void StartRecovering() => StartAnimation(TileState.Recovering);

        /// <summary>
        /// This function is called when the tile state changes.
        /// It triggers the appropriate animation based on the new state.
        /// </summary>
        /// <param name="newState">The new state of the tile.</param>
        public void OnTileStateChanged(TileState newState)
        {
            if (newState == _oldState)
                return;

            float randomDelay = Random.Range(minimumAnimationDelay, maximumAnimationDelay);

            SwitchActiveModel(newState);
            if (_stateToModelAndAnimator.ContainsKey(newState))
                ResetAnimation(newState);

            switch (newState)
            {
                case TileState.Neutral:
                    Invoke(nameof(StartNeutral), randomDelay);
                    break;
                case TileState.Burning:
                    Invoke(nameof(StartBurning), randomDelay);
                    break;
                case TileState.Recovering:
                    Invoke(nameof(StartRecovering), randomDelay);
                    break;
                case TileState.Empty:
                    break;
            }

            _oldState = newState;
        }

        private void SwitchActiveModel(TileState state)
        {
            foreach (KeyValuePair<TileState, (GameObject model, Animator animator)> kvp in _stateToModelAndAnimator)
            {
                kvp.Value.model.SetActive(kvp.Key == state);
            }
        }
    }
}
