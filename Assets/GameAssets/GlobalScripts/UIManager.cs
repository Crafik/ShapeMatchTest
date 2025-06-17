using TMPro;

public class UIManager
{
    private TextMeshProUGUI _fieldCounter;
    private TextMeshProUGUI _bagCounter;

    public UIManager(TextMeshProUGUI field, TextMeshProUGUI bag)
    {
        _fieldCounter = field;
        _bagCounter = bag;
    }

    public void RefreshCounters(int field, int bag)
    {
        _fieldCounter.text = field.ToString("####");
        _bagCounter.text = bag.ToString("####");
    }
}