using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    
    #region UnityMethod
    private void Update()
    {
        TargetSearch();
    }
    private void LateUpdate()
    {
        CameraMove(); 
    }
    #endregion
    
    ///플레이어 찾기
    private void TargetSearch()
    {
        if (target == false) 
        {
            GameObject go = GameObject.FindWithTag("Player");
            if (go)
            {
                target = go.transform;    
            }
            
        }
    }
    ///카메라 이동
    private void CameraMove()
    {
        if (target == true)
        {
            transform.position = target.position + offset;

        }
    }
}
