using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balle : MonoBehaviour
{

    private void OnCollisionEnter(Collision colli)
    {
        Debug.Log("destroyed");
        if (colli != null)
        {

            Health health = colli.transform.parent.GetComponent<Health>();
            if (health != null)
            {
                health.GetDamage(GV.BALLE_DAMAGE);
            }
        }

        GameObject.Destroy(gameObject);
    }
}
