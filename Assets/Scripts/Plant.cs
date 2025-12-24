using UnityEngine;

public class Plant : MonoBehaviour
{
    private SeedItem seed;
    private int currentDay = 0;

    public void Init(SeedItem seedData)
    {
        seed = seedData;
        transform.localScale = Vector3.zero;

        FindFirstObjectByType<DayCycle>().OnDayChanged += Grow;
    }

    private void OnDestroy()
    {
        FindFirstObjectByType<DayCycle>().OnDayChanged -= Grow;
    }

    private void Grow()
    {
        if (currentDay >= seed.maxGrowthDays) return;

        currentDay++;
        float t = (float)currentDay / seed.maxGrowthDays;
        transform.localScale = Vector3.one * t;
    }
}
