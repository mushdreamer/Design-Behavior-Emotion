using UnityEngine;
using UnityEngine.SceneManagement; // 如果需要重新加载场景等功能

public class GameManager : MonoBehaviour
{
    // --- 单例模式 ---
    public static GameManager Instance { get; private set; }

    // --- 游戏状态 ---
    public int coinsCollected { get; private set; }
    public bool hasKey { get; private set; }

    private void Awake()
    {
        // 这是确保单例模式正常工作的代码
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 确保GameManager在切换场景时不会被销毁
        }
    }

    // --- 公开方法，用于其他脚本调用 ---
    public void AddCoin(int amount)
    {
        coinsCollected += amount;
        // 更新UI
        UIManager.Instance.UpdateCoins(coinsCollected);
    }

    public void CollectKey()
    {
        hasKey = true;
        // 更新UI
        UIManager.Instance.SetKeyIconActive(true);
    }

    public void LevelComplete()
    {
        UIManager.Instance.ShowLevelCompletePanel();
    }

    public void GameOver()
    {
        UIManager.Instance.ShowGameOverPanel();
    }

    // --- 给按钮调用的公共方法 ---
    public void RestartLevel()
    {
        Time.timeScale = 1f; // 恢复游戏速度
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting Game..."); // 在编辑器中这行会打印
    }
}