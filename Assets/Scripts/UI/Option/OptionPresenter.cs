using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class OptionPresenter : UIGroup
{
    [SerializeField]private OptionMenuView _menuView;
    [SerializeField]private OptionExitView _exitView;
    [SerializeField]private OptionSettingView _settingView;
    [SerializeField]private OptionIntroductionView _introductionView;
    [SerializeField] private Image _bgImage;
    public override void Initialize()
    {
        base.Initialize();
        MessageBroker.Default.Receive<OptionEnable>()
            .Subscribe(_ =>
            {
                //オプション表示時の初期化処理
                _bgImage.gameObject.SetActive(true);
                _exitView.gameObject.SetActive(false);
                _settingView.gameObject.SetActive(false);
                _introductionView.gameObject.SetActive(false);
                gameObject.SetActive(true);
                _menuView.Show().Forget();

            }).AddTo(this);

        MessageBroker.Default.Receive<OptionDisable>()
            .Subscribe(_ =>
            {
                //オプションキャンセル時の終了処理
                gameObject.SetActive(false);
            }).AddTo(this);

        //メニューのボタン初期化
        _menuView.OnExitButtonPressed
            .Subscribe(_ =>
            {
                _menuView.Hide().Forget();
                _exitView.Show().Forget();
            }).AddTo(this);

        _menuView.OnOptionCancel
            .Subscribe(_ =>
            {
                MessageBroker.Default.Publish(new OptionDisable());

            }).AddTo(this);

        //終了ボタンの初期化
        _exitView.OnExitConfirmed
            .Subscribe(_=>
            {
                //ゲーム終了処理
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            }).AddTo(this);
    }
}

public class OptionEnable { }
public class OptionDisable { }