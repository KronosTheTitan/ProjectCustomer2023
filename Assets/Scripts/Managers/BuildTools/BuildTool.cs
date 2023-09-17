using System;

namespace Managers.BuildTools
{
    [Serializable]
    public abstract class BuildTool
    {
        public abstract bool CanSelect();
        public abstract bool UseTool();
        public abstract void OnDeselect();
    }
}