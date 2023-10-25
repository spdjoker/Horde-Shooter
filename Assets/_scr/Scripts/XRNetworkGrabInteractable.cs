using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable), typeof(PhotonView), typeof(PhotonRigidbodyView))]
public class XRNetworkGrabInteractable : MonoBehaviour
{
    // Start is called before the first frame update
    private PhotonView photonView;

    void Start()
    {
        GetComponent<XRGrabInteractable>().selectEntered.AddListener(OnSelectEnter);
        photonView = GetComponent<PhotonView>();
        photonView.OwnershipTransfer = OwnershipOption.Request;
    }

    // Update is called once per frame
    protected void OnSelectEnter(SelectEnterEventArgs args) {
        photonView.RequestOwnership();
    }
}
