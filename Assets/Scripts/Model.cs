using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Model", menuName = "ScriptableObjects/Model", order = 1)]
public class Model : ScriptableObject
{
    public enum DecomposeAxis
    {
        X, Y, Z
    }

    public string ModelName;

    public GameObject ModelPrefab;

    public List<IPlayable> playables = new List<IPlayable>();

    public DecomposeAxis decomposeAxis;

    public float DecomposeStrength = 1.5f;

    public List<Vector3> movableStartPos = new List<Vector3>();

    public void SetStartPositions()
    {
        foreach (Transform child in ModelPrefab.transform)
        {
            if (child.GetComponent<MovableByDecomposition>() != null)
            {
                movableStartPos.Add(child.transform.position); 
            }
        }
    }
}
