using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "New Enemy Wave", menuName = "FACTORY/ENEMY/Wave")]
public class EnemyWaveSO : ScriptableObject
{
    public EnemyWaveSquad[] waves;
}

[System.Serializable]
public class EnemyWaveSquad
{
    public EnemyTierSO enemy;
    public int quantity = 1;
}

[CustomEditor(typeof(EnemyWaveSO))]
public class EnemyWaveSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Dibujar el inspector original
        DrawDefaultInspector();
        EnemyWaveSO waveSO = (EnemyWaveSO)target;

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("=== SUMMARY ===", EditorStyles.boldLabel);

        float totalTier = 0f;

        if (waveSO.waves != null)
        {
            foreach (var squad in waveSO.waves)
            {
                if (squad.enemy == null) continue;

                totalTier += (squad.enemy.tierIndex + 1) * squad.quantity;
            }
        }

        EditorGUILayout.LabelField("Total Tier:", totalTier.ToString());

        EditorGUILayout.Space(5);
        EditorGUILayout.LabelField("Details per Squad:", EditorStyles.boldLabel);

        if (waveSO.waves != null)
        {
            foreach (var squad in waveSO.waves)
            {
                if (squad.enemy == null) continue;
                totalTier += (squad.enemy.tierIndex + 1) * squad.quantity;

                EditorGUILayout.LabelField(
                    $"{squad.enemy.tierName}  × {squad.quantity} | HP/Tier: {totalTier}"
                );
            }
        }
    }
}
