using UnityEngine;

/// <summary>
/// 플레이어 캐릭터를 전역적으로 관리하는 싱글톤 매니저 클래스
/// 게임의 어느 곳에서든 플레이어 정보에 접근할 수 있도록 중앙 집중식 관리를 제공
/// 씬 전환 시에도 유지되는 DontDestroyOnLoad 오브젝트
/// </summary>
public class CharacterManager : MonoBehaviour
{
    // 싱글톤 패턴을 위한 정적 인스턴스 변수
    private static CharacterManager _instance;

    /// <summary>
    /// 싱글톤 인스턴스에 접근하기 위한 공용 프로퍼티
    /// 인스턴스가 없으면 자동으로 생성하여 반환 (Lazy Initialization)
    /// </summary>
    public static CharacterManager Instance
    {
        get
        {
            // 인스턴스가 존재하지 않으면 새로 생성
            if (_instance == null)
            {
                // "CharacterManager"라는 이름의 GameObject 생성 후 이 컴포넌트 추가
                _instance = new GameObject("CharacterManager").AddComponent<CharacterManager>();
            }
            return _instance;
        }
    }

    // 플레이어 인스턴스를 저장하는 private 필드
    public Player _player;

    /// <summary>
    /// 플레이어 인스턴스에 접근하기 위한 공용 프로퍼티
    /// 다른 스크립트에서 플레이어 정보를 쉽게 가져오고 설정할 수 있도록 함
    /// </summary>
    public Player Player
    {
        get { return _player; }         // 플레이어 인스턴스 반환
        set { _player = value; }        // 플레이어 인스턴스 설정
    }

    /// <summary>
    /// 오브젝트 생성 시 호출되는 초기화 함수
    /// 싱글톤 패턴 보장 및 씬 전환 시 파괴 방지 설정
    /// </summary>
    private void Awake()
    {
        // 이미 인스턴스가 존재하지 않는 경우에만 이 오브젝트를 인스턴스로 설정
        if (_instance == null)
        {
            _instance = this;                           // 현재 오브젝트를 싱글톤 인스턴스로 설정
            DontDestroyOnLoad(gameObject);              // 씬 전환 시에도 이 오브젝트가 파괴되지 않도록 설정
        }
        else
        {
            // 이미 인스턴스가 존재하면 중복 생성된 오브젝트를 파괴
            // 싱글톤 패턴을 유지하기 위해 하나의 인스턴스만 존재하도록 보장
            Destroy(gameObject);
        }
    }
}
