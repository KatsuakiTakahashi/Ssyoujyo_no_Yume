using UnityEngine;
using UnityEngine.Events;

namespace DialogueScript
{
    [System.Serializable]
    public class UserCommand
    {
        // ���߂̖��O��ݒ肵�Ă������B
        [SerializeField]
        private string commandName;
        public string CommandName { get => commandName; set => commandName = value; }

        // ���߂̏�����ݒ肵�Ă��������B
        [SerializeField]
        private UnityEvent commandEvent;
        public UnityEvent CommandEvent { get => commandEvent; set => commandEvent = value; }
    }
}