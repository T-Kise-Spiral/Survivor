using UnityEngine;
using Suv;

[CreateAssetMenu(menuName = "Create WeaponStatus")]
public class WeaponStatusData : ScriptableObject
{
	// ----------------------------------------------
	// 設定項目
	[SerializeField]
	private Sprite _sprite = null;

	[SerializeField]
	private WeaponType _type = WeaponType.Knife;

	[SerializeField]
	private float _attackPow = 2.0f;

	[SerializeField]
	private float _moveSpped = 5.0f;

	[SerializeField]
	public float _useTimeSpan = 1.0f;

	[SerializeField]
	public int _maxHitCount = 1;

	[SerializeField]
	public bool _isRangeAttack = false;

	// ----------------------------------------------
	// アクセサ
	public Sprite Sprite => _sprite;

	public WeaponType Type => _type;

	public float AttackPow => _attackPow;

	public float MoveSpeed => _moveSpped;

	public float UseTimeSpan => _useTimeSpan;

	public int MaxHitCount => _maxHitCount;

	public bool IsRangeAttack => _isRangeAttack;
}
