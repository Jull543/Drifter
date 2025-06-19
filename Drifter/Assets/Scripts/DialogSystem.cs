using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("UI组件")]
    public Text textLabel;
    public Image faceImage;

    [Header("文本文件")]
    public TextAsset textFile;
    public int index;
    public float textSpeed;

    [Header("头像")]
    public Sprite face01,face02;

    public GameObject win;

    /*private GameObject player,doctor;*/

    bool textFinished;
    bool cancelTyping;

    List<string> textList = new List<string>();

    void Awake()
    {
        GetTextFromFile(textFile);
    }

    private void OnEnable(){
       // textLabel.text = textList[index];
       // index++;
       textFinished = true;
       StartCoroutine(SetTextUI());

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && index == textList.Count)
        {
            switch (gameObject.name)
            {
                case "scene1end":
                    UnityEngine.SceneManagement.SceneManager.LoadScene("Dark");
                    break;
                case "scene2end":
                    UnityEngine.SceneManagement.SceneManager.LoadScene("Ending");
                    break;
                case "countryend":
                    UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
                    break;
                case "ending":
                    win.SetActive(true);
                    break;
            }

            gameObject.SetActive(false);
            index = 0;
            return;
        }
        //if(Input.GetMouseButtonDown(0) && textFinished)
        //{
            //textLabel.text = textList[index];
            //index++;
          //  StartCoroutine(SetTextUI());

        //}

        if(Input.GetMouseButtonDown(0))
        {
            if (textFinished && !cancelTyping)
            {
                StartCoroutine(SetTextUI());
            }
            else if(!textFinished)
            {
                cancelTyping = true;
            }
        }
    }

    void GetTextFromFile(TextAsset file)
    {
        textList.Clear();
        index = 0;

        var lineData = file.text.Split('\n');

        foreach(var line in lineData)
        {
            textList.Add(line);
        }
    }

    IEnumerator SetTextUI()
    {
        textFinished = false;
        textLabel.text = "";

        switch(textList[index])
        {
            case "A\r":
                faceImage.sprite = face01;
                index++;
                break;
            case "B\r":
                faceImage.sprite = face02;
                index++;
                break;

        }

        //for (int i = 0; i < textList[index].Length; i++)
        //{
          //  textLabel.text += textList[index][i];
            //yield return new WaitForSeconds(textSpeed);        
        //}

        int letter = 0;
        while(!cancelTyping && letter < textList[index].Length -1)
        {
            textLabel.text += textList[index][letter];
            letter++;
            yield return new WaitForSeconds(textSpeed);
        }
        textLabel.text = textList[index];
        cancelTyping = false;
        textFinished = true;
        index++;
    }
}
