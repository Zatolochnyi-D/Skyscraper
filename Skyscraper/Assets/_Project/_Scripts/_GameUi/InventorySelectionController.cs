using Skyscraper.Inputs;
using ThreeDent.DevelopmentTools;
using ThreeDent.DevelopmentTools.Attributes;
using TMPro;
using UnityEngine;

public class InventorySelectionController : Singleton<InventorySelectionController>
{
    [IsChild(1), SerializeField] private GameObject sampleText;
    [SerializeField] private GameObject[] blocks;
    [SerializeField] private string[] names;

    private RectTransform[] textTransforms;
    private TextMeshProUGUI[] texts;
    private int currentSelection = 0;

    protected override void Awake()
    {
        base.Awake();
        
        sampleText.SetActive(false);

        textTransforms = new RectTransform[blocks.Length];
        texts = new TextMeshProUGUI[blocks.Length];
        for (int i = 0; i < blocks.Length; i++)
        {
            var newText = Instantiate(sampleText, sampleText.transform.parent);
            newText.SetActive(true);
            texts[i] = newText.GetComponent<TextMeshProUGUI>();
            texts[i].text = names[i];
            textTransforms[i] = newText.GetComponent<RectTransform>();
        }

        SetSelection(0);

        InputManager.OnCyclePressed += CycleSelection;
    }

    private void CycleSelection()
    {
        SetSelection((currentSelection + 1) % blocks.Length);
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
        return blocks[currentSelection];
    }
}