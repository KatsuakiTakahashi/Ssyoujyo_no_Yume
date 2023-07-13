using System;
using System.Threading.Tasks;

namespace DialogueScript
{
    public class Command
    {
        // ���߂̎��
        public readonly Func<string, Task> type = null;
        // ���߂̈���
        public readonly string argument = "";

        // ���߂̎�ނƈ�����ݒ肵�ăR�}���h���쐬���܂�
        public Command(string commandType, string commandArgument)
        {
            if (DialogueScript.CreateCommand(commandType) == null)
            {
                throw new ArgumentNullException("���݂��Ȃ����߂ł��B : <" + commandType + '>' + commandArgument);
            }

            type = DialogueScript.CreateCommand(commandType);
            argument = commandArgument.Trim();
        }
    }
}