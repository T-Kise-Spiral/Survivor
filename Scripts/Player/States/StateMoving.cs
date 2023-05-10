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
            private const float MovePow = 100.0f;

            private bool isChanging = false;
            private bool isInputCoolTime = false;
            private CancellationTokenSource _ctsChangeIdling = null;
            private CancellationTokenSource _ctsMoveCoolTime = null;

            public override void OnUpdate(PlayerCharacter owner)
            {
                Move(owner);
            }

            private void Move(PlayerCharacter owner)
            {
                Vector2 moveVec = owner._playerInput.actions["Move"].ReadValue<Vector2>();

                // 未入力中は一定時間後にIdlingへ遷移するようにする
                if (moveVec == Vector2.zero && !isChanging)
                {
                    isChanging = true;
                    _ctsChangeIdling?.Cancel();
                    _ctsChangeIdling = new CancellationTokenSource();
                    OnStandStateChanging(owner, _ctsChangeIdling.Token).Forget();
                }
                // 入力しているので移動処理
                else if (moveVec != Vector2.zero && !isInputCoolTime)
                {
                    isChanging = false;
                    _ctsChangeIdling?.Cancel();

                    isInputCoolTime = true;
                    moveVec *= MovePow;
                    owner._rigidbody2D.AddForce(moveVec, ForceMode2D.Impulse);
                    _ctsMoveCoolTime?.Cancel();
                    _ctsMoveCoolTime = new CancellationTokenSource();
                    OnMoveInputCoolTime(_ctsMoveCoolTime.Token).Forget();

                    //moveVec *= Time.deltaTime * 10;
                    //Vector3 newPos = new Vector3(owner._rectTransform.position.x + moveVec.x, owner._rectTransform.position.y + moveVec.y, owner._rectTransform.position.z);
                    //owner._rectTransform.position = newPos;
                }
            }

            // 立ちへの遷移
            private async UniTask OnStandStateChanging(PlayerCharacter owner, CancellationToken token)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(ChangeStandingStateAwaitTime), cancellationToken: token);

                // 遷移までにキャンセルが発生(再移動)が行われていない
                if (!token.IsCancellationRequested)
                {
                    owner.ChangeState(_stateIdling);
                    isChanging = false;
                }
            }

            private async UniTask OnMoveInputCoolTime(CancellationToken token)
            {
                await UniTask.DelayFrame(30, cancellationToken: token);

                if (!token.IsCancellationRequested)
                {
                    isInputCoolTime = false;
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