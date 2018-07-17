using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class TankHealth : NetworkBehaviour
{

    public Slider m_Slider;
    public Image m_FillImage;
    public Color m_FullHealthColor = Color.green;
    public Color m_ZeroHealthColor = Color.red;
    public GameObject m_ExplosionPrefab;


    private AudioSource m_ExplosionAudio;
    private ParticleSystem m_ExplosionParticles;

    [SyncVar]
    public int maxHealth = 100;

    Text gamePlayInformation;

    [SyncVar]
    public int curretHealth;



    private void Awake()
    {
        // Instantiate the explosion prefab and get a reference to the particle system on it.
        m_ExplosionParticles = Instantiate(m_ExplosionPrefab).GetComponent<ParticleSystem>();

        // Get a reference to the audio source on the instantiated prefab.
        m_ExplosionAudio = m_ExplosionParticles.GetComponent<AudioSource>();

        // Disable the prefab so it can be activated when it's required.
        m_ExplosionParticles.gameObject.SetActive(false);
    }


    private void Start()
    {


        curretHealth = maxHealth;

        if (isServer)
            DeatMatchManager.AddTank(this);

        RpcSetHealthUI();
    }

    public void TakeDamage(int damageAmount)
    {

        if (!isServer || curretHealth <= 0)
            return;

        curretHealth -= damageAmount;

        RpcSetHealthUI();

        if (curretHealth <= 0)
        {
            curretHealth = 0;

            RpcDied();

            if (DeatMatchManager.RemoveTankAndChekWinner(this))
            {
                TankHealth tankWinner = DeatMatchManager.GetWinner();
                tankWinner.RpcWon();
                Invoke("BackToLobbyManager", 5);
            }
            return;
        }

    }


    public void AddHealth(int _healthValue)
    {
        curretHealth += _healthValue;
        if (curretHealth >= 100)
            curretHealth = 100;
        RpcSetHealthUI();
    }


    [ClientRpc]
    void RpcDied()
    {
        GetComponent<TankColor>().HideTank();

        if (isLocalPlayer)
        {
            gamePlayInformation = GameObject.FindGameObjectWithTag("GamePlay").GetComponent<Text>();
            gamePlayInformation.text = "DEFEAT";
            GetComponent<TankController>().enabled = false;
            GetComponent<TankFiring>().enabled = false;


            // Move the instantiated explosion prefab to the tank's position and turn it on.
            m_ExplosionParticles.transform.position = transform.position;
            m_ExplosionParticles.gameObject.SetActive(true);

            // Play the particle system of the tank exploding.
            m_ExplosionParticles.Play();

            // Play the tank explosion sound effect.
            m_ExplosionAudio.Play();

        }
    }

    [ClientRpc]
    void RpcWon()
    {
        if (isLocalPlayer)
        {
            gamePlayInformation = GameObject.FindGameObjectWithTag("GamePlay").GetComponent<Text>();
            gamePlayInformation.text = "VICTORY";
        }
    }


    void BackToLobbyManager()
    {
        GameObject.FindObjectOfType<NetworkLobbyManager>().ServerReturnToLobby();
    }

    //[ClientRpc]
    public void RpcSetHealthUI()
    {
        // Set the slider's value appropriately.
        m_Slider.value = curretHealth;

        // Interpolate the color of the bar between the choosen colours based on the current percentage of the starting health.
        m_FillImage.color = Color.Lerp(m_ZeroHealthColor, m_FullHealthColor, curretHealth / maxHealth);
    }

}

