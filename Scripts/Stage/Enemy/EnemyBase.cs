using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Suv
{
	public partial class EnemyBase : MonoBehaviour
	{
		[SerializeField] private Image _image = null;
		private float _hp;
		private float _attackPow;
		private float _moveSpped;
		private float _moveWaitTime;
		private float _receiveDamageCoolTime;

		private static readonly StateMoving _stateMoving = new StateMoving();
		private static readonly StateReceivingDamage _stateReceivingDamage = new StateReceivingDamage();
        private EnemyStateBase _currentState = _stateMoving;

        private Rigidbody _rigidbody;

        public void Initialize(EnemyStatusData statusData)
        {
            _rigidbody = GetComponent<Rigidbody>();

			// パラメータ設定
			if (_image) _image.sprite = statusData.Sprite;
			_hp = statusData.Hp;
			_attackPow = statusData.AttackPow;
			_moveSpped = statusData.MoveSpeed;
			_moveWaitTime = statusData.MoveWaitTime;
			_receiveDamageCoolTime = statusData.ReceiveDamageCoolTime;

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

			if (_hp < 0)
            {

            }
			else
            {
				_stateMoving.MoveCancel();
				ChangeState(_stateReceivingDamage);
			}
		}

        private void OnTriggerStay(Collider other)
        {
			if (!other.gameObject.CompareTag(ConstStringManager.TAG_PLAYER)) return;

			// プレイヤー側のダメージ処理を呼ぶ
			StageManager.I.PlayerCharacter.OnCollisionEnemy(_attackPow, transform.position);

        }

        private void OnDestroy()
        {
			_stateMoving.OnDestroy();
			_stateReceivingDamage.OnDestroy();
        }
    }
}
