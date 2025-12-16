using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "FactoryBlock", menuName = "FACTORY/Block/FactoryBlock")]
public class FactoryBlock : Block<FactoryLogic>
{

    public Item input;
    public int inputAmount;
    
    public Item output;
    public int outputAmount;
    
    [FormerlySerializedAs("timeToCraft")] [Space] 
    public float timeToCraftOnTicks;

}
