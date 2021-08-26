using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPooledAsset
{
    void SetupAsset(Vector3 _position, Quaternion _rotation, Vector3 _localScale);

    void DisableAsset();
}
