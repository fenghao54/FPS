
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayStatics : MonoBehaviour
{

    public static PlayerState LocalPlayer
    {
        get
        {
            if (player == null)
                player = FindObjectOfType<PlayerState>();
            return player;
        }
    }

    public static InputManager InputManager
    {
        get
        {
            if (m_InputManager == null)
                m_InputManager = FindObjectOfType<InputManager>();
            return m_InputManager;
        }
    }

    private static InputManager m_InputManager;
    private static PlayerState player;


    private static AudioSource audioSource2D;

    public static void Play2D(AudioClip clip, float volume)
    {
        if (audioSource2D)
            audioSource2D.PlayOneShot(clip, volume);
    }


    public static SurfaceData SurfaceDatabase { get; private set; }

    [SerializeField]
    private SurfaceData m_SurfaceDatabase;

    private void Awake()
    {
        audioSource2D = GetComponent<AudioSource>();
        SurfaceDatabase = m_SurfaceDatabase;
        DontDestroyOnLoad(gameObject);
    }
}
