// using UnityEngine;
// using UnityEditor;
// using System.Collections.Generic;
//
// public class PrefabRegistrationTool : EditorWindow
// {
//     private GameObject prefabToRegister; // 등록할 프리팹
//     private PrefabRegistrationData registrationData; // 등록 정보를 저장하는 데이터
//     private List<string> registeredPrefabDescriptions = new List<string>(); // 각 프리팹에 대한 설명 목록
//    
//
//     [MenuItem("Custom/Register Prefabs")]
//     public static void ShowWindow()
//     {
//         EditorWindow.GetWindow(typeof(PrefabRegistrationTool));
//     }
//
//     private void OnGUI()
//     {
//         GUILayout.Label("Prefab Registration Tool", EditorStyles.boldLabel); // 윈도우 제목
//
//         prefabToRegister = EditorGUILayout.ObjectField("Prefab to register:", prefabToRegister, typeof(GameObject), false) as GameObject;
//
//         if (prefabToRegister != null)
//         {
//             if (GUILayout.Button("Register Prefab"))
//             {
//                 registrationData.registeredPrefabs.Add(prefabToRegister);
//                 registeredPrefabDescriptions.Add("Enter Description Here"); // 새로 등록된 프리팹에 대한 기본 설명 추가
//                 Debug.Log(prefabToRegister.name + "Prefab has been registered.");
//                 prefabToRegister = null;
//                 EditorUtility.SetDirty(registrationData); // 변경된 데이터를 저장하도록 표시
//             }
//         }
//         else
//         {
//             EditorGUILayout.LabelField("Select a prefab to register.");
//         }
//
//         GUILayout.Space(20);
//
//         for (int i = 0; i < registrationData.registeredPrefabs.Count; i++)
//         {
//             EditorGUILayout.BeginHorizontal();
//
//            
//
//             // 프리팹 목록의 크기가 설명 목록보다 큰지 확인하고 부족한 부분을 디폴트 설명으로 채웁니다.
//             while (registeredPrefabDescriptions.Count <= i)
//             {
//                 registeredPrefabDescriptions.Add("Enter Description Here");
//             }
//
//             // 각 프리팹에 대한 설명 입력 필드
//             registeredPrefabDescriptions[i] = EditorGUILayout.TextField("Prefab Description:", registeredPrefabDescriptions[i]);
//
//             if (GUILayout.Button("Create Prefab: " + registrationData.registeredPrefabs[i].name))
//             {
//                 // Hierarchy에 현재 선택한 프리팹 생성
//                 GameObject instantiatedPrefab = Instantiate(registrationData.registeredPrefabs[i]);
//                 instantiatedPrefab.name = registrationData.registeredPrefabs[i].name; // 원본 프리팹 이름 유지
//             }
//
//             if (GUILayout.Button("Delete Prefab: " + registrationData.registeredPrefabs[i].name))
//             {
//                 // 등록된 목록에서 선택한 프리팹 및 설명 삭제
//                 registrationData.registeredPrefabs.RemoveAt(i);
//                 registeredPrefabDescriptions.RemoveAt(i);
//                 EditorUtility.SetDirty(registrationData); // 변경된 데이터를 저장하도록 표시
//             }
//
//             EditorGUILayout.EndHorizontal();
//         }
//     }
//
//     private PrefabRegistrationData LoadRegistrationData()
//     {
//         // ScriptableObject 로드, 없으면 생성
//         PrefabRegistrationData data = AssetDatabase.LoadAssetAtPath<PrefabRegistrationData>("Assets/PrefabRegistrationData.asset");
//         if (data == null)
//         {
//             data = ScriptableObject.CreateInstance<PrefabRegistrationData>();
//             AssetDatabase.CreateAsset(data, "Assets/PrefabRegistrationData.asset");
//             AssetDatabase.SaveAssets();
//         }
//         return data;
//     }
//
//     private void OnEnable()
// {
//     // 스크립트가 활성화될 때 등록된 프리팹 목록을 로드
//     registrationData = LoadRegistrationData();
//     
//     // 등록된 프리팹 목록과 설명 목록을 맞추기 위해 더 많은 설명이 필요한 경우 추가
//     while (registeredPrefabDescriptions.Count < registrationData.registeredPrefabs.Count)
//     {
//         registeredPrefabDescriptions.Add("Enter Description Here");
//     }
//     
//     // 저장된 설명 데이터를 설명 목록에 복사
//     for (int i = 0; i < registrationData.prefabDescriptions.Count; i++)
//     {
//         if (i < registeredPrefabDescriptions.Count)
//         {
//             registeredPrefabDescriptions[i] = registrationData.prefabDescriptions[i];
//         }
//         else
//         {
//             registeredPrefabDescriptions.Add(registrationData.prefabDescriptions[i]);
//         }
//     }
// }
//     private void SaveRegistrationData()
// {
//     // Save the registered prefab list and description list to ScriptableObject
//     registrationData.prefabDescriptions.Clear(); // 먼저 설명 목록을 초기화합니다.
//
//     for (int i = 0; i < registeredPrefabDescriptions.Count; i++)
//     {
//         registrationData.prefabDescriptions.Add(registeredPrefabDescriptions[i]);
//     }
//
//     // 디버그를 사용하여 설명 데이터 출력
//     Debug.Log("Saved Descriptions:");
//     for (int i = 0; i < registrationData.prefabDescriptions.Count; i++)
//     {
//         Debug.Log("Description " + i + ": " + registrationData.prefabDescriptions[i]);
//     }
//
//     EditorUtility.SetDirty(registrationData);
//     AssetDatabase.SaveAssets();
// }
//
//
//     private void OnDestroy()
//     {
//         // 윈도우가 닫힐 때 데이터를 저장
//         SaveRegistrationData();
//     }
//
//    
// }
