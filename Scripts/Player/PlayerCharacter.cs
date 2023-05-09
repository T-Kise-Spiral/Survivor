using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Suv
{
    public partial class PlayerCharacter : MonoBehaviour
    {
		[SerializeField] SpinningTop _spinningTop;

		[SerializeField] float baseCurvePow = 1.0f;
		[SerializeField] float baseAddforcePow = 10.0f;

		private static readonly StateIdling _stateIdling = new StateIdling();
		private static readonly StateMoving _stateMoving = new StateMoving();
 		private PlayerStateBase _currentState = _stateIdling;

		private PlayerInput _playerInput;

		public float CurvePow => _stateMoving.CurvePow;
		public float AddforcePow => _stateMoving.AddForcePow;

		private void Awake()
        {
			_playerInput = GetComponent<PlayerInput>();
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

        private void OnDestroy()
        {
			_stateMoving.OnDestroy();
        }
    }
}