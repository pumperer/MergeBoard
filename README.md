# Merge Board
- 간단한 룰만 가지고 있는 2-merge board 게임 샘플입니다.
- 두 개의 보드가 별도로 존재하고, 각자 독립적으로 진행됩니다.
- 각 보드에서 우하단의 퀘스트 폴더에서 퀘스트를 받을 수 있습니다.
- 퀘스트는 보드별로 구분되어 동작합니다.
- 각 보드에서 하단의 랜덤박스 아이콘을 클릭하면 아이템을 획득할 수 있습니다.
- 보드 화면에서 G 는 골드 증가, E 는 에너지 증가 치트기 입니다.

## 소스 코드 위치
- 게임 관련 : [Assets/_Projects/Scripts](https://github.com/pumperer/MergeBoard/tree/main/Assets/_Project/Scripts)
- 별도의 repo 에 있는 alpoLib 을 사용하여 Scene, UI, Data 등을 관리 하고 있습니다.
  - alpoLib 패키지는 git url 로 패키지 매니저에서 설치되어 있습니다.
  - [AlpoLib-Core](https://github.com/pumperer/AlpoLib-Core)
  - [AlpoLib-Util](https://github.com/pumperer/AlpoLib-Util)
  - [AlpoLib-Data](https://github.com/pumperer/AlpoLib-Data)
  - [AlpoLib-UI](https://github.com/pumperer/AlpoLib-UI)
  - [AlpoLib-Res](https://github.com/pumperer/AlpoLib-Res)

## 환경
- Unity 6000.1.2f1 + Rider
- portrait 기준으로 구현
- 시작 씬 : Assets/_Project/Scenes/StartupScene.unity

## Codex 분석 요약
- 이 리포지터리는 Unity용 2-merge 보드 게임 샘플입니다. 두 개의 보드가 독립적으로 진행되며, 각 보드에서 퀘스트를 받거나 랜덤 박스를 통해 아이템을 얻을 수 있습니다. 게임 로직은 Assets/_Project/Scripts 디렉터리에 위치하고, 외부 라이브러리인 alpoLib을 사용해 Scene, UI, Data 등을 관리합니다.

- BoardScene은 보드 화면의 핵심 로직을 담당합니다. 씬이 열리면 MergeBoard 인스턴스를 생성하고 필요한 데이터 매퍼를 주입하여 초기화합니다. 보드에서는 G 키와 E 키를 눌러 각각 골드와 에너지를 증가시키는 치트 기능을 제공합니다

- MergeBoard 클래스는 슬롯과 아이템을 관리하며, 아이템 병합과 판매, 랜덤 박스 팝 등의 기능을 제공합니다. 예를 들어 SellItem 메서드는 아이템을 판매하고 골드를 지급한 뒤 퀘스트 상태를 갱신합니다

- 아이템은 Item 스크립트를 통해 동작합니다. 아이템 데이터에 따라 스프라이트를 로드하고, 팝 가능 여부와 쿨타임 등을 업데이트하며, 선택 애니메이션도 지원합니다. ItemData 클래스는 아이템의 팝 가능 여부와 쿨타임을 체크하고, 팝 시 새 아이템을 생성하거나 쿨타임 타이머를 설정합니다

- 사용자 정보와 아이템, 퀘스트 등은 UserData 계열 스크립트에서 관리됩니다. 예를 들어 UserInfoData는 레벨·경험치, 에너지 충전, 골드 변화 등을 처리하며 이벤트로 HUD에 알립니다.

- UI는 UI/Scenes, UI/Hud, UI/Popup 구조로 나뉩니다. TitleSceneUI는 로딩 진행률 표시와 보드 선택 메뉴 생성을 담당하고, PopupQuest와 PopupQuestSubmit은 퀘스트 목록과 제출 과정을 구현합니다

- 결론적으로, 이 프로젝트는 간단한 규칙의 2-merge 보드 게임을 구현한 Unity 샘플로, alpoLib 기반의 데이터 및 UI 관리, 아이템 병합과 팝, 퀘스트 시스템, 랜덤 박스 기능 등을 포함합니다.