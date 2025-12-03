using UnityEngine;
using System.Collections;

public class BossController : MonoBehaviour
{
    [Header("Configuración General")]
    public Transform player;
    public float rotationSpeed = 5f;
    public Transform centerPoint;

    [Header("Activación")]
    public float activationDistance = 15f;
    public float standUpDuration = 2.0f;

    [Header("TIEMPOS EXACTOS (TIMING)")]
    // DASH
    public float dashTelegraphTime = 2.0f;
    public float dashDistance = 25f;

    // JUMP (SALTO)
    public float jumpTelegraphTime = 4.0f;
    public float jumpTimeUntilImpact = 1.817f;

    // GROUND ZONES
    public float castAnimCastPart = 1.3f;
    public float groundZoneExplosionDelay = 1.0f;

    // SHOOT
    public float shootAnimDuration = 2.0f;

    [Header("CONFIGURACIÓN DE TAMAÑOS")]
    public float jumpAttackSize = 22f;
    public float groundZoneSize = 6f;

    [Header("AJUSTE DE HITBOX (NUEVO)")]
    [Range(0.5f, 1f)]
    public float hitBoxReduction = 0.85f; // El daño será el 85% del tamaño visual para ser justo

    [Header("Ataques & Prefabs")]
    public GameObject projectilePrefab;
    public GameObject aoeTelegraphPrefab;
    public GameObject dashTelegraphPrefab;
    public Transform firePoint;

    [Header("Fases (Lógica)")]
    public float phase2SpeedMultiplier = 1.5f;

    // Estados Públicos
    public bool IsPhase2 { get; private set; } = false;
    public bool IsDead { get; private set; } = false;

    private bool isBusy = false;
    private bool isSleeping = true;
    private float speedMult = 1f;

    // Corrección de Altura
    private float initialY;
    public float floorY = 0.15f;

    [Header("REFERENCIAS")]
    public Animator anim;

