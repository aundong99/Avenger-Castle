using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        // ตรวจสอบว่ามี GameObject นี้อยู่แล้วหรือไม่ ถ้ามีอยู่แล้วให้ทำลายตัวเองเพื่อไม่ให้ซ้ำซ้อน
        if (FindObjectsOfType<DontDestroyOnLoad>().Length > 1)
        {
            Destroy(gameObject);
        }
    }
}
