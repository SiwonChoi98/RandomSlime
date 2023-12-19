using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScene : MonoBehaviour
{
    private float delayTime = 1;

    // Use this for initialization
    private IEnumerator Start()
    {
        //SpecDataManager.Instance.Load(SpecDataResourceLoader.LoadSpecData());
        //splash에서 스펙데이터매니저를 실행시켜줘야한다.
        
        yield return new WaitForSeconds(delayTime);
        Application.LoadLevel("Main");
    }
    
}
