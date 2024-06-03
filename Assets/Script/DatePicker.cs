using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class DatePicker : MonoBehaviour
{
    public GameObject datePickerPanel;     // Panel containing the date picker
    public GameObject dayScrollViewContent;
    public GameObject monthScrollViewContent;
    public GameObject yearScrollViewContent;
    public GameObject selectedDatePrefab;  // Prefab for days
    public GameObject selectedMonthPrefab; // Prefab for months
    public GameObject selectedYearPrefab;  // Prefab for years
    public TMP_InputField dateInputField;  // Input field to display the selected date
    public Button doneButton;              // Button to confirm the date selection

    private int selectedDay;
    private int selectedMonth;
    private int selectedYear;

    private int currentYear, currentMonth, currentDay;

    private ScrollRect dayScrollRect;
    private ScrollRect monthScrollRect;
    private ScrollRect yearScrollRect;

    void Start()
    {
        DateTime now = DateTime.Now;
        currentYear = now.Year;
        currentMonth = now.Month;
        currentDay = now.Day;

        dayScrollRect = dayScrollViewContent.GetComponentInParent<ScrollRect>();
        monthScrollRect = monthScrollViewContent.GetComponentInParent<ScrollRect>();
        yearScrollRect = yearScrollViewContent.GetComponentInParent<ScrollRect>();

        // Initialize default selections
        selectedDay = currentDay;
        selectedMonth = currentMonth;
        selectedYear = currentYear;

        // Populate scroll views
        PopulateYears();
        PopulateMonths(12);
        PopulateDays(DateTime.DaysInMonth(selectedYear, selectedMonth));

        UpdateInputField();

        // Add listener to input field to activate date picker panel
        dateInputField.onSelect.AddListener(delegate { ShowDatePickerPanel(); });

        // Add listener to done button to update input field and deactivate date picker panel
        doneButton.onClick.AddListener(UpdateInputField);
        doneButton.onClick.AddListener(HideDatePickerPanel);

        // Add listeners to scroll rects
        dayScrollRect.onValueChanged.AddListener(delegate { UpdateSelectedDay(); });
        monthScrollRect.onValueChanged.AddListener(delegate { UpdateSelectedMonth(); });
        yearScrollRect.onValueChanged.AddListener(delegate { UpdateSelectedYear(); });

        // Ensure date picker panel is initially inactive
        datePickerPanel.SetActive(false);

        // Scroll to current day, month, and year
        ScrollToCurrentDate();
    }

    void ShowDatePickerPanel()
    {
        datePickerPanel.SetActive(true);
    }

    void HideDatePickerPanel()
    {
        datePickerPanel.SetActive(false);
    }

    void PopulateDays(int days)
    {
        foreach (Transform child in dayScrollViewContent.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 1; i <= days; i++)
        {
            // Disable future dates if in the current year and month
            if (selectedYear == currentYear && selectedMonth == currentMonth && i > currentDay)
                break;

            GameObject dayText = Instantiate(selectedDatePrefab, dayScrollViewContent.transform);
            TMP_Text textComponent = dayText.GetComponent<TMP_Text>();
            textComponent.text = i < 10 ? "0" + i.ToString() : i.ToString();
            dayText.name = "Day_" + i;
        }
    }

    void PopulateMonths(int months)
    {
        foreach (Transform child in monthScrollViewContent.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 1; i <= months; i++)
        {
            // Disable future months if in the current year
            if (selectedYear == currentYear && i > currentMonth)
                break;

            GameObject monthText = Instantiate(selectedMonthPrefab, monthScrollViewContent.transform);
            TMP_Text textComponent = monthText.GetComponent<TMP_Text>();
            textComponent.text = i < 10 ? "0" + i.ToString() : i.ToString();
            monthText.name = "Month_" + i;
        }
    }

    void PopulateYears()
    {
        foreach (Transform child in yearScrollViewContent.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = currentYear; i >= 1960; i--)
        {
            GameObject yearText = Instantiate(selectedYearPrefab, yearScrollViewContent.transform);
            yearText.GetComponent<TMP_Text>().text = i.ToString();
            yearText.name = "Year_" + i;
        }
    }

    void UpdateSelectedDay()
    {
        int totalDays = dayScrollViewContent.transform.childCount;
        float normalizedPosition = dayScrollRect.verticalNormalizedPosition;
        int dayIndex = Mathf.Clamp(Mathf.RoundToInt((1 - normalizedPosition) * (totalDays - 1)), 0, totalDays - 1);
        selectedDay = int.Parse(dayScrollViewContent.transform.GetChild(dayIndex).GetComponent<TMP_Text>().text);
    }

    void UpdateSelectedMonth()
    {
        int totalMonths = monthScrollViewContent.transform.childCount;
        float normalizedPosition = monthScrollRect.verticalNormalizedPosition;
        int monthIndex = Mathf.Clamp(Mathf.RoundToInt((1 - normalizedPosition) * (totalMonths - 1)), 0, totalMonths - 1);
        selectedMonth = int.Parse(monthScrollViewContent.transform.GetChild(monthIndex).GetComponent<TMP_Text>().text);
        UpdateDaysForMonth();
    }



    void UpdateSelectedYear()
    {
        int totalYears = yearScrollViewContent.transform.childCount;
        float normalizedPosition = yearScrollRect.verticalNormalizedPosition;
        int yearIndex = Mathf.Clamp(Mathf.RoundToInt((1 - normalizedPosition) * (totalYears - 1)), 0, totalYears - 1);
        selectedYear = int.Parse(yearScrollViewContent.transform.GetChild(yearIndex).GetComponent<TMP_Text>().text);
        UpdateMonthsForYear();
        UpdateDaysForMonth();
    }

    void UpdateMonthsForYear()
    {
        PopulateMonths(12);
        if (selectedYear == currentYear && selectedMonth > currentMonth)
        {
            selectedMonth = currentMonth;
        }
    }

    void UpdateDaysForMonth()
    {
        int daysInMonth = DateTime.DaysInMonth(selectedYear, selectedMonth);
        PopulateDays(daysInMonth);

        if (selectedDay > daysInMonth)
        {
            selectedDay = daysInMonth;
        }
    }

    void UpdateInputField()
    {
        string formattedDate = $"{selectedDay:00}-{selectedMonth:00}-{selectedYear}";
        dateInputField.text = formattedDate;
    }

    void ScrollToCurrentDate()
    {
        ScrollToPosition(dayScrollRect, currentDay - 1, dayScrollViewContent.transform.childCount);
        ScrollToPosition(monthScrollRect, 12 - currentMonth, monthScrollViewContent.transform.childCount);
        ScrollToPosition(yearScrollRect, currentYear - 1960, yearScrollViewContent.transform.childCount);
    }

    void ScrollToPosition(ScrollRect scrollRect, int index, int totalItems)
    {
        float normalizedPosition = 1f - ((float)index / (totalItems - 1));
        scrollRect.verticalNormalizedPosition = normalizedPosition;
    }
}
