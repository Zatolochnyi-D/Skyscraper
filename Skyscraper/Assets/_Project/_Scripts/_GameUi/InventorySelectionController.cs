using UnityEngine;
using TMPro;
using Skyscraper.Inputs;
using ThreeDent.DevelopmentTools;
using ThreeDent.DevelopmentTools.Attributes;
using ThreeDent.DevelopmentTools.Option;
using ThreeDent.EventBroker;

public class InventorySelectionController : Singleton<InventorySelectionController>
{
    [IsChild(2), SerializeField] private GameObject sampleText;
    [SerializeField] private float unselectedFontSize = 44f;
    [SerializeField] private float unselectedHeight = 45f;
    [SerializeField] private float selectedFontSize = 72f;
    [SerializeField] private float selectedHeight = 70f;

    private RectTransform[] textTransforms;
    private TextMeshProUGUI[] texts;
    private int currentSelection = 0;

    private bool working = true;

    protected override void Awake()
    {
        base.Awake();

        sampleText.SetActive(false);

        InputManager.OnCyclePressed += CycleSelection;
        EventBroker.Subscribe<InventoryEmptyEvent>(DeactivateController);
    }

    private void OnDestroy()
    {
        EventBroker.Unsubscribe<InventoryEmptyEvent>(DeactivateController);
    }

    private void Start()
    {
        PlayerInventory.Instance.OnItemsCountUpdated += UpdateTextDisplay;
        PlayerInventory.Instance.OnItemDepleted += HandleDepletedItem;

        var count = PlayerInventory.Instance.ItemsCount;
        textTransforms = new RectTransform[count];
        texts = new TextMeshProUGUI[count];
        for (int i = 0; i < count; i++)
        {
            var newText = Instantiate(sampleText, sampleText.transform.parent);
            newText.SetActive(true);
            texts[i] = newText.GetComponent<TextMeshProUGUI>();
            var name = PlayerInventory.Instance.GetItem(i).blockPrefab.name;
            var amount = PlayerInventory.Instance.GetItemAmount(i);
            texts[i].text = $"{amount}x {name}";
            textTransforms[i] = newText.GetComponent<RectTransform>();
        }

        SetSelection(0);
    }

    private void DeactivateController()
    {
        working = false;
        Deselect(currentSelection);
    }

    private void UpdateTextDisplay(int index)
    {
        var name = PlayerInventory.Instance.GetItem(index).blockPrefab.name;
        var amount = PlayerInventory.Instance.GetItemAmount(index);
        texts[index].text = $"{amount}x {name}";
        if (amount == 0)
            texts[index].color = texts[index].color.WithHsv(v: 0.6f);
        else
            texts[index].color = texts[index].color.WithHsv(v: 1f);
    }

    private void HandleDepletedItem(int index)
    {
        CycleSelection();
    }

    private void CycleSelection()
    {
        if (!working)
            return;
        int nextIndex;
        int offset = 1;
        do
        {
            nextIndex = (currentSelection + offset) % PlayerInventory.Instance.ItemsCount;
            offset++;
            if (offset > 100)
                throw new System.Exception("Infinite loop");
        } while (PlayerInventory.Instance.IsDepleted(nextIndex));
        SetSelection(nextIndex);
    }

    private void SetSelection(int newSelection)
    {
        Deselect(currentSelection);
        currentSelection = newSelection;
        Select(currentSelection);
    }

    private void Deselect(int index)
    {
        textTransforms[index].sizeDelta = new(0f, unselectedHeight);
        texts[index].fontSize = unselectedFontSize;
    }

    private void Select(int index)
    {
        textTransforms[index].sizeDelta = new(0f, selectedHeight);
        texts[index].fontSize = selectedFontSize;
    }

    public int GetCurrentSelectedBlockId()
    {
        return currentSelection;
    }
}