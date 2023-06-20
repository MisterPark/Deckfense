using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoblinGames
{
    public class Test : MonoBehaviour
    {
        [SerializeField] private Variable<int> testHp;

        private int internalVariable;
        public int InternalVariable { get { return internalVariable; } }


        private int internalVariable2;
        public int InternalVariable2 { get { return internalVariable2; } set { internalVariable2 = value; } }


        private void Start()
        {
            testHp.Value -= 10;

            //SceneController.LoadScene("InGame");

        }

        private void OnEnable()
        {
            testHp.OnValueChanged.AddListener(OnTestHpChanged);
        }

        private void OnDisable()
        {
            testHp.OnValueChanged.RemoveListener(OnTestHpChanged);
        }

        private void OnTestHpChanged(int hp)
        {
            Debug.Log($"체력이 {hp}로 변경됨.");
        }
    }
}
