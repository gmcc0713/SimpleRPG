using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    void Awake()
    {
        if (null == Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(gameObject);
    }
    private Inventory m_inventory;
    public Inventory _Inventory => m_inventory;

    public CharacterController controller;
    public Transform cam;
    private Vector2 input;
    [SerializeField] float speed = 0.8f;
    [SerializeField] float walkSpeed = 2f, runSpeed = 6f;
    private float gravity = -9.81f;
    private Vector3 moveDir;


    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    private bool isGround;
    private bool m_bCantDamaged;
    [SerializeField] private Animator animator;
    public Animator _playerAnim => animator;
    [SerializeField] private bool canAct;
    [SerializeField] public bool m_canAttack;
    private bool isMove = false;
    private bool isAttack;
    private PlayerCustom m_PlayerCustom;

    public bool _isAttack { get; set; }
    public bool _isMove { get; set; }
    public PlayerSkillController _PlayerSkill => m_PlayerSkill;
    public PlayerCustom _PlayerCustom=> m_PlayerCustom;
    public PlayerStats _statsData => m_statsData;
    private int attackCombo = 0;
    [SerializeField] private AttackCollider m_attackCollider;
    public bool _canAct => canAct;

    [SerializeField] private bool m_bisFirst;
    
     private PlayerData m_PlayerData;
    [SerializeField] private PlayerStats m_statsData;
    [SerializeField]  private float m_iCurPlayerHealth;
    [SerializeField]  private float m_iCurPlayerMP;
    [SerializeField] private Equipment m_Equipment;
    [SerializeField] private PlayerSkillController m_PlayerSkill;
    [SerializeField] private GameObject m_CustomPanel;
    [SerializeField]private AnimationEventSystem m_ParticleSystem;
    public AnimationEventSystem _ParticleSystem => m_ParticleSystem;
    void Update()
    {
        if(canAct)
            Move();
    }
   public void Initialize()
    {

        cam = Camera.main.transform;
        if(m_bisFirst)
        {
            m_PlayerData = new PlayerData();
            m_PlayerData.SetPlayerData("플레이어1", "무직", "초보자", 1, 0);
            m_inventory = new Inventory();
            m_statsData.Initialize();
            m_PlayerSkill.Initialize();
            QuickSlotManager.Instance.Initialize();
            m_ParticleSystem = GetComponent<AnimationEventSystem>();
            m_CustomPanel.gameObject.SetActive(true);
            animator = GetComponent<Animator>();
           
        }
        else
        {
            m_statsData.UpdatePlayerData(m_PlayerData);
            QuickSlotManager.Instance.UpdateAllUI();
            UIManager.Instance._PlayerDataUI.UpdateUI
                        (m_iCurPlayerHealth / m_statsData._StatsData.HP, m_iCurPlayerMP / m_statsData._StatsData.MP,
                        m_PlayerData._curEXP / m_PlayerData._EXP, m_PlayerData._sUserName);
            canAct = true;
        }
        NPCManager.Instance.Initialize();
        m_inventory.initialize();
        SetDamage();
        UIManager.Instance._PlayerDataUI.SetPlayeLVUI(m_PlayerData._LV);
        ResetPlayer();
        m_Equipment.Initialize();
        m_bisFirst = false;
    }
    public void ResetPlayer()
    {
        m_iCurPlayerHealth = m_statsData._StatsData.HP;
        m_iCurPlayerMP = m_statsData._StatsData.MP;

        animator.SetBool("IsMoving",false);
        gameObject.transform.position = Vector3.zero;
        canAct = true;
        m_canAttack = true;
        UIManager.Instance._PlayerDataUI.UpdateUI(m_iCurPlayerHealth / m_statsData._StatsData.HP, m_iCurPlayerMP / m_statsData._StatsData.MP,
                        m_PlayerData._curEXP / m_PlayerData._EXP, m_PlayerData._sUserName);
        UIManager.Instance._PlayerDataUI.SetPlayeLVUI(m_PlayerData._LV);
    }
    public void Respawn()
    {
        m_iCurPlayerHealth = m_statsData._StatsData.HP;
        m_iCurPlayerMP = m_statsData._StatsData.MP;

        canAct = true;
        UIManager.Instance._PlayerDataUI.UpdateUI(m_iCurPlayerHealth / m_statsData._StatsData.HP, m_iCurPlayerMP / m_statsData._StatsData.MP,
                        m_PlayerData._curEXP / m_PlayerData._EXP, m_PlayerData._sUserName);
        animator.SetBool("Restart", true);
        gameObject.transform.position = Vector3.zero;
    }
    private void Move()
    {
        if (isAttack)
        {
            return;
        }
        Vector3 direction = new Vector3(input.x, 0f, input.y).normalized;
        isMove = direction.magnitude >= 0.1f;


        if (isMove)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

        }
        if (!isGround)
        {
            moveDir.y += gravity * Time.deltaTime;

        }
        controller.Move(moveDir.normalized * speed * Time.deltaTime);
        animator.SetBool("IsMove", isMove);
        animator.SetFloat("Speed", speed);
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }
    public void RunModeOn(InputAction.CallbackContext context)
    {
        speed = runSpeed;

    }
    public void RunModeOff(InputAction.CallbackContext context)
    {
        speed = walkSpeed;
    }

    public void OnMouseButtonDown(InputAction.CallbackContext context)
    {
        if (canAct && m_canAttack)
        {
            attackCombo++;
            isAttack = false;
            if (attackCombo == 1)
            {

                animator.SetBool("AttackCombo_1", true);
                isAttack = true;

            }
            else if (attackCombo >= 2 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("combo1"))
            {
                animator.SetBool("AttackCombo_1", false);
                animator.SetBool("AttackCombo_2", true);
                isAttack = true;

            }
            else if (attackCombo >= 3 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("combo2"))
            {
                animator.SetBool("AttackCombo_2", false);
                animator.SetBool("AttackCombo_3", true);
                isAttack = true;

            }
        }
    }
    public void SetAdditionalDamage(int additionalDamage = 0)
    {
        m_attackCollider.AddAdditionalDamage(additionalDamage);
    }

    public void SetDamage(int additionalDamage = 0)
    {
        m_attackCollider.SetPlayerDamage(m_statsData._StatsData.AttackDamage);
        m_attackCollider.SetCritical(m_statsData._StatsData.Critical);
        m_attackCollider.SetWeaponDamage(0);
    }
    public void SetWeaponDamage(float damage = 0)
    {
        m_attackCollider.SetWeaponDamage(damage);
    }
    public void GetDamaged(float damage)
    {
        if(m_bCantDamaged)
        {
            return;
        }
        float damageOffset = 0;
        if(m_statsData._StatsData.Defence > damage)
        {
            damageOffset = 0.5f;
        }
        else
        {
            damageOffset = 1.0f;
        }

        m_iCurPlayerHealth -= damage * damageOffset;
        if(m_iCurPlayerHealth<=0)
        {
            Die();
        }
        Mathf.Clamp(m_iCurPlayerHealth, 0, m_statsData._StatsData.HP);
        UIManager.Instance._PlayerDataUI.SetPlayerHPUI(m_iCurPlayerHealth / m_statsData._StatsData.HP);
    }
    public void Attack()
    {

        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && animator.GetBool("AttackCombo_1"))
        {
            animator.SetBool("AttackCombo_1", false); 
            attackCombo = 0;
            isAttack = false;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && animator.GetBool("AttackCombo_2"))
        {
            animator.SetBool("AttackCombo_2", false); 
            attackCombo = 0;
            isAttack = false;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetBool("AttackCombo_3"))
        {
            animator.SetBool("AttackCombo_3", false);
            attackCombo = 0;
            isAttack = false;
        }


    }

    void FixedUpdate()
    {
        Attack();
    }
    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGround = true;
        }
        if (other.CompareTag("InteractionObject"))
        {

            other.GetComponent<InteractionObject>().ShowOrHideInteractionObject(true);
        }

    }
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("InteractionObject") && Input.GetKeyUp(KeyCode.Space))
        { 
            other.GetComponent<InteractionObject>().PressInteractionKey();
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGround = false;
        }
        if (other.CompareTag("InteractionObject"))
        {
            other.GetComponent<InteractionObject>().ShowOrHideInteractionObject(false);
        }
    }
   public void SetCanAct(bool canActing)
    {
            canAct = canActing;
    }
    
    public void AddExp(float exp)
    {
        m_PlayerData.AddExp(exp);

        UIManager.Instance._PlayerDataUI.SetPlayerEXPUI(m_PlayerData._curEXP/m_PlayerData._EXP);
    }
    public void UsePortionItem(PortionType type,int value)
    {
        switch (type)
        {
            case PortionType.HP:
                m_iCurPlayerHealth += value;
                m_iCurPlayerHealth = Mathf.Clamp(m_iCurPlayerHealth, 0, m_statsData._StatsData.HP);
                break;
            case PortionType.MP:
                m_iCurPlayerMP += value;
                m_iCurPlayerHealth = Mathf.Clamp(m_iCurPlayerMP, 0, m_statsData._StatsData.MP);
                break;
        }
        m_ParticleSystem.ParticlePlayDuringTime(8, 2);
        AudioManager.Instance.PlaySFX(1);
        UIManager.Instance._PlayerDataUI.UpdateUI(m_iCurPlayerHealth, m_iCurPlayerHealth, m_PlayerData._curEXP / m_PlayerData._EXP, m_PlayerData._sUserName);
    }
    public void AddMP( float mp)
    {
        
        m_iCurPlayerMP += mp;
        UIManager.Instance._PlayerDataUI.SetPlayeMPUI(m_iCurPlayerMP / m_statsData._StatsData.MP);


    }
    public void SetCantDamaged(bool b)
    {
        m_bCantDamaged = b;
        if (m_bCantDamaged)
        {
            m_ParticleSystem.ParticlePlay(10);
        }

    }
    
    public void LVUP()
    {
        m_statsData.StatsPointUp();
        m_iCurPlayerHealth = m_statsData._StatsData.HP;
        m_iCurPlayerMP = m_statsData._StatsData.MP;
        UIManager.Instance._PlayerDataUI.UpdateUI(m_iCurPlayerHealth / m_statsData._StatsData.HP, m_iCurPlayerMP / m_statsData._StatsData.MP, m_PlayerData._curEXP / m_PlayerData._EXP, m_PlayerData._sUserName);
        UIManager.Instance._PlayerDataUI.SetPlayeLVUI(m_PlayerData._LV);
        m_ParticleSystem.ParticlePlayDuringTime(3, 3);
        m_PlayerData.LVUP();
        AudioManager.Instance.PlaySFX(2);
    }

    public void SetPlayerDataByCustomizing(string playerName)
    {
        m_PlayerData.SetPlayerData(playerName, "무직", "초보자", 1, 0);
        m_statsData.UpdatePlayerData(m_PlayerData);
        UIManager.Instance._PlayerDataUI.UpdateUI(m_iCurPlayerHealth/m_statsData._StatsData.HP,m_iCurPlayerMP/ m_statsData._StatsData.MP, m_PlayerData._curEXP / m_PlayerData._EXP,m_PlayerData._sUserName);
    }
    public void Die()
    {
        canAct = false;
        
        animator.SetTrigger("IsDie");

        m_inventory.DieAndLostGold();
        m_PlayerData.LoseExp();
        UIManager.Instance._gameOverText.gameObject.SetActive(true);

        
    }
}
