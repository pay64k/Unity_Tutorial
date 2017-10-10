using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GunBehaviour : MonoBehaviour {

    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 30f;
    public int maxAmmo = 30;
    public float reloadTime = 1f;
    public float impactForce = 10f;

    public Text ammoText;
    public GameObject reloadBar;
    
    public Camera fpsCam;

    private float nextTimeToFire = 0f;
    private int currentAmmo;
    private bool isReloading;

	// Use this for initialization
	void Start () {
        currentAmmo = maxAmmo;
        ammoText.text = string.Concat(maxAmmo, "/", maxAmmo);
	}
	
	// Update is called once per frame
	void Update () {

        if (isReloading)
            return;

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire && currentAmmo > 0)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
            muzzleFlash.Play();
        }
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo)
        {
            StartCoroutine(Reload());
        }
	}

    IEnumerator Reload()
    {
        isReloading = true;
        reloadBar.GetComponent<Progress>().UpdateProgress(reloadTime);
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
        ammoText.text = string.Concat(currentAmmo, "/", maxAmmo);
    }

    void Shoot()
    {
        currentAmmo--;
        ammoText.text = string.Concat(currentAmmo, "/", maxAmmo);
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            TntEnemyBehaviour target = hit.transform.GetComponent<TntEnemyBehaviour>();
            if(target != null)
            {
                target.TakeDamage(damage);
            }

            if(hit.rigidbody != null)
            {
                //hit.rigidbody.AddExplosionForce(-hit.normal * impactForce);
            }

            GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impact, 1f); 
        }

    }
}
