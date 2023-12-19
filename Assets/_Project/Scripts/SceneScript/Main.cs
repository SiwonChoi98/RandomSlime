using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Main : MonoBehaviour
{
    //인게임 이동 버튼
    public void BattleButton()
    {
        SceneManager.LoadScene("InGame");
    }

    private void Awake()
    {
        Time.timeScale = 1;
        
    }

    private void Start()
    {
        SoundManager.Instance.BgmPlaySound("Lobby", 0.05f);
    }
}
