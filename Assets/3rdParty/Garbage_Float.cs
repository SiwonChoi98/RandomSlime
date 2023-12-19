// using UnityEngine;
// using UnityEditor;
//
// public class TruncateRectTransformValues : EditorWindow
// {
//     [MenuItem("Custom/Truncate RectTransform Values")]
//     static void Init()
//     {
//         TruncateRectTransformValues window = (TruncateRectTransformValues)EditorWindow.GetWindow(typeof(TruncateRectTransformValues));
//         window.Show();
//     }
//
//     private void OnGUI()
//     {
//         EditorGUILayout.LabelField("선택 한 부모의 자식들에게만 적용 됩니다.");
//         EditorGUILayout.LabelField("모든 게임오브젝트의 적용을 원하면 프리팹 제일 최상위 부모를 클릭후 적용.");
//
//         if (GUILayout.Button("Truncate Values"))
//         {
//             TruncateValues();
//         }
//     }
//
//     private void TruncateValues()
//     {
//         GameObject selectedPrefab = Selection.activeGameObject;
//
//         if (selectedPrefab != null)
//         {
//             RectTransform[] rectTransforms = selectedPrefab.GetComponentsInChildren<RectTransform>(true);
//
//             foreach (RectTransform rectTransform in rectTransforms)
//             {
//                 if (!rectTransform.name.Contains("fx"))
//                 {
//                     rectTransform.anchoredPosition = TruncateVector3(rectTransform.anchoredPosition);
//                     rectTransform.sizeDelta = TruncateVector2(rectTransform.sizeDelta);
//                     rectTransform.localPosition = new Vector3(rectTransform.localPosition.x, rectTransform.localPosition.y, 0f);
//                 }
//             }
//
//             Debug.Log("Truncated RectTransform values in the selected prefab.");
//         }
//         else
//         {
//             Debug.LogWarning("No prefab selected.");
//         }
//     }
//
//     private Vector3 TruncateVector3(Vector3 vector)
//     {
//         return new Vector3(Mathf.Round(vector.x), Mathf.Round(vector.y), Mathf.Round(vector.z));
//     }
//
//     private Vector2 TruncateVector2(Vector2 vector)
//     {
//         return new Vector2(Mathf.Round(vector.x), Mathf.Round(vector.y));
//     }
// }