using UnityEngine;
using System.Collections;

public class PlayerHitReaction : MonoBehaviour
{
    public float hitStunTime = 0.2f;  // زمان استون بعد از ضربه
    public float deathDelay = 3f;     // زمان قبل از نمایش پنل باخت
    
    private Animator anim;
    private Health health;
    private MonoBehaviour[] allScripts;
    private bool isPlayingDeathAnim = false;
    private bool isPlayingHitAnim = false;
    
    void Start()
    {
        anim = GetComponent<Animator>();
        health = GetComponent<Health>();
        
        // همه اسکریپت‌ها را ذخیره کن (به جز خودم)
        allScripts = GetComponents<MonoBehaviour>();
        
        // به رویدادها گوش بده
        health.onHit += OnHit;
        health.onDeath += OnDeath;
        
        Debug.Log("PlayerHitReaction راه‌اندازی شد");
    }
    
    void OnHit()
    {
        // اگر در حال مرگ هستیم یا قبلاً مرده‌ایم، کاری نکن
        if (health.isDead || isPlayingDeathAnim) return;
        
        Debug.Log("🔥 ضربه خورد! شروع انیمیشن Hit");
        
        // تریگر Hit را فعال کن
        anim.SetTrigger("Hit");
        isPlayingHitAnim = true;
        
        // سایر اسکریپت‌ها را موقتاً غیرفعال کن
        DisableOtherScriptsTemporarily();
        
        // بعد از پایان انیمیشن ضربه، اسکریپت‌ها را برگردان
        StartCoroutine(EnableScriptsAfterHit());
    }
    
    IEnumerator EnableScriptsAfterHit()
    {
        // صبر کن تا انیمیشن Hit کامل شود
        yield return new WaitForSeconds(hitStunTime);
        
        // فقط اگر هنوز نمرده‌ایم، اسکریپت‌ها را فعال کن
        if (!health.isDead && !isPlayingDeathAnim)
        {
            EnableOtherScripts();
            isPlayingHitAnim = false;
            Debug.Log("✅ انیمیشن Hit تمام شد - حرکت فعال شد");
        }
    }
    
    void OnDeath()
    {
        // اگر قبلاً در حال مرگ هستیم، کاری نکن
        if (isPlayingDeathAnim) return;
        
        Debug.Log("💀 مرگ! شروع انیمیشن Die");
        
        // پرچم مرگ را فعال کن
        isPlayingDeathAnim = true;
        
        // حتماً تریگر Hit را خاموش کن (اگر فعال بود)
        anim.ResetTrigger("Hit");
        
        // تریگر Die را فعال کن
        anim.SetTrigger("Die");
        
        // تمام اسکریپت‌های دیگر را غیرفعال کن (برای همیشه)
        DisableOtherScriptsPermanently();
        
        // بعد از انیمیشن مرگ، پنل باخت را نشان بده
        StartCoroutine(ShowGameOverAfterDeath());
    }
    
    IEnumerator ShowGameOverAfterDeath()
    {
        Debug.Log("⏳ منتظر پایان انیمیشن مرگ...");
        
        // صبر کن تا انیمیشن مرگ کامل شود
        yield return new WaitForSeconds(deathDelay);
        
        Debug.Log("🎮 نمایش پنل باخت");
        
        // پنل باخت را نشان بده
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ShowGameOver();
        }
        else
        {
            Debug.LogError("❌ GameManager پیدا نشد!");
        }
    }
    
    // 🔧 غیرفعال کردن موقتی سایر اسکریپت‌ها (برای ضربه)
    void DisableOtherScriptsTemporarily()
    {
        foreach (var script in allScripts)
        {
            // خود این اسکریپت و Health را غیرفعال نکن
            if (script != this && script != health && script.enabled)
            {
                script.enabled = false;
            }
        }
    }
    
    // 🔧 غیرفعال کردن دائمی سایر اسکریپت‌ها (برای مرگ)
    void DisableOtherScriptsPermanently()
    {
        foreach (var script in allScripts)
        {
            // فقط Health و این اسکریپت فعال بمانند
            if (script != this && script != health && script.enabled)
            {
                script.enabled = false;
            }
        }
    }
    
    // 🔧 فعال کردن مجدد اسکریپت‌ها
    void EnableOtherScripts()
    {
        foreach (var script in allScripts)
        {
            if (script != this && script != health && !script.enabled)
            {
                script.enabled = true;
            }
        }
    }
    
    void OnDestroy()
    {
        // از رویدادها unsubscribe کن
        if (health != null)
        {
            health.onHit -= OnHit;
            health.onDeath -= OnDeath;
        }
    }
}