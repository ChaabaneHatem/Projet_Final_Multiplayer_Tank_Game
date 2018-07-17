using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PowerUpsManager
{

    #region Singleton
    private static PowerUpsManager instance;

    private PowerUpsManager() { }

    public static PowerUpsManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PowerUpsManager();
            }
            return instance;
        }
    }
    #endregion



    private Transform PowerUpsLocations;
    public GameObject HealthPowerUp;
    public GameObject ammoPowerUp;
    private List<Transform> locations;

    private List<GameObject> listPowerUps;
    private float currentTime;
    private int randomLocation;

    // Use this for initialization
    public void Init(Transform _locations, GameObject _HealthPowerUp, GameObject _ammoPowerUp)
    {
        PowerUpsLocations = _locations;
        HealthPowerUp = _HealthPowerUp;
        ammoPowerUp = _ammoPowerUp;
        listPowerUps = new List<GameObject>();
        locations = new List<Transform>();
        foreach (Transform location in PowerUpsLocations)
        {
            locations.Add(location);
        }
        randomLocation = 0;
        currentTime = 0;
    }

    // Update is called once per frame
    public void UpdatePowerUpManager(float dt)
    {
        currentTime += dt;

        if (listPowerUps.Count == 0 && currentTime >= 3)
        {
            randomLocation = Random.Range(0, locations.Count);
            int randomPowerUp = Random.Range(0, 2);
            GameObject powerUp = null;

            switch (randomPowerUp)
            {
                case 0:
                    powerUp = GameObject.Instantiate(ammoPowerUp) as GameObject;
                    powerUp.transform.position = locations[randomLocation].position;
                    break;
                case 1:
                    powerUp = GameObject.Instantiate(HealthPowerUp) as GameObject;
                    powerUp.transform.position = locations[randomLocation].position;
                    break;
            }
            NetworkServer.Spawn(powerUp);

            AddPowerUp(powerUp);
            currentTime = 0;
        }


    }


    public void AddPowerUp(GameObject _powerUp)
    {
        listPowerUps.Add(_powerUp);
    }


    public void RemovePowerUp(GameObject _powerUpToRemove)
    {
        listPowerUps.Remove(_powerUpToRemove);
    }
}
