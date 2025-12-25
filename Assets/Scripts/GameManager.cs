using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Over UI")]
    public GameObject gameOverPanel;
    public Button restartButton;
    
    [Header("Player Reference")]
    public GameObject player; // بازیکن را اینجا وصل کن
    
    // تابعی که هنگام Awake اجرا می‌شود
    private void Awake()
    {
        // Pattern Singleton برای داشتن فقط یک GameManager
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // تابعی که هنگام Start اجرا می‌شود
    private void Start()
    {
        // اگر پنل GameOver وجود دارد، آن را مخفی کن
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        // اگر دکمه Restart وجود دارد، به آن گوش بده
        if (restartButton != null)
            restartButton.onClick.AddListener(SafeRestartLevel);
            
        // اگر بازیکن در Inspector وصل نشده، آن را پیدا کن
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    // تابع نمایش پنل باخت
    public void ShowGameOver()
    {
        // زمان بازی را متوقف کن
        Time.timeScale = 0f;
        
        // پنل باخت را نشان بده
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
            
        // قبل از Restart، کنترل‌های بازیکن را غیرفعال کن
        DisablePlayerControls();
    }

    // تابع غیرفعال کردن کنترل‌های بازیکن
    void DisablePlayerControls()
    {
        if (player != null)
        {
            // PlayerInputManager را پیدا کن و غیرفعال کن
            var inputManager = player.GetComponent<PlayerInputManager>();
            if (inputManager != null && inputManager.PlayerControls != null)
            {
                // کنترل‌های بازیکن را غیرفعال کن
                inputManager.PlayerControls.Disable();
                
                // همچنین کامپوننت PlayerInputManager را غیرفعال کن
                inputManager.enabled = false;
            }
        }
    }

    // تابع شروع مجدد امن
    void SafeRestartLevel()
    {
        // با کوروتین Restart کن تا خطا ندهد
        StartCoroutine(RestartCoroutine());
    }

    // کوروتین برای شروع مجدد امن
    IEnumerator RestartCoroutine()
    {
        Debug.Log("شروع فرآیند Restart امن");
        
        // 1. اول پنل را مخفی کن
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        
        // 2. زمان بازی را به حالت عادی برگردان
        Time.timeScale = 1f;
        
        // 3. یک فریم صبر کن
        yield return null;
        
        // 4. PlayerInputManager.Instance را بررسی و تمیز کن
        if (PlayerInputManager.Instance != null)
        {
            // اگر PlayerControls وجود دارد، آن را غیرفعال کن
            if (PlayerInputManager.Instance.PlayerControls != null)
            {
                PlayerInputManager.Instance.PlayerControls.Disable();
            }
            
            // Instance را از بین ببر چون DontDestroyOnLoad دارد
            Destroy(PlayerInputManager.Instance.gameObject);
            PlayerInputManager.Instance = null;
        }
        
        // 5. کمی بیشتر صبر کن
        yield return new WaitForSeconds(0.05f);
        
        // 6. حالا صحنه را دوباره بارگذاری کن
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}