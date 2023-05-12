using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;

namespace Suv
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        private CancellationTokenSource _ctsDestroy = null;
        private float _destroyWaitTime = 0.7f;

        public void StartPop(string damage)
        {
            _text.text = damage;
            _ctsDestroy = new CancellationTokenSource();
            OnDestroyAsync(_ctsDestroy.Token).Forget();
        }

        private async UniTask OnDestroyAsync(CancellationToken token)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_destroyWaitTime), cancellationToken: token);

            if (token.IsCancellationRequested) return;

            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            _ctsDestroy?.Cancel();
            _ctsDestroy?.Dispose();
            _ctsDestroy = null;
        }
    }
}
