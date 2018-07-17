using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Health : MonoBehaviour
{

    public float currentHealth;
    public RectTransform foreGround;


    // Use this for initialization
    void Start()
    {
        currentHealth = GV.MAX_HEALTH_PLAYER;
    }


    public void GetDamage(float amountDamage)
    {
        Debug.Log("Get damage Called");
        currentHealth -= amountDamage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Dead");
        }
        foreGround.sizeDelta = new Vector2(currentHealth * 2, foreGround.sizeDelta.y);
    }

}
