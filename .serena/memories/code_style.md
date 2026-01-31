# Code Style & Conventions

## 네이밍 컨벤션
- **클래스/구조체**: PascalCase (예: `UnitMover`, `UnitMoverAuthoring`)
- **메서드**: PascalCase (예: `OnUpdate`, `Execute`)
- **필드 (public)**: PascalCase (예: `MoveSpeed`, `TargetPosition`)
- **지역 변수**: camelCase (예: `moveDirection`, `unitMoverJob`)
- **네임스페이스**: PascalCase (예: `DotsRts.Systems`)

## ECS 패턴
### Authoring + Baker
```csharp
public class XxxAuthoring : MonoBehaviour
{
    public float SomeValue;

    private class XxxBaker : Baker<XxxAuthoring>
    {
        public override void Bake(XxxAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new XxxComponent { ... });
        }
    }
}

public struct XxxComponent : IComponentData { ... }
```

### System
```csharp
namespace DotsRts.Systems
{
    public partial struct XxxSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state) { ... }
    }
}
```

### Job
```csharp
[BurstCompile]
public partial struct XxxJob : IJobEntity
{
    public void Execute(ref T component, in T2 readOnlyComponent) { ... }
}
```

## 코드 스타일
- Authoring과 Component를 같은 파일에 정의
- `[BurstCompile]` 어트리뷰트 필수 사용
- `var` 키워드 선호
- 중괄호는 새 줄에 배치 (Allman style)
