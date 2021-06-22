using Glidrive.GlidingDisk;
using Glidrive.UI;
using Glidrive.Util;
using UniRx;
using UnityEngine;

public class DiskEnergyPresenter : MonoBehaviour
{
    [SerializeField] private GlidingDiskEnergy model;

    private DiskEnergyView view;

    private void Start()
    {
        var maxEnergy = model.RemainEnergy.Value;
        var minEnergy = 0.0f;
        view = GetComponent<DiskEnergyView>();
        
        model.RemainEnergy
            .Subscribe(remainEnergy =>
            {
                var fillAmount = Util.Map(remainEnergy,minEnergy,maxEnergy,0.0f,1.0f);
                view.UpdateFillAmount(fillAmount);
            });
    }
}