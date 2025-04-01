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
    public GameObject minigamePanel;
    public Image feedbackBackground;
    public GameObject openGamePanel;

    [Header("Objects to Destroy")]
    public GameObject[] objectsToDestroy = new GameObject[4];

    [Header("Button Settings")]
    [Range(0.1f, 0.3f)] public float spacingRatio = 0.15f;
    public Color hoverTextColor = new Color(0.7f, 0.7f, 0.7f);
    public Color hoverImageColor = new Color(0.85f, 0.85f, 0.85f);

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
    }

    void Update()
    {
        if (playerInTrigger && Input.GetKeyDown(KeyCode.F) && !isMinigameActive && !isCompleted)
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
        float totalPadding = gridLayout.padding.left + gridLayout.padding.right;
        float usableWidth = parentRect.rect.width - totalPadding;
    
        int columns = gridLayout.constraintCount;
        float totalSpacing = gridLayout.spacing.x * (columns - 1);
        float cellWidth = (usableWidth - totalSpacing) / columns;
        float cellHeight = cellWidth * 0.6f;

        gridLayout.cellSize = new Vector2(cellWidth, cellHeight);
        gridLayout.spacing = new Vector2(cellWidth * spacingRatio, gridLayout.spacing.y);
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

            // Add and configure hover effect
            ButtonHoverEffect hoverEffect = newButton.AddComponent<ButtonHoverEffect>();
            hoverEffect.buttonText = btnText;  // Now works because buttonText is public
            hoverEffect.buttonImage = btn.image;
            hoverEffect.hoverTextColor = hoverTextColor;
            hoverEffect.hoverImageColor = hoverImageColor;

            btn.onClick.AddListener(() => OnButtonClicked(btn));
        }
    }

    void OnButtonClicked(Button btn)
    {
        btn.transform.SetAsLastSibling();
    }

    void CheckOrder()
    {
        var currentOrder = buttonParent.GetComponentsInChildren<Button>()
            .Select(b => int.Parse(b.GetComponentInChildren<TMP_Text>().text))
            .ToList();

        if (currentOrder.SequenceEqual(Enumerable.Range(1, 10)))
        {
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
            feedbackText.text = "<size=70><color=#FF0000>Try Again!</color></size>";
            feedbackText.alignment = TextAlignmentOptions.Center;
            feedbackBackground.gameObject.SetActive(true);
            feedbackBackground.color = Color.white;
            StartCoroutine(ResetAfterFeedback(1.5f));
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
        ClearButtons();
        GenerateNumbers();
        UpdateButtonLayout();
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
            openGamePanel.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
            if (isMinigameActive) DeactivateMinigame();
            openGamePanel.SetActive(false);
        }
    }

    void ClearButtons()
    {
        foreach (Transform child in buttonParent)
            Destroy(child.gameObject);
    }

    public void DestroyAssignedObjects()
    {
        foreach (var obj in objectsToDestroy)
            if (obj != null) Destroy(obj);
    }
}

[RequireComponent(typeof(Button))]
public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Component References")]
    public TMP_Text buttonText;  
    public Image buttonImage;     
    
    [Header("Hover Settings")]
    public Color hoverTextColor = Color.gray;
    public Color hoverImageColor = new Color(0.9f, 0.9f, 0.9f);

    private Color normalTextColor;
    private Color normalImageColor;

    void Awake()
    {
        if (buttonText == null) buttonText = GetComponentInChildren<TMP_Text>();
        if (buttonImage == null) buttonImage = GetComponent<Image>();
        
        normalTextColor = buttonText.color;
        normalImageColor = buttonImage.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonText.color = hoverTextColor;
        buttonImage.color = hoverImageColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonText.color = normalTextColor;
        buttonImage.color = normalImageColor;
    }
}