using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UpgradeButtonUI : MonoBehaviour
{
    [Header("Links")]
    [HideInInspector] public Button ThisButton;
    [HideInInspector] public UpgradeType Type;
    [SerializeField] private TextMeshProUGUI _value;
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _level;


    private void Awake() => ThisButton = GetComponent<Button>();

    public void UpdateButton(float value, int level, string name = "")
    {
        _value.text = $"value: " + value.ToString();

        if (name != "")
            _name.text = name;

        _level.text = $"level: {level + 1}";
    }
}
