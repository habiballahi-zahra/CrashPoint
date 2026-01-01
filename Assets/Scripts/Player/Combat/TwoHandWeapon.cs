using UnityEngine;

public class SimpleTwoHandWeapon : MonoBehaviour
{
    [Header("References")]
    public Transform weapon;  // مدل تفنگ
    public Transform rightHand;  // استخوان دست راست
    public Transform leftHand;   // استخوان دست چپ
    
    [Header("Weapon Position")]
    public Vector3 weaponOffset = new Vector3(0.1f, -0.02f, 0.15f);
    public Vector3 weaponRotation = new Vector3(0f, 180f, 5f);
    
    [Header("Left Hand Position")]
    public Vector3 leftHandOffset = new Vector3(-0.15f, 0.01f, 0.1f);
    
    void Start()
    {
        AttachWeapon();
    }
    
    void AttachWeapon()
    {
        if (weapon == null || rightHand == null)
        {
            Debug.LogError("لطفا ابتدا weapon و rightHand را تنظیم کنید!");
            return;
        }
        
        // تفنگ را به دست راست متصل کن
        weapon.SetParent(rightHand);
        weapon.localPosition = weaponOffset;
        weapon.localEulerAngles = weaponRotation;
    }
    
    void Update()
    {
        if (leftHand != null && weapon != null)
        {
            // محاسبه موقعیت دست چپ
            Vector3 leftHandTargetPos = weapon.position + 
                weapon.right * leftHandOffset.x +
                weapon.up * leftHandOffset.y +
                weapon.forward * leftHandOffset.z;
            
            // حرکت دست چپ به موقعیت هدف
            leftHand.position = leftHandTargetPos;
            
            // چرخش دست چپ (نگاه به سمت دست راست)
            Vector3 lookDirection = rightHand.position - leftHand.position;
            if (lookDirection != Vector3.zero)
            {
                leftHand.rotation = Quaternion.LookRotation(lookDirection) * 
                    Quaternion.Euler(-20f, 0f, -15f);
            }
        }
    }
}