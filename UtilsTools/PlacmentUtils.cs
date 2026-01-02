using UnityEngine;

public class PlacmentUtils : MonoBehaviour
{
    [ContextMenu("RoundXZPositionToInt")]
    public void RoundXZPositionToInt()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Vector3 pos = transform.GetChild(i).transform.position;
            pos = new Vector3((int)pos.x, pos.y, (int)pos.z);
            transform.GetChild(i).transform.position = pos;
        }
    }
}
