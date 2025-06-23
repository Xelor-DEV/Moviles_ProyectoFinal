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
    [Tooltip("Arrastra aquí el GameObject que contiene el manager del minijuego actual")]
    public GameObject gameManagerObject;
    [Tooltip("Arrastra aquí el GameObject que contiene el UI manager del minijuego actual")]
    public GameObject uiManagerObject;

    private int currentPage = 0;
    private MonoBehaviour gameManager;
    private MonoBehaviour uiManager;

    void Start()
    {
        // Configuración inicial
        if (eventSystem == null) eventSystem = EventSystem.current;

        // Obtener referencias a los managers
        if (gameManagerObject != null) gameManager = gameManagerObject.GetComponent<MonoBehaviour>();
        if (uiManagerObject != null) uiManager = uiManagerObject.GetComponent<MonoBehaviour>();

        // Desactivar managers al inicio
        SetManagersEnabled(false);

        // Configurar botones
        leftArrow.onClick.AddListener(PreviousPage);
        rightArrow.onClick.AddListener(NextPage);
        startButton.onClick.AddListener(StartGame);

        // Mostrar primera página
        ShowCurrentPage();
        Time.timeScale = 0f;

        // Selección inicial
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
        // Ocultar todos los textos
        foreach (var text in instructionTexts)
        {
            if (text != null) text.gameObject.SetActive(false);
        }

        // Mostrar texto actual
        if (instructionTexts.Length > 0 && instructionTexts[currentPage] != null)
        {
            instructionTexts[currentPage].gameObject.SetActive(true);
        }

        // Actualizar flechas
        if (leftArrow != null) leftArrow.gameObject.SetActive(currentPage > 0);
        if (rightArrow != null) rightArrow.gameObject.SetActive(currentPage < instructionTexts.Length - 1);

        // Seleccionar botón apropiado
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
        // Ocultar tutorial
        if (tutorialPanel != null) tutorialPanel.SetActive(false);

        // Reanudar juego
        Time.timeScale = 1f;

        // Activar managers
        SetManagersEnabled(true);
    }

    void SetManagersEnabled(bool enabled)
    {
        if (gameManager != null) gameManager.enabled = enabled;
        if (uiManager != null) uiManager.enabled = enabled;
    }

    void Update()
    {
        // Mantener selección si se pierde
        if (eventSystem != null && eventSystem.currentSelectedGameObject == null)
        {
            SelectAppropriateButton();
        }
    }
}