using UnityEngine;

public abstract class BuildingLogic
{
    public Building building;
    public bool update = false;

    public virtual void Initialize(Block block) {}

    // Se llama cuando el edificio acaba de colocarse
    public virtual void OnPlaced() {}

    // Se llama cada frame
    public virtual void Update() {}

    // Se llama al eliminar el edificio
    public virtual void OnRemoved() {}
}