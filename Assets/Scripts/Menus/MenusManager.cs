using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class MenusManager : MonoBehaviour
{
    [SerializeField] private MoveCamera moveCamera;
    [SerializeField] private float photoEffectSpeed;
    [SerializeField] private Canvas cameraCanvas;
    [SerializeField] private Canvas zoomInCameraCanvas;
    [SerializeField] private Canvas inventoryCanvas;
    [SerializeField] private Canvas mainMenuCanvas;
    [SerializeField] private Canvas settingsCanvas;
    [SerializeField] private SettingsManager settingsManager;
    [SerializeField] private Image photoEffect;
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button exitButton;

    private bool isPaused;
    private bool isTransitioning;

    private void Start()
    {
        StopGame();
        isPaused = false;
        EnsureSelection();
    }

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnPlayButtonClicked()
    {
        if (!isTransitioning)
        {
            StartCoroutine(PhotoEffectCoroutine());
            isPaused = false;
        }
    }

    public void OnSettingsButtonClicked()
    {
        ShowSettingsMenu();
    }

    private void EnsureSelection()
    {
        if (EventSystem.current.currentSelectedGameObject == null && mainMenuCanvas.enabled)
        {
            EventSystem.current.SetSelectedGameObject(playButton.gameObject);
        }
    }


    private void Update()
    {
        EnsureSelection();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingsCanvas.enabled)
            {
                ShowMainMenu();
            }
            else if (!isPaused)
            {
                StopGame();
                isPaused = true;
            }
            else
            {
                ReturnGame();
                isPaused = false;
            }
        }

        HandleNavigation();
    }

    private void HandleNavigation()
    {
        if (isTransitioning) return;

        GameObject currentSelected = EventSystem.current.currentSelectedGameObject;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentSelected == exitButton.gameObject)
            {
                EventSystem.current.SetSelectedGameObject(settingsButton.gameObject);
            }
            else if (currentSelected == settingsButton.gameObject)
            {
                EventSystem.current.SetSelectedGameObject(playButton.gameObject);
            }
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentSelected == playButton.gameObject)
            {
                EventSystem.current.SetSelectedGameObject(settingsButton.gameObject);
            }
            else if (currentSelected == settingsButton.gameObject)
            {
                EventSystem.current.SetSelectedGameObject(exitButton.gameObject);
            }
        }
    }

    private IEnumerator PhotoEffectCoroutine()
    {
        isTransitioning = true;
        DisableButtons();
        
        photoEffect.enabled = true;
        float elapsedTime = 0f;
        Color startColor = new Color(photoEffect.color.r, photoEffect.color.g, photoEffect.color.b, 0);
        Color endColor = new Color(photoEffect.color.r, photoEffect.color.g, photoEffect.color.b, 1);

        while (elapsedTime < photoEffectSpeed)
        {
            elapsedTime += Time.deltaTime;
            photoEffect.color = Color.Lerp(startColor, endColor, elapsedTime / photoEffectSpeed);
            yield return null;
        }

        ReturnGame();

        yield return new WaitForSeconds(0.1f);

        elapsedTime = 0f;

        while (elapsedTime < photoEffectSpeed)
        {
            elapsedTime += Time.deltaTime;
            photoEffect.color = Color.Lerp(endColor, startColor, elapsedTime / photoEffectSpeed);
            yield return null;
        }

        photoEffect.enabled = false;
        EnableButtons();
        isTransitioning = false;
    }

    private void StopGame()
    {
        moveCamera.enabled = false;
        cameraCanvas.enabled = false;
        zoomInCameraCanvas.enabled = false;
        inventoryCanvas.enabled = false;
        mainMenuCanvas.enabled = true;
        settingsCanvas.enabled = false;
        Cursor.lockState = CursorLockMode.None;
    }

    private void ReturnGame()
    {
        moveCamera.enabled = true;
        cameraCanvas.enabled = true;
        zoomInCameraCanvas.enabled = true;
        inventoryCanvas.enabled = true;
        mainMenuCanvas.enabled = false;
        settingsCanvas.enabled = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void ShowSettingsMenu()
    {
        mainMenuCanvas.enabled = false;
        settingsCanvas.enabled = true;

        settingsManager.gameObject.SetActive(true);
        settingsManager.SelectFirstElement();
    }

    private void ShowMainMenu()
    {
        settingsCanvas.enabled = false;
        mainMenuCanvas.enabled = true;

        settingsManager.gameObject.SetActive(false);

        this.enabled = true;

        EventSystem.current.SetSelectedGameObject(playButton.gameObject);
    }

    private void DisableButtons()
    {
        playButton.interactable = false;
        settingsButton.interactable = false;
        exitButton.interactable = false;
    }

    private void EnableButtons()
    {
        playButton.interactable = true;
        settingsButton.interactable = true;
        exitButton.interactable = true;
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