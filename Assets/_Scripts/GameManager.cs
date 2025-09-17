using UnityEngine;
using UnityEngine.SceneManagement; // �����Ҫ���¼��س����ȹ���

public class GameManager : MonoBehaviour
{
    // --- ����ģʽ ---
    public static GameManager Instance { get; private set; }

    // --- ��Ϸ״̬ ---
    public int coinsCollected { get; private set; }
    public bool hasKey { get; private set; }

    private void Awake()
    {
        // ����ȷ������ģʽ���������Ĵ���
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ȷ��GameManager���л�����ʱ���ᱻ����
        }
    }

    // --- �������������������ű����� ---
    public void AddCoin(int amount)
    {
        coinsCollected += amount;
        // ����UI
        UIManager.Instance.UpdateCoins(coinsCollected);
    }

    public void CollectKey()
    {
        hasKey = true;
        // ����UI
        UIManager.Instance.SetKeyIconActive(true);
    }

    public void LevelComplete()
    {
        Debug.Log("--- LEVEL COMPLETE ---");
        // ��������������߼������������һ������
        // SceneManager.LoadScene("NextLevelName");
    }
}