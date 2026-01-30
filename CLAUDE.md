# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Unity DOTS (Data-Oriented Technology Stack) RTS game project using Entity Component System architecture.

- **Unity Version**: 6000.3.6f1
- **Render Pipeline**: Universal Render Pipeline (URP)
- **Core Packages**: Unity Entities 1.4.4, Unity Physics 1.4.4, Entities Graphics 1.4.17

## Architecture

### ECS Pattern

This project follows Unity DOTS architecture with clear separation:

```
Assets/Scripts/
├── Authoring/     # MonoBehaviour bakers that convert GameObjects to Entities
├── Systems/       # ISystem structs with game logic (Burst-compiled)
├── MonoBehaviours/ # Traditional Unity scripts for input/UI bridge
```

**Data Flow**: Authoring (Baker) → Entity + Components → Systems process components

### Key Patterns

- **Authoring + Baker**: `UnitMoverAuthoring` defines inspector fields, nested `Baker<T>` converts to ECS components
- **Component**: `IComponentData` structs (e.g., `UnitMover`) hold pure data
- **System**: `ISystem` with `[BurstCompile]` processes entities via `SystemAPI.Query<>`
- **MonoBehaviour Bridge**: Traditional scripts handle Unity Input → ECS World communication

### Scene Structure

- `GameScene.unity` - Main scene with MonoBehaviour managers
- `GameScene/EntitiesSubscene.unity` - ECS subscene for baked entities

## Namespaces

- `DotsRts` - Root namespace for components and authoring
- `DotsRts.Systems` - ECS systems
- `DotsRts.MonoBehaviours` - Traditional Unity scripts

## Git Commit Rules

- 커밋 메시지는 한글로 작성
- `Co-Authored-By` 라인 추가하지 않음
