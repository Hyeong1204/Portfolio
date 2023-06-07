using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OpenCellType
{
    Empty = 0,
    Number1,
    Number2,
    Number3,
    Number4,
    Number5,
    Number6,
    Number7,
    Number8,
    Mine_NotFound,      // 못 찾은 지뢰
    Mine_Explosion,      // 밟은 지뢰
    Mine_Mistake         // 지뢰가 아닌데 지뢰라고 표시한 경우
}

public enum CloseCellType
{
    Close = 0,
    Close_Press,
    Question,
    Question_Press,
    Flag
}
