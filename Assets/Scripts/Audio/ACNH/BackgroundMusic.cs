using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] List<AudioClip> hourlyMusic = new List<AudioClip>();
    [SerializeField] AudioSource audioSource;

    int clipIndex = 0;

    private void Start()
    {
        clipIndex = Random.Range(0, hourlyMusic.Count);
        audioSource.clip = hourlyMusic[clipIndex];
        audioSource.Play();
        StartCoroutine(ChangeMusic());
    }

    private IEnumerator ChangeMusic()
    {
        yield return new WaitForSeconds(60); // 20 min

        clipIndex++;
        audioSource.clip = hourlyMusic[clipIndex];
        audioSource.Play();


    }

}
