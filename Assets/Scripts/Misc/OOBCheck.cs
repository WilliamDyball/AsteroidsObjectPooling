using UnityEngine;

//Out of bounds check to allow the player and asteroids to wrap around and stop the bullets.
public class OOBCheck : MonoBehaviour
{
    enum Bound { Vertical, Horizontal };
    [SerializeField] Bound bound;

    public float test = -.9f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.gameObject.CompareTag("Player") || other.transform.root.gameObject.CompareTag("Asteroid"))
        {
            Vector3 v3Temp = other.transform.root.position;
            switch (bound)
            {
                case Bound.Vertical:
                    v3Temp.z = other.transform.root.position.z * -.9f;
                    other.transform.root.position = v3Temp;
                    break;
                case Bound.Horizontal:
                    v3Temp.x = other.transform.root.position.x * -.85f;
                    other.transform.root.position = v3Temp;
                    break;
            }
        }
        if (other.transform.root.gameObject.CompareTag("Bullet"))
        {
            other.transform.root.gameObject.SetActive(false);
        }
    }
}
