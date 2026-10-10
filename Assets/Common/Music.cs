using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Music : MonoBehaviour
{
    // Start is called before the first frame updatepublic AudioSource audioSource;

    public Sprite onMusic;
    public Sprite offMusic;

    public Button MusicButton;
    public bool isOn;
    public AudioSource ad;


    // Start is called before the first frame update
    void Start()
    {
       isOn = true;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void OffSound(){
       if(!isOn){
         ad.enabled = true;
            isOn = true;
            MusicButton.GetComponent<Image>().sprite = onMusic;
       } else if(isOn){
       ad.enabled = false;
            isOn = false;
             MusicButton.GetComponent<Image>().sprite = offMusic;
       }
    }
}
