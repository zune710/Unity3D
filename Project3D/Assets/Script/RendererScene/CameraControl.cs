using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;  // Language-Integrated Query


public class CameraControl : MonoBehaviour
{
    private Vector3 direction;

    public Transform target;

    public LayerMask mask;

    public const string path = "Custom/Transparent/MyShader";

    [SerializeField] private List<Renderer> objectRenderers = new List<Renderer>();

    private Vector3[] outCorners = new Vector3[4];

    [Header("Invisibility")]
    public bool FrustumList;

    [Range(0.0f, 1.0f)]
    public float X;

    [Range(0.0f, 1.0f)]
    public float Y;

    [Range(0.0f, 1.0f)]
    public float W;

    [Range(0.0f, 1.0f)]
    public float H;

    [Range(0.3f, 50.0f)]
    public float distance;


    private void Awake()
    {
        target = GameObject.Find("Player").transform;
    }

    private void Start()
    {
#if UNITY_EDITOR
        if (!target)
        {
            EditorApplication.ExitPlaymode();
            Debug.Log("Target could not be loaded.");
        }

        if (path.Length <= 0)
        {
            EditorApplication.ExitPlaymode();
            Debug.Log("Path could not be loaded.");
        }
#endif

        //프러스텀 컬링이 아니라 투명화 체크하기 위한 것이므로 변수에 아래 카메라 설정을 대입해서 쓰지 않는다.
        // 대입해서 쓰면 진짜 프러스텀 컬링이 되기 때문
        //X = Camera.main.rect.x;
        //Y = Camera.main.rect.y;
        //W = Camera.main.rect.width;
        //H = Camera.main.rect.height;
        // 투명화 체크만 할 것이므로 X, Y, W, H 변수에 따로 값을 줘서 사용한다.

        X = 0.48f;
        Y = 0.35f;
        W = 0.04f;
        H = 0.3f;

        FrustumList = false;

        direction = (target.position - transform.position).normalized;

        distance = Vector3.Distance(target.position, transform.position);
    }

