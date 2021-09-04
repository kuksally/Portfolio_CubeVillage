using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData;

    void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        GenerateData();
    }

    void GenerateData()
    {
        // 1. Add 함수를 사용해서 대화 데이트 입력을 추가한다 
        // 2. 여러 문장이 들어가므로 sting[] 배열 사용
        // 촌장 : 1000, 창고지기 : 2000, 마법사 : 3000, 대장장이 : 4000 ,상인 : 5000, 검사 : 6000
        // 나무도끼 : 500, 철 : 600

        // talkData
        talkData.Add(1000, new string[] { "안녕?", "우리 마을에 온걸 환영해." });
        talkData.Add(2000, new string[] { "넌 누구야?", "마을에서 처음보는 얼굴인데." });
        talkData.Add(3000, new string[] { "나중에 찾아와", "원하는 곳으로 보내줄게." });
        talkData.Add(4000, new string[] { "재료 가져왔어?", "장비를 업그레이드 하려면 재료를 가져와." });
        talkData.Add(5000, new string[] { "없는거 빼고 다 있어.", "한 번 둘러봐." });
        talkData.Add(6000, new string[] { "강해지고싶나?", "훈련소에 가서 훈련하도록해." });


        // questDate
        // 퀘스트 번호 +  NPC Id에 해당하는 대화 데이터 작성
        #region quest 1
        talkData.Add(10 + 1000, new string[] { "만나서 반가워 나는 이 마을의 촌장이야.",
                                               "마을에 온 기념으로 내 옆의 창고지기에게 가보는 건 어때?" });
        talkData.Add(11 + 2000, new string[] { "안녕?",
                                               "나는 창고지기야. 앞으로 잘 부탁해!",
                                               "그런 의미로 내 나무 도끼를 찾아줄 수 있을까?",
                                               "주변에 떨어져 있을거야." });
        #endregion

        #region quest 2
        talkData.Add(20 + 1000, new string[] { "창고지기의 도끼?", "글쎄 나는 못 봤는데" });
        talkData.Add(20 + 2000, new string[] { "꼭 찾아줘 중요한거야." });
        talkData.Add(20 + 500, new string[] { "나무 도끼를 찾았다!", "창고지기에게 돌려주자" });
        talkData.Add(21 + 2000, new string[] { "찾아줘서 고마워!", "포탈 앞의 마법사에게 가봐." });
        #endregion

        #region quest 3
        talkData.Add(30 + 3000, new string[] { "마을에 새로온 아이가 너구나?", "나중에 찾아오면 원하는 곳으로 보내줄게.", "대장장이에게 가보도록해!" });
        talkData.Add(31 + 4000, new string[] { "만나서 반가워", "안 바쁘면 옆의 광산에 가서 철 하나를 가져와줄래?" });

        #endregion

        #region quest 4
        talkData.Add(40 + 4000, new string[] { "광산은 마을 동쪽에 있어." });
        talkData.Add(40 + 600, new string[] { "철을 얻었다! 대장장이에게 돌아가자." });
        talkData.Add(41 + 4000, new string[] { "도와줘서 고마워!" });
        #endregion
    }

    // 지정된 대화 문장을 반환하는 함수
    public string GetTalk(int id, int talkIndex)
    {
        // 퀘스트 끝났을 시 예외처리
        if (!talkData.ContainsKey(id)) // ContainsKey() : Dictionary에 Key가 존재 하는지 검사
        {
            if (!talkData.ContainsKey(id - id % 10))
            {
                // 퀘스트 맨 처음 대사마저 없을 때는 퀘스트 번호까지 제거 후 재탐색
                // 기본 대사를 가지고 온다
                return GetTalk(id - id % 100, talkIndex);   // 아래의 식을 간략하게 만듦, 반환 값이 있는 재귀함수는 return까지 꼭 쓰기
                //if (talkIndex == talkData[id - id % 100].Length)
                //{
                //    return null;
                //}
                //else
                //{
                //    return talkData[id - id % 100][talkIndex];
                //}
            }
            else
            {
                // 해당 퀘스트 진행 순서 대사가 없을 때
                // 퀘스트 맨 처음 대사를 가지고 온다
                return GetTalk(id - id % 10, talkIndex);    // 아래의 식을 간략하게 만듦
                //if (talkIndex == talkData[id - id % 10].Length)
                //{
                //    return null;
                //}
                //else
                //{
                //    // Id가 없으면 퀘스트 대화순서 제거 후 재탐색
                //    return talkData[id - id % 10][talkIndex];
                //}
            }
        }

        if (talkIndex == talkData[id].Length)    // talkIndex와 대화의 문장 개수를 비교하여 대화의 끝을 확인한다
        {
            return null;
        }
        else
        {
            return talkData[id][talkIndex];
        }
    }
}