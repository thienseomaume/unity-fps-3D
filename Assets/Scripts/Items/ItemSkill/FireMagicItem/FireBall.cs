using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour,ISkill
{
    private SkillInfor skillInfor;
    private LayerMask interactionLayer;
    private bool isInitialized = false;
    private Vector3 startPosition;
    private Collider[] interactedCollider = new Collider[1];
    [SerializeField] private GameObject explodeParticle;
    
    public void Initialize(Vector3 position, Quaternion rotation, SkillInfor skillInfor, LayerMask interactionLayer)
    {
        transform.position = position;
        startPosition = position;
        transform.rotation = rotation;
        this.skillInfor = skillInfor;
        this.interactionLayer = interactionLayer;
        isInitialized = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isInitialized)
        {
            CheckingDestroy();
            transform.position += Time.deltaTime * transform.forward * skillInfor.speed;
            Physics.OverlapSphereNonAlloc(transform.position, skillInfor.interactionRadius, interactedCollider,interactionLayer);
            if (interactedCollider[0] != null)
            {
                interactedCollider[0].GetComponent<IHealth>()?.DecreaseHealth(skillInfor.baseDamage);
                interactedCollider[0].GetComponent<EffectManager>()?.ApplyEffect(new BurningEffect(),skillInfor);
                Instantiate(explodeParticle, transform.position,Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
    private void CheckingDestroy()
    {
        float skillRange = skillInfor.skillRange;
        if ((startPosition - transform.position).sqrMagnitude >= skillRange * skillRange)
        {
            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        
    }
}
