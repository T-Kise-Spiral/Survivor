using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Suv
{
    public partial class PlayerCharacter : MonoBehaviour
    {
		[SerializeField] Slider _slider;
			 
		private static readonly StateIdling _stateIdling = new StateIdling();
		private static readonly StateMoving _stateMoving = new StateMoving();
		private static readonly StateReceivingDamage _stateReceivingDamage = new StateReceivingDamage();
 		private PlayerStateBase _currentState = _stateIdling;

		private PlayerInput _playerInput;
		private Rigidbody _rigidbody;

		private float _hp = 100;

		public bool IsDead => _hp <= 0;
		public Vector3 LastInputVec => _stateMoving.LastInputVec;

		private void Awake()
        {
			_playerInput = GetComponent<PlayerInput>();
			_rigidbody = GetComponent<Rigidbody>();
			UpdateHpBar();
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
		private void ChangeState(PlayerStateBase nextState)
		{
			_currentState.OnExit(this, nextState);
			nextState.OnEnter(this, _currentState);
			_currentState = nextState;
		}

        // 敵と接触したときに敵側から呼ばれる
		public void OnCollisionEnemy(float damage, Vector3 enemyPos)
        {
			// 無敵時間ならダメージを受け付けない
			if (_stateReceivingDamage.IsReceiveDamageCoolTime) return;

			_hp = Mathf.Clamp(_hp - damage, 0, _hp);
			UpdateHpBar();
			if (IsDead)
			{

			}
			else
			{
				_stateReceivingDamage._enemyPos = enemyPos;
				ChangeState(_stateReceivingDamage);
			}
		}

		private void UpdateHpBar()
        {
			if (_slider)
				_slider.value = _hp;
        }

        private void OnDestroy()
        {
			_stateMoving.OnDestroy();
			_stateReceivingDamage.OnDestroy();
        }
    }
}