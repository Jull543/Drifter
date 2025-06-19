using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUp_Herb1 : MonoBehaviour
{
    [SerializeField]
    private Text pickUpText;
    private bool pickUpAllowed;
    private GameObject player;


    public Inventory playerInventory;   //所属背包

    // Start is called before the first frame update
    void Start()
    {
        pickUpText.gameObject.SetActive(false);
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (pickUpAllowed && Input.GetKeyDown(KeyCode.E))
        {
            PickUp();
            
        }            
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name.Equals("Player"))
        {
            pickUpText.gameObject.SetActive(true);
            pickUpAllowed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.Equals("Player"))
        {
            pickUpText.gameObject.SetActive(false);
            pickUpAllowed = false;
        }
    }

    private void PickUp()
    {
        gameObject.GetComponent<ItemOnWorld>().AddNewItem();
        player.GetComponent<AudioSource>().clip = player.GetComponent<HeroKnight>().audios[8]; 
        player.GetComponent<AudioSource>().Play();
        Destroy(gameObject);
        
    }
}
