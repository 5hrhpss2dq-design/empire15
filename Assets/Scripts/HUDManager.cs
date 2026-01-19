using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public Text roleText;
    public Text timerText;
    public Text zoneStatusText;

    void Start()
    {
        if (roleText) roleText.text = "Role: Soldier";
        if (timerText) timerText.text = "";
        if (zoneStatusText) zoneStatusText.text = "Zone: ---";
    }

    public void SetRole(string role)
    {
        if (roleText) roleText.text = $"Role: {role}";
    }

    public void SetTimer(string s)
    {
        if (timerText) timerText.text = s;
    }

    public void SetZoneStatus(string s)
    {
        if (zoneStatusText) zoneStatusText.text = $"Zone: {s}";
    }
}