    private void Start()
    {
        initialY = transform.position.y;

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    private void Update()
    {
        if (IsDead) return;

        // 1. SINCRONIZAR VELOCIDAD DE ANIMACIÓN
        if (anim != null)
        {
            anim.speed = IsPhase2 ? phase2SpeedMultiplier : 1f;
        }

        // 2. CORRECCIÓN ALTURA
        if (Mathf.Abs(transform.position.y - initialY) > 0.1f)
        {
            transform.position = new Vector3(transform.position.x, initialY, transform.position.z);
        }

        // 3. DORMIDO
        if (isSleeping)
        {
            if (player != null)
            {
                float dist = Vector2.Distance(new Vector2(transform.position.x, transform.position.z),
                                              new Vector2(player.position.x, player.position.z));
                if (dist < activationDistance) StartCoroutine(WakeUpSequence());
            }
            return;
        }

        // 4. ROTACIÓN
        if (!isBusy && player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0;
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed * speedMult);
            }
        }
    }

    private void LateUpdate()
    {
        if (anim != null)
        {
            Vector3 currentLocalPos = anim.transform.localPosition;
            anim.transform.localPosition = new Vector3(0, currentLocalPos.y, 0);
        }
    }

    // --- COMUNICACIÓN ---
    public void NotifyDamageReceived() { if (isSleeping) StartCoroutine(WakeUpSequence()); }
    public void StartPhase2() { StartCoroutine(EnterPhase2Routine()); }
    public void DieSequence()
    {
        IsDead = true;
        StopAllCoroutines();
        if (anim != null)
        {
            anim.speed = 1f;
            anim.SetTrigger("Die");
        }
    }

    IEnumerator EnterPhase2Routine()
    {
        IsPhase2 = true;
        isBusy = true;
        if (anim != null) anim.SetBool("IsMoving", false);
        if (anim != null) anim.SetTrigger("Roar");
        yield return new WaitForSeconds(2.5f);
        isBusy = false;
    }

    // --- LÓGICA COMBATE ---
    IEnumerator WakeUpSequence()
    {
        isSleeping = false;
        isBusy = true;
        if (anim != null) anim.SetTrigger("WakeUp");
        yield return new WaitForSeconds(standUpDuration);
        isBusy = false;
        StartCoroutine(BehaviorLoop());
    }

    IEnumerator BehaviorLoop()
    {
        yield return new WaitForSeconds(1f);

        while (!IsDead)
        {
            float distToCenter = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z),
                                                  new Vector3(centerPoint.position.x, 0, centerPoint.position.z));

            if (distToCenter > 2.0f)
            {
                yield return StartCoroutine(ReturnToCenterTeleport());
            }

            int attackIndex = Random.Range(0, 4);
            float cooldown = IsPhase2 ? 0.5f : 1.5f;
            speedMult = IsPhase2 ? phase2SpeedMultiplier : 1f;

            switch (attackIndex)
            {
                case 0: yield return StartCoroutine(Attack_FanProjectiles()); break;
                case 1: yield return StartCoroutine(Attack_JumpSmash()); break;
                case 2: yield return StartCoroutine(Attack_Dash()); break;
                case 3: yield return StartCoroutine(Attack_GroundZones()); break;
            }

            yield return new WaitForSeconds(cooldown);
        }
    }

    // --- 1. DASH CON DAÑO ---
    IEnumerator Attack_Dash()
    {
        isBusy = true;
        Vector3 flatPlayerPos = new Vector3(player.position.x, transform.position.y, player.position.z);
        Vector3 dashDir = (flatPlayerPos - transform.position).normalized;
        Vector3 targetPos = transform.position + (dashDir * dashDistance);

        Vector3 telegraphPos = transform.position + (dashDir * (dashDistance / 2f));
        telegraphPos.y = floorY;

        GameObject tele = Instantiate(dashTelegraphPrefab, telegraphPos, Quaternion.LookRotation(dashDir));
        tele.transform.Rotate(90, 0, 0);
        TelegraphVisual tv = tele.GetComponent<TelegraphVisual>();
        tv.ActivateTelegraph(dashTelegraphTime / speedMult, new Vector3(2, dashDistance, 1));

        yield return new WaitForSeconds(dashTelegraphTime / speedMult);

        float moveTime = 0.4f;
        float timer = 0f;
        Vector3 startPos = transform.position;
        bool hitPlayer = false;

        transform.rotation = Quaternion.LookRotation(dashDir);

        while (timer < moveTime)
        {
            timer += Time.deltaTime;
            float t = timer / moveTime;
            t = t * t * (3f - 2f * t);
            transform.position = Vector3.Lerp(startPos, targetPos, t);

            // LOGICA DAÑO DASH
            if (!hitPlayer)
            {
                float dist = Vector2.Distance(new Vector2(transform.position.x, transform.position.z),
                                              new Vector2(player.position.x, player.position.z));
                // Radio aumentado a 3 para asegurar golpe
                if (dist < 3.0f)
                {
                    DamagePlayer(20);
                    hitPlayer = true;
                }
            }
            yield return null;
        }

        yield return new WaitForSeconds(1.0f);
        isBusy = false;
    }

    IEnumerator ReturnToCenterTeleport()
    {
        isBusy = true;
        yield return new WaitForSeconds(0.5f);
        transform.position = new Vector3(centerPoint.position.x, initialY, centerPoint.position.z);
        if (player != null)
        {
            Vector3 lookDir = (player.position - transform.position).normalized;
            lookDir.y = 0;
            if (lookDir != Vector3.zero) transform.rotation = Quaternion.LookRotation(lookDir);
        }
        yield return new WaitForSeconds(0.5f);
        isBusy = false;
    }

    // --- 2. JUMP SMASH (ARREGLADO EL TAMAÑO DE HITBOX) ---
    IEnumerator Attack_JumpSmash()
    {
        isBusy = true;
        Vector3 groundPos = new Vector3(transform.position.x, floorY, transform.position.z);
        GameObject tele = Instantiate(aoeTelegraphPrefab, groundPos, Quaternion.Euler(90, 0, 0));
        TelegraphVisual tv = tele.GetComponent<TelegraphVisual>();

        // 1. TELEGRAPH VISUAL (Tamaño Completo)
        tv.ActivateTelegraph(jumpTelegraphTime / speedMult, new Vector3(jumpAttackSize, jumpAttackSize, 1));

        yield return new WaitForSeconds(jumpTelegraphTime / speedMult);

        if (anim != null) anim.SetTrigger("JumpAttack");

        // Esperamos, ajustando por la velocidad actual de la animación
        yield return new WaitForSeconds(jumpTimeUntilImpact / speedMult);

        // 2. DAÑO FÍSICO (REDUCIDO para ser justo)
        // Usamos (Tamaño / 2) * factor de reducción (0.85)
        float damageRadius = (jumpAttackSize / 2f) * hitBoxReduction;

        Collider[] hits = Physics.OverlapSphere(groundPos, damageRadius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                PlayerHealth hp = hit.GetComponent<PlayerHealth>();
                if (hp != null) hp.DealDamage(30);
            }
        }
        yield return new WaitForSeconds(0.5f);
        isBusy = false;
    }

    // --- 3. GROUND ZONES ---
    IEnumerator Attack_GroundZones()
    {
        isBusy = true;
        int count = IsPhase2 ? 5 : 3;

        for (int i = 0; i < count; i++)
        {
            if (anim != null) anim.SetTrigger("CastSpell");
            Vector3 spawnPos = new Vector3(player.position.x, floorY, player.position.z);
            GameObject tele = Instantiate(aoeTelegraphPrefab, spawnPos, Quaternion.Euler(90, 0, 0));
            TelegraphVisual tv = tele.GetComponent<TelegraphVisual>();
            tv.ActivateTelegraph(groundZoneExplosionDelay, new Vector3(groundZoneSize, groundZoneSize, 1));
            StartCoroutine(ResolveZoneDamage(spawnPos, groundZoneExplosionDelay));

            // Espera ajustada por velocidad
            yield return new WaitForSeconds(castAnimCastPart / speedMult);
        }
        isBusy = false;
    }

    IEnumerator ResolveZoneDamage(Vector3 pos, float delay)
    {
        yield return new WaitForSeconds(delay);
        float dist = Vector2.Distance(new Vector2(player.position.x, player.position.z),
                                      new Vector2(pos.x, pos.z));

        // Aquí también aplicamos una pequeña reducción para ser justos (0.9)
        if (dist < (groundZoneSize / 2f) * 0.9f)
        {
            DamagePlayer(15);
        }
    }

    // --- 4. PROJECTILES ---
    IEnumerator Attack_FanProjectiles()
    {
        isBusy = true;
        if (anim != null) anim.SetTrigger("Shoot");

        // Espera ajustada por velocidad
        yield return new WaitForSeconds(shootAnimDuration / speedMult);

        if (IsPhase2)
        {
            int projectiles = 24;
            float angleStep = 360f / projectiles;
            for (int i = 0; i < projectiles; i++)
            {
                float currentAngle = i * angleStep;
                Quaternion rot = transform.rotation * Quaternion.Euler(0, currentAngle, 0);
                Instantiate(projectilePrefab, firePoint.position, rot);
                yield return new WaitForSeconds(0.04f);
            }
        }
        else
        {
            int projectiles = 5;
            float angleStep = 15f;
            float startAngle = -((projectiles - 1) * angleStep) / 2;
            for (int i = 0; i < projectiles; i++)
            {
                Quaternion rot = transform.rotation * Quaternion.Euler(0, startAngle + (angleStep * i), 0);
                Instantiate(projectilePrefab, firePoint.position, rot);
            }
        }
        yield return new WaitForSeconds(0.2f);
        isBusy = false;
    }

    void DamagePlayer(int amount)
    {
        if (player != null)
        {
            PlayerHealth hp = player.GetComponent<PlayerHealth>();
            if (hp != null) hp.DealDamage(amount);
        }
    }
  
   
}