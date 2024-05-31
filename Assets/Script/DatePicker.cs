using System.Collections;
using System.Collections.Generic;
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

    void Start()
    {
        PopulateDays(31); // Initially populate with maximum days
        PopulateMonths();
        PopulateYears();

        // Initialize default selections
        selectedDay = 1;
        selectedMonth = 1;
        selectedYear = 2000;

        UpdateInputField();
    }

    void PopulateDays(int days)
    {
        foreach (Transform child in dayScrollViewContent.transform)
        {
            Destroy(child.gameObject);
        }

        // Add padding items before actual days
        // AddPaddingItems(dayScrollViewContent, 4);

        for (int i = 1; i <= days; i++)
        {
            GameObject dayText = Instantiate(selectedDatePrefab, dayScrollViewContent.transform);
            TMP_Text textComponent = dayText.GetComponent<TMP_Text>();
            if (i < 10)
            {
                textComponent.text = "0" + i.ToString();
            }
            else
            {
                textComponent.text = i.ToString();
            }
            dayText.name = "Day_" + i;

            Button button = dayText.GetComponent<Button>();
            int day = i;
            button.onClick.AddListener(() => OnDaySelected(day));
        }

        // Add padding items after actual days
        // AddPaddingItems(dayScrollViewContent, 4);
    }

    void PopulateMonths()
    {
        foreach (Transform child in monthScrollViewContent.transform)
        {
            Destroy(child.gameObject);
        }

        // Add padding items before actual months
        // AddPaddingItems(monthScrollViewContent, 4);

        for (int i = 1; i <= 12; i++)
        {
            GameObject monthText = Instantiate(selectedMonthPrefab, monthScrollViewContent.transform);
            TMP_Text textComponent = monthText.GetComponent<TMP_Text>();
            if (i < 10)
            {
                textComponent.text = "0" + i.ToString();
            }
            else
            {
                textComponent.text = i.ToString();
            }
            monthText.name = "Month_" + i;

            Button button = monthText.GetComponent<Button>();
            int month = i;
            button.onClick.AddListener(() => OnMonthSelected(month));
        }

        // Add padding items after actual months
        // AddPaddingItems(monthScrollViewContent, 4);
    }

    void PopulateYears()
    {
        foreach (Transform child in yearScrollViewContent.transform)
        {
            Destroy(child.gameObject);
        }

        // Add padding items before actual years
        // AddPaddingItems(yearScrollViewContent, 4);

        for (int i = 2000; i <= 2030; i++)
        {
            GameObject yearText = Instantiate(selectedYearPrefab, yearScrollViewContent.transform);
            yearText.GetComponent<TMP_Text>().text = i.ToString();
            yearText.name = "Year_" + i;

            Button button = yearText.GetComponent<Button>();
            int year = i;
            button.onClick.AddListener(() => OnYearSelected(year));
        }

        // Add padding items after actual years
        // AddPaddingItems(yearScrollViewContent, 4);
    }

    // void AddPaddingItems(GameObject scrollViewContent, int paddingCount)
    // {
    //     for (int i = 0; i < paddingCount; i++)
    //     {
    //         GameObject paddingItem = new GameObject("PaddingItem");
    //         paddingItem.transform.SetParent(scrollViewContent.transform);
    //         RectTransform rectTransform = paddingItem.AddComponent<RectTransform>();
    //         rectTransform.sizeDelta = new Vector2(0, 30); // Adjust height as needed
    //     }
    // }

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
        int daysInMonth = 31;

        if (selectedMonth == 2)
        {
            daysInMonth = IsLeapYear(selectedYear) ? 29 : 28;
        }
        else if (selectedMonth == 4 || selectedMonth == 6 || selectedMonth == 9 || selectedMonth == 11)
        {
            daysInMonth = 30;
        }

        PopulateDays(daysInMonth);

        // Ensure the selected day is valid
        if (selectedDay > daysInMonth)
        {
            selectedDay = daysInMonth;
        }
    }

    bool IsLeapYear(int year)
    {
        return (year % 4 == 0 && year % 100 != 0) || (year % 400 == 0);
    }

    void UpdateInputField()
    {
        string formattedDate = $"{selectedDay:00}-{selectedMonth:00}-{selectedYear}";
        dateInputField.text = formattedDate;
    }
}