    void Update()
    {
        // ** 조건 1
        // 기존 리스트에 있는 Renderer와 현재 감지된 오브젝트의 Renderer를 비교
        // 기존 Renderer와 감지된 오브젝트의 Renderer가 동일하면 지우지 않음
        // ** 조건 2
        // 새롭게 감지된 Renderer는 리스트에 추가
        // 그리고 투명화 작업을 실행
        // ** 조건 3
        // 기존 리스트에는 존재하나 감지된 배열에 없다면
        // 기존 값으로 되돌려야 할 오브젝트

        if (FrustumList)
        {
            Camera.main.CalculateFrustumCorners(
            new Rect(X, Y, W, H),
            distance,                     // 거리. cf. Camera.main.farClipPlane: 카메라 인스펙터 뷰의 Far 값
            Camera.main.stereoActiveEye,  // 카메라 옵션
            outCorners);                  // out: outCorners에 값을 받음. 각 FrustumCorner에서부터 distance만큼의 위치 Vector 값

            Debug.DrawLine(transform.position, transform.position + direction * distance, Color.green);  // 월드 좌표

            // 투명화 범위?
            for (int i = 0; i < outCorners.Length; ++i)
                Debug.DrawLine(transform.position, transform.position + (outCorners[i] - transform.position).normalized * distance, Color.blue);
        }

        List<RaycastHit>[] hits = new List<RaycastHit>[4];
        List<Renderer> renderers = new List<Renderer>();

        // ** 모든 충돌을 감지
        for (int i = 0; i < 4; ++i)
        {
            hits[i] = Physics.RaycastAll(transform.position, outCorners[i], distance, mask).ToList();

            //** 충돌된 모든 원소들 중에 Renderer만 추출한 새로운 리스트를 생성
            // Union: 중복 제거하고 결합, AddRange: 중복 상관없이 다 합침
            //renderers.Union(hits[i].Select(hit => hit.transform.GetComponent<Renderer>()).Where(renderer => renderer != null).ToList());  // 오브젝트가 깜박이는 현상 있음
            renderers.AddRange(hits[i].Select(hit => hit.transform.GetComponent<Renderer>()).Where(renderer => renderer != null).ToList());
        }

        // ** 기존 리스트에는 포함되었지만 현재 ray에 감지된 리스트에는 없는 Renderer
        List<Renderer> extractionList = objectRenderers.Where(renderer => !renderers.Contains(renderer)).ToList();

        // ** 추출이 완료된 Renderer를 기존 알파값으로 되돌린다.
        // ** 그리고 삭제
        foreach (Renderer renderer in extractionList)
        {
            StartCoroutine(SetFadeIn(renderer));
            objectRenderers.Remove(renderer);
        }

        for (int i = 0; i < 4; ++i)
        {
            // ** hits 배열의 모든 원소를 확인
            foreach (RaycastHit hit in hits[i])
            {
                // ** ray의 충돌이 감지된 Object의 Renderer를 받아옴 
                Renderer renderer = hit.transform.GetComponent<Renderer>();

                // ** renderer == null 이라면 다음 원소를 확인
                if (renderer == null)
                    continue;

                // ** 이전 리스트 중에 동일한 원소가 포함되어 있는지 확인
                if (!objectRenderers.Contains(renderer))
                {
                    // ** 포함되지 않았다면...
                    // ** 추가
                    objectRenderers.Add(renderer);
                    StartCoroutine(SetFadeOut(renderer));
                }
            }
        }


        /*
         * ray 하나로 감지하여 투명화 script
         * 
        Debug.DrawLine(transform.position, transform.position + direction * distance, Color.green);  // 월드 좌표

        // ** 모든 충돌을 감지
        List<RaycastHit> hits = Physics.RaycastAll(transform.position, direction, distance, mask).ToList();

        //** 충돌된 모든 원소들 중에 Renderer만 추출한 새로운 리스트를 생성
        List<Renderer> renderers = hits.Select(hit => hit.transform.GetComponent<Renderer>())
            .Where(renderer => renderer != null).ToList();

        // ** 기존 리스트에는 포함되었지만 현재 ray에 감지된 리스트에는 없는 Renderer
        List<Renderer> extractionList = objectRenderers.Where(renderer => !renderers.Contains(renderer)).ToList();

        // ** 추출이 완료된 Renderer를 기존 알파값으로 되돌린다.
        // ** 그리고 삭제
        foreach (Renderer renderer in extractionList)
        {
            StartCoroutine(SetFadeIn(renderer));
            objectRenderers.Remove(renderer);
        }

        // ** hits 배열의 모든 원소를 확인
        foreach (RaycastHit hit in hits)
        {
            // ** ray의 충돌이 감지된 Object의 Renderer를 받아옴 
            Renderer renderer = hit.transform.GetComponent<Renderer>();

            // ** renderer == null 이라면 다음 원소를 확인
            if (renderer == null)
                continue;

            // ** 이전 리스트 중에 동일한 원소가 포함되어 있는지 확인
            if (!objectRenderers.Contains(renderer))
            {
                // ** 포함되지 않았다면...
                // ** 추가
                objectRenderers.Add(renderer);
                StartCoroutine(SetFadeOut(renderer));
            }

        }
*/

        #region 필요X
        //// ** objectRenderers 리스트의 모든 원소를 확인
        //foreach (Renderer element in objectRenderers)
        //{
        //    // Where(원소 => 조건): 조건에 맞는 원소를 모두 찾아 그 위치를 반환하는 람다식(hit는 foreach 문처럼 원소 의미) -> 받아온 원소들을 ToArray 사용하여 배열로 변경
        //    //RaycastHit[] hitArr = hits.Where(hit => hit.transform.GetComponent<Renderer>() == element).ToArray();

        //    if(renderers.Contains(element))
        //    {
        //        // ** 투명화된 객체를 원래 상태로 되돌림
        //        StartCoroutine(SetFadeIn(element));
        //    }
        //}
        //
        //// ** 충돌이 있다면 Renderer를 확인
        //if (renderer != null)
        //{
        //    // ** List에 이미 포함된 Renderer인지 확인
        //    if (!objectRenderers.Contains(renderer))
        //    {
        //        StartCoroutine(SetFadeOut(renderer));
        //    }
        //}

        /*
        // ** 1회라도 실행된다면 감지된 충돌이 있다는 것
        foreach (RaycastHit hit in hits)
        {
            Renderer renderer = hit.transform.GetComponent<Renderer>();

            // ** 충돌이 있다면 Renderer를 확인
            if (renderer != null)
            {
                // ** List에 이미 포함된 Renderer인지 확인
                if (!objectRenderers.Contains(renderer))
                {
                    objectRenderers.Add(renderer);  // ** 추가
                    StartCoroutine(SetFadeOut(renderer));
                }
            }
        }

        // ** 확인된 모든 Renderer의 투명화 작업을 진행
        foreach (Renderer element in objectRenderers)
            StartCoroutine(SetFadeOut(element));
         */

        // ==================================================================

        #endregion

        /*
        // MyScript!

        Debug.DrawLine(transform.position, transform.position + direction * distance, Color.green);

        // ** 모든 충돌을 감지
        RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, distance, mask);

        // hit의 Renderer를 담을 리스트
        List<Renderer> HitList = new List<Renderer>();

        // ** 1회라도 실행된다면 감지된 충돌이 있다는 것
        // ** hits 배열의 모든 원소를 확인
        foreach (RaycastHit hit in hits)
        {
            Renderer renderer = hit.transform.GetComponent<Renderer>();
            HitList.Add(renderer);

            // ** 충돌이 있다면 Renderer를 확인
            if (renderer != null)
            {
                // ** List에 이미 포함된 Renderer인지 확인 -> 조건 2에 해당
                if (!objectRenderers.Contains(renderer))
                {
                    // ** List에 추가
                    objectRenderers.Add(renderer);

                    // ** 확인된 Renderer의 투명화 작업을 진행
                    StartCoroutine(SetFadeOut(renderer));
                }
            }
        }

        // ** objectRenderers 리스트의 모든 원소를 확인
        for (int i = 0; i < objectRenderers.Count;)  // ++i 없음 주의
        {
            // ** 기존 리스트(objectRenderers)에는 존재하나 감지된 배열(hits)에 없을 때 -> 조건 3에 해당
            if (!HitList.Contains(objectRenderers[i]))
            {
                StartCoroutine(SetFadeIn(objectRenderers[i]));
                objectRenderers.Remove(objectRenderers[i]);
                // 원소 하나가 삭제되었으므로 i를 증가하지 않아도 다음 원소가 이어서 나온다.
            }
            // ** 기존 리스트(objectRenderers)와 감지된 배열(hits)에 모두 존재한다면 -> 조건 3에 해당 X
            else
                // i를 증가해 주어야 다음 원소가 이어서 나온다.
                ++i;
        }
        
        // ** ray의 충돌이 감지된 Object가 하나도 없다면
        if (hits.Length == 0)
        {
            // ** objectRenderers의 모든 원소 FadeIn 및 제거
            for (int i = 0; i < objectRenderers.Count; ++i)
                StartCoroutine(SetFadeIn(objectRenderers[i]));

            // ** 기존에 있던 Renderer를 Clear
            objectRenderers.Clear();
        }
         */
    }

