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

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Coin")
        {
            object p = ShopManager.coins++;
            //shop_script.CoinsTXT.text = "Coins: " + shop_script.coins.ToString();
            Destroy(other.gameObject);
        }
    }
}
