using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DeatMatchManager
{

    static List<TankHealth> tanks = new List<TankHealth>();


    public static void AddTank(TankHealth _tank)
    {
        tanks.Add(_tank);
    }


    public static bool RemoveTankAndChekWinner(TankHealth _tankToRemove)
    {
        tanks.Remove(_tankToRemove);


        if (tanks.Count == 1)
            return true;

        return false;
    }


    public static TankHealth GetWinner()
    {

        if (tanks.Count != 1)
            return null;
        return tanks[0];
    }


}
