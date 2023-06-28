using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace InformationDisplayers
{

    public class DisplayKillCount : MonoBehaviour
    {
        [SerializeField] TMP_Text m_Text;
        private void OnEnable()
        {
            m_Text ??= GetComponent<TMP_Text>();
            GameManager.Instance.OnEnemyCountChanged.AddListener(UpdateCounter);
        }
        private void OnDisable()
        {
            GameManager.Instance.OnEnemyCountChanged.RemoveListener(UpdateCounter);
        }

        public void UpdateCounter(int value)
        {
            Debug.Log("Counter updated");
            m_Text.text = value.ToString();
        }
    }

}