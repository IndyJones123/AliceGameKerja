using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Gem : MonoBehaviour, ICollectible
{
    public static event HandleGemCollected OnGemCollected;
    public delegate void HandleGemCollected(ItemData itemData);
    public ItemData gemData;

    public void Collect()
    {
        Destroy(gameObject);
        OnGemCollected?.Invoke(gemData);
    }
}
