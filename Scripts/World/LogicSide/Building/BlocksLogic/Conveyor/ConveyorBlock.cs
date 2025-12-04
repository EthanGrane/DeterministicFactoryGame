using UnityEngine;

[CreateAssetMenu(fileName = "New Conveyor Block", menuName = "FACTORY/Block/Conveyor Block")]
public class ConveyorBlock : Block<ConveyorLogic>
{
    public int conveyorTickSpeed = 60;
}
