# Task Completion Checklist

## 코드 작성 완료 시
1. **네임스페이스 확인**: 올바른 네임스페이스 사용 (`DotsRts`, `DotsRts.Systems` 등)
2. **Burst 컴파일**: System과 Job에 `[BurstCompile]` 어트리뷰트 적용
3. **파일 위치**: 올바른 폴더에 파일 배치
   - Authoring → `Assets/Scripts/Authoring/`
   - System → `Assets/Scripts/Systems/`
   - MonoBehaviour → `Assets/Scripts/MonoBehaviours/`

## Git 커밋 시
- 커밋 메시지는 **한글**로 작성
- `Co-Authored-By` 라인 추가하지 않음
- 예: `git commit -m "유닛 이동 시스템 버그 수정"`

## ECS 코드 확인
- Authoring과 Component가 같은 파일에 있는지 확인
- Baker가 올바른 TransformUsageFlags 사용하는지 확인
- System이 `partial struct`로 선언되었는지 확인
