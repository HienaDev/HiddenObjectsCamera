using UnityEngine;

public class MenusManager : MonoBehaviour
{
    [SerializeField] private MoveCamera moveCamera;
    [SerializeField] private Canvas     cameraCanvas;
    [SerializeField] private Canvas     zoomInCameraCanvas;
    [SerializeField] private Canvas     inventoryCanvas;
    [SerializeField] private Canvas      mainMenuCanvas;

    private bool isPaused;

    private void Start()
    {
        StopGame();

        isPaused = false;
    }

    public void OnPlayButtonClicked()
    {
        ReturnGame();

        isPaused = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused)
        {
            StopGame();

            isPaused = true;
        }

        else if (Input.GetKeyDown(KeyCode.Escape) && isPaused)
        {
            ReturnGame();

            isPaused = false;
        }
    }

    private void StopGame()
    {
        moveCamera.enabled          = false;
        cameraCanvas.enabled        = false;
        zoomInCameraCanvas.enabled  = false;
        inventoryCanvas.enabled     = false;
        mainMenuCanvas.enabled      = true;
    }

    private void ReturnGame()
    {
        moveCamera.enabled          = true;
        cameraCanvas.enabled        = true;
        zoomInCameraCanvas.enabled  = true;
        inventoryCanvas.enabled     = true;
        mainMenuCanvas.enabled      = false;
    }

    public void ExitGame()
    {
#if UNITY_STANDALONE
        Application.Quit();
#endif
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}