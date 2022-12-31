using System;
using UnityEngine;

namespace CodeBase.Logic
{
    public class UniqueId : MonoBehaviour
    {
        public string Id;

        public void Generate()
        {
            Id = $"{gameObject.scene.name}_{Guid.NewGuid().ToString()}";
        }
    }
}