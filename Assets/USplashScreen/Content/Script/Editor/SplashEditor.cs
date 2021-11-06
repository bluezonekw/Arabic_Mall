using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEditorInternal;

[CustomEditor(typeof(USplash))]
public class SplashEditor : Editor
{

    private ReorderableList SoundList;

    void OnEnable()
    {
        SoundList = new ReorderableList(serializedObject, serializedObject.FindProperty("SoundAnimation"), true, true, true, true);
        SoundList.drawHeaderCallback = OnDrawHeader;
        SoundList.drawElementCallback = OnDrawElement;
    }

    void OnDrawHeader(Rect rect)
    {
        EditorGUI.LabelField(rect, "Sound of Animations");
    }

    void OnDrawElement(Rect rect, int index, bool isactive, bool isfocus)
    {
        var element = SoundList.serializedProperty.GetArrayElementAtIndex(index);
        rect.y += 2;
        EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element, GUIContent.none);
    }

    public override void OnInspectorGUI()
    {
        USplash splash = (USplash)target;
        bool allowSceneObjects = !EditorUtility.IsPersistent(target);
        GUILayout.BeginVertical(EditorStyles.helpBox);
        GUILayout.BeginVertical(EditorStyles.helpBox);
        GUILayout.Label("Main Settings", EditorStyles.boldLabel);
        splash.m_Type = (USplash.SplashType)EditorGUILayout.EnumPopup("Type: ", splash.m_Type);
        splash.DelayStart = EditorGUILayout.Slider("Delay Start: ", splash.DelayStart, 0, 5);
        GUILayout.EndVertical();

        if (splash.m_Type == USplash.SplashType.Movie)
        {
            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Label("Movie Settings", EditorStyles.boldLabel);
            splash.Movie = EditorGUILayout.ObjectField("Movie: ", splash.Movie, typeof(RawImage), allowSceneObjects) as RawImage;
            splash.CanvasAlpha = EditorGUILayout.ObjectField("Canvas Alpha: ", splash.CanvasAlpha, typeof(CanvasGroup), allowSceneObjects) as CanvasGroup;
            GUILayout.EndVertical();
        }

        if (splash.m_Type == USplash.SplashType.Animation)
        {
            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Label("Animations Settings", EditorStyles.boldLabel);
            splash.m_animation = EditorGUILayout.ObjectField("Animation: ", splash.m_animation, typeof(Animation), allowSceneObjects) as Animation;
            splash.ShowAnimation = EditorGUILayout.ObjectField("Show Animation Clip: ", splash.ShowAnimation, typeof(AnimationClip), allowSceneObjects) as AnimationClip;
            splash.HideAnimation = EditorGUILayout.ObjectField("Hide Animation Clip: ", splash.HideAnimation, typeof(AnimationClip), allowSceneObjects) as AnimationClip;
            splash.ShowAnimSpeed = EditorGUILayout.Slider("Show Speed: ", splash.ShowAnimSpeed, 0.1f, 10.0f);
            splash.HideAnimSpeed = EditorGUILayout.Slider("Hide Speed: ", splash.HideAnimSpeed, 0.1f, 10.0f);

            GUILayout.EndVertical();

            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Label("Sounds Settings", EditorStyles.boldLabel);
            splash.ShowSound = EditorGUILayout.ObjectField("Show Sound: ", splash.ShowSound, typeof(AudioClip), allowSceneObjects) as AudioClip;
            splash.HideSound = EditorGUILayout.ObjectField("Hide Sound: ", splash.HideSound, typeof(AudioClip), allowSceneObjects) as AudioClip;
            splash.ShowSoundDelay = EditorGUILayout.Slider("Show Delay: ", splash.ShowSoundDelay, 0f, 5.0f);
            splash.HideSoundDelay = EditorGUILayout.Slider("Hide Delay: ", splash.HideSoundDelay, 0f, 5.0f);
            splash.m_volume = EditorGUILayout.Slider("Volumen: ", splash.m_volume, 0f, 1.0f);
            splash.m_pitch = EditorGUILayout.Slider("Pitch: ", splash.m_pitch, 0f, 2.0f);

            serializedObject.Update();
            SoundList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();

            GUILayout.EndVertical();
        }
        GUILayout.EndVertical();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(splash);
        }

    }
}