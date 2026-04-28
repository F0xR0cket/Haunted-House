using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Animator m_Animator;
    
    AudioSource m_AudioSource;

    public InputAction MoveAction;

    private List<string> m_OwnedKeys = new List<string>();

    public float walkSpeed = 1.33333f;
    public float turnSpeed = 20f;

    public GameObject ScaredUI;
    public GameObject SprintUI;

    private bool IsScared = false;
    private bool canSprint = true;

    private int HowScared = 0;
    //private int anxiety = 0;

    Rigidbody m_Rigidbody;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        MoveAction.Enable();
        m_Animator = GetComponent<Animator>();
        m_AudioSource = GetComponent<AudioSource>();
        InvokeRepeating("Panic",20,40);
        //HowScared = 5;
    }

    private void Panic()
    {
        HowScared = Random.Range(0, 5);
    }

    private void Update()
    {
        if (HowScared > 0)
        {
            IsScared = true;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                HowScared -= 1;
                Debug.Log("HowScared is " + HowScared);
            }

        }
        else
        {
            IsScared = false;
        }
        

        if (IsScared)
        {
            MoveAction.Disable();
            ScaredUI.SetActive(true);
        }
        else
        {
            MoveAction.Enable();
            ScaredUI.SetActive(false);
        }


        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (canSprint)
            {
                //StartCoroutine(StopCountdown());
                StartCoroutine(Sprint());
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            StopCoroutine(Sprint());
            StartCoroutine(SprintCooldown());
        }

    }


    IEnumerator Sprint()
    {
        walkSpeed = 3f;

        /*for (float timer = 2.8f; timer >= 0; timer -= Time.deltaTime)
        {
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                StartCoroutine(SprintCooldown());
                yield break;
            }
        }*/

        float sprintTime = 2.8f;
        yield return new WaitForSeconds(sprintTime);
        canSprint = false;
        StartCoroutine(SprintCooldown());
        //StartCoroutine(Sprint());
    }

    IEnumerator SprintCooldown()
    {
        canSprint = false;
        SprintUI.SetActive(false);
        walkSpeed = 1.33333f;
        float sprintCooldown = 3.2f;
        yield return new WaitForSeconds(sprintCooldown);
        canSprint = true;
        SprintUI.SetActive(true);
    }

    /*void StopWait()
    {

    }*/




    void FixedUpdate()
    {
        var pos = MoveAction.ReadValue<Vector2>();

        float horizontal = pos.x;
        float vertical = pos.y;

        


        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize();

        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput && IsScared != true;
        m_Animator.SetBool("IsWalking", isWalking);

        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation(desiredForward);

        m_Rigidbody.MoveRotation(m_Rotation);
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * walkSpeed * Time.deltaTime);




        if (isWalking)
        {
            if (!m_AudioSource.isPlaying)
            {
                m_AudioSource.Play();
            }
        }
        else
        {
            m_AudioSource.Stop();
        }
    }




    public void AddKey(string keyName)
    {
        m_OwnedKeys.Add(keyName);
    }

    public bool OwnKey(string keyName)
    {
        return m_OwnedKeys.Contains(keyName);
    }



}