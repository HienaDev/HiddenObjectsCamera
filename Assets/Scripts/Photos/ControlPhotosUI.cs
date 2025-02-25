using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControlPhotosUI : MonoBehaviour
{
    private RenderTextureCapture rtc;
    private int index = 0;

    [SerializeField] private GameObject photosUI;
    [SerializeField] private Image currentPhoto;
    [SerializeField] private GameObject cameraUI;
    [SerializeField] private GameObject blur;

    void Start()
    {
        rtc = FindObjectOfType<RenderTextureCapture>();
    }

    private void OnEnable()
    {
        rtc = FindObjectOfType<RenderTextureCapture>();
        currentPhoto.sprite = rtc.sprites[index];
    }

    void Update()
    {


        if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextPhoto();
        }
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PreviousPhoto();
        }

        // Check if the Space or E key is pressed
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E))
        {
            // Reload the current active scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void EnableUIPhotos()
    {
        photosUI.SetActive(true);
        Debug.Log(rtc.sprites.Count);
        currentPhoto.sprite = rtc.sprites[index];
    }

    private void DisableUIPhotos()
    {
        photosUI.SetActive(false);
    }

    public void NextPhoto()
    {
        if (index < rtc.sprites.Count - 1)
            index++;
        else
            index = 0;

        Debug.Log(index);
        
        currentPhoto.sprite = rtc.sprites[index];
    }

    public void PreviousPhoto()
    {
        if (index > 0)
            index--;
        else
            index = rtc.sprites.Count - 1;

        Debug.Log(index);
        currentPhoto.sprite = rtc.sprites[index];
    }
}
