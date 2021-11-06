using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_5_3
using UnityEngine.SceneManagement;
#endif

public class USplashScreen : MonoBehaviour {

    public string m_NextLevel = "MainMenu";
    public float StartWait = 0.5f;
    [System.Serializable]
    public class USS
    {
        public Texture2D SplashLogo;
        public Rect CustomRect = new Rect(Screen.width / 2 - 200, Screen.height / 2 - 200, 400, 400);
        [Header("Time to Show")]
        [Range(1.0f, 10.0f)]
        public float m_time = 1.0f;
        [Range(1.0f, 15.0f)]
        public float m_fadeinspeed = 2.0f;
        [Range(1.0f, 15.0f)]
        public float m_fadeoutspeed = 2.0f;
        [Range(0.1f,5.0f)]
        public float m_alpha = 2.0f;
        [Range(1.0f, 5.0f)]
        public float m_TimeForNext = 1.0f;
        [Header("Audio")]
        public AudioClip m_splashIn;
        public AudioClip m_splashOut;
        [Range(0.0f,1.0f)]
        public float m_volume;
        [Range(0.0f,2.0f)]
        public float m_pitch;
        public bool isFullScreen = true;
        [HideInInspector]
        public bool m_show = false;
    }
    public List<USS> m_uss = new List<USS>();
    private int t_current = 0;

	// Use this for initialization
	void Start () {
        StartCoroutine(SplashCorrutine());
	}

    void OnGUI()
    {
        for (int i = 0; i < m_uss.Count; i++)
        {
            if (m_uss[i].m_show == true)
            {
                if (m_uss[i].isFullScreen)
                {
                    GUI.color = new Color(1, 1, 1, m_uss[i].m_alpha);
                    GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), m_uss[i].SplashLogo);
                    GUI.color = Color.white;
                }
                else
                {
                    GUI.color = new Color(1, 1, 1, m_uss[i].m_alpha);
                    GUI.DrawTexture(new Rect(Screen.width / 2 + m_uss[i].CustomRect.x, Screen.height / 2 + m_uss[i].CustomRect.y,
                     m_uss[i].CustomRect.width,m_uss[i].CustomRect.height), m_uss[i].SplashLogo);
                    GUI.color = Color.white;
                }
            }
        }
    }

    IEnumerator SplashCorrutine()
    {
        yield return new WaitForSeconds(StartWait);

        for (int i = 0; i < m_uss.Count; i++)
        {
            float t_alpha = m_uss[t_current].m_alpha;
            m_uss[t_current].m_alpha = 0;
            m_uss[t_current].m_show = true;
            if (m_uss[t_current].m_splashIn != null)
            {
                PlayAudioClip(m_uss[t_current].m_splashIn, m_uss[t_current].m_volume, m_uss[t_current].m_pitch);
            }
            while (m_uss[t_current].m_alpha < t_alpha)
            {
                m_uss[t_current].m_alpha += Time.deltaTime * m_uss[t_current].m_fadeinspeed;
                yield return 0;
            }
            yield return new WaitForSeconds(m_uss[i].m_time);
            if (m_uss[t_current].m_splashOut != null)
            {
                PlayAudioClip(m_uss[t_current].m_splashOut, m_uss[t_current].m_volume, m_uss[t_current].m_pitch);
            }
            while (m_uss[t_current].m_alpha > 0.0f)
            {
                m_uss[t_current].m_alpha -= Time.deltaTime * m_uss[t_current].m_fadeoutspeed;
                yield return 0;
            }
            yield return new WaitForSeconds(m_uss[t_current].m_TimeForNext);
            if (t_current >= m_uss.Count - 1)
            {
                yield return new WaitForSeconds(1f);
#if UNITY_5_3
                SceneManager.LoadScene(m_NextLevel);
#else
                Application.LoadLevel(m_NextLevel);
#endif
            }
            else
            {
                t_current++;
            }

        }
    }

    AudioSource PlayAudioClip(AudioClip clip, float volume,float pitch)
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
        source.Play();
        Destroy(go, clip.length);
        return source;
    }
}
