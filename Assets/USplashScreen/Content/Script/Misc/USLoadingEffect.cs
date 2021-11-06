using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class USLoadingEffect : MonoBehaviour
{

    public UIType m_Type = UIType.UGUI;
    public CanvasGroup m_Alpha = null;
    public float m_Speed = 500;
    public bool Loading = false;

    private Image mImage = null;

    public Texture2D LoadingTex = null;
    public float m_Size = 50f;
    private float RotAxis = 0.0f;


    /// <summary>
    /// 
    /// </summary>
    void Awake()
    {
        if (m_Type == UIType.UGUI)
        {
            mImage = this.GetComponent<Image>();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        if (m_Type == UIType.UGUI)
        {
            UGUILoading();
        }
        if (m_Type == UIType.OnGUI)
        {
            if (Loading)
            {
                RotAxis += m_Speed * Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void UGUILoading()
    {
        if (Loading)
        {
            this.transform.Rotate(((Vector3.forward * this.m_Speed) * Time.deltaTime), Space.World);//rotate effect
            Color alpha = new Color(1, 1, 1, 1);
            if (m_Alpha == null) { alpha = mImage.color; } else { alpha.a = m_Alpha.alpha; }
            if (alpha.a < 1f)
            {
                alpha.a = Mathf.Lerp(alpha.a, 1f, Time.deltaTime * 2);//fade out
            }
            if (m_Alpha == null) { mImage.color = alpha; } else { m_Alpha.alpha = alpha.a; }
        }
        else
        {
            Color alpha = new Color(1, 1, 1, 1);
            if (m_Alpha == null) { alpha = mImage.color; } else { alpha.a = m_Alpha.alpha; }
            if (alpha.a > 0f)
            {
                alpha.a = Mathf.Lerp(alpha.a, 0f, Time.deltaTime * 2);//fade in
            }
            if (m_Alpha == null) { mImage.color = alpha; } else { m_Alpha.alpha = alpha.a; }

        }
    }
    /// <summary>
    /// 
    /// </summary>
    void OnGUI()
    {
        if (m_Type != UIType.OnGUI)
            return;

        if (Loading)
        {
            Vector2 pivot = new Vector2(10 + (m_Size / 2), Screen.height - (m_Size / 2));
            GUI.Label(new Rect((10 + m_Size) + 10, Screen.height - 25, 200, 30), "Loading...");
            GUIUtility.RotateAroundPivot(RotAxis % 360, pivot);
            GUI.DrawTexture(new Rect( 10 , (Screen.height - m_Size)  , m_Size, m_Size), LoadingTex);
           
        }
    }

    [System.Serializable]
    public enum UIType
    {
        UGUI,
        OnGUI,
    }
}