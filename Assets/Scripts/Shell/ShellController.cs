using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ShellController : NetworkBehaviour
{

    [SerializeField]
    float shellLifeTime = 2f;
    [SerializeField]
    bool danger = false;

    //[SerializeField] bool isDeathMatch = false;

    bool isAlive = true;
    float currentTime;
    public ParticleSystem explosionParticule;
    MeshRenderer shellRenderer;


    private void Start()
    {
        //explosionParticule = GetComponentInChildren<ParticleSystem>();
        shellRenderer = GetComponent<MeshRenderer>();

        explosionParticule = Instantiate(explosionParticule).GetComponent<ParticleSystem>();
        explosionParticule.gameObject.transform.SetParent(transform);
        explosionParticule.gameObject.transform.position = transform.position;
        explosionParticule.gameObject.SetActive(false);

        Physics.IgnoreLayerCollision(9, 10);
    }

    [ServerCallback]
    private void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= 0.1)
            Physics.IgnoreLayerCollision(9, 10, false);

        if (currentTime >= shellLifeTime)
            NetworkServer.Destroy(gameObject);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (!isAlive)
            return;


        isAlive = false;

        shellRenderer.enabled = false;

        explosionParticule.gameObject.SetActive(true);
        explosionParticule.Play(true);
        //Debug.Log("shell explode");


        //if (!isServer)
        //return;

        if (!danger || collision.gameObject.tag != "Player")
            return;

        TankHealth health = collision.gameObject.GetComponent<TankHealth>();

        if (health != null)
            health.TakeDamage(10);
    }

}
