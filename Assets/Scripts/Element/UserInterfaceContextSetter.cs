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
            if (contextText == null)
            {
                contextText = GetComponent<TMPro.TextMeshProUGUI>();
                if (contextText == null)
                {
                    Debug.LogError("TextMeshProUGUI Text is not found in UserInterfaceContextSetter. Game Object: " + gameObject.name);
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