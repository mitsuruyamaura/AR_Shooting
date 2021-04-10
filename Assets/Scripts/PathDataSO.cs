using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PathDataSO", menuName = "Create PathDataSO")]
public class PathDataSO : ScriptableObject {

    public List<PathData> pathDatasList = new List<PathData>();
}
