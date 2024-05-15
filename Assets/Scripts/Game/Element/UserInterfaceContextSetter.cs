using ProjectMIL.Utlity;
using UnityEngine;

namespace ProjectMIL.Game.Element
{
    public class UserInterfaceContextSetter : MonoBehaviour
    {
        [SerializeField] private int contextID;

        private TMPro.TextMeshProUGUI contextText;
        private ContextHandler contextHandler;

        public void SetUp(ContextHandler contextHandler)
        {
            if (contextHandler == null)
            {
                contextText = GetComponent<TMPro.TextMeshProUGUI>();
                if (contextText == null)
                {
                    Debug.LogError("Context Text is not found. Game Object: " + gameObject.name);
                    return;
                }
            }

            if (contextHandler == null)
            {
                Debug.LogError("Context Handler cannot be null.");
                contextText.text = "NULL";
                return;
            }

            this.contextHandler = contextHandler;
            contextText.text = contextHandler.GetContext(contextID);
        }
    }
}