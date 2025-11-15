using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [SerializeField] private MusicLibrary musicLibrary;
    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void PlayMusic(string trackName, float fadeDuration = 0.5f)
    {
        AudioClip nextTrack = musicLibrary.GetTrackByName(trackName);
        if (nextTrack != null && audioSource.clip != nextTrack)
        {
            StartCoroutine(AnimateMusicCrossfade(nextTrack, fadeDuration));
        }
    }

    IEnumerator AnimateMusicCrossfade(AudioClip nextTrack, float fadeDuration = 0.5f)
    {
        float percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime / fadeDuration;
            audioSource.volume = Mathf.Lerp(1, 0, percent);
            yield return null;
        }

        audioSource.clip = nextTrack;
        audioSource.Play();

        percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime / fadeDuration;
            audioSource.volume = Mathf.Lerp(0, 1, percent);
            yield return null;
        }
    }
}
