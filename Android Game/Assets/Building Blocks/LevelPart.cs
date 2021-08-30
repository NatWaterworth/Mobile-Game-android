using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPart : MonoBehaviour, IPooledAsset
{
    [SerializeField] List<Transform> spawnPoints;

    public void DisableAsset()
    {
        gameObject.SetActive(false);
    }

    public void SetupAsset(Vector3 _position, Quaternion _rotation, Vector3 _localScale)
    {
        gameObject.transform.position = _position;
        gameObject.transform.rotation = _rotation;
        gameObject.transform.localScale = _localScale;
        gameObject.SetActive(true);
    }

    public bool InUse()
    {
        return gameObject.activeSelf;
    }

    public Vector3 GetSpawnPoint()
    {
        if(spawnPoints !=null && spawnPoints.Count > 0)
            return spawnPoints[Random.Range(0, spawnPoints.Count)].position;
        return transform.position;
    }

}
