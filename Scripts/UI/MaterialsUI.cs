using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MaterialsUI : MonoBehaviour
{
    public RectTransform layout;

    GameObject prefab;
    
    List<GameObject> prefabPool = new List<GameObject>();

    private void Start()
    {
        GameManager.Instance.onPlayerInventoryChanged += UpdateMaterials;
        prefab = layout.transform.GetChild(0).gameObject;
        prefab.SetActive(false);
    }

    void UpdateMaterials()
    {
        Inventory inv = GameManager.Instance.GetPlayerInventory();
        int length = inv.slots.Length;

        while (prefabPool.Count < length)
        {
            GameObject go = Instantiate(prefab, layout.transform);
            go.SetActive(true);
            prefabPool.Add(go);
        }

        for (int i = 0; i < length; i++)
        {
            if (inv.slots[i].item == null)
            {
                prefabPool[i].SetActive(false);
                continue;
            }

            prefabPool[i].SetActive(true);
            prefabPool[i].GetComponentInChildren<Image>().sprite = inv.slots[i].item.icon;
            prefabPool[i].GetComponentInChildren<TextMeshProUGUI>().text =
                inv.slots[i].amount.ToString();
        }

        for (int i = length; i < prefabPool.Count; i++)
            prefabPool[i].SetActive(false);
    }

}
