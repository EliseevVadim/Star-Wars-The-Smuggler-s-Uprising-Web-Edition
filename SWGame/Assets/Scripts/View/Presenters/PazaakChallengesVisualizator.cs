﻿using SWGame.Activities.PazaakTools.OnlinePazaak;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SWGame.View.Presenters
{
    public class PazaakChallengesVisualizator : MonoBehaviour
    {
        [SerializeField] private Transform _container;
        [SerializeField] private PazaakChallengePresenter _challengeTemplate;

        public void Render (List<PazaakChallenge> challenges)
        {
            foreach(Transform challenge in _container)
            {
                Destroy(challenge.gameObject);
            }
            challenges.ForEach(challenge =>
            {
                PazaakChallengePresenter cell = Instantiate(_challengeTemplate, _container);
                cell.Visualize(challenge);
            });
        }
    }
}
