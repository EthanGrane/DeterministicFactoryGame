using UnityEngine;
using UnityEngine.Splines;

public class SplineYNormalization : MonoBehaviour
{
    public float Y = 0;

    [ContextMenu("Normalize Y")]
    public void NormalizeY()
    {
        var container = GetComponent<SplineContainer>();

        foreach (var spline in container.Splines)
        {
            for (int i = 0; i < spline.Count; i++)
            {
                BezierKnot knot = spline[i];

                Vector3 pos = knot.Position;
                pos.y = Y;
                knot.Position = pos;

                knot.TangentIn = new Vector3(knot.TangentIn.x, 0, knot.TangentIn.z);
                knot.TangentOut = new Vector3(knot.TangentOut.x, 0, knot.TangentOut.z);

                spline[i] = knot;
            }
        }
    }
}