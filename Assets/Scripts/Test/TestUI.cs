using Mono.CompilerServices.SymbolWriter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoblinGames
{
    public class TestUI : MonoBehaviour
    {
        [SerializeField] Canvas canvas;

        private void Start()
        {
            
        }


        [DebugButton]
        public void Test()
        {
            Debug.Log($"{canvas.pixelRect.width}x{canvas.pixelRect.height}");
        }
    }
}
