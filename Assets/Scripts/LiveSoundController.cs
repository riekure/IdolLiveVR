using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveSoundController : MonoBehaviour {

    public AudioClip audioClip;  // 再生する音源
    private AudioSource audioSource;

    // Use this for initialization
    void Start () 
    {
        StartCoroutine("SoundPlay");
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.clip = audioClip;
    }
    
    // Update is called once per frame
    void Update () {
        
    }

    IEnumerator SoundPlay()
    {
        yield return new WaitForSeconds(1.9f);
        Debug.Log("1.9 minutes");
        audioSource.Play();  // 音を再生
    }
}
