using SWGame.Entities;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class CurrentPlayerInfoPresenter : MonoBehaviour
{
    [SerializeField] private Text _creditsLabel;
    [SerializeField] private Text _prestigeLabel;
    [SerializeField] private Text _wisdomLabel;

    public void UpdateView(Player target)
    {
        _creditsLabel.text = SplitNumber(target.Credits);
        _prestigeLabel.text = SplitNumber(target.Prestige);
        _wisdomLabel.text = SplitNumber(target.WisdomPoints);
    }

    private string SplitNumber(long value)
    {
        return string.Format("{0:#,###0.#}", value);
    }
}
