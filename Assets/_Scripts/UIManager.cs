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
    public GameObject levelCompletePanel;
    public GameObject gameOverPanel;

    // --- ��ť���� (����) ---
    public Button restartButton;
    public Button quitButton;
    void Awake()
    {
        Instance = this;

        // --- ��̬�󶨰�ť�¼� (�����޸�) ---
        // �����ҵ���ť���������������ʱȥ�����Ǹ�Ψһ��GameManagerʵ���ķ���
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(() => GameManager.Instance.RestartLevel());
        }
        if (quitButton != null)
        {
            quitButton.onClick.AddListener(() => GameManager.Instance.QuitGame());
        }
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
    public void ShowLevelCompletePanel()
    {
        levelCompletePanel.SetActive(true);
        Time.timeScale = 0f; // ��ͣ��Ϸ
    }

    public void ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // ��ͣ��Ϸ
    }
}