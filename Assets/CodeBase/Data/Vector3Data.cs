using System;

namespace CodeBase.Data
{
    [Serializable]
    public class Vector3Data : IVector3DataReadonly
    {
        public float X;
        public float Y;
        public float Z;

        public float ReadonlyX => X;
        public float ReadonlyY => Y;
        public float ReadonlyZ => Z;
        
        public Vector3Data(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }

    public interface IVector3DataReadonly
    {
        public float ReadonlyX { get; }
        public float ReadonlyY { get; }
        public float ReadonlyZ { get; }
    }
}