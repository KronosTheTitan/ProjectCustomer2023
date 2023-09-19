using System;
using Map;
using UnityEngine;

namespace Managers.BuildTools
{
    [Serializable]
    public abstract class BuildTool
    {
        [SerializeField] private int cost;
        public int Cost => cost;
        public abstract bool CanSelect();
        public abstract bool UseTool(Tile target);
        public abstract void OnDeselect();
        public abstract void Charge(Tile tile);
    }
}