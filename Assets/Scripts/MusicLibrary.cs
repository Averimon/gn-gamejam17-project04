using UnityEngine;

[System.Serializable]
public struct MusicTrack
{
    public string trackName;
    public AudioClip audioClip;
}

public class MusicLibrary : MonoBehaviour
{
    public MusicTrack[] tracks;

    public AudioClip GetTrackByName(string name)
    {
        foreach (var track in tracks)
        {
            if (track.trackName == name)
            {
                return track.audioClip;
            }
        }
        return null;
    }
}
