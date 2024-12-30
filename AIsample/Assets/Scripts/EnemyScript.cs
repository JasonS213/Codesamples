using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum StateAction
{
    PATROL,
    SEEK,
    IDLE
};
public class EnemyScript : MonoBehaviour
{
    [SerializeField] protected Transform targetLocation;

    public StateAction action {  get; set; }


    
    void Start()
    {
       
    }

    
    void Update()
    {
        
    }
}
