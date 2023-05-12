using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Suv
{
    public partial class PlayerCharacter
    {
        public class StateReceivingDamage : PlayerStateBase
        {
            private bool _isDamageCoolTime = false;
            private float _movableCoolTime = 0.3f;
            private float _recieveDamageCoolTime = 1.0f;
            private CancellationTokenSource _ctsReceiveDamage = null;

            public Vector3 _enemyPos = Vector3.zero;

            // このステートに来る前に確認する用
            public bool IsReceiveDamageCoolTime => _isDamageCoolTime;

            public override void OnEnter(PlayerCharacter owner, PlayerStateBase prevState)
            {
                Vector3 moveVec = owner.transform.position - _enemyPos;
                moveVec.Normalize();
                moveVec *= 10.0f;
                owner._rigidbody.AddForce(moveVec, ForceMode.Impulse);

                _ctsReceiveDamage?.Cancel();
                _ctsReceiveDamage = new CancellationTokenSource();
                OnReceiveDamageCoolTimeCount(owner, _ctsReceiveDamage.Token).Forget();
            }

            private async UniTask OnReceiveDamageCoolTimeCount(PlayerCharacter owner, CancellationToken token)
            {
                // 一定時間後に移動可能
                await UniTask.Delay(TimeSpan.FromSeconds(_movableCoolTime), cancellationToken: token);
                owner.ChangeState(_stateIdling);

                // 一定時間だけ無敵状態にしておく
                await UniTask.Delay(TimeSpan.FromSeconds(_recieveDamageCoolTime - _movableCoolTime), cancellationToken: token);
                if (!token.IsCancellationRequested)
                {
                    _isDamageCoolTime = false;
                }
            }

            public void OnDestroy()
            {
                _ctsReceiveDamage?.Cancel();
                _ctsReceiveDamage?.Dispose();
                _ctsReceiveDamage = null;
            }
        }
    }
}
