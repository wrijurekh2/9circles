using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DashUI : MonoBehaviour
{
    public GameObject dashBarPrefab;
    public PlayerMovement playerMovement;
    public float visualCharges = 0f;

    private List<Image> fillBars = new List<Image>();
    void Start()
    {
        for (int i = 0; i < playerMovement.maxDashCharges; i++)
        {
            GameObject bar = Instantiate(dashBarPrefab, transform);
            RectTransform rt = bar.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(80 * i, 0);
            Image[] images = bar.GetComponentsInChildren<Image>();
            Image fill = bar.GetComponentsInChildren<Image>()[1];
            fillBars.Add(fill);
        }
    }

    void Update()
    {
        float visualCharges = playerMovement.currDashCharges + 
            (playerMovement.timer / playerMovement.dashCooldown);
        for (int i = 0; i < fillBars.Count; i++)
        {
            float fill = visualCharges - i;
            fillBars[i].fillAmount = Mathf.Clamp(fill, 0f, 1f);
        }
    }
}
