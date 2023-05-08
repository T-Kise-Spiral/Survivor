using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Suv
{
    public partial class PlayerCharacter : MonoBehaviour
    {
		private static readonly StateStanding _stateStanding = new StateStanding();
		private static readonly StateMoving _stateMoving = new StateMoving();
 		private PlayerStateBase _currentState = _stateStanding;

		private PlayerInput playerInput;
		private RectTransform rectTransform;

        private void Awake()
        {
			playerInput = GetComponent<PlayerInput>();
			rectTransform = GetComponent<RectTransform>();
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