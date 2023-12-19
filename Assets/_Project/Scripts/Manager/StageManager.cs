using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class StageManager : MonoBehaviour
{
     public static StageManager instance; 
     

     private void Awake()
     {
          Singleton();
     }
     
     private void Singleton()
     {
          if (instance == null)
          {
               instance = this;
          }
          else if (instance != this)
          {
               Destroy(gameObject);
          }
          DontDestroyOnLoad(gameObject);
     }
     
     
     
}
