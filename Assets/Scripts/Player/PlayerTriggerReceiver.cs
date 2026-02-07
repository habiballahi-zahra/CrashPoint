using UnityEngine;
using System.Collections;
public class PlayerTriggerReceiver : MonoBehaviour
{
    private PlayerRespawn respawn;
    private Animator anim;
    private MonoBehaviour[] allScripts;
     public float deathDelay = 3f;     // زمان قبل از نمایش پنل باخت

    private void Awake()
    {
        anim = GetComponent<Animator>();
         allScripts = GetComponents<MonoBehaviour>();
        respawn = GetComponent<PlayerRespawn>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // گدازه
        if (other.CompareTag("Lava"))
        {
            // anim.SetTrigger("Die");
             OnDeath();
        }

        // چک‌پوینت
        if (other.CompareTag("Checkpoint"))
        {
            Checkpoint checkpoint = other.GetComponent<Checkpoint>();
            if (checkpoint != null)
                checkpoint.Activate(respawn);
        }
    }


 void OnDeath()
    {
        anim.SetFloat(Animator.StringToHash("inputX"), 0);
         anim.SetFloat(Animator.StringToHash("inputY"), 0);
        
        // تریگر Die را فعال کن
        anim.SetTrigger("Die");
        
        // تمام اسکریپت‌های دیگر را غیرفعال کن (برای همیشه)
        DisableOtherScriptsPermanently();
        
        // بعد از انیمیشن مرگ، پنل باخت را نشان بده
        StartCoroutine(ShowGameOverAfterDeath());
    }

     // 🔧 غیرفعال کردن دائمی سایر اسکریپت‌ها (برای مرگ)
    void DisableOtherScriptsPermanently()
    {
        foreach (var script in allScripts)
        {
            // فقط Health و این اسکریپت فعال بمانند
            if (script != this )
            {
                script.enabled = false;
            }
        }
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
}
