using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.State;

[RequireComponent(typeof(XRGrabInteractable))]
public class Gun : MonoBehaviour
{
    [SerializeField] private GunData gunData;
    [SerializeField] private ParticleSystem flashParticleSystem;
    [SerializeField] private ParticleSystem impactParticleSystem;
    [SerializeField] private TrailRenderer bulletTrailRenderer;

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

        Debug.DrawRay(flashParticleSystem.transform.position, flashParticleSystem.transform.forward, Color.red);
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

    private bool CanFire() => Time.time > timeOfNextShot;

    // Shoots bullet
    private void Fire() {
        RaycastHit hit;
        Physics.Raycast(flashParticleSystem.transform.position, transform.forward, out hit, gunData.range);

        flashParticleSystem.Play();

        TrailRenderer trail = Instantiate(bulletTrailRenderer, flashParticleSystem.transform.position, Quaternion.identity);
        StartCoroutine(SpawnTrail(trail, hit));

        timeOfNextShot = Time.time + 1.0f / gunData.fireRate;
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

    private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit) {
        float time = 0;
        Vector3 startPosition = trail.transform.position;

        if (hit.collider == null) {
            hit.point = startPosition + flashParticleSystem.transform.forward * gunData.range;
        }

        while (time < 1git .0f) {
            trail.transform.position = Vector3.Lerp(startPosition, hit.point, time);
            time += Time.deltaTime / trail.time;

            yield return null;
        }

        trail.transform.position = hit.point;
        // Spawn if there was a hit
        if (hit.collider != null) {
            Instantiate(impactParticleSystem, hit.point, Quaternion.LookRotation(hit.normal));
        }
        Destroy(trail.gameObject, trail.time);
    }
}
