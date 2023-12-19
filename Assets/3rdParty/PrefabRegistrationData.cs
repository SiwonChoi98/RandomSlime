using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PrefabRegistrationData", menuName = "Custom/Prefab Registration Data")]
public class PrefabRegistrationData : ScriptableObject
{
    public List<GameObject> registeredPrefabs = new List<GameObject>();
    
    
      public List<string> prefabDescriptions = new List<string>(); // 각 프리팹에 대한 설명 목록
}
