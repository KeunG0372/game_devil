# 소악마 키우기

![123123](https://github.com/KeunG0372/game_devil/assets/127164234/c3b16ec7-c62f-4871-bd2e-6890618f8b11)


2.5D 기반의 방치형 모바일 게임

<br><br>

## 목차  <a name='main'></a>

### 1. 게임 컨셉
### 2. 디자인 컨셉
### 3. 시스템 구현
### 4. 개발

---
<br><br>

## 게임 컨셉

+ 상하좌우 움직일 수 있는 2.5D 기반으로 왼쪽의 조이스틱을 이용해 움직일 수 있고 자동사냥 또는 직접 스테이지의 몬스터를 처치해
  자원을 얻어 캐릭터 강화 또는 무기 강화로 캐릭터를 성장해 다음 스테이지로 점점 나아가는 방식의 게임
+ 스테이지가 높아질수록 몬스터의 HP 공격력이 상승함
+ 스테이지 통과 조건은 일정 시간 동안 일정 수의 몬스터를 처치하는 것이 조건
+ 몬스터가 플레이어와 일정 거리 이상 가까워지면 플레이어의 HP 감소
+ 몬스터는 플레이어의 카메라 범위 밖에서 스폰하여 플레이어를 쫓아가는 방식
+ 스킬을 통해 다양하게 몬스터를 잡을 수 있게 설정

### 컨셉 이미지 :

![Screenshot_20220622-165951](https://github.com/KeunG0372/game_devil/assets/127164234/9ba4221b-63cd-4e58-830c-5797142c707f)

<br><br>

## 디자인 컨셉

### 캐릭터 디자인 :

+ 지팡이를 들고 있는 미소녀 마법사 (SD)
+ 마법을 사용
+ 공격, 기본, 움직임 상황에 맞는 애니메이션 필요

#### 구상 이미지 :
  
![54c4a95c8d73c4213c73b098ef8bae74](https://github.com/KeunG0372/game_devil/assets/127164234/c81df743-26ab-4012-b02b-9f7555958cf1)
![unnamed](https://github.com/KeunG0372/game_devil/assets/127164234/2b74f1ef-0d8e-4ebd-8c0c-5132d704f303)

### 몬스터 디자인 : 

+ 박쥐, 슬라임, 던전에서 나올법한 디자인
+ 생동감 있는 애니메이션 필요

### 스테이지 디자인 :

+ 던전에 맞는 배경

<br><br>

## 시스템 구현

### 중요도 ★★★ : 
+ ~~캐릭터 이동 및 애니메이션 (조이스틱)~~   
+ ~~몬스터 이동 및 애니메이션~~
+ ~~몬스터가 유저의 카메라 일정 범위 밖에서 생성 및 플레이어를 따라오게 지정~~
+ ~~캐릭터와 몬스터 HP, 공격에 의한 HP 감소~~
+ ~~무기의 자동공격 및 몬스터에게 자동발사 구현~~
+ ~~카메라가 플레이어를 바라보게 고정, 맵 밖을 보여주지 않게 설정~~
+ ~~스킬 구현~~


### 중요도 ★★ : 
+ ~~캐릭터 강화 시스템~~
+ ~~캐릭터 스테이터스~~
+ ~~캐릭터 레벨업 시스템~~
+ ~~소지품 및 가방 스토리지 구현~~ (강화창에서 소지한 골드, 보석 보여주기로 대체)
+ ~~몬스터를 잡았을 때 아이템 드롭 및 자동 획득~~ (아이템 드랍은 UI로 보여주는 용도)
+ ~~무기 공격, 피격 효과 및 이펙트~~
+ ~~캐릭터 HP 자동회복~~
+ ~~엘리트 몬스터 추가~~ (엘리트 몬스터는 기본 몬스터의 컬러, 크기 조정 후 기본보다 강력하게 설정)
+ ~~사운드 추가~~
+ ~~자동 공격 시스템 추가~~ (버튼을 누를 시 스킬과 이동을 자동으로)
+ 전체적인 UI
> ~~플레이어 HP, XP~~
>
> ~~플레이어 강화~~
>
> ~~플레이어 스테이터스~~
>
> ~~설정~~
>
> ~~스테이지 이동~~



### 중요도 ★ : 
+ ~~스테이지 구현~~
+ ~~스테이지 이동~~
+ ~~메인 씬 구현~~
+ 보스 스테이지 구현 (미구현)
+ 미니맵 구현 (미구현)

<br><br>

## 개발

#### 개발 일정 : 
![화면 캡처 2024-06-04 204434](https://github.com/KeunG0372/game_devil/assets/127164234/ace340cc-424c-412a-b1e1-abd8a7af17e0)

## [1주차]

<br>

1. 게임 기획
2. 게임 컨셉 결정 및 시스템 설계

<br>

## [2주차]

<br>

1. 캐릭터, 애니메이션 추가
> 기본, 이동, 공격 애니메이션 추가
2. 플레이어 움직임 추가 (조이스틱)
3. 몬스터 스폰 및, 몬스터 애니메이션 추가
> 몬스터는 플레이어와 일정 거리 범위 밖에서 스폰
> 플레이어를 추적해서 쫓아옴
4. 카메라 뷰 설정
> 위에서 사선으로 바라보게 설정
> 플레이어를 쫓아다님


<br>

## [3주차]

<br>

1. 플레이어, 몬스터 HP 추가
> 플레이어의 주변 몬스터의 수에 따라 데미지 증가
> 몬스터가 플레이어와 접촉 시 HP 감소
> 몬스터의 HP는 공격받으면 감소
3. 기본공격 추가
> 가장 가까운 몬스터를 파악해서 몬스터를 쫓아감

<br>

## [4주차]

<br>

1. 스킬 추가
> 파이어(소드) 스킬 추가
> 주변에 몬스터가 없다면 플레이어가 바라보는 방향, 주변에 있다면 몬스터의 위치로 스킬 생성
> 파이어(소드) 스킬 애니메이션 추가
> 아이스 추가 예정
2. 이펙트 추가중
> 기본공격, 스킬에 몬스터 이펙트 구현중 몬스터는 피격 시 몬스터가 하얀색으로 잠깐 바뀌고 몬스터에게 이펙트를 추가 할 예정
3. 카메라 뷰 수정 (탑뷰로 수정)
4. 엘리트 몬스터 추가
> 일반 몬스터의 체력 1.5배 몬스터 5마리 잡을 시 생성
> ~~현재 몬스터가 죽을 시 카운트 올라가는 개념이라 엘리트 몬스터를 죽일 시 몬스터 생성 제한이 1 늘어나는 버그 수정중~~ (5주차 수정완료)
5. 재화 드랍
> 골드 및 보석 드랍 골드는 확정적으로 몬스터를 잡을 시 등장하고 보석은 50% 확률로 드랍
6. 강화 시스템
> 기본 공격, 스킬 강화버튼을 누르면 골드를 소비해 공격력 10씩 증가 필요 골드는 2배수 증가

<br>

## [5주차]

<br>

1. 스킬 추가
> 아이스 스킬 추가 (파이어(소드)와 동일한 작동구조)
> 스킬 버튼을 연타하면 애니메이션은 재생되지만 스킬은 안나오는 버그 수정중
2. 바닥 마테리얼 추가중
> 투벽 추가 완료
> 위부분 벽이 잘 보이지 않는 관계로 그림 수정 예정
3. 카메라 뷰 수정 완료
> 벽에 가까워질 시 카메라가 움직이지 않음
4. 몬스터 스폰 방식 변경중
> 기존 : ~~플레이어 주변 최소반경 이상에서 스폰~~
> 변경 : ~~플레이어 주변 최소반경 이상 맵 범위 안에서 스폰~~ (6주차 수정완료)
5. 이펙트 추가
> 스킬을 맞았을 시 이펙트 애니메이션 발동
### 이펙트 예시
![123123](https://github.com/KeunG0372/game_devil/assets/127164234/438f316d-ea56-464c-ac48-e476d54d9397)


<br>


## [6주차]

<br>

1. 자동공격 ON / OFF 시스템
> 몬스터에게 일정 범위까지 자동으로 이동
> 플레이어가 움직일 시 자동이동 꺼짐
> 플레이어 스킬은 상시 발동
2. 스킬, UI 버튼 수정 및 추가
> 기존 스킬을 그대로 두고 업그레이드 형식으로 할 것인지 더 좋은 것으로 변경 할 것인지 고려중
> 왼쪽 상단 hp바 xp바 추가
3. 피격 이펙트 추가
> 몬스터, 플레이어 피격 시 스프라이트 변화
4. 레벨업 시스템 추가
> 플레이어가 몬스터를 잡으면 경험치를 얻으며 레벨업을 할 시 최대 체력이 늘어나며 체력회복 함

<br>

## [9주차]

<br>

1. 캐릭터 UI 및 강화 UI 추가
> 재화, 공격 데미지, 스킬 데미지, 이동속도, 공격 속도, 경험치 UI추가
> 강화 버튼을 누를 시 각각의 수치가 올라가도록 변경
> 각 강화 버튼을 눌렀을 때 필요한 골드, 보석, 강화했을 때 변경되는 수치 텍스트 작성중
2. 사운드 찾는중

<br>

## [10주차]

<br>

1. UI 마무리
> 캐릭터 이미지 추가 및 세부 스테이터스 표시, 강화 시 업데이트
> 강화 시 현재, 강화 후 스테이터스 강화 창에 표시 및 업데이트
2. 메인 로딩 창
> 메인, 스테이지 씬 분리
> 로딩에 대한 시스템 파악 중
3. 사운드
> 배경음, 효과음 추가 중

<br>

## [11주차]

<br>

1. 사운드 추가
> 플레이어 스킬, 기본 공격, UI 클릭, 메인, 스테이지 배경음 추가
> ~~사운드 볼륨 조절 가능하지만 유저 조작기능은 아직 없음~~ (13주차 수정완료)
2. 자잘한 애니메이션 수정
> 플레이어 움직이는 애니메이션 수정
3. 스테이지정
> 스테이지가 증가하면 몬스터의 HP 추가로 증가
> 씬 전환할 때 게임매니져를 dontdestroy로 설정해 스테이지가 전환되어도 플레이어의 정보는 변하지 않게 관리
> 스테이지 전환 UI 추가
> 스테이지 필드는 컬러와 좌우반전


  
