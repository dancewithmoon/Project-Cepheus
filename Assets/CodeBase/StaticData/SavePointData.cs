using System;
using UnityEngine;

namespace CodeBase.StaticData
{
    [Serializable]
    public class SavePointData
    {
        public string Id;
        public Vector3 Position;
        public Vector3 Scale;
        
        public SavePointData(string id, Vector3 position, Vector3 scale)
        {
            Id = id;
            Position = position;
            Scale = scale;
        }
    }
}