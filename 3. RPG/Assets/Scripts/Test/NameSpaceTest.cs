using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityStudy
{
    public class NameSpaceTest : MonoBehaviour
    {
        public GameObject ob;

        private void Start()
        {
            try
            {
                Debug.Log(ob.gameObject.name);
            }
            catch (UnassignedReferenceException ex)
            {
                Debug.Log("오브젝트가 할당되지 않았습니다.");
            }
            catch(Exception ex)
            {
                Debug.Log("알 수 없는 오류가 발생했습니다");
            }
            finally
            {
                Debug.Log("성공, 실패 둘다 실행");
            }

            throw new Exception("테스트용 오류");
        }
    }
}

