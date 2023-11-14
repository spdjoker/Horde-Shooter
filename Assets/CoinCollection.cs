using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class CoinCollection : MonoBehaviour
{
    public ShopManagerScript ShopManager;

    private void Start()
    {
        //shop_script = GetComponent<ShopManagerScript>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Coin")
        {
            ShopManager.coins++;
            ShopManager.CoinsTXT.text = "Coins: " + ShopManager.coins.ToString();
            Destroy(other.gameObject);
        }
    }
}
