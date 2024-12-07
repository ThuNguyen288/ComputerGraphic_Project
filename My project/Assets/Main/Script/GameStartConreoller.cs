using UnityEngine;

public class GameStartController : MonoBehaviour
{
    public GameObject startScreenUI;      // Màn hình Start
    public GameObject gameOverScreenUI;  // Màn hình Game Over
    public GameObject winScreenUI;       // Màn hình Win
    public GameObject player;            // Tham chiếu tới Player
    public GameObject enemies;           // Tham chiếu tới các kẻ địch (hoặc spawner)

    private void Start()
    {
        // Hiển thị màn hình Start khi bắt đầu game
        ShowStartScreen();
    }

    // Hiển thị màn hình Start và dừng game
    public void ShowStartScreen()
    {
        if (startScreenUI != null) startScreenUI.SetActive(true);
        if (gameOverScreenUI != null) gameOverScreenUI.SetActive(false);
        if (winScreenUI != null) winScreenUI.SetActive(false);

        // Tạm dừng game
        Time.timeScale = 0f;
    }

    // Bắt đầu trò chơi khi bấm nút "Start"
    public void StartGame()
    {
        if (startScreenUI != null) startScreenUI.SetActive(false);

        // Tiếp tục game
        Time.timeScale = 1f;

        // Reset lại trạng thái của game nếu cần
        ResetGameState();
    }

    // Hiển thị màn hình Game Over
    public void ShowGameOverScreen()
    {
        if (gameOverScreenUI != null) gameOverScreenUI.SetActive(true);
        if (startScreenUI != null) startScreenUI.SetActive(false);
        if (winScreenUI != null) winScreenUI.SetActive(false);

        // Dừng game
        Time.timeScale = 0f;
    }

    // Hiển thị màn hình Win
    public void ShowWinScreen()
    {
        if (winScreenUI != null) winScreenUI.SetActive(true);
        if (startScreenUI != null) startScreenUI.SetActive(false);
        if (gameOverScreenUI != null) gameOverScreenUI.SetActive(false);

        // Dừng game
        Time.timeScale = 0f;
    }

    // Khi bấm "Start Again", quay lại màn hình Start Screen
    public void StartGameAgain()
    {
        ShowStartScreen(); // Quay lại màn hình Start Screen
    }

    // Reset lại trạng thái game (đặt lại máu, vị trí player, enemy, v.v.)
    private void ResetGameState()
    {
        // Reset trạng thái của người chơi
        if (player != null)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.ResetHealth(); // Đặt lại máu
            }
            player.transform.position = Vector3.zero; // Đặt lại vị trí ban đầu
        }

        // Reset trạng thái của kẻ địch hoặc spawner (tùy thiết kế của bạn)
        if (enemies != null)
        {
            // Thêm logic reset kẻ địch (nếu cần)
        }
    }
    public void ExitApplication()
    {
        #if UNITY_EDITOR
                    // Nếu đang chạy trong Unity Editor, dừng trò chơi.
                    UnityEditor.EditorApplication.isPlaying = false;
        #else
                // Nếu đang chạy trong phiên bản build, thoát game.
                Application.Quit();
        #endif
    }
}
