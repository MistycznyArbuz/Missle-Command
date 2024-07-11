using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] audioTracks;

    public AudioClip RandomAudioTrack()
    {
        return audioTracks[Random.Range(0, audioTracks.Length)];
    }

    public void Start()
    {
        StartCoroutine(WaitForClipEnd());

        audioSource.PlayOneShot(RandomAudioTrack());
    }

    public IEnumerator WaitForClipEnd()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (!audioSource.isPlaying)
                audioSource.PlayOneShot(RandomAudioTrack());
        }
    }
}
