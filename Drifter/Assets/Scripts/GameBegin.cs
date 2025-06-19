using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameBegin : MonoBehaviour
{
    public GameObject Button;


    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other){
        Button.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other){
        Button.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Button.activeSelf && Input.GetMouseButtonDown(0)){
            SceneManager.LoadScene("SampleScene");
        }
        
    }
    // Start is called before the first frame update

}
