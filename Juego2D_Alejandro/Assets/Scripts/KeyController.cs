using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up);
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if(collision.tag == "Player")
        {
            FindFirstObjectByType<ChestController>().hasKey = true;
            Destroy(gameObject);
        }
    }

}
