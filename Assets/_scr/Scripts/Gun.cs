using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.State;

public class Gun : MonoBehaviour
{
    [SerializeField] private GunData gunData;
    float timeOfNextShot = 0.0f;
    bool isFiring = false;

    private XRGrabInteractable grabInteractable;

    private void Start() {
        grabInteractable = GetComponent<XRGrabInteractable>();

        // Subscribing to the events
        grabInteractable.activated.AddListener(OnActivate);
        grabInteractable.deactivated.AddListener(OnDeactivate);
    }

#if UNITY_EDITOR
    private void Update() {
        // For no Oculus testing purposes.
        if (Input.GetMouseButtonDown(0)) {
            OnActivate(new ActivateEventArgs());
        }
        if (Input.GetMouseButtonUp(0)) {
            OnDeactivate(new DeactivateEventArgs());
        }
    }
#endif

    private void OnActivate(ActivateEventArgs args) {
        isFiring = true;

        // Starts a coroutine for automatic weapons that shoot repeatedly
        if (gunData.properties.HasFlag(GunData.GunFlags.Automatic)) {
            StartCoroutine(AutoFire());
            return;
        }

        // Shoots for guns that are semi-auto
        if (CanFire()) {
            Fire();
        }
    }

    private void OnDeactivate(DeactivateEventArgs args) {
        isFiring = false;
    }

    // Runs alongside update for automatic weapons
    private IEnumerator AutoFire() {
        while (isFiring) {
            if (CanFire()) {
                Fire();
            }
            yield return null;
        }
    }

    private bool CanFire() => Time.time > timeOfNextShot;

    // Shoots bullet
    private void Fire() {
        string line = "Shot";

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, gunData.range)) {
            line = "Shot and hit " + hit.transform.name + ' ' + (Vector3.Distance(transform.position, hit.point)) + "m away.";
        }

        Debug.Log(line);
        timeOfNextShot = Time.time + 1.0f / gunData.fireRate;
    }

    private void OnGunShot() {
        // Some muzzle flash effects or bullet particle
    }
}
