﻿using UnityEngine;

namespace CodeBase.Infrastructure.Instantiating
{
    public interface IInstantiateService
    {
        GameObject Instantiate(GameObject prefab);
        GameObject Instantiate(GameObject prefab, Vector3 at);
        GameObject Instantiate(GameObject prefab, Vector3 at, Transform parent);
    }
}