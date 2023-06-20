using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoblinGames
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class DebugButton : Attribute
    {
        public DebugButton()
        {

        }
    }
}


