using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TankColor : NetworkBehaviour
{

    [SyncVar]
    public Color tankColorToChange;

    MeshRenderer[] rends;



    private void Start()
    {
        rends = GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < rends.Length; i++)
        {
            rends[i].material.color = tankColorToChange;
        }
    }

    //private void Update()
    //{
    //    Debug.Log(transform.GetComponent<TankHealth>().curretHealth);
    //}


    public void HideTank()
    {

        for (int i = 0; i < rends.Length; i++)
        {
            rends[i].material.color = Color.clear;
        }
    }

}
