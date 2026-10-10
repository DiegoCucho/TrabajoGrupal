using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGeneration2 : MonoBehaviour
{
    public int numberObject; // какое кол-во надо заспавнить
    public GameObject[] objects;
    private int generatedObject = 0;
    public float minRange, maxRange; //территория спавна

    void Update()
    {
        //считает сколько объектов заспавнено
        if (generatedObject < numberObject)
        {
            Generate();
            generatedObject++;
        }

    }
    //сама генерация 
    public void Generate()
    {
        int rand = Random.Range(0, objects.Length);
        var cell = Instantiate(objects[rand], transform.position, Quaternion.identity);
        cell.transform.position = new Vector3(Random.Range(minRange - 5, maxRange + 5), Random.Range(minRange, maxRange), transform.position.z);

    }
}
