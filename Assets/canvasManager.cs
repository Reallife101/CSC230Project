using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class canvasManager : MonoBehaviour
{
    [SerializeField] private GameObject HealthItemPrefab;
    [SerializeField] private List<Sprite> characterImages;

    public List<canvasItem> HealthItems = new List<canvasItem>();

    public canvasItem AddHealthItem(string nickname, int actornum, Player player)
    {
        GameObject healthItem = Instantiate(HealthItemPrefab, transform);
        canvasItem item = healthItem.GetComponent<canvasItem>();
        Sprite character = characterImages[(int)player.CustomProperties["playerAvatar"]];
        item.Initialize(nickname, actornum, character);
        HealthItems.Add(item);
        SortHealthItems();
        return item;
    }

    public void SortHealthItems()
    {
        //sort the list (it still does not work, but it is not essential for health to work)
        if (HealthItems.Count > 1)
        {
            for (int i = 0; i < HealthItems.Count - 1; i++)
            {
                for (int j = i + 1; j < HealthItems.Count; j++)
                {
                    if (HealthItems[i].GetOwnerActorNumber() > HealthItems[j].GetOwnerActorNumber())
                    {
                        //For orderliness in the list
                        canvasItem tempitem = HealthItems[i];
                        HealthItems[i] = HealthItems[j];
                        HealthItems[j] = tempitem;
                    }
                }
                HealthItems[i].transform.SetSiblingIndex(i);
            }
        }
    }
}
