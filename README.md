# SoulEater
인하대학교 컴퓨터공학과 2021-1 게임프로그래밍  프로젝트  
INHA Univ. 2021-1 Game Programming Class Project

2인 팀으로 개발되었으며, 팀장을 맡아 전체적인 프로젝트 관리를 겸한 본인의 개발 영역은

- 입력 처리 및 애니메이션
- 물리 처리
- UI/UX 및 기능 구현(미니맵 제외)
- 대부분의 캐릭터 스킬 구현
- 드랍 아이템 구현

등이 있다.

## [Demo video]

[![Watch the video](https://images.velog.io/images/cedongne/post/77b1bfa0-8064-4729-b82d-191ce4300848/SoulEater%20Thumbnail.png)](https://youtu.be/NiL4u9SOjL0)

- # 게임 소개
  로그라이크 장르의 3D 핵앤슬래시 게임으로, 게임을 플레이할 때마다 캐릭터가 플레이하는 맵, 나오는 몬스터, 스킬 등의 다양한 요소들이 랜덤하게 등장하며 매판 새로운 게임을 하는 신선한 느낌을 가져다준다. 출현하는 몬스터를 잡으면 해당 몬스터의 영혼이 일정 확률로 드랍되고, 플레이어가 영혼을 흡수하여 자신의 능력으로 만들어 전투에 활용할 수 있다.
  
  - 스토리
  
    특별한 능력으로 소울이터라 불리는 주인공이 세계를 위협하는 괴물과 악마들의 영혼으로 그들을 상대한다. 그는 세계의 안정을 되찾기 위해 악의 원흉을 찾아 여행을 떠난다. 일부 악마들은 자신의 모습을 다른 것들과 전혀 다른 형태의 이질적인 모습으로 만들어 인간들을 혼란에 빠뜨린다.
    
- # 시점 및 조작 방법
  
    - **시점 및 컨트롤** 
    
      스타일리시한 전투 방식을 위해 쿼터 뷰 시점을 차용한다. 마우스 커서로 공격 방향을 지정하며, 키보드로 이동 / 공격 / 대시 / 스킬 사용 / 상호작용 등의 조작을 할 수 있다.
    
    - **마우스 커서 공격 방향 지정**
      
      마우스 커서로 공격 방향을 지정하는 방식은 RayCast를 이용하였고, 사선으로 화면을 내려보는 시점 방식을 고려하여 스크린 좌표계로 이동하여 마우스 방향을 올바르게 따라갈 수 있도록 처리해주었다. 
      
      Ray가 진행하는 방향에 스킬 이펙트나 몬스터 오브젝트 등 다양한 장애물이 존재할 때 기대와 다른 방향으로 투사체가 발사되기도 하였는데, 해당 오브젝트들을 Ignore Raycast layer에 놓음으로써 문제를 해결하였다. 결과적으로 화면에서 원하는 지점을 찍으면 해당 방향을 플레이어가 바라보는 LookMouseCursor 함수를 구현하여 다양한 공격 모션에 활용하였다.
    
    - **기본공격**
      
      LookMouseCursor 함수로 원하는 방향을 향해 플레이어가 회전하면 해당 방향으로 Velocity를 주어 투사체를 발사하였다.
  
    - **대시**
   
      대시를 위한 애니메이션을 실행시킴과 동시에 플레이어의 속도를 일시적으로 빠르게 조절하여 구현하였다. 동시에 무한정 대시를 사용할 수 없도록 isDodgeReady 플래그를 설정하고 Time.deltaTime을 연산하여 일정한 쿨타임을 두고 대시를 사용할 수 있게 설정하였다.

    - **스킬 사용**
 
      보유 가능한 세 개의 스킬을 숫자키 1, 2, 3번으로 사용할 수 있게 하였고, 기본공격이나 대시와 마찬가지로 다른 행동과 동시에 실행할 수 없도록 플래그를 설정해주었다. 보유한 스킬이 액티브 스킬일 경우 개별 쿨타임을 설정해주었고, 이 쿨타임은 스킬마다 각자의 쿨타임 변수를 만들어 따로 관리가 가능하도록 설계하였다. 
      
      <p align="center"><img src = "https://user-images.githubusercontent.com/57585303/149352843-97912d9a-cf5c-4e33-8ccf-b2bc42b9beac.png"></p>
      
      이러한 쿨타임 관리는 코루틴 함수를 이용해 여러 스킬의 쿨타임이 병렬적으로 처리가 가능하도록 해주었고, Filled image type에서 Radial 360 방식을 적용해 원형으로 쿨타임 표현이 가능하도록 함과 동시에 쿨타임 동안 재사용해도 다시 게이지가 돌지 않도록 처리해주었다.
      
      또한, 각 스킬 초상화 위에 존재하는 번호판의 색깔을 액티브/패시브 스킬에 따라 다르게 하여 어떤 스킬이 사용 가능한 스킬인지 구분되도록 디자인해 주었다.
      
    - **상호작용**
    
      몬스터를 처치하고 나온 영혼을 획득하여 자신의 스킬로 만드는 상호작용을 구현하였다.
      
      <p align="center"><img src = "https://user-images.githubusercontent.com/57585303/149353270-cab83d81-ad10-482a-8fcc-06b0cc33e4df.png"></p>
      
      이때 똑같은 스킬을 두 개 가지지 않도록 가지고 있는 영혼 리스트를 만들어 현재 플레이어가 습득 가능한 범위에 있는 영혼이 플레이어가 보유 중인 영혼인지 판단하여 이미 존재하고 있다면 중복으로 획득할 수 없도록 처리해주었다.
            
      <p align="center"><img src = "https://user-images.githubusercontent.com/57585303/149353939-bbebcd77-ab08-4f06-89d6-4c0e68127afa.png"></p>
      
      이미 세 개의 영혼을 가지고 있을 때 새로운 영혼을 습득하려 한다면 자신이 가지고 있는 다른 영혼과 바꿀 것인지, 혹은 말 것인지 선택할 수 있도록 처리해주었고, 이동키로 어떤 스킬과 교환할 것인지 선택이 가능하도록 설계해주었다. 이때 현재 보유 중인 패시브 스킬을 변경하려 한다면 적용된 패시브 스킬의 인스턴스 등을 모두 해제하고 새롭게 습득할 스킬을 적용해주었다.
      
- # UI
  - **HUD**
  
    에셋 스토어의 [GUI Parts](https://assetstore.unity.com/packages/2d/gui/icons/gui-parts-159068) 에셋을 이용 및 수정하여 아래의 Head-up display를 제작해 인게임에 적용하였고, UI component 중 Slider를 이용해 HP bar를 구현하였다. HUD는 FHD(1920x1080)와 QHD(2560x1440)에서 호환이 가능하도록 구현하였다.
    
    <p align="center"><img src = "https://user-images.githubusercontent.com/57585303/149356222-70ba4a7b-cfce-4001-86f0-fb0d06bd5e43.png"></p>

    
    UI Manager를 이용해 플레이어의 체력을 실시간으로 관찰하고 즉각적으로 감소폭을 확인할 수 있도록 처리해 주었다.
    
  - **미니맵**
  
    <p align="center"><img src = "https://user-images.githubusercontent.com/57585303/149355002-d28a97ef-f1e0-4777-a6b3-2df40218dba6.png"></p>
    
    화면 좌측 하단에 현재 플레이어가 위치한 맵을 직교로 투영하는 또다른 카메라를 배치하여 맵 구조와 플레이어/몬스터/포탈의 위치를 파악할 수 있는 미니맵을 구현하였다.

  - **몬스터 HP bar**
  
    <p align="center"><img src = "https://user-images.githubusercontent.com/57585303/149355040-20ec8f19-86ce-473a-9ed1-ad559afed735.png"></p>
    
    필드에 존재하는 모든 몬스터에게 플레이어와 같이 남은 체력을 확인할 수 있는 HP bar를 할당해주었다. 현재 몬스터의 월드 좌표계 위치를 실시간으로 불러와 스크린 좌표계로 변환한 위치에 HP bar를 위치시키고 현재 체력에 비례하여 Slider의 게이지를 변경해주었다.

- # 캐릭터 기본 정보

  플레이어가 가지는 능력치는 총 6종류이며, 각각의 기본값은 다음과 같다.
  
  |  능력치                      | 수치  |
  | :-------------------------  | :---: |
  | `체력`                       | 100  |
  | `공격력` 		  	            | 10   |
  | `이동속도` `(최대 10)`       | 5	   |
  | `공격속도` `(초당 공격 횟수)` | 2    |
  | `쿨타임 감소` `(최대 60%)`   | 0     |

  기본공격은 특별한 힘으로 전방에 투사체를 발사한다. 적중한 몬스터를 기준으로 작은 범위에 대미지를 준다. 영혼은 3개까지 소지가 가능하며, 영혼을 최대로 소지하고 있을 때 이미 가지고 있는 영혼을 새로운 영혼으로 변경하는 것이 가능하다.
  
- # 스킬
  몬스터 처치 시 일정 확률로 그 몬스터의 영혼이 나온다. 각 영혼을 획득할 시 플레이어에게 몬스터의 특징에 어울리는 스킬을 부여한다. 스킬 타입에는 `패시브`, `액티브`, `투사체`가 있으며, 몬스터들의 영혼이 제공하는 스킬은 다음과 같다. 
  
  | 등급 | 마물 종류       | 스킬 효과                                    |
  |:----:|:--------------:|:---------------------------------------------|
  | 일반 | 그런트          | 커다란 바위를 던져 범위에 높은 대미지          |
  | 일반 | 고슴도치        | 캐릭터 주변 범위로 들어오는 몬스터에 대미지     |
  | 일반 | 미믹            | 보유 중인 스킬 쿨타임 50% 감소                |
  | 일반 | 검정 악어       | 영구적으로 공격력 + 공격속도 증가              |
  | 일반 | 초록 악어       | 주변에 독 장판을 생성해 지속 대미지 + 둔화      |
  | 일반 | 드레이크        | 세 갈래로 나가는 파이어 볼 발사                |
  | `희귀` | 파이어 드래곤 | 지정한 위치로 메테오 발사                      |
  | `희귀` | 오크          | 전방을 강하게 내리쳐 지진 발생 + 넓은 범위 둔화 |
  
  위 표에 기재된 각 몬스터의 영혼 인게임 디자인은 다음과 같다. 일반 몬스터의 영혼은 30%, 미믹은 20% 확률로 드랍되며, 네임드 몬스터의 영혼은 30% 확률로 드랍된다.
  
  <p align="center"><img src = "https://user-images.githubusercontent.com/57585303/149355089-24986c27-c42e-4587-a469-1f46cfa15ce2.png"></p>

  이후 아래는 각 몬스터의 영혼이 발휘하는 스킬의 정보이다.
  
  - **그런트**
  
    ![그런트 스킬](https://user-images.githubusercontent.com/57585303/149452099-4794078e-55b6-49bc-bead-be2d6ab47271.png)

    |스탯        | 내용               | 스탯        | 내용  |
    |:----------:|:-----------------:|:----------:|:------:|
    | `몬스터`   | 그런트             | `스킬 타입` |투사체  |
    | `대미지`   | 투사체 20, 폭발 15 | `쿨타임`    | 8초    |
    | `선딜레이` | 0.5초              | `후딜레이`  | 1초    |
    
  - **고슴도치**
    
    ![고슴도치 스킬](https://user-images.githubusercontent.com/57585303/149452666-cfdab2f2-e165-452a-8549-5737a1a42f8d.png)
    
    |스탯        | 내용          | 스탯        | 내용 |
    |:----------:|:-------------:|:----------:|:----:|
    | `몬스터`   | 고슴도치       | `스킬 타입` |패시브|
    | `대미지`   | 범위 내 초당 1 | `쿨타임`    | -    |
    | `선딜레이` | -             | `후딜레이`  | -    |
    
  - **미믹**

    ***[이펙트 X]***
    
    |스탯        | 내용                     | 스탯        | 내용  |
    |:----------:|:-----------------------:|:-----------:|:-----:|
    | `몬스터`   | 미믹                     | `스킬 타입` | 패시브 |
    | `효과`     | 보유 스킬 쿨타임 50% 감소 | `쿨타임`    | -     |
    | `선딜레이` | -                        | `후딜레이`  | -     |
    
  - **검정 악어**

    ***[이펙트 X]***
    
    |스탯        | 내용                                            |스탯         | 내용   |
    |:----------:|:----------------------------------------------:|------------:|:------:|
    | `몬스터`   | 검정 악어                                        | `스킬 타입` | 패시브 |
    | `효과`     | 기본공격 대미지 50%, 최대체력 및 현재체력 50% 증가 | `쿨타임`    | -      |
    | `선딜레이` | -                                               | `후딜레이`  | -      |
    
  - **초록 악어**
    
    ![초록 악어 스킬](https://user-images.githubusercontent.com/57585303/149454418-8ca873ae-9f84-4faa-8fff-5cbdb33a88ba.png)
        
    |스탯        | 내용                     | 스탯        | 내용   |
    |:----------:|:-----------------------:|:-----------:|:------:|
    | `몬스터`   | 초록 악어                | `스킬 타입`  | 액티브 |
    | `대미지`   | 범위 내 초당 2 (6초 지속) | `쿨타임`    | -      |
    | `선딜레이` | -                        | `후딜레이`  | -      |
    
  - **드레이크**
  
    ![드레이크 스킬](https://user-images.githubusercontent.com/57585303/149453201-9b138250-52d0-4996-a50a-94d98095a304.png)

    |스탯        | 내용              | 스탯        | 내용   |
    |:----------:|:----------------:|:-----------:|:------:|
    | `몬스터`   | 드레이크           | `스킬 타입` | 투사체 |
    | `대미지`   | 투사체 5 / 폭발 10 | `쿨타임`    | 6초    |
    | `선딜레이` | -                 | `후딜레이`  | 0.1초  |

  - **파이어 드래곤**
  
    ![파이어 드래곤 스킬](https://user-images.githubusercontent.com/57585303/149454329-ebd4cb12-c877-4dec-8527-819c7b47cfbd.png)
    
    |스탯        | 내용               | 스탯        | 내용  |
    |:----------:|:------------------:|:----------:|:-----:|
    | `몬스터`   | 파이어 드래곤       | `스킬 타입` | 투사체 |
    | `대미지`   | 투사체 40 / 폭발 20 | `쿨타임`    | 12초  |
    | `선딜레이` | -                  | `후딜레이`  | 0.1초 |
    
  - **골렘**
  
    ![골렘 스킬](https://user-images.githubusercontent.com/57585303/149452801-ec0a2d1e-f4f2-4112-bfb3-0dfef91f8ade.png)

    |스탯        | 내용                        | 스탯       | 내용   |
    |:----------:|:--------------------------:|:----------:|:------:|
    | `몬스터`   | 골렘                        | `스킬 타입` | 패시브 |
    | `효과`     | 기본공격 크기, 대미지 10 증가 | `쿨타임`   | -      |
    | `선딜레이` | -                           | `후딜레이`  | -     |
    
    ![image](https://user-images.githubusercontent.com/57585303/149452611-f58c22dd-362a-4a1d-8ec7-0947e2f80ca6.png)


  각 스킬 타입의 구현 방식은 다음과 같다.
  
  - **패시브**
  
    Passives class를 추가하여 모든 패시브 스킬들을 통합적으로 관리해주며, 보유 중인 영혼에 따라 TurnOnPassive/TurnOffPassive 함수를 이용해 패시브 스킬을 활성화/비활성화해주는 기능을 구현하였다. 공격형 패시브 스킬의 경우 Collider를 가져 직접 피해를 줄 수 있는 오브젝트를 프리팹으로 만들어 스킬을 얻을 때 Instantiate 하여 상시 효과를 발휘하도록 하고 다른 스킬로 교체할 때 해당 인스턴스를 파괴해주었다.
    
  - **액티브**
  
    Attacks class로 액티브 스킬을 관리한다. 투사체가 없는 액티브 스킬을 칭하며, 사용하였을 때 플레이어의 위치에 따라 스킬 이펙트가 Instantiate 되고, 스킬에 따라 알맞은 효과를 주도록 설계하였다.
    
  - **투사체**
  
    액티브와 마찬가지로 Attacks class로 관리하며, 투사체가 존재하는 모든 액티브 스킬을 투사체로 분류하였다. 액티브 타입과의 차이는 충돌이 2회 일어난다는 것인데, 플레이어로부터 생성되는 투사체가 1회 충돌하고 투사체가 충돌하며 발생하는 폭발과 같은 이펙트가 발생했을때 추가적인 충돌이 발생한다.
    
- # 몬스터

  몬스터의 등급은 일반/네임드/보스 몬스터로 나뉜다. 일반 몬스터는 기본공격만으로 전투하고 네임드 몬스터는 한 개, 보스 몬스터는 세 개의 스킬을 가진다. 스킬의 타입은 3가지로,
그 종류와 기본적인 구현 로직은 다음과 같다.

  - **투사체를 발사하는 스킬**
  
    몬스터의 위치에 Instantiate를 한 후 몬스터로부터 플레이어로의 방향을 계산해 그 방향으로 구형의 투사체에게 물리적인 힘을 가해 발사하고 해당 구체에 플레이어가 닿으면 피
해를 준다.

  - **랜덤하게 몬스터 주위에 구체들을 소환해 피해를 주는 스킬**

    몬스터를 기준으로 특정 범위 내의 좌표를 랜덤하게 지정해 해당 좌표에 구체를 Instantiate 해 구체에 플레이어가 닿으면 피해를 준다.

  - **몬스터를 기준으로 주변에 원형의 범위로 피해를 주는 스킬**

    몬스터의 위치에 원형의 장판 스킬을 Instantiate 해 장판의 크기만큼의 범위에 피해를 준다.

  이러한 스킬들이 발동되는 조건은 아래와 같이 설계되었다.

  1. 해당 몬스터의 MonsterSkillController script의 Update 함수를 이용해 Frame마다 시간을 확인해 5초마다 Coroutine 함수로 스킬을 발동하는 코드를 실행한다.
  2. 스킬 가동 범위에 플레이어가 있다면 캐스팅 상태에 돌입한다.
  3. 캐스팅이 완료되는 동안 MonsterSkill script에서 해당 몬스터가 가진 스킬 중에서 하나를 랜덤하게 선택하여 투사체를 제외한 위치가 정해져 있는 스킬의 예상 공격 범위를 설정하고 표시해준다.
  4. 캐스팅이 완료되면 다시 MonsterSkill script에서 해당 스킬을 발동한다.
  
  아래는 게임에 등장하는 몬스터들의 정보이다.
  
   - **그런트**
  
     ![그런트 이미지](https://user-images.githubusercontent.com/57585303/149457562-a1e7ce20-f5fa-4763-ba04-d8b1f061693d.png)
  
     | 체력 | 공격력 | 공격속도 | 이동속도 |
     |:----:|:-----:|:-------:|:--------:|
     | 20   | 10    | 0.8     | 4        |
    
   - **고슴도치**

     ![고슴도치 이미지](https://user-images.githubusercontent.com/57585303/149457788-c12125ce-cdb7-4f30-8c1b-6ecb115bb2fe.png)
  
     | 체력 | 공격력 | 공격속도 | 이동속도 |
     |:----:|:-----:|:-------:|:--------:|
     | 20   | 10    | 0.8     | 4        |
    
   - **미믹**

     ![미믹 이미지](https://user-images.githubusercontent.com/57585303/149457831-dfb7b2e7-a859-4032-8eb0-59215f6943c2.png)
  
     | 체력 | 공격력 | 공격속도 | 이동속도 |
     |:----:|:-----:|:-------:|:--------:|
     | 20   | 10    | 0.7     | 4        |
    
   - **검정 악어**
   
     ![검정 악어 이미지](https://user-images.githubusercontent.com/57585303/149457962-0eeb3a95-8f19-4462-9029-90e6f4431065.png)
  
     | 체력 | 공격력 | 공격속도 | 이동속도 |
     |:----:|:-----:|:-------:|:--------:|
     | 20   | 10    | 1.2    | 4        |
    
   - **초록 악어**

     ![초록 악어 이미지](https://user-images.githubusercontent.com/57585303/149458035-de279a3e-3530-457c-a531-72b91f3c9e20.png)
  
     | 체력 | 공격력 | 공격속도 | 이동속도 |
     |:----:|:-----:|:-------:|:--------:|
     | 20   | 10    | 1.2     | 4        |
    
   - **드레이크**

     ![드레이크 이미지](https://user-images.githubusercontent.com/57585303/149458094-5c6aa29d-94de-48a5-b68d-c93f21ee7859.png)
  
     | 체력 | 공격력 | 공격속도 | 이동속도 |
     |:----:|:-----:|:-------:|:--------:|
     | 20   | 10    | 0.7     | 4        |

   - **파이어 드래곤**

     ![파이어 드래곤 이미지](https://user-images.githubusercontent.com/57585303/149458249-19411a22-acae-4b96-ad23-c9f4e308ac14.png)
  
     | 체력  | 공격력 | 공격속도 | 이동속도 |
     |:-----:|:-----:|:-------:|:--------:|
     | 100   | 20    | 1.5     | 6        |
     
     - **스킬**
     
       [액티브]
        
       ![image](https://user-images.githubusercontent.com/57585303/149458819-8b47ae85-81e4-42c8-9059-d242bfee25e1.png)

   - **골렘**

     ![골렘 이미지](https://user-images.githubusercontent.com/57585303/149458284-3842ef85-71d1-411b-901e-71e2a0af9111.png)
  
     | 체력  | 공격력 | 공격속도 | 이동속도 |
     |:-----:|:-----:|:-------:|:--------:|
     | 100   | 20    | 2       | 4        |
     
     - **스킬**
     
       [패시브] 골렘이 플레이어와 접촉만 하더라도 대미지를 준다.
       
   - **리치**

     ![리치 이미지](https://user-images.githubusercontent.com/57585303/149458366-da895aed-84e1-44b6-bbdf-6dea087043b5.png)
  
     | 체력  | 공격력 | 공격속도 | 이동속도 |
     |:-----:|:-----:|:-------:|:--------:|
     | 400   | 30    | 1.5     | 3        |
     
     - **스킬**
     
       [액티브]
       
       ![image](https://user-images.githubusercontent.com/57585303/149459195-a85de881-e58c-4cd0-bc08-9477bc7388f7.png)

- # 맵
  매판마다 랜덤하게 7개가량의 맵이 일직선으로 연결되어 생성된다. 맵의 테마에 따라 등장하는 몬스터 종류, 수, 각 맵의 출현 빈도 및 맵의 형태는 랜덤으로 설정된다. 맵 테마 종류는 다음과 같다. 맵이 랜덤으로 생성되는 로직을 조금 더 자세하게 표현하면 아래와 같다.

  ![image](https://user-images.githubusercontent.com/57585303/149460387-b229f6ad-ef09-413a-9f87-fa0dd0f4e715.png) 

  1. MapSpawner 오브젝트에서 MapSpawner를 수행하면서 프리팹으로 만들어 놓은 일반 맵을 랜덤한 순서로 랜덤하게 생성하고 마지막으로 보스 맵을 생성한다.
  2. 맵이 생성되면서 MapGenerator를 수행한다. MapGenerator는 맵에서 이동 가능한 타일과 불가능한 타일을 0과 1로 구분해주는 작업을 해준다
  3. 그렇게 평면상에서 맵이 구현되면 각 맵에서 Spawner를 작동시켜 이동 가능한 타일 중에 랜덤하게 몬스터를 스폰해준다. 일반 몬스터는 5마리로 고정되어 소환되며, 그 종류는 랜덤하다. 또한 맵 컨셉에 맞는 네임드 몬스터의 스폰 확률은 30%로 설정하였다.
  4. 그 후 MeshGenerator를 수행해서 이동이 불가능한 구역에 벽을 생성해 3D 공간의 맵을 만들어준다.
  5. 그렇게 바닥과 벽을 구분하여 생성하면 해당 맵에 Texture를 입히고 Collider를 만든다.
  6. 다음 맵으로 가는 포탈을 top, left, right 방향 중 랜덤하게 생성해주고 다음 맵에서 캐릭터가 반대 방향으로 스폰되도록 방향을 저장해 두 맵이 연속으로 이어진 듯한 효과를 준다.
  7. MeshGenrator에서의 작업이 끝나면 MapGenerator에서 일반 맵들의 포탈을 잠시 비활성화한다.
  8. Frame마다 현재 맵 상에서 몬스터가 존재하는지 조사해 몬스터가 더 이상 없다면 포탈을 활성화해준다. 
 
  몬스터는 유니티 자체에서 제공하는 Navigation으로 플레이어를 자동으로 추적하게 하였다. Navigation을 설정하기 전 맵 생성과 설정이 끝난 후 경로를 탐색한다. 현재 자신의 위치를 기준으로 대상이 추적 거리 안으로 들어오면 추적하고 공격 사정거리 안으로 들어오면 공격 또는 스킬이 발동되도록 구현하였다. 
  
  또한, 몬스터가 피격을 당할 때 경직 효과를 주기 위해 GetHit 애니메이션을 실행하면서 짧은 시간 Nav Mesh Agent을 정지시키는 처리를 해주었다.
