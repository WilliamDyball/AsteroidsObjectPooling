using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private float fSpeed = 3f;

    public enum PowerUpType { FireRate, Immune } //Ideas for extra power ups; Laser, Spread shot, Multishot in straight lines

    public PowerUpType type = PowerUpType.FireRate;

    private void OnEnable()
    {
        //Set random rotation
        switch (Random.Range(0, 2))
        {
            case 0:
                type = PowerUpType.FireRate;
                break;

            case 1:
                type = PowerUpType.Immune;
                break;

            default:
                break;
        }
    }

    private void Update()
    {
        transform.Translate((transform.forward * fSpeed) * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (type)
            {
                case PowerUpType.FireRate:
                    //Trigger power up
                    other.gameObject.GetComponent<PlayerCont>().IncreaseFireRate();
                    Destroy(this.gameObject);
                    break;

                case PowerUpType.Immune:
                    //Trigger power up
                    other.gameObject.GetComponent<PlayerCont>().SetImmune();
                    Destroy(this.gameObject);
                    break;
            }
        }
    }
}
