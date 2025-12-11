using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BuildingUI : MonoBehaviour
{

    public Transform layout;

    private void Start()
    {
        GameObject buttonTemplate = layout.GetChild(0).gameObject;

        Block[] blocks = BuildingManager.Instance.blocks;
        for (int i = 0; i < blocks.Length; i++)
        {
            GameObject temaplte =Instantiate(buttonTemplate, layout);
            Block block = blocks[i];
            temaplte.GetComponentInChildren<TextMeshProUGUI>().text = block.blockName;
            
            temaplte.GetComponent<Button>().onClick.AddListener(() =>
            {
                BuildingManager.Instance.SelectBlock(block);
            });
        }
        
        buttonTemplate.SetActive(false);
    }
}
