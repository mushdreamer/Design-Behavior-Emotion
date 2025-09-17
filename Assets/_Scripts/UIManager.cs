using UnityEngine;
using TMPro; // ���� TextMeshPro �����ռ�
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    // --- UI Ԫ������ ---
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI coinText;
    public GameObject keyIcon; // ��GameObject����������

    void Awake()
    {
        Instance = this;
    }

    // --- �����ĸ��·��� ---
    public void UpdateHealth(int currentHealth)
    {
        healthText.text = "Health: " + currentHealth;
    }

    public void UpdateCoins(int totalCoins)
    {
        coinText.text = "Coins: " + totalCoins;
    }

    public void SetKeyIconActive(bool isActive)
    {
        if (keyIcon != null)
        {
            keyIcon.SetActive(isActive);
        }
    }
}