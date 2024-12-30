using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.Burst.CompilerServices;

public class FoeScript : MonoBehaviour
{
    [SerializeField] Transform PatrolLocation1;
    [SerializeField] Transform PatrolLocation2;
    [SerializeField] Transform PlayerLocation;
    [SerializeField] public float RotationSpeed;
    [SerializeField] public float FoeSpeed;
    [SerializeField] TMP_Text TimerText;
    [SerializeField] float whiskersLength;
    [SerializeField] float whiskerAngle;
    [SerializeField] TMP_Text StateText;
    [SerializeField] TMP_Text IdleFailSafeText;
    Rigidbody2D PlayerRigidBody;
    public int FoeState;
    public int CurrentPatrol;
    public int ChancetoSwitchToIdle;
    public int Switchtingtries;
    public int WaitingTime;
    public int ChanceToSwitchToPatrol;
    public bool ThisConeCollision;
    public int IdleFailsafe;
    public float DetectionRange;
    bool IsInRange;




    void Start()
    {
        PatrolPlace1 = PatrolLocation1.position;
        PatrolPlace2 = PatrolLocation2.position;
        PlayerPlace = PlayerLocation.position;
        PlayerRigidBody = GetComponent<Rigidbody2D>();
        FoeState = 0;
        CurrentPatrol = 0;
        Switchtingtries = 0;         
        WaitingTime = 3;
        IdleFailsafe = 0;
        IsInRange = false;
        
        


    }

    public Vector3 PatrolPlace1
    {
        get { return PatrolLocation1.position; }
        set { PatrolLocation1.position = value; }
    }

    public Vector3 PatrolPlace2
    {
        get { return PatrolLocation2.position; }
        set { PatrolLocation2.position = value; }
    }

    public Vector3 PlayerPlace
    {
        get { return PlayerLocation.position; }
        set { PlayerLocation.position = value; }
    }

    void Update()
    {
        TimerText.text = "Idle Timer: " + WaitingTime.ToString();
        IdleFailSafeText.text = "IdleFailsafe: " + IdleFailsafe.ToString();
        if (FoeState == 0)
        {
            FoeSpeed = 1.2f;
            StateText.text = "Current State: Patrol";
            PatrolState();

            
        }
        else if (FoeState == 1)
        {
            StateText.text = "Current State: Idle";
        }
        else if (FoeState == 2)
        {
            Seek(PlayerPlace);
            StateText.text = "Current State: Seek";
        }
        if(ChancetoSwitchToIdle == 2)
        {
            FoeState = 1;
            ChanceToSwitchToPatrol = 0;
            ChancetoSwitchToIdle = 0;
            PlayerRigidBody.velocity = Vector3.zero;
            PlayerRigidBody.angularVelocity = 0;
            IdleFailsafe = 0;
            Timer();

        }
        if (ChanceToSwitchToPatrol == 2)
        {
            FoeState = 0;
            ChancetoSwitchToIdle = 0;
            ChanceToSwitchToPatrol = 0;
            Switchtingtries = 0;
        }
        if (ThisConeCollision)
        {
            FoeState = 2;
            
        }
        IsInRange = Vector3.Distance(transform.position, PlayerPlace) <= DetectionRange;
        if (IsInRange)
        {
            if (FoeState == 2)
            {
                SceneManager.LoadScene("LoseScene");
            }
            else
            {
                SceneManager.LoadScene("WinScene");
            }
        }
    }

    void PatrolState()
    {
        if (CurrentPatrol == 0)
        {
            Seek(PatrolPlace1);
        }
        else if (CurrentPatrol == 1)
        {
            Seek(PatrolPlace2);
        }

    }

    void Seek(Vector3 TargetPlace)
    {
        Vector2 DirectionOfTarget = (TargetPlace - transform.position).normalized;
        float IdealTurn = Mathf.Atan2(DirectionOfTarget.y, DirectionOfTarget.x) * Mathf.Rad2Deg + 90.0f;
        float AngleChanges = Mathf.DeltaAngle(IdealTurn, transform.eulerAngles.z);
        float rotationStep = RotationSpeed * Time.deltaTime;
        float rotationAction = Mathf.Clamp(AngleChanges, -rotationStep, rotationStep);
        transform.Rotate(Vector3.forward, rotationAction);
        PlayerRigidBody.velocity = transform.up * FoeSpeed;
    }

    void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if(collision.gameObject.tag == "PatrolPoint" && FoeState == 0)
        {
            if(CurrentPatrol == 0)
            {
                CurrentPatrol = 1;
                if (IdleFailsafe == 2)
                {
                    ChancetoSwitchToIdle = 2;
                    WaitingTime = 3;
                    IdleFailsafe = 0;
                }
                else
                {
                    ChancetoSwitchToIdle = Random.Range(0, 3);
                    WaitingTime = 3;
                    IdleFailsafe++;
                }

            }
            else if (CurrentPatrol == 1)
            {
                CurrentPatrol = 0;
                if(IdleFailsafe == 2)
                {
                    ChancetoSwitchToIdle = 2;
                    WaitingTime = 3;
                    IdleFailsafe = 0;
                }
                else
                {
                    ChancetoSwitchToIdle = Random.Range(0, 3);
                    WaitingTime = 3;
                    IdleFailsafe++;
                }
            }
        }

    }

    void Timer()
    {
        if(WaitingTime == 0)
        {
            if (Switchtingtries == 2)
            {
                Switchtingtries = 0;
                ChanceToSwitchToPatrol = 2;
                WaitingTime = 3;
            }
            else
            {
                ChanceToSwitchToPatrol = Random.Range(0, 3);
                WaitingTime = 3;
                Switchtingtries++;
                if (ChanceToSwitchToPatrol  == 0 || ChanceToSwitchToPatrol == 1)
                {
                    Invoke("Timer", 1);
                }
            }
        }
        else
        {
            WaitingTime--;
            Invoke("Timer", 1);
            
        }
    }



}
