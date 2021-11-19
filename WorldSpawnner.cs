using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WorldSpawnner : MonoBehaviour
{
    [SerializeField]
    private Text wave = null;

    [SerializeField]
    private GameObject Zombie = null;

    [SerializeField]
    private Transform[] paths = null;

    [SerializeField]
    private Transform target = null;

    private RectTransform rectTransform = null;
    private Vector3 initialPosition = Vector3.zero;

    private float spawnTime = 1.5f;
    private int count = 0;
    private int waveNumber = 1;
    private float multiplier = 1;
    private int aliveZombieCount = 0;
    private bool flag = true;
    private float hitPoint = 1;

    public int AliveZombieCount { get => aliveZombieCount; set => aliveZombieCount = value; }

    void Start()
    {
        StartCoroutine(SpawnZombie());
        
        rectTransform = wave.rectTransform;
        initialPosition = rectTransform.localPosition;

        Color color = wave.color;
        color.a = 0;
        wave.color = color;
    }
    

    private IEnumerator SpawnZombie()
    {
        while(true)
        {
            if(flag)
            {
                int random = UnityEngine.Random.Range(0, 4);
                GameObject zombie_1 = Instantiate(Zombie, paths[random].transform.position, paths[random].transform.rotation) as GameObject;
                zombie_1.GetComponent<ZombieHealthSystem>().SetMaxHealth(40 * multiplier);
                zombie_1.GetComponent<ZombieAI>().Target = target;
                zombie_1.GetComponent<ZombieAI>().HitPoint += hitPoint;

                random = UnityEngine.Random.Range(4, 7);
                GameObject zombie_2 = Instantiate(Zombie, paths[random].transform.position, paths[random].transform.rotation) as GameObject;
                zombie_2.GetComponent<ZombieHealthSystem>().SetMaxHealth(40 * multiplier);
                zombie_2.GetComponent<ZombieAI>().Target = target;
                zombie_2.GetComponent<ZombieAI>().HitPoint += hitPoint;

                count += 2;
                aliveZombieCount += 2;
            }

            if (count % 12 == 0)
            { 
                if (spawnTime > 1f && flag)
                {
                    waveNumber++;
                    wave.text = "Wave " + waveNumber.ToString();

                    spawnTime -= 0.05f;
                    multiplier += 0.40f;
                    hitPoint += 0.5f;
                }

                flag = false;

                if(aliveZombieCount == 0)
                {
                    yield return StartCoroutine(ShowWave());
                }
                
            }
            yield return new WaitForSecondsRealtime(spawnTime);
        }
    }

    private IEnumerator ShowWave()
    {
        Color color = wave.color;
        color.a = 1;
        wave.color = color;
        rectTransform.localPosition = initialPosition;

        while (wave.color.a > 0)
        {
            color.a -=  Time.deltaTime; 
            wave.color = color;
            rectTransform.localPosition += Vector3.up * 1.5f; 

            yield return null;
        }

        yield return new WaitForSecondsRealtime(2f);

        flag = true;
    }
}
