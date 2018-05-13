using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    static public Weapons instance;

    public WeaponData[] allWeapons;

    public Transform frontWeapon;
    public Transform rearWeapon;
    public Transform topWeapon;
    public Transform bottomWeapon;
    public Transform bulletsParent;
    public GameObject[] bullets;//0=FrontGatling,1=RearGatling

    bool usingSide = true;//true=dcha, false=izda

    public enum WeaponTypes
    {
        None,
        FrontGatling,
        RearGatling,
    }

    public enum WeaponPosition
    {
        Front = 0,
        Rear = 1,
        Top = 2,
        Bottom = 3
    }

    myWeapon currentWeapon;
    public struct myWeapon
    {
        public WeaponTypes type;
        public WeaponPosition position;
        public GameObject prefab;
        public float damage;
        public float firingRate;
        public float overheat;
        public float rotSpeed;
        public float rotRange;

        //current rotation in degrees
        public float currentRot;

        public myWeapon(WeaponTypes _type, float _currentRot = 0)
        {
            WeaponData[] weps = Weapons.instance.allWeapons;
            currentRot = _currentRot;
            bool found = false;
            int index = -1;
            for (int i = 0; i < weps.Length; i++)
            {
                print("checking if " + weps[i].weaponType + " is of type " + _type);
                if (_type == weps[i].weaponType)
                {
                    found = true;
                    index = i;
                    break;
                }
            }
            if (!found)
            {
                //default settings
                type = WeaponTypes.None;
                position = WeaponPosition.Front;
                prefab = null;
                damage = 0;
                firingRate = 0;
                overheat = 0;
                rotSpeed = 0;
                rotRange = 0;
                Debug.LogError("Error: Weapon " + _type + " not found in weapon database");
            }
            else
            {
                type = _type;
                position = weps[index].weaponPosition;
                prefab = weps[index].prefab;
                damage = weps[index].damage;
                firingRate = weps[index].firingRate;
                overheat = weps[index].overheat;
                rotSpeed = weps[index].rotateSpeed;
                rotRange = weps[index].rotateRange;
            }
        }

    }

    List<myWeapon> currentWeapons;//0=FRONT, 1=REAR, 2=TOP, 3=BOTTOM

    private void Awake()
    {
        instance = this;
        currentWeapons = new List<myWeapon>();

        //para prototipo cargamos ambas torretas
        currentWeapons.Add(new myWeapon(WeaponTypes.FrontGatling));
        currentWeapons.Add(new myWeapon(WeaponTypes.RearGatling));
        currentWeapons.Add(new myWeapon());//empty
        currentWeapons.Add(new myWeapon());//empty
        currentWeapon = currentWeapons[0];
    }

    private void Start()
    {
        usingSide = Player.instance.direction;
        switch (Player.instance.direction)
        {
            case true:
                currentWeapon = currentWeapons[0];
                break;
            case false:
                currentWeapon = currentWeapons[1];
                break;
        }
        print("CurrentWeapon=> name: " + currentWeapon.type + "; rotRange: " + currentWeapon.rotRange);
    }

    private void Update()
    {
        float input = Input.GetAxisRaw("jVertical");
        if (input != 0)
        {
            print("moveTurret");
            MoveTurret(input);
        }
        float inpWepSelH = Input.GetAxisRaw("WeaponFront");
        float inpWepSelV = Input.GetAxisRaw("WeaponTop");
        if (inpWepSelH > 0.2f)
        {
            ChangeWeapon(WeaponPosition.Front);
        }
        else if (inpWepSelH < -0.2f)
        {
            ChangeWeapon(WeaponPosition.Rear);
        }
        if (inpWepSelV > 0.2f)
        {
            ChangeWeapon(WeaponPosition.Top);
        }
        else if (inpWepSelV < -0.2f)
        {
            ChangeWeapon(WeaponPosition.Bottom);
        }
    }

    void ChangeWeapon(WeaponPosition pos)
    {
        print("Changed weapon to pos " + pos);
        //if(Input ..
        //currentWeapon=currentWeapons[x]
        //guardar datos del arma
        switch (currentWeapon.position)
        {
            case WeaponPosition.Front:
                currentWeapons[0] = currentWeapon;
                break;
            case WeaponPosition.Rear:
                currentWeapons[1] = currentWeapon;
                break;
            case WeaponPosition.Top:
                currentWeapons[2] = currentWeapon;
                break;
            case WeaponPosition.Bottom:
                currentWeapons[3] = currentWeapon;
                break;
        }

        switch (pos)
        {
            case WeaponPosition.Front:
                currentWeapon = currentWeapons[0];
                print("Front wep");
                break;
            case WeaponPosition.Rear:
                currentWeapon = currentWeapons[1];
                print("Rear wep");
                break;
            case WeaponPosition.Top:
                currentWeapon = currentWeapons[2];
                break;
            case WeaponPosition.Bottom:
                currentWeapon = currentWeapons[3];
                break;
        }
        
    }

    void MoveTurret(float input)
    {
        float auxRot = 0;
        float finalRot = 0;
        switch (currentWeapon.position)
        {
            case WeaponPosition.Front:
                /*print("Input: " + input + "; currentRot: " + currentWeapon.currentRot + "; RealRot: " + frontWeapon.rotation.eulerAngles.z + "; maxRotRange: " + currentWeapon.rotRange);
                print("Under max Rot Range: moving for " + (-input * currentWeapon.rotSpeed * Time.deltaTime) + " degrees");*/
                auxRot = currentWeapon.currentRot + (-input * currentWeapon.rotSpeed * Time.deltaTime);
               //print("auxRot: " + auxRot);
                finalRot = Mathf.Clamp(auxRot, -currentWeapon.rotRange, currentWeapon.rotRange);
                currentWeapon.currentRot = finalRot;
               //print("finalRot: " + finalRot);
                frontWeapon.localRotation = Quaternion.Euler(0, 0, finalRot);
                break;
            case WeaponPosition.Rear:
               /*print("Input: " + input + "; currentRot: " + currentWeapon.currentRot + "; RealRot: " + rearWeapon.rotation.eulerAngles.z + "; maxRotRange: " + currentWeapon.rotRange);
                print("Under max Rot Range: moving for " + (input * currentWeapon.rotSpeed * Time.deltaTime) + " degrees");*/
                auxRot = currentWeapon.currentRot + (input * currentWeapon.rotSpeed * Time.deltaTime);
               //print("auxRot: " + auxRot);
                finalRot = Mathf.Clamp(auxRot, -currentWeapon.rotRange, currentWeapon.rotRange);
                currentWeapon.currentRot = finalRot;
               //print("finalRot: " + finalRot);
                rearWeapon.localRotation = Quaternion.Euler(0, 0, finalRot);
                break;
            case WeaponPosition.Top:
                break;
            case WeaponPosition.Bottom:
                break;
        }
    }

    void Shoot()
    {
        switch (currentWeapon.position)
        {
            case WeaponPosition.Front:
                Quaternion rot = Quaternion.Euler(0,0,0);
                Instantiate(currentWeapon.prefab,frontWeapon.position,rot,bulletsParent);
                break;
            case WeaponPosition.Rear:
                break;
            case WeaponPosition.Top:
                break;
            case WeaponPosition.Bottom:
                break;
        }
    }

}
