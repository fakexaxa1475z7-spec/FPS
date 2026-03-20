using UnityEngine;

public class PressQ : MonoBehaviour
{
    public GameObject aK;
    public GameObject hG;

    bool activeAK = true;

    public float switchDelay = 0.2f; // ดีเลย์
    float lastQTime = -999f;
    float lastRTime = -999f;

    void Start()
    {
        aK.SetActive(true);
        hG.SetActive(false);
    }

    void Update()
    {
        // กด Q (เปลี่ยนปืน)
        if (Input.GetKeyDown(KeyCode.Q) && Time.time - lastQTime >= switchDelay)
        {
            ChangeGun();
            lastQTime = Time.time;
        }

        // กด R (เช่น Reload)
        if (Input.GetKeyDown(KeyCode.R) && Time.time - lastRTime >= switchDelay)
        {
            Reload();
            lastRTime = Time.time;
        }
    }

    void ChangeGun()
    {
        if (activeAK)
        {
            aK.SetActive(false);
            hG.SetActive(true);
            activeAK = false;
        }
        else
        {
            hG.SetActive(false);
            aK.SetActive(true);
            activeAK = true;
        }
    }

    void Reload()
    {
        Debug.Log("Reloading...");
        // ใส่โค้ดรีโหลดตรงนี้
    }
}