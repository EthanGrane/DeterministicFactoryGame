using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Drill Block", menuName = "FACTORY/Block/Drill Block")]
public class DrillBlock : Block<DrillLogic>
{
    [FormerlySerializedAs("terrain")] [Header("Drill Block")]
    public TerrainSO terrainSO;
    [Range(0.01f,1f)]public float efficiencyPerTile = 0.75f;
    public Item outputItem;
}