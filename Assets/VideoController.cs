using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
public class VideoController : MonoBehaviour
{
    public RenderTexture rend;
    public bool isPlaying;
    private VideoPlayer videoPlayer;
    private AudioSource audioSource;
    // Use this for initialization
    void Start()
    {

        videoPlayer = this.GetComponent<VideoPlayer>();
        //Add AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        //Disable Play on Awake for both Video and Audio
        videoPlayer.playOnAwake = false;
        audioSource.playOnAwake = false;
        audioSource.Pause();
        
        // Video clip from Url
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = "https://drive.google.com/drive/folders/1Z4qnKRaojN7--f3uRjO3mbNGYKOUsU9T";
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        videoPlayer.targetTexture = rend;
        //Set Audio Output to AudioSource
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        //Assign the Audio from Video to AudioSource to be played
        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.SetTargetAudioSource(0, audioSource);
        //Set video To Play then prepare Audio to prevent Buffering        
        videoPlayer.Prepare();

       
    }
    public void play()
    {
        isPlaying = !isPlaying;
    }
    // Update is called once per frame
    void Update()
    {
        if (isPlaying)
        {
            //Play Video
            videoPlayer.Play();
            //Play Sound
            audioSource.Play();
        }
        else
        {
            //Play Video
            videoPlayer.Pause();
            //Play Sound
            audioSource.Pause();
        }
       

    }
}