using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public void OnStartAgainButtonPressed()
    {
        // Quay lại màn hình chính (Scene đầu tiên trong game)
        SceneManager.LoadScene(0);  // 0 là chỉ số của Scene đầu tiên (Main Menu hoặc màn hình bắt đầu)
    }
}
