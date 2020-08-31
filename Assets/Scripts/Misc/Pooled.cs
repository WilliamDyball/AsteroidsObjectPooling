using UnityEngine;
//Class to trigger a function when spawning a pooled object and reset them all when the player is hit
public class Pooled : MonoBehaviour
{
    //Used when reseting the level currently deactivates all objects.
    public static bool bReset;
    public virtual void OnObjectSpawn() { }
}