// TestDeath.cs
using UnityEngine;

public class tEST : MonoBehaviour
{
    public KeyCode testHitKey = KeyCode.H;    // کلید تست ضربه
    public KeyCode testDeathKey = KeyCode.R;  // کلید تست مرگ
    
    private Animator anim;
    
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    
    void Update()
    {
        // با H انیمیشن Hit تست می‌شود
        if (Input.GetKeyDown(testHitKey))
        {
            Debug.Log("🎬 تست انیمیشن Hit");
            anim.SetTrigger("Hit");
        }
        
        // با D انیمیشن Die تست می‌شود
        if (Input.GetKeyDown(testDeathKey))
        {
            Debug.Log("🎬 تست انیمیشن Die");
            anim.SetTrigger("Die");
        }
    }
}