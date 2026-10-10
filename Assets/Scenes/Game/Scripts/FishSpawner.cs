using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    public GameObject[] fishPrefabs;// префаб рыбки
    public float spawnDelay = 1f; // задержка между спавном рыбок
    //public float speed = 2f; // скорость движения рыбок
   // public float startY = 3f; // Y-позиция, на которой рыбки должны появляться
    //public float minY = -3f; // минимальная Y-позиция, которую рыбки должны достичь перед удалением

    // Start is called before the first frame update
    void Start()
    {
        // начать спавн рыбок
        StartCoroutine(SpawnFish());
    }

    IEnumerator SpawnFish()
    {
        while (true)
        {
            int randFish = Random.Range(0, fishPrefabs.Length);

            // создать новую рыбку
            GameObject newFish = Instantiate(fishPrefabs[randFish], new Vector3(12f, Random.Range(-4, 6), 0), Quaternion.identity);

            // задержка
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
