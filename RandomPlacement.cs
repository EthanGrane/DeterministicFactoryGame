using UnityEngine;

public class RandomPlacement : MonoBehaviour
{
    [ContextMenu("RotateRandomleyOnY")]
    void RotateRandomleyOnY()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).localRotation = Quaternion.Euler(0,Random.Range(0,360),0);
        }
    }
}
