using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ECparticleLinerendererColorChangerMaster : MonoBehaviour {

    [System.Serializable]
    public class colorChange
    {
        public string Name;
        public ParticleSystem[] colored_ParticleSystem;
        public LineRenderer[] lineRenderers;
        public Gradient Gradient_custom; 
    }
    public float Speed_custom = 1;
    public colorChange[] colorChangeList;

    public bool applyChanges = false;
    public bool Keep_applyChanges = false;

    void Start()
    {

    }

    void Update()
     {  
        if (applyChanges || Keep_applyChanges)
        {
            for (int i = 0; i < colorChangeList.Length; i++)
            {
                for (int a = 0; a < colorChangeList[i].colored_ParticleSystem.Length; a++)
                {
                    var col = colorChangeList[i].colored_ParticleSystem[a].colorOverLifetime;
                    col.color = colorChangeList[i].Gradient_custom;
                    var main = colorChangeList[i].colored_ParticleSystem[a].main;
                    main.simulationSpeed = Speed_custom;
                }
            }
            for (int i = 0; i < colorChangeList.Length; i++)
            {
                for (int a = 0; a < colorChangeList[i].lineRenderers.Length; a++)
                {
                    var col = colorChangeList[i].lineRenderers[a].colorGradient = colorChangeList[i].Gradient_custom;
                    /*col.color = colorChangeList[i].Gradient_custom;
                     var main = colorChangeList[i].colored_ParticleSystem[a].main;*/
                }
            }
            applyChanges = false;
        }
     }
  
}
