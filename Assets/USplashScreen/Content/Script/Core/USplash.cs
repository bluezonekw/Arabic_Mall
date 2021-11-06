using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class USplash : MonoBehaviour
{

    [Header("Splash Settings")]
    public float DelayStart = 0.5f;
    public SplashType m_Type = SplashType.Animation;
    public RawImage Movie;
    public CanvasGroup CanvasAlpha;[Header("Animation Settings")]
    public Animation m_animation;
    public AnimationClip ShowAnimation;
    public AnimationClip HideAnimation;
    [Range(0.1f, 10.0f)]
    public float ShowAnimSpeed = 1.0f;
    [Range(0.1f, 10.0f)]
    public float HideAnimSpeed = 1.0f;
    [Header("Sound Settings")]
    public AudioClip ShowSound;
    public AudioClip HideSound;[Range(0.0f, 5.0f)]
    public float ShowSoundDelay = 0.0f;[Range(0.0f, 5.0f)]
    public float HideSoundDelay = 0.0f;[Range(0.0f, 1.0f)]
    public float m_volume;[Range(0.0f, 2.0f)]
    public float m_pitch = 1.0f;
    public AudioClip[] SoundAnimation;

    /// <summary>
    /// 
    /// </summary>
    void OnEnable()
    {
        //On this is active, start function
        StartCoroutine(ShowCorrutine());
    }

    /// <summary>
    /// Call this when splash if end for hide it.
    /// </summary>
    public void Hide()
    {
        if (m_Type == SplashType.Animation)
        {
            StartCoroutine(HideCorrutine());
        }
        else if (m_Type == SplashType.Movie)
        {
            StartCoroutine(FadeSplash());
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator ShowCorrutine()
    {
        //If have delay for start, wait for it pass.
        if (DelayStart > 0.0f)
        {
            yield return new WaitForSeconds(DelayStart);
        }
        if (m_Type == SplashType.Animation)
        {
            if (ShowSound)
            {
                PlayAudioClip(ShowSound, m_volume, m_pitch, ShowSoundDelay);
            }
            if (m_animation != null)
            {
                m_animation[ShowAnimation.name].speed = ShowAnimSpeed;
                m_animation.Play(ShowAnimation.name);
            }
        }
        else if (m_Type == SplashType.Movie)
        {
#if !UNITY_ANDROID && !UNITY_IPHONE
            MovieTexture movie = (MovieTexture)Movie.mainTexture;
            GetComponent<AudioSource>().clip = movie.audioClip;
            GetComponent<AudioSource>().Play();
            movie.Play();
#else
            Debug.LogWarning("IOS and Android doesn't support movie texture!");
#endif
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator HideCorrutine()
    {
        if (HideSound)
        {
            PlayAudioClip(HideSound, m_volume, m_pitch, HideSoundDelay);
        }
        if (m_animation != null)
        {
            m_animation[HideAnimation.name].speed = HideAnimSpeed;
            m_animation.Play(HideAnimation.name);
            yield return new WaitForSeconds(m_animation[HideAnimation.name].length);
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public float GetLeghtMovie
    {
        get
        {
            float l = 0;
            //Android and IOS not ssuported Movie Texture
#if !UNITY_ANDROID && !UNITY_IPHONE
            MovieTexture movie = (MovieTexture)Movie.mainTexture;
            l = movie.duration;
#endif
            return l;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public IEnumerator FadeSplash()
    {
        while (CanvasAlpha.alpha > 0)
        {
            CanvasAlpha.alpha -= Time.deltaTime;
            yield return null;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="volume"></param>
    /// <param name="pitch"></param>
    /// <returns></returns>
    AudioSource PlayAudioClip(AudioClip clip, float volume, float pitch, float delay = 0.0f)
    {
        GameObject go = new GameObject("One shot audio");
        if (Camera.main != null)
        {
            go.transform.position = Camera.main.transform.position;
        }
        else
        {
            go.transform.position = Camera.current.transform.position;
        }
        AudioSource source = go.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        if (delay > 0.0f)
        {
            source.PlayDelayed(delay);
        }
        else
        {
            source.Play();
        }
        Destroy(go, clip.length);
        return source;
    }

    /// <summary>
    /// 
    /// </summary>
    public void PlaySound(int id)
    {
        if (id <= SoundAnimation.Length)
        {
            PlayAudioClip(SoundAnimation[id], m_volume, m_pitch);
        }
    }

    [System.Serializable]
    public enum SplashType
    {
        Animation,
        Movie,
    }
}