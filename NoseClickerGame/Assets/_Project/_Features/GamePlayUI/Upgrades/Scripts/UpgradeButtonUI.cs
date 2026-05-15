using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UpgradeButtonUI : MonoBehaviour
{
    [Header("Links")]
    [HideInInspector]
    public Button ThisButton
    {
        get
        {
            if (_tempButton == null) _tempButton = GetComponent<Button>();
            return _tempButton;
        }
    }
    private Button _tempButton;
    [HideInInspector] public UpgradeType Type;
    [SerializeField] private TextMeshProUGUI _price;
    [SerializeField] private TextMeshProUGUI _value;
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _level;


    public void UpdateButton(float price, float value, int level, string name = "")
    {
        _price.text = "$: " + price.ToString();
        _value.text = $"value: " + value.ToString();
        if (name != "")
            _name.text = name;
        _level.text = $"level: {level + 1}";
    }
}
