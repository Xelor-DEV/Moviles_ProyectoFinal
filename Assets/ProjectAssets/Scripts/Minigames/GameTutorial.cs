using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameTutorial : MonoBehaviour
{
    [Header("Tutorial Settings")]
    public GameObject tutorialPanel;
    public TextMeshProUGUI[] instructionTexts;
    public Button leftArrow;
    public Button rightArrow;
    public Button startButton;
    public EventSystem eventSystem;

    [Header("Game References")]

    public GameObject gameManagerObject;
    
    public GameObject uiManagerObject;

    private int currentPage = 0;
    private MonoBehaviour gameManager;
    private MonoBehaviour uiManager;

    void Start()
    {

        if (eventSystem == null)
        {
            eventSystem = EventSystem.current;

        }
        if (gameManagerObject != null)
        {
            gameManager = gameManagerObject.GetComponent<MonoBehaviour>();
        }
        if (uiManagerObject != null)
        {
            uiManager = uiManagerObject.GetComponent<MonoBehaviour>();
        }
    

      
        SetManagersEnabled(false);

        
        leftArrow.onClick.AddListener(PreviousPage);
        rightArrow.onClick.AddListener(NextPage);
        startButton.onClick.AddListener(StartGame);

    
        ShowCurrentPage();
        Time.timeScale = 0f;

    
        StartCoroutine(SelectButtonDelayed(rightArrow));
    }

    System.Collections.IEnumerator SelectButtonDelayed(Button button)
    {
        yield return null;
        if (button != null && button.gameObject.activeInHierarchy)
        {
            eventSystem.SetSelectedGameObject(button.gameObject);
        }
    }

    void ShowCurrentPage()
    {
  
        foreach (var text in instructionTexts)
        {
            if (text != null) text.gameObject.SetActive(false);
        }

      
        if (instructionTexts.Length > 0 && instructionTexts[currentPage] != null)
        {
            instructionTexts[currentPage].gameObject.SetActive(true);
        }


        if (leftArrow != null)
        {
            leftArrow.gameObject.SetActive(currentPage > 0);
        }
        if (rightArrow != null)
        {
            rightArrow.gameObject.SetActive(currentPage < instructionTexts.Length - 1);
        }

       
        SelectAppropriateButton();
    }

    void SelectAppropriateButton()
    {
        GameObject buttonToSelect = null;

        if (currentPage < instructionTexts.Length - 1 && rightArrow != null && rightArrow.gameObject.activeInHierarchy)
        {
            buttonToSelect = rightArrow.gameObject;
        }
        else if (startButton != null && startButton.gameObject.activeInHierarchy)
        {
            buttonToSelect = startButton.gameObject;
        }

        if (buttonToSelect != null && eventSystem != null)
        {
            eventSystem.SetSelectedGameObject(buttonToSelect);
        }
    }

    void NextPage()
    {
        if (currentPage < instructionTexts.Length - 1)
        {
            currentPage++;
            ShowCurrentPage();
        }
    }

    void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            ShowCurrentPage();
        }
    }

    void StartGame()
    {
     
        if (tutorialPanel != null) tutorialPanel.SetActive(false);

     
        Time.timeScale = 1f;

    
        SetManagersEnabled(true);
    }

    void SetManagersEnabled(bool enabled)
    {
        if (gameManager != null)
        {
            gameManager.enabled = enabled;

        }
        if (uiManager != null)
        {
            uiManager.enabled = enabled;
        }
    }

    void Update()
    {
        
        if (eventSystem != null && eventSystem.currentSelectedGameObject == null)
        {
            SelectAppropriateButton();
        }
    }
}