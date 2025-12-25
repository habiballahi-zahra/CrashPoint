using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyHitReaction : MonoBehaviour
{
    //  مدت زمانی که دشمن بعد از ضربه بی حرکت می‌ماند
    public float stunTime = 0.7f;
    
    //  نیروی عقب‌رانندگی وقتی دشمن ضربه می‌خورد
    public float knockbackForce = 1.0f;

    //  کنترل کننده انیمیشن‌های دشمن
    Animator anim;
    
    //  عامل ناوبری دشمن (برای حرکت روی نقشه)
    NavMeshAgent agent;
    
    //  کامپوننت سلامت دشمن
    Health health;

    void Start()
    {
        
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();

        //  به رویدادهای سلامت گوش بده
        // وقتی دشمن ضربه می‌خورد، تابع OnHit اجرا می‌شود
        health.onHit += OnHit;
        
        // وقتی دشمن می‌میرد، تابع OnDeath اجرا می‌شود
        health.onDeath += OnDeath;
    }

    //  وقتی دشمن ضربه می‌خورد
    void OnHit()
    {
        //  اگر دشمن مرده است، کاری نکن
        if (health.isDead) return;

        //  انیمیشن "ضربه خوردن" را پخش کن
        anim.SetTrigger("Hit");

        //  تمام کوروتین‌های قبلی را متوقف کن
        StopAllCoroutines();
        
        //  کوروتین جدید برای بی‌هوشی شروع کن
        StartCoroutine(StunCoroutine());
    }

    //  کوروتین برای مدیریت دوره بی‌هوشی
    IEnumerator StunCoroutine()
    {
        //  حرکت دشمن را متوقف کن
        agent.isStopped = true;

        //  جهت عقب‌رفتن را محاسبه کن (برعکس جهت نگاه دشمن)
        Vector3 knockDir = -transform.forward;
        
        //  میزان حرکت به عقب را محاسبه کن
        Vector3 move = knockDir * knockbackForce;

        //  دشمن را به عقب هل بده (با در نظر گرفتن NavMesh)
        agent.Move(move);

        //  منتظر بمان تا زمان بی‌هوشی تمام شود
        yield return new WaitForSeconds(stunTime);

        //  حرکت دشمن را دوباره فعال کن
        agent.isStopped = false;
    }

    //  وقتی دشمن می‌میرد
    void OnDeath()
    {
        //  تمام کوروتین‌ها را متوقف کن
        StopAllCoroutines();
        
        //  حرکت دشمن را کاملاً متوقف کن
        agent.isStopped = true;
        
        // کامپوننت ناوبری را غیرفعال کن
        agent.enabled = false;

        // انیمیشن "مردن" را پخش کن
        anim.SetTrigger("Die");

        //  دشمن را بعد از ۳ ثانیه حذف کن
        Destroy(gameObject, 3f);
    }

    //  وقتی این شیء از بازی حذف می‌شود
    private void OnDestroy()
    {
        //  اگر کامپوننت سلامت وجود دارد، از گوش دادن به رویدادها دست بکش
        if (health != null)
        {
            health.onHit -= OnHit;
            health.onDeath -= OnDeath;
        }
    }
}