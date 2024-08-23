using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBar : MonoBehaviour
{
    public List<GameObject> playerHearts;

    void Awake()
    {
        if(playerHearts == null)
        {
            playerHearts = new List<GameObject>();
            foreach(Transform child in transform)
            {
                if(!playerHearts.Contains(child.gameObject))
                {
                    playerHearts.Add(child.gameObject);
                }
                
            }
        }
    }

    public void CheckHearths(float currentHealth, float maxHealth)
    {
        float heartValue = maxHealth / playerHearts.Count;
        int fullHearts = (int)(currentHealth/heartValue);

        for(int i = 0; i < playerHearts.Count; i++)
        {
            if(fullHearts > 0)
            {
                fullHearts--;
                playerHearts[i].SetActive(true);
            }
            else
            {
                playerHearts[i].SetActive(false);
            }
        }
    }
}
