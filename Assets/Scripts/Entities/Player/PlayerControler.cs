using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerControler : NetworkBehaviour
{
    GameObject ballePrefab;
    public Transform canonGun;


    private void Start()
    {
        ballePrefab = Resources.Load<GameObject>(GV.BALLE_PREFAB_PATH);
    }


    // Update is called once per frame
    void Update()
    {

        //preciser si le player est local (ton joueur) on fait des truc sinon return
        if (!isLocalPlayer)
        {
            return;
        }

        //player movement 
        float x = Input.GetAxis("Horizontal2") * Time.deltaTime * GV.PLAYER_ROTATION_SPEED;
        float y = Input.GetAxis("Vertical2") * Time.deltaTime * GV.PLAYER_MOVE_SPEED;


        transform.Rotate(0, x, 0);
        transform.Translate(y, 0, 0);

        //shotting system for player
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CmdFire();
        }

    }

    [Command]
    private void CmdFire()
    {
        //create balle
        Quaternion qat = new Quaternion();
        qat.eulerAngles = new Vector3(canonGun.rotation.eulerAngles.x, canonGun.rotation.eulerAngles.y + 90, canonGun.rotation.eulerAngles.z);
        GameObject balle = GameObject.Instantiate<GameObject>(ballePrefab, canonGun.position, qat);
        //add velesity
        balle.GetComponent<Rigidbody>().velocity = balle.transform.forward * GV.BALLE_VELOSITY;

        //create balle in the server 
        NetworkServer.Spawn(balle);

        //destroy balle after 3 secondes
        GameObject.Destroy(balle, 3f);
    }

    public override void OnStartLocalPlayer()
    {
        transform.Find("Body").GetComponent<MeshRenderer>().material = Resources.Load<Material>(GV.PlAYER_BODY_MATERIAL_PATH);
    }
}
