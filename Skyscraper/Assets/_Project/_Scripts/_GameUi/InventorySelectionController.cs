using System.Linq;
using Skyscraper.Inputs;
using ThreeDent.DevelopmentTools;
using ThreeDent.DevelopmentTools.Attributes;
using TMPro;
using UnityEngine;

public class InventorySelectionController : Singleton<InventorySelectionController>
{
    [IsChild(1), SerializeField] private GameObject sampleText;

    private InventoryItem[] items;
    private RectTransform[] textTransforms;
    private TextMeshProUGUI[] texts;
    private int currentSelection = 0;

    protected override void Awake()
    {
        base.Awake();
        
        sampleText.SetActive(false);

        InputManager.OnCyclePressed += CycleSelection;
    }

    private void Start()
    {
        items = PlayerInventory.Instance.Items.ToArray();
        textTransforms = new RectTransform[items.Length];
        texts = new TextMeshProUGUI[items.Length];
        for (int i = 0; i < items.Length; i++)
        {
            var newText = Instantiate(sampleText, sampleText.transform.parent);
            newText.SetActive(true);
            texts[i] = newText.GetComponent<TextMeshProUGUI>();
            texts[i].text = items[i].blockPrefab.name;
            textTransforms[i] = newText.GetComponent<RectTransform>();
        }

        SetSelection(0);
    }

    private void CycleSelection()
    {
        SetSelection((currentSelection + 1) % items.Length);
    }

    private void SetSelection(int newSelection)
    {
        textTransforms[currentSelection].sizeDelta = new(0f, 50f);
        texts[currentSelection].fontSize = 36;

        currentSelection = newSelection;
        textTransforms[currentSelection].sizeDelta = new(0f, 100f);
        texts[currentSelection].fontSize = 72;
    }

    public GameObject GetCurrentSelectedBlock()
    {
        return items[currentSelection].blockPrefab;
    }
}