using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public GameObject blockLand;
    public GameObject playerJump;
    public GameObject playerDeath;

    public void CreateSound(GameObject sound)
    {
        Debug.Log(sound.name);
        Instantiate(sound);
    }
}
