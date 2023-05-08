using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Suv
{
    public partial class Player
    {
        public class PlayerCharacterController : MonoBehaviour
        {
            private static readonly StateStanding stateStanding = new StateStanding();

            private PlayerStateBase _currentState = stateStanding;

			// Start()から呼ばれる
			private void OnStart()
			{
				_currentState.OnEnter(this, null);
			}

			// Update()から呼ばれる
			private void OnUpdate()
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
		}
    }
}