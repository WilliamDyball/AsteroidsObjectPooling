using System.Collections;
using UnityEngine;

public class PlayerCont : MonoBehaviour
{
    //The transform of the start point for the player.
    [SerializeField] private Transform transStartPos;
    //Used to check if the player is onscreen using the isvisible property of the meshrenderer.
    [SerializeField] private MeshRenderer mRend;

    //Movement
    private Rigidbody rb;
    [SerializeField] private float fThrust = 12f;
    [SerializeField] private float fRotSpeed = 180f;
    [SerializeField] private float fMaxSpeed = 20f;

    //Shooting
    [SerializeField] private Transform transFirePos;
    [SerializeField] private float fFireRate;
    [SerializeField] private float fNextFire;

    //Powerups (Not fully implemented)
    private bool bImmune;
    private float fImmuneMax;
    private float fImmuneTimer;

    private bool bResapawn;

    private void Awake()
    {
        if (TryGetComponent(out Rigidbody _rb))
        {
            rb = _rb;
        }
        else
        {
            Debug.LogError("Failed to get Rigidbody on player.");
        }
    }

    private void Update()
    {
        if (bResapawn)
        {
            return;
        }

        PlayerInput();

        if (fImmuneTimer >= 0)
        {
            fImmuneTimer -= Time.deltaTime;
            bImmune = true;
        }
        else
        {
            bImmune = false;
        }
    }

    private void PlayerInput()
    {
        transform.Rotate(0, Input.GetAxis("Horizontal") * fRotSpeed * Time.deltaTime, 0);
        rb.AddForce(transform.forward * fThrust * Input.GetAxis("Vertical"));
        rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -fMaxSpeed, fMaxSpeed), 0, Mathf.Clamp(rb.velocity.z, -fMaxSpeed, fMaxSpeed));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //FireBullet from pool
            PoolsManager.instance.SpawnObjFromPool("BulletBase", transFirePos.position, transFirePos.rotation);
        }
    }

    public void TryKillPlayer()
    {
        if (!mRend.isVisible || bResapawn || bImmune)
        {
            return;
        }
        GameManager.instance.PlayerDeath();
        StartCoroutine(IERespawn());
    }

    private readonly WaitForSeconds respawnDuration = new WaitForSeconds(2f);
    //Respawn and make immune to damage for 2 seconds on respawn.
    private IEnumerator IERespawn()
    {
        transform.position = transStartPos.position;
        rb.velocity = Vector3.zero;
        bResapawn = true;
        yield return respawnDuration;
        bResapawn = false;
    }

    public void SetImmune()
    {
        fImmuneTimer = fImmuneMax;
    }

    public void IncreaseFireRate()
    {
        fFireRate -= .02f;
        Mathf.Clamp(fFireRate, 0.15f, 1f);
    }
}
