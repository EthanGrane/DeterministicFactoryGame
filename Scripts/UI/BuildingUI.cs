using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BuildingUI : MonoBehaviour
{

    public Transform layout;
    [Space]
    public TextMeshProUGUI buildingCostText;

    private void Start()
    {
        BuildingManager.Instance.onBlockSelected += UpdateBuildingCostText;
        
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

    void UpdateBuildingCostText(Block block)
    {
        buildingCostText.text = "";

        if (block == null)
            return;
        
        for (int i = 0; i < block.buildingCost.Length; i++)
        {
            string name = block.buildingCost[i].requieredItem.name;
            int amount = block.buildingCost[i].amount;
            buildingCostText.text += name + " x" + amount + "\n";
        }
    }
}
