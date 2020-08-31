using UnityEngine;
public class Asteroid : Pooled
{
    //Base scale to use as initial scale
    private readonly Vector3 BaseScale = new Vector3(2, 2, 2);

    //Used for both the scale and the asteroid generation
    private int iScaleGen = 1;
    private float fSpeed = 5f;

    private bool bOnScreen;
    [SerializeField] private GameObject goHitEffect;

    //Called when an object is spawned from the pool
    override public void OnObjectSpawn()
    {
        fSpeed *= iScaleGen;
        transform.localScale = BaseScale / iScaleGen;

        //Set random rotation
        transform.Rotate(0, Random.Range(0, 360), 0);

        if (GameManager.instance != null)
        {
            GameManager.instance.iSpawnedAsteroids++;
        }
    }

    private void Update()
    {
        if (bReset)
        {
            gameObject.SetActive(false);
        }
        transform.Translate((transform.forward * fSpeed) * Time.deltaTime);
    }

    //Prevents them being shot before being on screen
    private void OnBecameVisible()
    {
        if (!bOnScreen)
        {
            bOnScreen = true;
        }
    }

    public void SetGeneration(int _iGen)
    {
        iScaleGen = _iGen;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!bOnScreen)
        {
            return;
        }
        if (other.transform.root.CompareTag("Player"))
        {
            if (other.transform.root.gameObject.TryGetComponent<PlayerCont>(out PlayerCont player))
            {
                player.TryKillPlayer();
            }
        }
        else if (other.transform.root.CompareTag("Bullet"))
        {
            other.transform.root.gameObject.SetActive(false);
            GameManager.instance.IncrementScore();
            //TODO: Spawn Powerup
            HitByBullet();
        }
    }

    public void HitByBullet()
    {
        //Instantiate(goHitEffect, transform.position, transform.rotation, null);
        if (iScaleGen < 3)
        {
            //Create 2 smaller ones increase speed
            PoolsManager.instance.SpawnObjFromPool("AsteroidBase", transform.position + (Vector3.right * 2), transform.rotation, iScaleGen + 1);
            PoolsManager.instance.SpawnObjFromPool("AsteroidBase", transform.position - (Vector3.right * 2), transform.rotation, iScaleGen + 1);
        }
        gameObject.SetActive(false);
    }
}