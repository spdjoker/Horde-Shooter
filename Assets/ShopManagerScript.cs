using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class ShopManagerScript : MonoBehaviourPunCallbacks
{
    public int[,] shopItems = new int[5, 5];
    public float coins;
    public TMPro.TMP_Text CoinsTXT;
    public string[] guns;

    // Start is called before the first frame update
    void Start()
    {
        CoinsTXT.text = "Coins:" + coins.ToString();

        //ID's
        shopItems[1, 1] = 1;
        shopItems[1, 2] = 2;
        shopItems[1, 3] = 3;
        shopItems[1, 4] = 4;

        //Price
        shopItems[2, 1] = 10;
        shopItems[2, 2] = 20;
        shopItems[2, 3] = 30;
        shopItems[2, 4] = 40;

        //Quantity
        shopItems[3, 1] = 0;
        shopItems[3, 2] = 0;
        shopItems[3, 3] = 0;
        shopItems[3, 4] = 0;
    }

    public void Buy()
    {
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;

        if(coins >= shopItems[2, ButtonRef.GetComponent<ButtonInfo>().ItemID])
        {
            coins -= shopItems[2, ButtonRef.GetComponent<ButtonInfo>().ItemID];
            //shopItems[2, ButtonRef.GetComponent<ButtonInfo>().ItemID]++; (Increases Price)
            shopItems[3, ButtonRef.GetComponent<ButtonInfo>().ItemID]++;
            CoinsTXT.text = "Coins:" + coins.ToString();
            ButtonRef.GetComponent<ButtonInfo>().QuantityTxt.text = shopItems[3, ButtonRef.GetComponent<ButtonInfo>().ItemID].ToString();
            
            PhotonNetwork.Instantiate(guns[ButtonRef.GetComponent<ButtonInfo>().ItemID], transform.position, Quaternion.identity, 0); 
            
            
        }
    }

    public void OnTriggerEnter(Collider other)
    {

        if (other.transform.tag == "Coin")
        {
            Destroy(other.gameObject);
            coins++;
            CoinsTXT.text = "Coins: " + coins.ToString();
            
        }
    }
}
