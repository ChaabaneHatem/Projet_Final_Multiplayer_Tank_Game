using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScript : MonoBehaviour
{

    public Transform powerUpLocation;
    public GameObject HealthPowerUp;
    public GameObject ammoPowerUp;

    // Use this for initialization
    void Start()
    {
        PowerUpsManager.Instance.Init(powerUpLocation, HealthPowerUp, ammoPowerUp);
    }

    // Update is called once per frame
    void Update()
    {
        PowerUpsManager.Instance.UpdatePowerUpManager(Time.deltaTime);
    }
}
