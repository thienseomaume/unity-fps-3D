using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGun : MonoBehaviour, IItem, IUsable, IRPress, ILeftClick, IQPress
{
    [SerializeField] private ItemsEnum itemType;
    [SerializeField] string id;
    [SerializeField] int damage;
    [SerializeField] int magazineLimit;
    [SerializeField] float reloadCooldown;
    [SerializeField] float reloadTimer;
    [SerializeField] float fireCooldown;
    [SerializeField] float fireTimer;
    [SerializeField] int _magazineCurent;
    [SerializeField]
    int magazineCurrent
    {
        get
        {
            return _magazineCurent;
        }
        set
        {
            _magazineCurent = value;
            EventCenter.Instance().OnMagazineChange(magazineCurrent);
        }
    }

    [SerializeField] bool canSelectMode;
    [SerializeField] bool isAutoOn;
    [SerializeField] bool isActivated;
    [SerializeField] bool isOwned;
    [SerializeField] bool isReloading;
    [SerializeField] bool isFiring;
    [SerializeField] Transform barrelOfGun;
    [SerializeField] GameObject bulletTrail;
    private Queue<GameObject> bulletPool;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] Animator gunAnimator;
    [SerializeField] public AnimatorOverrideController armAnimatorController;
    [SerializeField] private AudioClip impactDirt;
    [SerializeField] private AudioClip impactMetal;
    [Header("Gun Recoil")]
    [SerializeField] private float speedDownRecoil;
    [SerializeField] private float verticleRecoilStep;
    [SerializeField] private float verticleRecoil;
    [SerializeField] private float maxVerticleRecoil;
    [SerializeField] private float horizontalRecoilStep;
    [SerializeField] private float horizontalRecoil;
    private Transform cameraTransform;
    private Transform rootTransform;
    private LayerMask interactableLayer ;
    private void OnEnable()
    {
        Debug.Log("active: " + id);
    }
    private void Start()
    {
        interactableLayer = LayerMask.GetMask("Ground","Enemy","Default");
        gunAnimator = GetComponent<Animator>();
        bulletPool = new Queue<GameObject>();

        GameObject bulletHolder = new GameObject();
        DontDestroyOnLoad(bulletHolder);
        for (int i = 0; i < 10; i++)
        {
            GameObject bullet = Instantiate(bulletTrail);
            bulletPool.Enqueue(bullet);
            bullet.transform.parent = bulletHolder.transform;
            bullet.GetComponent<BulletScript>().gun = this.gameObject;
            bullet.SetActive(false);
        }
    }
    private void Update()
    {
        updateFireCoolDown();
        if (isReloading)
        {
            updateReloadCoolDown();
        }
        DownRecoilToRoot();
    }
    private void Fire(Transform cameraTransform, Transform rootTransform)
    {
        magazineCurrent -= 1;
        RaycastHit hit;
        Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, 100,interactableLayer);
        Collider interactionCollider = hit.collider;
        var bullet = bulletPool.Dequeue();
        bullet.transform.position = barrelOfGun.position;
        bullet.SetActive(true);
        if (interactionCollider != null)
        {
            if ((hit.point - cameraTransform.position).magnitude < 5)
            {
                bullet.GetComponent<BulletScript>().setBegin(barrelOfGun.position, cameraTransform.position + 5 * cameraTransform.forward);
            }
            else
            {
                bullet.GetComponent<BulletScript>().setBegin(barrelOfGun.position, hit.point);
            }
            if (interactionCollider.GetComponent<IHealth>() == null)
            {
                Debug.Log("ihealth component is null");
                GameObject impactClone = BulletImpactManager.Instance().dirtImpactPool.Dequeue();
                if(impactClone == null)
                {
                    Debug.Log("impact is null");
                }
                impactClone.transform.position = hit.point;
                impactClone.transform.rotation = Quaternion.LookRotation(hit.normal);
                impactClone.SetActive(true);
                SoundFxManager.Instance().SpawnSound(impactDirt, hit.point);
                BulletImpactManager.Instance().dirtImpactPool.Enqueue(impactClone);
            }
            else
            {
                interactionCollider.GetComponent<IHealth>().DecreaseHealth(damage);
                Debug.Log("checked hit");
                GameObject impactClone = BulletImpactManager.Instance().metalImpactPool.Dequeue();
                impactClone.transform.position = hit.point;
                impactClone.transform.rotation = Quaternion.LookRotation(hit.normal);
                impactClone.SetActive(true);
                SoundFxManager.Instance().SpawnSound(impactMetal, hit.point);
                BulletImpactManager.Instance().metalImpactPool.Enqueue(impactClone);
            }
        }
        else
        {
            bullet.GetComponent<BulletScript>().setBegin(barrelOfGun.position, cameraTransform.position + 400 * cameraTransform.forward);
        }
        muzzleFlash.Play();
        gunAnimator.Play(AnimationData.ITEM_LEFT_CLICK, -1, 0);
        isFiring = true;
        fireTimer = fireCooldown;
        maxVerticleRecoil = 60;
        verticleRecoilStep = 3;
        verticleRecoilStep = Mathf.Clamp(verticleRecoilStep * (float)(verticleRecoil / maxVerticleRecoil), verticleRecoilStep, verticleRecoilStep * 2.0f);
        verticleRecoil += verticleRecoilStep;
        //Debug.Log("verticle recoil = " + verticleRecoil);
        horizontalRecoilStep = 2;
        horizontalRecoil = Random.RandomRange(-horizontalRecoilStep, horizontalRecoilStep);
    }
    private void DownRecoilToRoot()
    {
        if (cameraTransform == null || rootTransform == null)
        {
            return;
        }
        speedDownRecoil = Mathf.Clamp(verticleRecoil * 2, 20, 50);
        if (verticleRecoil > 0)
        {
            verticleRecoil -= speedDownRecoil * Time.deltaTime;
        }
        else
        {
            verticleRecoil = 0;
        }
        if (Mathf.Abs(horizontalRecoil) != 0)
        {
            if (horizontalRecoil < 0)
            {
                horizontalRecoil = Mathf.Clamp(horizontalRecoil + speedDownRecoil * Time.deltaTime, horizontalRecoil, 0);
            }
            else
            {
                horizontalRecoil = Mathf.Clamp(horizontalRecoil - speedDownRecoil * Time.deltaTime, 0, horizontalRecoil);

            }
        }
        else
        {
            horizontalRecoil = 0;
        }
        //Debug.Log("horizontal recoil = " + horizontalRecoil);

        cameraTransform.forward = Quaternion.AngleAxis(-verticleRecoil, rootTransform.right) * rootTransform.forward;
        cameraTransform.forward = Quaternion.AngleAxis(horizontalRecoil, cameraTransform.up) * cameraTransform.forward;
    }
    private void updateFireCoolDown()
    {
        if (fireTimer > 0)
        {
            fireTimer -= Time.deltaTime;
        }
        else
        {
            isFiring = false;
        }
    }
    private void updateReloadCoolDown()
    {
        reloadTimer -= Time.deltaTime;
        if (reloadTimer <= 0)
        {
            InventoryController.Instance().RemoveItem(magazineLimit - magazineCurrent, ItemsEnum.ammo);
            //Debug.Log("current: " + magazineCurrent + "||||||Ammo left: " + InventoryManager.Instance().getNumber(ItemsEnum.ammo));
            magazineCurrent += (magazineLimit - magazineCurrent);
            isReloading = false;
            reloadTimer = reloadCooldown;
        }
    }
    //public void ChangeMode()
    //{
        
    //}
    public bool IsActivated()
    {
        return isActivated;
    }
    public void setActivated(bool isActivated)
    {
        this.isActivated = isActivated;
    }
    public bool IsOwned()
    {
        return isOwned;
    }
    public bool IsFiring()
    {
        return isFiring;
    }
    public bool IsReloading()
    {
        return isReloading;
    }
    public void ReturnBulletPool(GameObject bullet)
    {
        bulletPool.Enqueue(bullet);
    }
    public int GetMagazine()
    {
        return magazineCurrent;
    }
    public string GetGunID()
    {
        return id;
    }
    public void SetMagazine(int magazine)
    {
        magazineCurrent = magazine;
    }
    public void SetOwned(bool owned)
    {
        isOwned = owned;
    }
    private void OnDisable()
    {
        horizontalRecoil = 0;
        verticleRecoil = 0;
    }
    public ItemsEnum GetItemType()
    {
        return this.itemType;
    }

    public bool IsUsing()
    {
        if (isFiring || isReloading)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public AnimatorOverrideController GetAnimatorOverride()
    {
        if(armAnimatorController != null)
        {
            return armAnimatorController;
        }
        else
        {
            return null;
        }
    }

    public void UseLeftClick(System.Action actionUseSuccess)
    {
        if (magazineCurrent > 0 && !isReloading && !isFiring)
        {
            if (this.cameraTransform == null)
            {
                this.cameraTransform = PlayerInformation.Instance().GetEnvironmentCamera();
            }
            if (this.rootTransform == null)
            {
                this.rootTransform = PlayerInformation.Instance().GetRootTransform();
            }
            if (isAutoOn)
            {
                Fire(cameraTransform, rootTransform);
                actionUseSuccess();
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Fire(cameraTransform, rootTransform);
                    actionUseSuccess();
                }
            }
        }
    }

    public void UseRPress(System.Action actionUseSuccess)
    {
        if (magazineCurrent < magazineLimit && (magazineLimit - magazineCurrent) < InventoryController.Instance().GetItemAmount(ItemsEnum.ammo) && !isFiring)
        {
            isReloading = true;
            reloadTimer = reloadCooldown;
            gunAnimator.Play(AnimationData.ITEM_RPRESS, -1, 0);
            actionUseSuccess();
        }
    }

    public void UseQPress(System.Action actionUseSuccess)
    {
        if (!isFiring && !isReloading && canSelectMode)
        {
            isAutoOn = !isAutoOn;
        }
    }

    public void SetUp()
    {
       
    }
}