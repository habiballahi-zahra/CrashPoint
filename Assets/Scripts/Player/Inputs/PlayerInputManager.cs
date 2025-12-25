using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-3)]
public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager Instance;
    public PlayerControls PlayerControls { get; private set; }

    // یک متغیر برای بررسی وضعیت Restart
    private bool isApplicationQuitting = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        PlayerControls = new PlayerControls();
        PlayerControls.Enable();
    }

    private void OnDisable()
    {
        // خط 32 - مشکل اصلی اینجاست
        // فقط اگر بازی در حال بسته شدن نیست و PlayerControls null نیست
        if (!isApplicationQuitting && PlayerControls != null)
        {
            PlayerControls.Disable();
        }
    }
    
    // این متد وقتی بازی در حال بسته شدن است صدا زده می‌شود
    private void OnApplicationQuit()
    {
        isApplicationQuitting = true;
    }
    
    // این متد وقتی GameObject در حال از بین رفتن است صدا زده می‌شود
    private void OnDestroy()
    {
        // اگر این Instance اصلی است، آن را null کن
        if (Instance == this)
        {
            Instance = null;
        }
        
        // PlayerControls را تمیز کن
        if (PlayerControls != null)
        {
            PlayerControls.Disable();
        }
    }
}