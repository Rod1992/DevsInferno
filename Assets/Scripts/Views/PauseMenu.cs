using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class PauseMenu : MonoBehaviour
{
    //to avoid input spam
    float coolDownTime = 0.0f;
    const float MAXCOOLDOWNTIME = 0.2f;

    [SerializeField]
    GameObject rootGameObject = null; 
    [SerializeField]
    Button buttonSaveLogs = null;

    void Start()
    {
        Observable.EveryUpdate().Where(x => coolDownTime > 0f).Subscribe(x => { coolDownTime -= Time.deltaTime;}).AddTo(this);
        Observable.EveryUpdate().Where(x => Input.GetKeyUp(KeyCode.Escape)).Subscribe(x => { ChangeState(); }).AddTo(this);
        buttonSaveLogs.OnClickAsObservable().Subscribe(x => { Game.Instance.SaveLogs(); }).AddTo(this);
    }

    private void ChangeState()
    {
        //avoid input spam
        if (coolDownTime > 0f)
            return;

        if (Game.Instance.IsSystemOnPause)
        {
            rootGameObject.SetActive(false);
            EventBus.TriggerEvent(EventMessage.Unpause, null);
        }
        else
        {
            rootGameObject.SetActive(true);
            EventBus.TriggerEvent(EventMessage.Pause, null);
        }

        coolDownTime = MAXCOOLDOWNTIME;
    }

}
