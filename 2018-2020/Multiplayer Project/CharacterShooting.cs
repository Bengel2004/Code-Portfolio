using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CharacterShooting : MonoBehaviour
{
    private PhotonView pv;
    private CharacterStats C_Stats;
    public Weapon currentWeapon;
    public List<Weapon> weapons = new List<Weapon>();
    public GameObject shovel; // ingeval er meerdere tools komen, maak een aparte class aan
    [SerializeField] private LineRenderer line = default;
    [SerializeField] private Camera camera = default;

    // Start is called before the first frame update
    void Awake()
    {
        pv = GetComponent<PhotonView>();
        C_Stats = GetComponent<CharacterStats>();
        currentWeapon.gameObject.SetActive(false);
        currentWeapon.pv = pv;
        shovel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // kijkt of t multiplayer object van de huidige speler is
        if (pv.IsMine)
        {
            Shoot();
            GetGun();
            SwitchWeapon();
        }
    }

    private void SwitchWeapon()
    {
        // Stuurt commands naar de server en kiest het juiste wapen uit de List
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            pv.RPC("SwitchWeaponRPC", RpcTarget.AllBuffered, 0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            pv.RPC("SwitchWeaponRPC", RpcTarget.AllBuffered, 1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            pv.RPC("GetShovel", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    private void SwitchWeaponRPC(int _index)
    {
        // checkt of t wapen geen schep is, Maak hier een enum voor
        C_Stats.hasShovel = false;
        shovel.SetActive(false);
        if (currentWeapon.gameObject.activeSelf)
        {
            // kijkt of t huidige wapen is, zo ja, zet uit
            if (currentWeapon == weapons[_index])
            {
                currentWeapon.gameObject.SetActive(false);
                C_Stats.hasGun = false;
            }
            else
            {
                // niet het huidige wapen, switch wapen
                currentWeapon.gameObject.SetActive(false);
                currentWeapon = weapons[_index];
                currentWeapon.gameObject.SetActive(true);
                C_Stats.C_Audio.shootingClip = currentWeapon.shootingClip;
            }
        }
        else
        {
            C_Stats.hasGun = true; // nieuw
            currentWeapon = weapons[_index];
            currentWeapon.gameObject.SetActive(true);
            C_Stats.C_Audio.shootingClip = currentWeapon.shootingClip;
        }
    }
    // pakt de schep en zet het huidige wapen uit
    [PunRPC]
    private void GetShovel()
    {
        C_Stats.hasGun = false;
        C_Stats.hasGun = false;
        if (!shovel.activeSelf)
        {
            C_Stats.hasShovel = true;
            currentWeapon.gameObject.SetActive(false);
            shovel.SetActive(true);
        }
        else
        {
            shovel.SetActive(false);
            C_Stats.hasShovel = false;
        }
    }

    private void GetGun()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            pv.RPC("RetrieveGun", RpcTarget.AllBuffered);
        }
    }
    // verouderd, kan worden verwijderd
    [PunRPC]
    private void RetrieveGun()
    {
        C_Stats.hasGun = !C_Stats.hasGun;
        currentWeapon.gameObject.SetActive(C_Stats.hasGun);
    }
    // richten server commando
    [PunRPC]
    private void Aim()
    {
        C_Stats.isAiming = Input.GetMouseButton(1);
    }
    // schiet functie
    private void Shoot()
    {
        if (C_Stats.GetHealth > 0)
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                transform.gameObject.SetActive(false);
            }

            pv.RPC("Aim", RpcTarget.All); // aim signal to server

            if (C_Stats.isAiming)
            {
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    // berekent speler positie
                    Vector3 _tempLocalPosition = transform.position;
                    _tempLocalPosition.y += 1f;

                    // set lines to 2
                    line.positionCount = 2;
                    // kijk of hij aimt
                    if (C_Stats.hasGun)
                        line.SetPosition(0, currentWeapon.bulletSpawnPoint.position); // lijn start bij wapen
                    else
                        line.SetPosition(0, _tempLocalPosition); // lijn start bij speler

                    line.SetPosition(1, hit.point);


                    Vector3 _lookAt = new Vector3(hit.point.x, transform.position.y, hit.point.z); // kijkt naar aimed position
                    transform.LookAt(_lookAt);
                }
                if (C_Stats.hasGun)
                {
                    if (Input.GetMouseButton(0) && Time.time > currentWeapon.timeStamp)
                    {
                        currentWeapon.pv.RPC("Shoot", RpcTarget.All, hit.point.x, hit.point.y, hit.point.z);
                        C_Stats.C_Audio.pv.RPC("ShootAudio", RpcTarget.All);
                        // OUDE CODE Schoon op binnekort
                        //if (hit.transform.tag == "Character" && hit.transform.gameObject != this.gameObject)
                        //{
                        //    hit.transform.GetComponent<CharacterStats>().pv.RPC("GetDamaged", RpcTarget.AllBuffered, currentWeapon.damage);
                        //}
                    }
                }
            }
            else
            {
                line.positionCount = 0;
            }
        }
    }
}
