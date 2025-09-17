using UnityEngine;
using TMPro; // 引入 TextMeshPro 命名空间
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    // --- UI 元素引用 ---
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI coinText;
    public GameObject keyIcon; // 用GameObject来控制显隐

    void Awake()
    {
        Instance = this;
    }

    // --- 公开的更新方法 ---
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