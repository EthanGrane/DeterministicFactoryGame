using System.Collections.Generic;
using UnityEngine;

public class LogicManager : MonoBehaviour
{
    public static LogicManager Instance { get; private set; }

    private List<BuildingLogic> logics = new List<BuildingLogic>();

    private void Awake()
    {
        Instance = this;
    }

    public void Register(BuildingLogic logic)
    {
        logics.Add(logic);
        logic.Initialize(logic.building.block);
    }

    public void Unregister(BuildingLogic logic)
    {
        logics.Remove(logic);
    }

    void Update()
    {
        foreach (var logic in logics)
            if(logic.update)
                logic.Tick();
    }
}
