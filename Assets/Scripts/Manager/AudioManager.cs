using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource BGM;
    public AudioSource SFX;
    public AudioClip[] BGMArray;
    public AudioClip[] SFXArray;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void PlaySFX(int index)
    {
        SFX.clip = SFXArray[index];
        SFX.Play();
    }
}
