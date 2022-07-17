using static Define;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeManager : MonoSingleton<FadeManager>
{
    private RectTransform _topBar = null;

    private RectTransform _bottomBar = null;

    private float _hideBarY = 0f;
    private void Start()
    {
        _topBar = FadeParent.Find(FADECHILDS.TOPBAR.ToString()).GetComponent<RectTransform>();
        _bottomBar = FadeParent.Find(FADECHILDS.BOTTOMBAR.ToString()).GetComponent<RectTransform>();

        _hideBarY = Mathf.Abs(_topBar.anchoredPosition.y);

    }

    public void ShowBar(bool isShow)
    {
        if (isShow)
        {
            _topBar.DOAnchorPosY(_hideBarY, 1f);
            _bottomBar.DOAnchorPosY(-_hideBarY, 1f);
        }
        else{
            _topBar.DOAnchorPosY(-_hideBarY, 1f);
            _bottomBar.DOAnchorPosY(_hideBarY, 1f);
        }
    }


}
