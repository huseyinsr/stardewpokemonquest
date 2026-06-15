using System;
using System.Collections;
using UnityEngine;

public class WaterCan : MonoBehaviour
{
    [SerializeField] private ItemPrefabMapping waterCanMapping;
    [SerializeField] private float returnToInventoryDelay = 2.0f;

    public static event Action OnSeedWatered;

    private void Start()
    {
        StartCoroutine(WateringProcess());
    }

    private IEnumerator WateringProcess()
    {
        OnSeedWatered?.Invoke();

        //Debug.Log("Watering the seed...");

        yield return new WaitForSeconds(returnToInventoryDelay);

        GameObject targetReceiverObject = GameObject.Find("Seed");

        if (targetReceiverObject != null && waterCanMapping != null)
        {
            MonoBehaviour[] allScripts = targetReceiverObject.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour script in allScripts)
            {
                if (script != null)
                {
                    UnlockReceiverFlags(script);
                }
            }
        }

        if (Inventory.Instance != null && waterCanMapping != null && waterCanMapping.item != null)
        {
            Inventory.Instance.AddItem(waterCanMapping.item);
        }

        Destroy(gameObject);
    }

    private void UnlockReceiverFlags(object receiverInstance)
    {
        Type receiverType = receiverInstance.GetType();

        System.Reflection.FieldInfo itemMappingsField = receiverType.GetField("itemPrefabMappings", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        System.Reflection.FieldInfo itemFlagsField = receiverType.GetField("usedItemFlags", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        System.Reflection.FieldInfo spawnFlagsField = receiverType.GetField("usedSpawnFlags", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        if (itemMappingsField != null && itemFlagsField != null)
        {
            ItemPrefabMapping[] mappings = (ItemPrefabMapping[])itemMappingsField.GetValue(receiverInstance);
            bool[] itemFlags = (bool[])itemFlagsField.GetValue(receiverInstance);

            int index = Array.FindIndex(mappings, m => m.item == waterCanMapping.item);

            if (index != -1)
            {
                itemFlags[index] = false;

                if (spawnFlagsField != null)
                {
                    bool[] spawnFlags = (bool[])spawnFlagsField.GetValue(receiverInstance);
                    for (int i = 0; i < spawnFlags.Length; i++) spawnFlags[i] = false;
                }
            }
        }
    }
}