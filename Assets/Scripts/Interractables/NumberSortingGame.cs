using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;

public class NumberSortingGame : MonoBehaviour
{
    [Header("UI References")]
    public GameObject buttonPrefab;
    public Transform buttonParent;
    public TMP_Text feedbackText;
    public Button checkOrderButton;
    public Button resetButton;
    public GameObject minigamePanel;
    public Image feedbackBackground; // Add in Inspector

    [Header("Objects to Destroy")]
    public GameObject[] objectsToDestroy = new GameObject[4];

    [Header("Button Settings")]
    public Color normalColor = Color.white;
    public Color hoverColor = Color.gray;
    public Color clickColor = new Color(0.8f, 0.9f, 1f);
    [Range(0.1f, 0.3f)] public float spacingRatio = 0.15f;

    private List<int> numbers = new List<int>();
    private bool playerInTrigger = false;
    private bool isMinigameActive = false;
    private bool isCompleted = false;
    private GridLayoutGroup gridLayout;

    void Start()
    {
        gridLayout = buttonParent.GetComponent<GridLayoutGroup>();
        minigamePanel.SetActive(false);
        feedbackBackground.gameObject.SetActive(false);
        checkOrderButton.onClick.AddListener(CheckOrder);
        resetButton.onClick.AddListener(ResetGame);
    }

    void Update()
    {
        if (playerInTrigger && Input.GetKeyDown(KeyCode.D) && !isMinigameActive && !isCompleted)
        {
            InitializeGame();
            ActivateMinigame();
        }
    }

    void InitializeGame()
    {
        ClearButtons();
        GenerateNumbers();
        feedbackText.text = "";
        feedbackBackground.gameObject.SetActive(false);
        StartCoroutine(AdjustLayoutNextFrame());
    }

    IEnumerator AdjustLayoutNextFrame()
    {
        yield return null;
        UpdateButtonLayout();
    }

    void UpdateButtonLayout()
    {
        if (gridLayout == null) return;

        RectTransform parentRect = buttonParent.GetComponent<RectTransform>();
        float usableWidth = parentRect.rect.width - gridLayout.padding.left - gridLayout.padding.right;
        
        float cellWidth = (usableWidth - (gridLayout.spacing.x * (gridLayout.constraintCount - 1))) / gridLayout.constraintCount;
        float cellHeight = cellWidth * 0.7f;
        
        gridLayout.cellSize = new Vector2(cellWidth, cellHeight);
        gridLayout.spacing = new Vector2(cellWidth * spacingRatio, cellWidth * spacingRatio);
    }

    void GenerateNumbers()
    {
        numbers = Enumerable.Range(1, 10).OrderBy(x => Random.value).ToList();

        foreach (int num in numbers)
        {
            GameObject newButton = Instantiate(buttonPrefab, buttonParent);
            Button btn = newButton.GetComponent<Button>();
            TMP_Text btnText = btn.GetComponentInChildren<TMP_Text>();
            btnText.text = num.ToString();
            
            btn.image.color = normalColor;
            
            var hoverHandler = btn.gameObject.AddComponent<ButtonHoverHandler>();
            hoverHandler.Initialize(btn, normalColor, hoverColor);
            
            btn.onClick.AddListener(() => StartCoroutine(OnButtonClicked(btn)));
        }
    }

    IEnumerator OnButtonClicked(Button btn)
    {
        btn.image.color = clickColor;
        btn.transform.SetAsLastSibling();
        yield return new WaitForSecondsRealtime(0.2f);
        btn.image.color = normalColor;
    }

    void CheckOrder()
    {
        var currentOrder = buttonParent.GetComponentsInChildren<Button>()
            .Select(b => int.Parse(b.GetComponentInChildren<TMP_Text>().text))
            .ToList();

        if (currentOrder.SequenceEqual(Enumerable.Range(1, 10)))
        {
            // Correct order handling (unchanged)
            feedbackText.text = "<size=70><color=#00FF00>Correct!</color></size>";
            feedbackText.alignment = TextAlignmentOptions.Center;
            feedbackBackground.gameObject.SetActive(true);
            StartCoroutine(HideFeedbackAfterDelay(2f));
            DestroyAssignedObjects();
            isCompleted = true;
            GetComponent<Collider2D>().enabled = false;
            DeactivateMinigame();
        }
        else
        {
            // Modify the "Try Again" feedback
            feedbackText.text = "<size=70><color=#FF0000>Try Again!</color></size>";
            feedbackText.alignment = TextAlignmentOptions.Center; // Ensure text is centered
            
            // Ensure the background is visible and properly sized
            feedbackBackground.gameObject.SetActive(true);
            
            // Optionally, set background to white if needed
            feedbackBackground.color = Color.white;
            
            StartCoroutine(ResetAfterFeedback(1.5f));
        }
    }
    void ConfigureFeedbackDisplay()
    {
        if (feedbackText != null && feedbackBackground != null)
        {
            // Ensure the background matches the text size
            RectTransform textRect = feedbackText.GetComponent<RectTransform>();
            RectTransform backgroundRect = feedbackBackground.GetComponent<RectTransform>();
            
            // Optional: Adjust background size to match text
            backgroundRect.sizeDelta = textRect.sizeDelta * 1.2f; // Add some padding
            
            // Ensure they're positioned together
            backgroundRect.position = textRect.position;
        }
    }
    IEnumerator HideFeedbackAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        feedbackBackground.gameObject.SetActive(false);
        feedbackText.text = "";
    }

    IEnumerator ResetAfterFeedback(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        feedbackBackground.gameObject.SetActive(false);
        feedbackText.text = "";
        ResetGame();
    }

    void ActivateMinigame()
    {
        isMinigameActive = true;
        minigamePanel.SetActive(true);
        Time.timeScale = 0f;
        UpdateButtonLayout();
    }

    void DeactivateMinigame()
    {
        isMinigameActive = false;
        minigamePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isCompleted)
            playerInTrigger = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
            if (isMinigameActive) DeactivateMinigame();
        }
    }

    void ClearButtons()
    {
        foreach (Transform child in buttonParent)
            Destroy(child.gameObject);
    }

    public void ResetGame()
    {
        ClearButtons();
        GenerateNumbers();
        UpdateButtonLayout();
    }

    public void DestroyAssignedObjects()
    {
        foreach (var obj in objectsToDestroy)
            if (obj != null) Destroy(obj);
    }
}

public class ButtonHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Button button;
    private Color normalColor;
    private Color hoverColor;

    public void Initialize(Button btn, Color normalCol, Color hoverCol)
    {
        button = btn;
        normalColor = normalCol;
        hoverColor = hoverCol;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (button != null) button.image.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (button != null) button.image.color = normalColor;
    }
}