    IEnumerator SetFadeOut(Renderer renderer)
    {
        // ** Color 값 변경이 가능한 Shader로 변경
        // 1. Material이 교체돼 Albedo 초기화됨
        //renderer.material = new Material(Shader.Find(path));
        // 2. Material은 그대로, Shader만 변경
        renderer.material.shader = Shader.Find(path);
        // 3. 기존 Material의 Shader가 투명도 표현이 되는 Shader라면 Shader 변경 필요 X (예: Rendering Mode가 Transparent인 Standard Shader)
        // - 오브젝트마다 Shader가 다르므로 여기서 Shader를 변경해 주는 것이 좋다.
        // - 텍스처와 특정한 색상값을 가지는 오브젝트가 있다면 1번보다 2번 방식이 낫다.

        // ** 변경된 Shader의 Color 값들 받아옴
        Color color = renderer.material.color;

        // ** color.a 값이 0.5f보다 큰 경우에만 반복
        while (0.5f < color.a)
        {
            yield return null;

            // ** Alpha(1) -= Time.deltaTime(약 0.02)
            color.a -= Mathf.Clamp(Time.deltaTime * 10.0f, 0.1f, 0.5f);

            renderer.material.color = color;
        }
    }

    IEnumerator SetFadeIn(Renderer renderer)
    {
        // ** 변경된 Shader의 Color 값들 받아옴
        Color color = renderer.material.color;

        // ** color.a 값이 1.0f보다 작은 경우에만 반복
        while (color.a < 1.0f)
        {
            yield return null;

            // ** Alpha(1) += Time.deltaTime(약 0.02)
            color.a += Mathf.Clamp(Time.deltaTime * 10.0f, 0.1f, 0.5f);

            renderer.material.color = color;
        }

        // ** Shader 되돌리기
        renderer.material.shader = Shader.Find("Standard");
    }
}
