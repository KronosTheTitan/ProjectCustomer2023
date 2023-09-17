using System;
using Map;

namespace Managers.BuildTools
{
    [Serializable]
    public abstract class BuildTool
    {
        public abstract bool CanSelect();
        public abstract bool UseTool(Tile target);
        public abstract void OnDeselect();
    }
}