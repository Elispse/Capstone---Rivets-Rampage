using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUIManager : MonoBehaviour
{
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private FloatVariable playerHealth;
    [SerializeField] private VoidEvent gameEventStart;
    [SerializeField] List<HealthHeart> hearts = new List<HealthHeart>();

    private void Start()
    {
        DrawHearts();
    }

    public void ClearHearts()
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        hearts = new List<HealthHeart>();
    }

    public void CreateEmptyHeart()
    {
        GameObject newHeart = Instantiate(heartPrefab);
        newHeart.transform.SetParent(transform);

        HealthHeart heartComponent = newHeart.GetComponent<HealthHeart>();
        heartComponent.SetHeartImage(HeartStatus.Empty);
        hearts.Add(heartComponent);
    }

    public void DrawHearts()
    {
        ClearHearts();

        float maxHealthRemainder = playerHealth.initialValue % 2;
        int heartsToMake = (int)(playerHealth.initialValue / 2 + maxHealthRemainder);
        for (int i = 0; i < heartsToMake; i++)
        {
            CreateEmptyHeart();
        }

        for (int i = 0; i < hearts.Count; i++)
        {
            int heartStatusRemainder = (int)Mathf.Clamp(playerHealth.value - (i * 2), 0, 2);
            hearts[i].SetHeartImage((HeartStatus)heartStatusRemainder);
        }
    }
}