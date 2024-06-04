using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DatePicker : MonoBehaviour
{
    private TMP_InputField dateInputField;  // Input field to display the selected date
    public GameObject datePickerPrefab;    // Prefab of the date picker

    private GameObject datePickerInstance; // Instance of the date picker

    private void Awake()
    {
        this.dateInputField = GetComponent<TMP_InputField>();
    }
    void Start()
    {
        dateInputField.onSelect.AddListener(delegate { ShowDatePicker(); });
    }

    void ShowDatePicker()
    {
        if (datePickerInstance == null)
        {
            datePickerInstance = Instantiate(datePickerPrefab, transform.parent);
            DatePickerController datePickerController = datePickerInstance.GetComponent<DatePickerController>();
            datePickerController.OnDateSelected += UpdateInputField;
        }
        datePickerInstance.SetActive(true);
    }

    void UpdateInputField(string selectedDate)
    {
        dateInputField.text = selectedDate;
    }
}
