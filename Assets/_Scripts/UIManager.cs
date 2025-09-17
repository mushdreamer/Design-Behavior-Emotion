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
    public GameObject levelCompletePanel;
    public GameObject gameOverPanel;

    // --- 按钮引用 (新增) ---
    public Button restartButton;
    public Button quitButton;
    void Awake()
    {
        Instance = this;

        // --- 动态绑定按钮事件 (核心修复) ---
        // 我们找到按钮，并告诉它，点击时去调用那个唯一的GameManager实例的方法
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(() => GameManager.Instance.RestartLevel());
        }
        if (quitButton != null)
        {
            quitButton.onClick.AddListener(() => GameManager.Instance.QuitGame());
        }
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
    public void ShowLevelCompletePanel()
    {
        levelCompletePanel.SetActive(true);
        Time.timeScale = 0f; // 暂停游戏
    }

    public void ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // 暂停游戏
    }
}