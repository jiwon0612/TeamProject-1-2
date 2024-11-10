using System;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Pool/Item")]
public class PoolItemSO : ScriptableObject
{
    public string poolName;
    public GameObject prefab;
    public int count;

    private void OnValidate()
    {
        if (prefab != null)
        {
            IPoolable item = prefab.GetComponent<IPoolable>();
            if (item == null)
            {
                Debug.LogError("이거 아니다");
                prefab = null;
                return;
            }
            else
            {
                poolName = item.PoolName;
            }
        }
    }
}
