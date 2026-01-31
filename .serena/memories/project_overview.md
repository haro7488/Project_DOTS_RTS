# Project Overview

## 프로젝트 목적
Unity DOTS (Data-Oriented Technology Stack) 기반 RTS 게임 프로젝트

## 기술 스택
- **Unity Version**: 6000.3.6f1
- **Render Pipeline**: Universal Render Pipeline (URP)
- **Core Packages**: 
  - Unity Entities 1.4.4
  - Unity Physics 1.4.4
  - Entities Graphics 1.4.17
- **Language**: C# 9.0+

## 아키텍처
ECS (Entity Component System) 패턴 사용:
- **Authoring (Baker)**: MonoBehaviour로 Inspector 필드 정의 → Entity로 변환
- **Component**: `IComponentData` 순수 데이터 구조체
- **System**: `ISystem` + `[BurstCompile]`로 고성능 처리

## 코드베이스 구조
```
Assets/Scripts/
├── Authoring/       # Baker가 포함된 Authoring 클래스
├── Systems/         # ECS 시스템 (Burst 컴파일)
├── MonoBehaviours/  # 전통적 Unity 스크립트 (Input/UI 브릿지)
├── UI/              # UI 관련 스크립트
└── Faction.cs       # 공통 enum/타입
```

## 씬 구조
- `GameScene.unity` - 메인 씬 (MonoBehaviour 매니저)
- `GameScene/EntitiesSubscene.unity` - ECS 서브씬 (베이크된 엔티티)

## 네임스페이스
- `DotsRts` - 컴포넌트와 Authoring
- `DotsRts.Systems` - ECS 시스템
- `DotsRts.MonoBehaviours` - 전통적 Unity 스크립트
