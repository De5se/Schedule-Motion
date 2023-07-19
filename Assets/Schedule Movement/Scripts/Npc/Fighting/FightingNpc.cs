using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Schedule_Movement.Scripts.Npc.Fighting
{
    public class FightingNpc : MonoBehaviour
    {
        #region Variables
        [SerializeField] private float maxHealth;
        [SerializeField] private float attackStrength;
        [SerializeField] private float attackDistance;
        [Space(10)]
        [SerializeField] private AnimatorOverrideController fightingAnimator;

        [ShowInInspector, ReadOnly]
        private float _currentHealth;
        [ShowInInspector, ReadOnly]
        private FightingNpc _nearestEnemy;
        
        private float _stoppingDistance;
        private const float UpdateSpeed = 0.2f;
        private NPC _npc;

        [Inject] private FightManager _fightManager;

        public float MaxHealth => maxHealth;
        public float AttackStrength => attackStrength;

        public bool IsFighting { get; private set;}
        
        [Inject] private NpcAnimator _npcAnimator;

        private Coroutine _chaseCoroutine;
        private Action _deathAction;
        #endregion

        #region Properties
        public bool IsAlive => _currentHealth > 0;

        private bool IsAttackEnabled => _nearestEnemy &&
                                        Vector3.Distance(transform.position, _nearestEnemy.transform.position) <=
                                        attackDistance;
        #endregion

        #region Monobehaviour Callbacks
        private void Start()
        {
            _npc = GetComponent<NPC>();
            _stoppingDistance = _npc.NavMeshAgent.stoppingDistance;
            ResetHealth();
        }
        #endregion
        
        public void SetCharacteristics(float maxHealth, float speed, float strength)
        {
            this.maxHealth = maxHealth;
            attackStrength = strength;
            ResetHealth();
        }

        public void StartFighting()
        {
            if (IsFighting)
                return;
            
            IsFighting = true;
            UpdateNearestEnemy();
            _npc.SetStoppingDistance(attackDistance - 0.01f);
            
            _chaseCoroutine = StartCoroutine(ChaseEnemy());
            // 0.01f - accuracy
        }

        public void FinishFighting()
        {
            IsFighting = false;
            StopCoroutine(_chaseCoroutine);
            _npc.SetStoppingDistance(_stoppingDistance);
            ResetHealth();
            throw new NotImplementedException();
        }

        private void SetFightingAnimator() => _npc.UpdateAnimatorController(fightingAnimator);
        private void ResetHealth() => _currentHealth = maxHealth;

        private void UpdateNearestEnemy()
        {
            var targetNearestEnemy = _fightManager.GetNearestEnemy(this);
            if (_nearestEnemy != null 
                &&  _nearestEnemy != targetNearestEnemy)
            {
                _nearestEnemy._deathAction -= UpdateNearestEnemy;
            }

            _nearestEnemy = targetNearestEnemy;
            if (targetNearestEnemy != null)
            {
                _nearestEnemy._deathAction += UpdateNearestEnemy;
            }
        }
        
        private IEnumerator ChaseEnemy()
        {
            var wait = new WaitForSeconds(UpdateSpeed);
            
            while (true)
            {
                if (IsAttackEnabled)
                {
                    SetFightingAnimator();
                }
                else if (_nearestEnemy)
                {
                    _npc.SetDestination(_nearestEnemy.transform.position);
                }
                yield return wait;
            }
            // ReSharper disable once IteratorNeverReturns
        }

        #region Actions
        /// <summary>
        /// Should be called in animation in punch moment
        /// </summary>
        private void Attack()
        {
            if (IsAttackEnabled)
            {
                //Debug.Log("Successful Attack on " + nearestEnemy.name);
                _nearestEnemy.GetDamage(attackStrength);
            }
            else
            {
                //Debug.Log("Not successful Attack");
            }
        }
        
        private void GetDamage(float damage)
        {
            if (!IsAlive) return;
            
            _currentHealth = Mathf.Max(0, _currentHealth - damage);
            if (!IsAlive)
                Die();
        }
        
        private void Die()
        {
            _deathAction?.Invoke();
            gameObject.SetActive(false);
            StopCoroutine(_chaseCoroutine);
            //todo
            _fightManager.TryFinishFight();
        }
        #endregion
    }
}