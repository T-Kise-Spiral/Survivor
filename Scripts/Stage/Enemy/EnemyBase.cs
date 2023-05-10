using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Suv
{
	public partial class EnemyBase : MonoBehaviour
	{
		[SerializeField] float _baseHp = 100.0f;
		[SerializeField] float _baseAttackPow = 2.0f;
		[SerializeField] float _baseMoveSpeed = 5.0f;
		[SerializeField] float _baseMoveWaitTime = 0.5f;
		[SerializeField] float _baseReceiveDamageCoolTime = 0.5f;

		public class EnemyStatus
		{
			public float _hp;
			public float _attackPow;
			public float _moveSpped;
			public float _moveWaitTime;
			public float _receiveDamageCoolTime;

			public EnemyStatus(float hp, float attackPow, float moveSpeed, float moveWaitTime, float receiveDamageCoolTime)
			{
				_hp = hp;
				_attackPow = attackPow;
				_moveSpped = moveSpeed;
				_moveWaitTime = moveWaitTime;
				_receiveDamageCoolTime = receiveDamageCoolTime;
			}
		}
		protected EnemyStatus _enemyStatus;

		private static readonly StateMoving _stateMoving;
		private static readonly StateReceivingDamage _stateReceivingDamage;
        private EnemyStateBase _currentState = _stateMoving;

        private Rigidbody _rigidbody;		

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
			_enemyStatus = new EnemyStatus(_baseHp, _baseAttackPow, _baseMoveSpeed, _baseMoveWaitTime, _baseReceiveDamageCoolTime);
        }

		private void Start()
		{
			_currentState.OnEnter(this, null);
		}

		private void Update()
		{
			_currentState.OnUpdate(this);
		}

		// ステート変更
		private void ChangeState(EnemyStateBase nextState)
		{
			_currentState.OnExit(this, nextState);
			nextState.OnEnter(this, _currentState);
			_currentState = nextState;
		}

        private void OnTriggerEnter(Collider other)
        {
			if (!other.gameObject.CompareTag(ConstStringManager.TAG_WEAPON)) return;

			if (_enemyStatus._hp < 0)
            {

            }
			else
            {
				ChangeState(_stateReceivingDamage);
			}
		}

        private void OnTriggerStay(Collider other)
        {
			if (!other.gameObject.CompareTag(ConstStringManager.TAG_PLAYER)) return;

			// TODO: プレイヤー側のダメージ処理を呼ぶ

        }

        private void OnDestroy()
        {
			_stateMoving.OnDestroy();
			_stateReceivingDamage.OnDestroy();
        }
    }
}
