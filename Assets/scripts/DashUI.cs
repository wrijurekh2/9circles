using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class DashUI : MonoBehaviour
{
    public GameObject dashBarPrefab;
    public PlayerMovement playerMovement;

    private List<Image> fillBars = new List<Image>();
    void Start()
    {
        for (int i = 0; i < playerMovement.maxDashCharges; i++)
        {
            GameObject bar = Instantiate(dashBarPrefab, transform);
            Image fill = bar.GetComponentsInChildren<Image>()[1];
            fillBars.Add(fill);
        }
    }

    void Update()
    {
        for (int i = 0; i < fillBars.Count; i++)
        {
            if (i < playerMovement.currDashCharges - 1)
                fillBars[i].fillAmount = 1f;
            else if (i == playerMovement.currDashCharges - 1)
                fillBars[i].fillAmount = playerMovement.timer / playerMovement.dashCooldown;
            else
                fillBars[i].fillAmount = 0f;
        }
    }
}
