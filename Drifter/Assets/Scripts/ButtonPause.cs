using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPause : MonoBehaviour {

    //the ButtonPauseMenu
    public GameObject ingameMenu;
    public GameObject Help;

    public void OnPause()//点击“暂停”时执行此方法
    {
        Time.timeScale = 0;
        ingameMenu.SetActive(true);
    }

    public void OnResume()//点击“回到游戏”时执行此方法
    {
        Time.timeScale = 1f;
        ingameMenu.SetActive(false);
    }

    public void OnRestart()//点击“重新开始”时执行此方法
    {
        //Loading Scene1
        UnityEngine.SceneManagement.SceneManager.LoadScene("country");
        Time.timeScale = 1f;
    }

    public void OnRelife()//点击“重新开始”时执行此方法
    {
        //Loading Scene1
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
        Time.timeScale = 1f;
    }

    public void OnHelp()
    {
        Help.SetActive(true);
    }

    public void OffHelp()
    {
        Help.SetActive(false);
    }




}

