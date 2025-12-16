using UnityEngine;

public class FactoryLogic : BuildingLogic, IItemAcceptor, IItemProvider
{
    int currentInput = 0;
    int currentOutput = 0;

    int craftTimerOnTicks = 0;

    FactoryBlock factoryBlock;

    public override void OnPlaced()
    {
        base.OnPlaced();
        factoryBlock = building.block as FactoryBlock;
    }

    public override void Tick()
    {
        if (factoryBlock == null)
            return;

        if (currentInput < factoryBlock.inputAmount)
            return;

        craftTimerOnTicks++;

        if (craftTimerOnTicks >= factoryBlock.timeToCraftOnTicks)
        {
            craftTimerOnTicks = 0;

            currentInput -= factoryBlock.inputAmount;
            currentOutput += factoryBlock.outputAmount;
        }
    }

    // =========================
    // IItemAcceptor
    // =========================

    public bool CanAccept(Item item)
    {
        if (factoryBlock == null)
            return false;

        return item == factoryBlock.input;
    }

    public bool Insert(Item item)
    {
        if (!CanAccept(item))
            return false;

        currentInput++;
        return true;
    }

    // =========================
    // IItemProvider
    // =========================

    public Item Extract(Item item)
    {
        if (factoryBlock == null)
            return null;

        if (item != factoryBlock.output)
            return null;

        if (currentOutput <= 0)
            return null;

        currentOutput--;
        return factoryBlock.output;
    }

    public Item ExtractFirst()
    {
        if (factoryBlock == null)
            return null;

        if (currentOutput <= 0)
            return null;

        currentOutput--;
        return factoryBlock.output;
    }
}