using System;

namespace CodeBase.Data
{
    [Serializable]
    public class PositionOnLevel : IReadonlyPositionOnLevel
    {
        public Vector3Data Position;
        public string Level;

        public IVector3DataReadonly PositionReadonly => Position;
        public string LevelReadonly => Level;
        
        public PositionOnLevel(string level, Vector3Data position)
        {
            Position = position;
            Level = level;
        }

        public PositionOnLevel(string initialLevel)
        {
            Level = initialLevel;
        }
    }

    public interface IReadonlyPositionOnLevel
    {
        public IVector3DataReadonly PositionReadonly { get; }
        public string LevelReadonly { get; }
    }
}