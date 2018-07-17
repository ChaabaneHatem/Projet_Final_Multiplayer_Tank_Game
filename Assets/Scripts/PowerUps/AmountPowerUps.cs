using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class AmountPowerUps : NetworkBehaviour
{

    private int AmountValue;


    private void Start()
    {
        AmountValue = Random.Range(3, 11);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.GetComponent<TankController>())
            {
                TankFiring tankControlFire = other.GetComponent<TankFiring>();
                tankControlFire.AddAmount(AmountValue);
                tankControlFire.ammoInfo = GameObject.FindGameObjectWithTag("Ammo").GetComponent<Text>();
                tankControlFire.ammoInfo.text = "Ammo : " + tankControlFire.amount;
            }
            NetworkServer.Destroy(gameObject);
            PowerUpsManager.Instance.RemovePowerUp(gameObject);
        }
    }

}
