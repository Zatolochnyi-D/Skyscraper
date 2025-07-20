using System.Linq;
using Skyscraper.Inputs;
using ThreeDent.DevelopmentTools;
using ThreeDent.DevelopmentTools.Attributes;
using ThreeDent.DevelopmentTools.Option;
using ThreeDent.Helpers.Extensions;
using TMPro;
using UnityEngine;

public class InventorySelectionController : Singleton<InventorySelectionController>
{
    [IsChild(1), SerializeField] private GameObject sampleText;

    private RectTransform[] textTransforms;
    private TextMeshProUGUI[] texts;
    private int currentSelection = 0;

    protected override void Awake()
    {
        base.Awake();

        sampleText.SetActive(false);

        InputManager.OnCyclePressed += CycleSelection;
        PlayerInventory.Instance.OnItemsCountUpdated += UpdateTextDisplay;
    }

    private void Start()
    {
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

    private void UpdateTextDisplay(int index)
    {
        var name = PlayerInventory.Instance.GetItem(index).blockPrefab.name;
        var amount = PlayerInventory.Instance.GetItemAmount(index);
        texts[index].text = $"{amount}x {name}";
    }

    private void CycleSelection()
    {
        SetSelection((currentSelection + 1) % PlayerInventory.Instance.ItemsCount);
    }

    private void SetSelection(int newSelection)
    {
        textTransforms[currentSelection].sizeDelta = new(0f, 50f);
        texts[currentSelection].fontSize = 36;

        currentSelection = newSelection;
        textTransforms[currentSelection].sizeDelta = new(0f, 100f);
        texts[currentSelection].fontSize = 72;
    }

    public int GetCurrentSelectedBlockId()
    {
        return currentSelection;
    }
}