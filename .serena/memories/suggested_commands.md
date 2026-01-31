# Suggested Commands

## Windows 시스템 유틸리티
```powershell
# 파일 목록
dir
Get-ChildItem

# 파일 내용 보기
type filename
Get-Content filename

# 파일 검색
Get-ChildItem -Recurse -Filter "*.cs"

# 텍스트 검색
Select-String -Path "*.cs" -Pattern "검색어"
findstr /s /i "검색어" *.cs
```

## Git 명령어
```bash
git status
git add .
git commit -m "메시지"    # 커밋 메시지는 한글로
git push
git pull
git log --oneline -10
```

## Unity 관련
- Unity Editor에서 직접 빌드/실행
- 솔루션 파일: `Project_DOTS_RTS.sln`
- IDE: Visual Studio 또는 Rider 권장

## 프로젝트 특이사항
- ECS 코드는 Unity Editor 내에서 테스트
- Burst 컴파일 확인은 Burst Inspector 사용
- 엔티티 디버깅은 Entity Debugger 창 사용
