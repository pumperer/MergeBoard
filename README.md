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
- 시작 씬 : Assets/_Project/Scenes/Startup.unity
