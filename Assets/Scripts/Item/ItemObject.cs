using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public interface IInteractable
{
    public string GetInteractableName();
    public void OnInteract();
}

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;

    public string GetInteractableName()
    {
        string str = $"{data.displayName}\n{data.description}";

        return str;
    }

    public void OnInteract()
    {
        CharacterManager.Instance.Player.itemData = data;
        CharacterManager.Instance.Player.addItem?.Invoke();
        Destroy(gameObject);
    }
}
