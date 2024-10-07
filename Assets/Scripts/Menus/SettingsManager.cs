using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private TMP_Dropdown qualityDropdown;
    
    [SerializeField] private float sliderSpeed = 2.0f;

    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color selectedColor = Color.yellow;

    private void Update()
    {
        EnsureSelection();
        HandleNavigation();
        UpdateVisualSelection();
    }

    private void EnsureSelection()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            SelectFirstElement();
        }
    }

    private void HandleNavigation()
    {
        GameObject currentSelected = EventSystem.current.currentSelectedGameObject;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            NavigateToPrevious(currentSelected);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            NavigateToNext(currentSelected);
        }

        HandleInputForSelected(currentSelected);
    }

    private void NavigateToPrevious(GameObject currentSelected)
    {
        if (currentSelected == sensitivitySlider.gameObject)
        {
            EventSystem.current.SetSelectedGameObject(volumeSlider.gameObject);
        }
        else if (currentSelected == resolutionDropdown.gameObject)
        {
            EventSystem.current.SetSelectedGameObject(sensitivitySlider.gameObject);
        }
        else if (currentSelected == fullscreenToggle.gameObject)
        {
            EventSystem.current.SetSelectedGameObject(resolutionDropdown.gameObject);
        }
        else if (currentSelected == qualityDropdown.gameObject)
        {
            EventSystem.current.SetSelectedGameObject(fullscreenToggle.gameObject);
        }
        else if (currentSelected == volumeSlider.gameObject)
        {
            EventSystem.current.SetSelectedGameObject(qualityDropdown.gameObject);
        }
    }

    private void NavigateToNext(GameObject currentSelected)
    {
        if (currentSelected == volumeSlider.gameObject)
        {
            EventSystem.current.SetSelectedGameObject(sensitivitySlider.gameObject);
        }
        else if (currentSelected == sensitivitySlider.gameObject)
        {
            EventSystem.current.SetSelectedGameObject(resolutionDropdown.gameObject);
        }
        else if (currentSelected == resolutionDropdown.gameObject)
        {
            EventSystem.current.SetSelectedGameObject(fullscreenToggle.gameObject);
        }
        else if (currentSelected == fullscreenToggle.gameObject)
        {
            EventSystem.current.SetSelectedGameObject(qualityDropdown.gameObject);
        }
        else if (currentSelected == qualityDropdown.gameObject)
        {
            EventSystem.current.SetSelectedGameObject(volumeSlider.gameObject);
        }
    }

    private void HandleInputForSelected(GameObject currentSelected)
    {
        if (currentSelected == volumeSlider.gameObject || currentSelected == sensitivitySlider.gameObject)
        {
            Slider selectedSlider = currentSelected.GetComponent<Slider>();
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                selectedSlider.value = Mathf.Lerp(selectedSlider.value, selectedSlider.value - sliderSpeed, sliderSpeed * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                selectedSlider.value = Mathf.Lerp(selectedSlider.value, selectedSlider.value + sliderSpeed, sliderSpeed * Time.deltaTime);
            }
        }
        else if (currentSelected == resolutionDropdown.gameObject || currentSelected == qualityDropdown.gameObject)
        {
            TMP_Dropdown selectedDropdown = currentSelected.GetComponent<TMP_Dropdown>();
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                selectedDropdown.value = Mathf.Max(selectedDropdown.value - 1, 0);
                EventSystem.current.SetSelectedGameObject(selectedDropdown.gameObject);
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                selectedDropdown.value = Mathf.Min(selectedDropdown.value + 1, selectedDropdown.options.Count - 1);
                EventSystem.current.SetSelectedGameObject(selectedDropdown.gameObject);
            }
        }
        else if (currentSelected == fullscreenToggle.gameObject)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                fullscreenToggle.isOn = !fullscreenToggle.isOn;
                EventSystem.current.SetSelectedGameObject(fullscreenToggle.gameObject);
            }
        }
    }

    private void UpdateVisualSelection()
    {
        GameObject currentSelected = EventSystem.current.currentSelectedGameObject;

        SetSliderVisual(volumeSlider, currentSelected == volumeSlider.gameObject);
        SetSliderVisual(sensitivitySlider, currentSelected == sensitivitySlider.gameObject);

        SetDropdownColor(resolutionDropdown, currentSelected == resolutionDropdown.gameObject);
        SetDropdownColor(qualityDropdown, currentSelected == qualityDropdown.gameObject);

        SetToggleBackgroundColor(fullscreenToggle, currentSelected == fullscreenToggle.gameObject);
    }

    private void SetSliderVisual(Slider slider, bool isSelected)
    {
        Transform handle = slider.handleRect;
        Image handleImage = handle.GetComponent<Image>();
        Image fillImage = slider.fillRect.GetComponent<Image>();

        handleImage.color = isSelected ? selectedColor : normalColor;
        fillImage.color = isSelected ? selectedColor : normalColor;
    }

    private void SetDropdownColor(TMP_Dropdown dropdown, bool isSelected)
    {
        Image box = dropdown.GetComponent<Image>();
        box.color = isSelected ? selectedColor : normalColor;
    }

    private void SetToggleBackgroundColor(Toggle toggle, bool isSelected)
    {
        Transform backgroundTransform = toggle.transform.Find("Background");

        if (backgroundTransform != null)
        {
            Image backgroundImage = backgroundTransform.GetComponent<Image>();

            if (backgroundImage != null)
            {
                backgroundImage.color = isSelected ? selectedColor : normalColor;
            }
            else
            {
                Debug.LogWarning("Background Image component not found.");
            }
        }
        else
        {
            Debug.LogWarning("Background Transform not found.");
        }
    }

    public void SelectFirstElement()
    {
        EventSystem.current.SetSelectedGameObject(volumeSlider.gameObject);
    }
}