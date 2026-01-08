using UnityEngine;

public class GunManager : MonoBehaviour
{
    [Header("Weapon Settings")]
    public int maxAmmo = 30;
    public int currentAmmo;
    public float reloadTime = 2f;
    
    [Header("References")]
    public PlayerGun playerGun;
    public Animator playerAnimator;
    public Transform weaponTransform;
    
    private bool isReloading = false;
    
    void Start()
    {
        currentAmmo = maxAmmo;
        
        if (playerAnimator == null)
        {
            playerAnimator = GetComponentInChildren<Animator>();
        }
    }
    
    void Update()
    {
        // نمایش UI مهمات
        UpdateAmmoUI();
        
        // اگر مهمات تمام شد، ریلیود خودکار
        if (currentAmmo <= 0 && !isReloading)
        {
            StartCoroutine(Reload());
        }
    }
    
    // تابعی که توسط PlayerGun فراخوانی می‌شود
    public void OnBulletFired()
    {
        if (currentAmmo > 0)
        {
            currentAmmo--;
        }
    }
    
    System.Collections.IEnumerator Reload()
    {
        isReloading = true;
        
        // انیمیشن ریلیود
        if (playerAnimator != null)
        {
            playerAnimator.SetTrigger("Reload");
        }
        
        Debug.Log("Reloading...");
        
        // منتظر بمان
        yield return new WaitForSeconds(reloadTime);
        
        // پر کردن مهمات
        currentAmmo = maxAmmo;
        isReloading = false;
        
        Debug.Log("Reload Complete!");
    }
    
    void UpdateAmmoUI()
    {
        // اینجا می‌توانید UI مهمات را آپدیت کنید
        // برای تست در کنسول نمایش می‌دهیم
        Debug.Log($"Ammo: {currentAmmo}/{maxAmmo}");
    }
}