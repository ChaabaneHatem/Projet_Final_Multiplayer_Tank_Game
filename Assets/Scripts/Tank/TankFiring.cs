using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TankFiring : NetworkBehaviour
{

    [SerializeField] float power = 800f;
    public int amount = 10;
    [SerializeField] GameObject shellPrefab;
    [SerializeField] Transform gunCanon;


    private int playerID;

    public Text ammoInfo;

    private void Reset()
    {
        gunCanon = transform.Find("GunCanon");
    }

    // Use this for initialization
    void Start()
    {
        playerID = transform.GetComponent<TankController>().playerID;
        ammoInfo = GameObject.FindGameObjectWithTag("Ammo").GetComponent<Text>();
        ammoInfo.text = "Ammo : " + amount;
    }

    // Update is called once per frame
    void Update()
    {

        if (!isLocalPlayer)
            return;

        if (CrossPlatformInputManager.GetButtonDown("Fire" + playerID))
        {
            if (amount > 0)
            {
                CmdFire();
                ammoInfo = GameObject.FindGameObjectWithTag("Ammo").GetComponent<Text>();
                amount--;
                ammoInfo.text = "Ammo : " + amount;
            }
        }


        //for debuging 
        if (Input.GetKeyDown(KeyCode.M))
        {
            AddAmount(10);
            ammoInfo = GameObject.FindGameObjectWithTag("Ammo").GetComponent<Text>();
            ammoInfo.text = "Ammo : " + amount;
        }

    }


    [Command]
    void CmdFire()
    {
        GameObject shellInstance = GameObject.Instantiate<GameObject>(shellPrefab, gunCanon.position, gunCanon.rotation);

        shellInstance.GetComponent<Rigidbody>().AddForce(gunCanon.forward * power);

        NetworkServer.Spawn(shellInstance);
    }


    public void AddAmount(int valueToAdd)
    {
        amount += valueToAdd;
        if (amount >= 10)
            amount = 10;
    }

}
