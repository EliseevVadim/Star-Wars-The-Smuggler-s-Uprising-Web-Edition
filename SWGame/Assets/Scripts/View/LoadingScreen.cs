﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

namespace SWGame.View
{
    public class LoadingScreen : MonoBehaviour
    {
        private void OnEnable()
        {  
            SceneManager.LoadScene("Main", LoadSceneMode.Single);
        }
    }
}
