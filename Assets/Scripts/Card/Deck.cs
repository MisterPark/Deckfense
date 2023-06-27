using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoblinGames
{
    public class Deck : MonoBehaviour
    {
        private List<int> cards = new List<int>();

        public IReadOnlyList<int> Cards { get { return cards; } }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
