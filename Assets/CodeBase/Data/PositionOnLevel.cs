using System;

namespace CodeBase.Data
{
    [Serializable]
    public class PositionOnLevel : IReadonlyPositionOnLevel
    {
        public Vector3Data Position;
        public string Level;

        public Vector3Data PositionReadonly => Position;
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
        public Vector3Data PositionReadonly { get; }
        public string LevelReadonly { get; }
    }
}