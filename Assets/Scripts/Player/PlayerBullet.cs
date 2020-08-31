using UnityEngine;

//A single pooled bullet
public class PlayerBullet : Pooled
{
    [SerializeField] private float fSpeed = 15f;

    //Called when an object is spawned from the pool
    override public void OnObjectSpawn()
    { }

    private void Update()
    {
        if (bReset)
        {
            gameObject.SetActive(false);
        }
        //Constant movement in a forward direction
        transform.Translate(Vector3.forward * fSpeed * Time.deltaTime);
    }
}