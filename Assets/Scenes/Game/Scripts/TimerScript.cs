using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    public float startTime = 300f; // время в секундах
    private float currentTime;
    public Text timerText;

    void Start()
    {
        currentTime = startTime;
        timerText = gameObject.GetComponent<Text>();
    }

    void Update()
    {
        currentTime -= Time.deltaTime;

        float minutes = Mathf.FloorToInt(currentTime / 60); // количество минут
        float seconds = Mathf.FloorToInt(currentTime % 60); // количество секунд

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds); // форматирование времени

        if (currentTime <= 0)
        {
            var changeScene = new SceneChanger();

            changeScene.ChangeScene("GameOver");
        }
    }
}
