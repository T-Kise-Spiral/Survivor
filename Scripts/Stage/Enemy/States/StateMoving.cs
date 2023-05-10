using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Suv
{
    public partial class EnemyBase
    {
        public class StateMoving : EnemyStateBase
        {
            private CancellationTokenSource _ctsMove = null;

            public override void OnEnter(EnemyBase owner, EnemyStateBase prevState)
            {
                _ctsMove?.Cancel();
                _ctsMove = new CancellationTokenSource();
                OnMove(_ctsMove.Token).Forget();
            }

            private async UniTask OnMove(CancellationToken token)
            {
                while(true)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: token);

                    if (token.IsCancellationRequested) break;


                }
            }

            public void MoveCancel()
            {
                _ctsMove?.Cancel();
            }

            public void OnDestroy()
            {
                _ctsMove?.Cancel();
                _ctsMove?.Dispose();
                _ctsMove = null;
            }
        }
    }
}
