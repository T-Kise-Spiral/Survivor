using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;

namespace Suv
{
    public partial class PlayerCharacter
    {
        public class StateMoving : PlayerStateBase
        {
            private const float ChangeStandingStateAwaitTime = 1.0f;

            private bool _isChanging = false;
            private Vector3 _lastInputVec = Vector3.zero;
            private CancellationTokenSource _ctsChangeIdling = null;

            public Vector3 LastInputVec => _lastInputVec;

            public override void OnUpdate(PlayerCharacter owner)
            {
                if (owner.IsDead) return;
                Move(owner);
            }

            private void Move(PlayerCharacter owner)
            {
                Vector2 inputVec = owner._playerInput.actions["Move"].ReadValue<Vector2>();

                // 未入力中は一定時間後にIdlingへ遷移するようにする
                if (inputVec == Vector2.zero && !_isChanging)
                {
                    _isChanging = true;
                    _ctsChangeIdling?.Cancel();
                    _ctsChangeIdling = new CancellationTokenSource();
                    OnStandStateChanging(owner, _ctsChangeIdling.Token).Forget();
                }
                // 入力しているので移動処理
                else if (inputVec != Vector2.zero)
                {
                    _isChanging = false;
                    _ctsChangeIdling?.Cancel();

                    Vector3 moveVec = new Vector3(inputVec.x, 0, inputVec.y);
                    moveVec *= 0.5f;
                    owner._rigidbody.AddForce(moveVec, ForceMode.Impulse);
                    _lastInputVec = moveVec;
                }
            }

            // 立ちへの遷移
            private async UniTask OnStandStateChanging(PlayerCharacter owner, CancellationToken token)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(ChangeStandingStateAwaitTime), cancellationToken: token);

                // 遷移までにキャンセルが発生(再移動)が行われていないなら遷移確定
                if (!token.IsCancellationRequested)
                {
                    owner.ChangeState(_stateIdling);
                    _isChanging = false;
                }
            }

            public void OnDestroy()
            {
                _ctsChangeIdling?.Cancel();
                _ctsChangeIdling?.Dispose();
                _ctsChangeIdling = null;
            }
        }
    }
}