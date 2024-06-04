using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class DatePickerController : MonoBehaviour
{
    public TMP_Dropdown dayDropdown;       // Dropdown for days
    public TMP_Dropdown monthDropdown;     // Dropdown for months
    public TMP_Dropdown yearDropdown;      // Dropdown for years
    public Button doneButton;              // Button to confirm the date selection

    private int selectedDay;
    private int selectedMonth;
    private int selectedYear;

    private int currentYear, currentMonth, currentDay;

    public Action<string> OnDateSelected;  // Callback for when the date is selected

    void Start()
    {
        DateTime now = DateTime.Now;
        currentYear = now.Year;
        currentMonth = now.Month;
        currentDay = now.Day;

        // Initialize default selections
        selectedDay = currentDay;
        selectedMonth = currentMonth;
        selectedYear = currentYear;

        // Populate dropdowns
        PopulateYears();
        PopulateMonths(12);
        PopulateDays(DateTime.DaysInMonth(selectedYear, selectedMonth));

        // Add listener to done button to update input field and deactivate date picker panel
        doneButton.onClick.AddListener(OnClickDoneButton);

        // Add listeners to dropdowns
        dayDropdown.onValueChanged.AddListener(delegate { UpdateSelectedDay(); });
        monthDropdown.onValueChanged.AddListener(delegate { UpdateSelectedMonth(); });
        yearDropdown.onValueChanged.AddListener(delegate { UpdateSelectedYear(); });

        // Set current selections
        SetCurrentSelections();
    }

    void OnClickDoneButton()
    {
        string formattedDate = $"{selectedDay:00}-{selectedMonth:00}-{selectedYear}";
        OnDateSelected?.Invoke(formattedDate);
        gameObject.SetActive(false);
    }

    void PopulateDays(int days)
    {
        dayDropdown.ClearOptions();
        for (int i = 1; i <= days; i++)
        {
            if (selectedYear == currentYear && selectedMonth == currentMonth && i > currentDay)
                break;

            string dayText = i < 10 ? "0" + i.ToString() : i.ToString();
            dayDropdown.options.Add(new TMP_Dropdown.OptionData(dayText));
        }
        dayDropdown.RefreshShownValue();
    }

    void PopulateMonths(int months)
    {
        monthDropdown.ClearOptions();
        for (int i = 1; i <= months; i++)
        {
            if (selectedYear == currentYear && i > currentMonth)
                break;

            string monthText = i < 10 ? "0" + i.ToString() : i.ToString();
            monthDropdown.options.Add(new TMP_Dropdown.OptionData(monthText));
        }
        monthDropdown.RefreshShownValue();
    }

    void PopulateYears()
    {
        yearDropdown.ClearOptions();
        for (int i = currentYear; i >= 1960; i--)
        {
            yearDropdown.options.Add(new TMP_Dropdown.OptionData(i.ToString()));
        }
        yearDropdown.RefreshShownValue();
    }

    void UpdateSelectedDay()
    {
        selectedDay = int.Parse(dayDropdown.options[dayDropdown.value].text);
    }

    void UpdateSelectedMonth()
    {
        selectedMonth = int.Parse(monthDropdown.options[monthDropdown.value].text);
        UpdateDaysForMonth();
    }

    void UpdateSelectedYear()
    {
        selectedYear = int.Parse(yearDropdown.options[yearDropdown.value].text);
        UpdateMonthsForYear();
        UpdateDaysForMonth();
    }

    void UpdateMonthsForYear()
    {
        PopulateMonths(12);
        if (selectedYear == currentYear && selectedMonth > currentMonth)
        {
            selectedMonth = currentMonth;
            monthDropdown.value = currentMonth - 1; // Update the dropdown to show the correct month
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

        // If the current month is selected, ensure the current day is also selected
        if (selectedYear == currentYear && selectedMonth == currentMonth)
        {
            selectedDay = currentDay;
            dayDropdown.value = currentDay - 1; // Update the dropdown to show the correct day
        }
    }

    void SetCurrentSelections()
    {
        dayDropdown.value = selectedDay - 1;
        monthDropdown.value = selectedMonth - 1;
        yearDropdown.value = currentYear - selectedYear;

        // Ensure the current day is selected if the current month and year are selected
        if (selectedYear == currentYear && selectedMonth == currentMonth)
        {
            dayDropdown.value = currentDay - 1;
        }
    }
}
