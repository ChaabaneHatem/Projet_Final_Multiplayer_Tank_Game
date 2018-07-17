using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Networking;

public class TankController : NetworkBehaviour
{
    [Header("player information")]

    public int playerID = 2;
    public AudioSource m_MovementAudio;
    public AudioClip m_EngineIdling;
    public AudioClip m_EngineDriving;

    public float m_PitchRange = 0.2f;
    private float m_OriginalPitch;

    public bool islocalTank;


    [Header("mouvement varibales")]
    [SerializeField]
    float movementSpeed = 5f;
    [SerializeField]
    float turnSpeed = 45f;

    private float turnAmount = 0;
    private float moveAmount = 0;

    [Header("Camera position")]
    [SerializeField]
    float cameraDistance = 16f;
    [SerializeField]
    float cameraHeight = 16f;



    Rigidbody rg;
    Transform mainCamera;
    Vector3 cameraOffSet;


    TankHealth tankHealth;


    private void Start()
    {

        if (!isLocalPlayer)
        {
            Destroy(this);

            return;
        }

        if (isLocalPlayer)
            islocalTank = true;
        else
            islocalTank = false;

        rg = GetComponent<Rigidbody>();

        cameraOffSet = new Vector3(0, cameraHeight, -cameraDistance);

        mainCamera = Camera.main.transform;

        MoveCamera();


        m_OriginalPitch = m_MovementAudio.pitch;

        tankHealth = transform.gameObject.GetComponent<TankHealth>();

    }

    private void Update()
    {
        turnAmount = CrossPlatformInputManager.GetAxis("Horizontal" + playerID);
        moveAmount = CrossPlatformInputManager.GetAxis("Vertical" + playerID);
        EngineAudio();


        //test part for debuggiing
        tankHealth.RpcSetHealthUI();

    }


    private void FixedUpdate()
    {

        Vector3 deltaTransition = transform.position + transform.forward * movementSpeed * moveAmount * Time.deltaTime;
        rg.MovePosition(deltaTransition);


        Quaternion deltaRotation = Quaternion.Euler(turnSpeed * new Vector3(0, turnAmount, 0) * Time.deltaTime);
        rg.MoveRotation(rg.rotation * deltaRotation);

        MoveCamera();
    }


    void MoveCamera()
    {
        mainCamera.position = transform.position;
        mainCamera.rotation = transform.rotation;

        mainCamera.Translate(cameraOffSet);

        mainCamera.LookAt(transform);
    }


    private void EngineAudio()
    {
        // If there is no input (the tank is stationary)...
        if (Mathf.Abs(moveAmount) < 0.1f && Mathf.Abs(turnAmount) < 0.1f)
        {
            // ... and if the audio source is currently playing the driving clip...
            if (m_MovementAudio.clip == m_EngineDriving)
            {
                // ... change the clip to idling and play it.
                m_MovementAudio.clip = m_EngineIdling;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }
        else
        {
            // Otherwise if the tank is moving and if the idling clip is currently playing...
            if (m_MovementAudio.clip == m_EngineIdling)
            {
                // ... change the clip to driving and play.
                m_MovementAudio.clip = m_EngineDriving;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }
    }

}
