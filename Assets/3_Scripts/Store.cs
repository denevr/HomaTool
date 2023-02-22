using HomaGames.Internal.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StoreItem
{
    public int Id;
    public string Name;
    public int Price;
    public Sprite Icon;
    public GameObject Prefab;
}

public class Store : Singleton<Store>
{
    public List<StoreItem> StoreItems;
    public Action<StoreItem> OnItemSelected;

    public void SelectItem(StoreItem item)
    {
        OnItemSelected?.Invoke(item);
    }
}
