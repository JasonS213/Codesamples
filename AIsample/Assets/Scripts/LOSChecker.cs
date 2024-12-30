using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LOSChecker : MonoBehaviour
{
    [SerializeField] GameObject Foe;

    private bool ConeCollision;

    void Start()
    {
        ConeCollision = false;
    }
    
    public bool GetConeCollision()
    {
        return ConeCollision;
    }

    void Update()
    {
        
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("HIT");
            Foe.GetComponent<FoeScript>().ThisConeCollision = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("OK");
            Foe.GetComponent<FoeScript>().ThisConeCollision = false;
            Foe.GetComponent<FoeScript>().FoeState = 0;
        }
    }

}
