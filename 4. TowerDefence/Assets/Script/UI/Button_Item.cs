using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Button_Item : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Transform root;                     // 최상단 부모
    Transform parent;                   // 자기 부모를 찾음
    TextMeshProUGUI itemText;           // 아이템 수량 표시용
    RectTransform rectTransform;         //
    new Camera camera;                  // 마우스 좌표을 신 좌표 값으로 변환하기 용
    public Image image;
    public Image item_out_image;

    Vector3 MousDir;                    // 아이템 최종 생성 좌표
    RaycastHit2D hit;                     // 레이캐스트 용 레이저
    float distance = 11.0f;             // 레이캐스트 거리  

    bool itemUse = false;               // 아이템을 사용할 수 있는지 표시(flase면 불가능, true면 가능)

    ItemDataManager itemDataManager;    // 아이템 데이터 메니저
    uint itemIndex;                     // 아이템 인덱스

    protected virtual void Awake()
    {
        //image = transform.GetChild(0).GetComponent<Image>();
        itemText = transform.parent.GetComponentInChildren<TextMeshProUGUI>();      // 부모에 있는 자식들 중에 TextMeshProUGUI를 찾아라
        //item_out_image = transform.parent.transform.GetChild(0).GetComponent<Image>();
    }

    //protected virtual void Start()
    //{
    //    root = transform.root;  // 최상위 오브젝트를 불러온다.
    //    parent = transform.parent;      // parent의 부모 트랜스폼을 넣는다.
    //    transform.position = parent.position;   // 자신의 위치를 parent의 위치로 가게한다.
    //    camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();      // 하이라키창에 있는 게임오브젝트 중에 "MainCamera"이름이 들어간 오브젝트 찾기
    //    itemDataManager = GameManager.Instance.ItemDta;
    //    item_out_image.sprite = itemDataManager[itemIndex].itemIcon;
    //    image.sprite = itemDataManager[itemIndex].itemIcon;
    //}

    public void StartButton()
    {
        root = transform.root;  // 최상위 오브젝트를 불러온다.
        parent = transform.parent;      // parent의 부모 트랜스폼을 넣는다.
        transform.position = parent.position;   // 자신의 위치를 parent의 위치로 가게한다.
        camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();      // 하이라키창에 있는 게임오브젝트 중에 "MainCamera"이름이 들어간 오브젝트 찾기
        itemDataManager = GameManager.Instance.ItemData;
        item_out_image.sprite = itemDataManager[itemIndex].itemIcon;
        image.sprite = itemDataManager[itemIndex].itemIcon;
        transform.parent.name = itemDataManager[itemIndex].itemName;
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)  // 드래그 시작
    {
        if (itemDataManager[itemIndex].count != 0)        // 아이템 갯수가 0이 아니면
        {
            itemUse = true;     // 아이템을 사용할 수 있다.
            root.BroadcastMessage("BeginDarg", transform, SendMessageOptions.DontRequireReceiver);  // 해당함수가 없어도 오류가 나지 않도록 함
        }
        else
        {
            itemUse = false;    // 아이템을 사용할 수 없다.
            Debug.Log("아이템이 없습니다.");
        }
    }

    public void OnDrag(PointerEventData eventData)      // 드래그 중
    {
        if (itemUse)        // 아이템을 사용할 수 있으면
        {
            root.BroadcastMessage("Darg", transform, SendMessageOptions.DontRequireReceiver);
            transform.position = eventData.position;    // 마우스를 따라가게 함
        }
    }

    public void OnEndDrag(PointerEventData eventData)   // 드래그 끝
    {
        // 아이템 사용
        if (itemUse)        // 아이템을 사용할 수 있으면
        {
            MousDir = camera.ScreenToWorldPoint(eventData.position);         // 스크린 좌표를 월드좌표로 변환
            Vector2 laydir = MousDir;
            Ray2D ray = new Ray2D(laydir, Vector2.zero);
            MousDir.z = 0.0f;           // 카메라 기준의 값이기때문에 -10이 들어가 있다(-10이 있으면 게임 카메라에 안그려진다.)

            if (itemDataManager[itemIndex].layerNames.Length == 0)         // 검출할 레이어가 필요없으면 (설치 불가)
            {
                Debug.Log("Itemdata에 LayerName 설정을 안했습니다.");
            }
            else                        // 검출할 레이어가 있다면 (해당 구역만 설치 가능)
            {
                if (!IsValidPostion(eventData.position) && Physics2D.Raycast(ray.origin, ray.direction, distance, LayerMask.GetMask(itemDataManager[itemIndex].layerNames)))       // 그 해당 좌표에 해당 레이어가 있으면 동작
                {
                    hit = Physics2D.Raycast(ray.origin, ray.direction, distance, LayerMask.GetMask(itemDataManager[itemIndex].layerNames));
                    ItemSawpn();
                }
                else
                {
                    Debug.Log("그 위치에는 사용할 수 없습니다.");
                }
            }

            itemUse = false;        // 아이템을 사용할 수 없다.
            transform.position = parent.position;   // 원래 자리로 돌아가기
        }
    }

    void ItemSawpn()        // 해당 아이템을 소환하는 함수
    {
        Tile_Obstacle tile = hit.transform.GetComponent<Tile_Obstacle>();
        if (tile != null && itemDataManager[itemIndex].modelprefab.CompareTag("Item_Bomb"))      // 프리펩에 넣은 오브젝트가 Item_Bomb이고 tile이 null이 아니면 실행
        {
            if (tile.IsBuildItem == true)
            {
                Debug.Log("이미 설치 되어있습니다.");
                return;
            }
            tile.IsBuildItem = true;
        }
        root.BroadcastMessage("EndDarg", transform, SendMessageOptions.DontRequireReceiver);
        itemDataManager[itemIndex].count--;           // 아이템 갯수 1 감소
        Refresh();                                    // 아이템 현재 수량 갱신
        Instantiate(itemDataManager[itemIndex].modelprefab, hit.transform.position, Quaternion.identity);       // 아이템 프리펩 생성 (MousDir 위치에 생성한다.) MousDir : 최종 좌표

        ItemUse();          // 그 아이템 효과 사용
    }

    /// <summary>
    /// 아이템 사용 함수
    /// </summary>
    protected virtual void ItemUse()
    {

    }

    /// <summary>
    /// 각 슬롯에 인덱스 할당
    /// </summary>
    /// <param name="id">인데스</param>
    public void ItemInitialize(uint id)
    {
        itemIndex = id;
    }

    /// <summary>
    /// 아이템 수량 갱신
    /// </summary>
    public void Refresh()
    {
        itemText.text = $"{itemDataManager[itemIndex].count}";   // 현재 아이템 소유량
        if (itemDataManager[itemIndex].count == 0)               // 아이템 갯수가 0이랑 같으면 alpha값 변경 (아이템이 0개면 회색으로 보이게 하기 위한 기능)
        {
            image.color = Color.clear;          // 0이면 투명
        }
        else
        {
            image.color = Color.white;          // 1이면 정상으로 바꾸기
        }
    }

    public void ParentTransform(Transform transform)
    {
        rectTransform = (RectTransform)transform;
    }

    bool IsValidPostion(Vector2 screenPos)
    {
        Vector2 min = new Vector2(rectTransform.position.x - rectTransform.sizeDelta.x / 2, (rectTransform.position.y + 10.0f) - rectTransform.sizeDelta.y / 2);
        Vector2 max = new Vector2(rectTransform.position.x + rectTransform.sizeDelta.x / 2, (rectTransform.position.y - 10.0f) + rectTransform.sizeDelta.y / 2);
        
        return (min.x < screenPos.x && screenPos.x < max.x && min.y < screenPos.y && screenPos.y < max.y);      // min, max 사이에 있는지 확인
    }
}
