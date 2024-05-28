using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DatePicker : MonoBehaviour
{
    public GameObject dayScrollViewContent;
    public GameObject monthScrollViewContent;
    public GameObject yearScrollViewContent;
    public GameObject selectedDatePrefab;  // Prefab for days
    public GameObject selectedMonthPrefab; // Prefab for months
    public GameObject selectedYearPrefab;  // Prefab for years
    public TMP_InputField dateInputField;  // Input field to display the selected date

    private int selectedDay;
    private int selectedMonth;
    private int selectedYear;

    private int currentYear;
    private int currentMonth;
    private int currentDay;

    void Start()
    {
        DateTime currentDate = System.DateTime.Now;
        currentYear = currentDate.Year;
        currentMonth = currentDate.Month;
        currentDay = currentDate.Day;

        Debug.Log("Current Date: " + currentDate);
        PopulateDays(currentDay);
        PopulateMonths(currentMonth);
        PopulateYears();

        // Initialize default selections
        selectedDay = currentDay;
        selectedMonth = currentMonth;
        selectedYear = currentYear;

        dateInputField.text = string.Empty;

        // UpdateInputField();
    }

    void PopulateDays(int maxDays)
    {
        foreach (Transform child in dayScrollViewContent.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 1; i <= maxDays; i++)
        {
            GameObject dayText = Instantiate(selectedDatePrefab, dayScrollViewContent.transform);
            TMP_Text textComponent = dayText.GetComponent<TMP_Text>();
            textComponent.text = i.ToString("00");
            dayText.name = "Day_" + i;

            Button button = dayText.GetComponent<Button>();
            int day = i;
            button.onClick.AddListener(() => OnDaySelected(day));
        }
    }

    void PopulateMonths(int maxMonth)
    {
        foreach (Transform child in monthScrollViewContent.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 1; i <= maxMonth; i++)
        {
            GameObject monthText = Instantiate(selectedMonthPrefab, monthScrollViewContent.transform);
            TMP_Text textComponent = monthText.GetComponent<TMP_Text>();
            textComponent.text = i.ToString("00");
            monthText.name = "Month_" + i;

            Button button = monthText.GetComponent<Button>();
            int month = i;
            button.onClick.AddListener(() => OnMonthSelected(month));
        }
    }

    void PopulateYears()
    {
        foreach (Transform child in yearScrollViewContent.transform)
        {
            Destroy(child.gameObject);
        }

        int startYear = currentYear - 50; // 50 years back from the current year
        int endYear = currentYear; // Up to the current year

        for (int i = startYear; i <= endYear; i++)
        {
            GameObject yearText = Instantiate(selectedYearPrefab, yearScrollViewContent.transform);
            yearText.GetComponent<TMP_Text>().text = i.ToString();
            yearText.name = "Year_" + i;

            Button button = yearText.GetComponent<Button>();
            int year = i;
            button.onClick.AddListener(() => OnYearSelected(year));
        }
    }

    void OnDaySelected(int day)
    {
        selectedDay = day;
        UpdateInputField();
    }

    void OnMonthSelected(int month)
    {
        selectedMonth = month;
        UpdateDaysForMonth();
        UpdateInputField();
    }

    void OnYearSelected(int year)
    {
        selectedYear = year;
        UpdateDaysForMonth();
        UpdateInputField();
    }

    void UpdateDaysForMonth()
    {
        int daysInMonth = DateTime.DaysInMonth(selectedYear, selectedMonth);

        PopulateDays(daysInMonth);

        // Ensure the selected day is valid
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
}
