using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;

namespace Suv
{
    public enum WeaponType
    {
        Knife,
        Bomb,
    }

    public class WeaponBase : MonoBehaviour
    {
        protected virtual Vector3 StartPos => StageManager.I.PlayerCharacter.transform.position;

        [SerializeField] protected WeaponStatusData _weaponData;
        public float WeaponAttackPow => _weaponData.AttackPow;

        private CancellationTokenSource _ctsUseWeapon = null;
        private GameObject _weaponCanvasObject;
        private BoxCollider _boxCollider;

        private int _currentHitCount = 0;

        private void Start()
        {
            if (!_weaponCanvasObject)
                _weaponCanvasObject = transform.GetComponentInChildren<Image>().gameObject;

            if (!_boxCollider)
                _boxCollider = GetComponent<BoxCollider>();

            _ctsUseWeapon?.Cancel();
            _ctsUseWeapon = new CancellationTokenSource();
            UseWeaponAsync(_ctsUseWeapon.Token).Forget();

            UseEnd();
        }

        private async UniTask UseWeaponAsync(CancellationToken token)
        {
            while(true)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_weaponData.UseTimeSpan), cancellationToken: token);
                if (token.IsCancellationRequested) break;

                OnUseWeapon();
            }
        }

        protected virtual void OnUseWeapon()
        {
            // 初期設定
            transform.position = StartPos;
            _currentHitCount = _weaponData.MaxHitCount;

            // 表示
            _weaponCanvasObject.SetActive(true);
            _boxCollider.enabled = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag(ConstStringManager.TAG_ENEMY)) return;

            // 範囲攻撃の場合はヒット数の上限なし
            if (_weaponData.IsRangeAttack) return;

            // 上限回数ヒット
            _currentHitCount--;
            if (_currentHitCount <= 0)
                UseEnd();
        }

        protected virtual void UseEnd()
        {
            // 表示
            _weaponCanvasObject.SetActive(false);

            // コライダー有効化
            _boxCollider.enabled = false;
        }

        private void OnDestroy()
        {
            _ctsUseWeapon?.Cancel();
            _ctsUseWeapon?.Dispose();
            _ctsUseWeapon = null;
        }
    }
}