using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStoreItem : MonoBehaviour
{
    [SerializeField]
    Text nameText;
    [SerializeField]
    Text priceText;
    [SerializeField]
    Image itemImage;
    [SerializeField]
    Button button;

    public void Initialize(StoreItem item)
    {
        nameText.text = item.Name;
        priceText.text = item.Price.ToString();
        itemImage.sprite = item.Icon;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => Store.Instance.SelectItem(item));
    }
    
}
