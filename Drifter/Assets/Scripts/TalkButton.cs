using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkButton : MonoBehaviour
{
    public GameObject Button;
    public GameObject talkUI;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other){
        Button.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other){
        Button.SetActive(false);
    }

    private void Update()
    {
        if(Button.activeSelf && Input.GetMouseButtonDown(0)){
            talkUI.SetActive(true);
        }
    }
}