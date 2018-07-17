using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HealthPowerUp : NetworkBehaviour
{

    private int healthValue;


    private void Start()
    {
        healthValue = 30;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.GetComponent<TankController>())
            {
                TankHealth tankHealth = other.GetComponent<TankHealth>();
                tankHealth.AddHealth(healthValue);
            }

            NetworkServer.Destroy(gameObject);
            PowerUpsManager.Instance.RemovePowerUp(gameObject);
        }
    }


}
