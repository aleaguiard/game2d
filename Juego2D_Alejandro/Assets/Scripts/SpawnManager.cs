using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab1;
    [SerializeField] GameObject enemyPrefab2;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("CrearEnemigos");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator CrearEnemigos()
    {
        yield return new WaitForSeconds(5f);
        while(true)
        {
        int randomEnemy = Random.Range(0, 2);
        if (randomEnemy == 0)
        {
            Instantiate(enemyPrefab1, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(enemyPrefab2, transform.position, Quaternion.identity);
        }
        yield return new WaitForSeconds(Random.Range(4f, 7f));
    }
    }

    public void CancelarEnemigos () {
        StopCoroutine("CrearEnemigos");
        GameObject[] zombiesF = GameObject.FindGameObjectsWithTag("ZombieF");
        for(int i = 0; i < zombiesF.Length; i++) {
            zombiesF[i].GetComponent<ZombieFController>().endGame = true;
        }
        GameObject[] zombiesM = GameObject.FindGameObjectsWithTag("ZombieM");
        for(int i = 0; i < zombiesM.Length; i++) {
            zombiesM[i].GetComponent<ZombieMController>().endGame = true;
        }
    }

}
