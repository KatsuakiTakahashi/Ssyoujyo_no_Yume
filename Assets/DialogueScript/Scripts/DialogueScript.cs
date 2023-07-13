using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DialogueScript
{
    public class DialogueScript : MonoBehaviour
    {
        // �V�i���I�t�@�C���̃t�@�C�������w�肵�Ă��������B
        private static readonly string scenarioFileName = "Scenario.txt";

        // �V�i���I�t�@�C������f�[�^�����o���܂�
        private static readonly ScenarioFile scenarioFile = new (scenarioFileName);

        // �V�i���I�t�@�C���̃f�[�^����R�}���h���X�g���쐬���܂�
        private readonly CommandList commandList = new (scenarioFile.scenario, scenarioFile.ScenarioFormat);
        private Func<string, Task> commandMethod = null;

        // ���݂̖��ߔԍ�
        private int currentCommandNum = -1;
        // �I������
        private bool isEnd = false;

        // �V�i���I�����������삷�邩�m�F�o���܂��B
        [SerializeField]
        private bool isDebugMode = false;
        // ���b�Z�[�W���Ō�܂ŕ\�����Ă��玟�̃��b�Z�[�W��\�����郂�[�h
        [SerializeField]
        private bool displayMessagesToTheEnd = true;
        private enum DialogueState
        {
            Debug,
            Skip,
            Auto,
            Normal,
        }
        private DialogueState currentDialogueState = DialogueState.Normal;

        [Serializable]
        private struct MessageSpeed
        {
            // �����̕\���Ԋu
            [SerializeField]
            public int messageInterval;
            [SerializeField]
            public int skipSpeed;
            [SerializeField]
            public int autoSpeed;
        }
        [SerializeField]
        private MessageSpeed messageSpeed;
        private bool isMessageEnd = false;
        private CancellationTokenSource messageCancellationSource;

        private GameObject dialogueGameObject = null;
        // ���b�Z�[�W�̕\������w�肵�Ă�������
        [SerializeField]
        private TextMeshProUGUI messageTextBox = null;
        // �����҂̖��O�̕\������w�肵�Ă�������
        [SerializeField]
        private TextMeshProUGUI nameTextBox = null;
        private Character[] characters = null;

        // ���߂�ǉ��������ꍇ�͂����Őݒ���s���Ă��������B
        [SerializeField]
        private List<UserCommand> userCommands = new ();

        private void Awake()
        {
            dialogueGameObject = gameObject;
            characters = FindObjectsOfType<Character>();

            Debug.Log("�V�i���I : \n" + scenarioFile.scenario);
        }

        private void Start()
        {
            if (isDebugMode)
            {
                currentDialogueState = DialogueState.Debug;
            }
            else
            {
                NextCommand();
            }
        }

        private void Update()
        {
            UpdateDialogue();
        }

        private void OnDisable()
        {
            // �^�X�N�̃L�����Z�����s���܂�
            if (messageCancellationSource != null)
            {
                messageCancellationSource.Cancel();
                messageCancellationSource = null;
            }
        }

        private void UpdateDialogue()
        {
            if (isEnd)
            {
                return;
            }

            switch (currentDialogueState)
            {
                case DialogueState.Debug:
                    NextCommand();
                    break;

                case DialogueState.Skip:
                    if (Input.GetButtonUp("Fire1"))
                    {
                        currentDialogueState = DialogueState.Normal;
                    }
                    else
                    {
                        if (isMessageEnd)
                        {
                            NextCommand();
                        }
                    }
                    break;

                case DialogueState.Auto:
                    if (Input.GetButtonUp("Fire2"))
                    {
                        currentDialogueState = DialogueState.Normal;
                    }
                    else
                    {
                        if (isMessageEnd)
                        {
                            NextCommand();
                        }
                    }
                    break;

                case DialogueState.Normal:
                    if (Input.GetButtonDown("Fire1"))
                    {
                        currentDialogueState = DialogueState.Skip;
                        if (isMessageEnd)
                        {
                            NextCommand();
                        }
                    }
                    if (Input.GetButtonDown("Fire2"))
                    {
                        currentDialogueState = DialogueState.Auto;
                        if (isMessageEnd)
                        {
                            NextCommand();
                        }
                    }
                    else if (Input.GetButtonDown("Submit"))
                    {
                        if (isMessageEnd && displayMessagesToTheEnd)
                        {
                            NextCommand();
                        }
                    }
                    break;
            }
        }

        // �n���ꂽ���O�̊֐��̃f���Q�[�g�^���쐬���܂��B
        public static Func<string, Task> CreateCommand(string commandType)
        {
            MethodInfo methodInfo = typeof(DialogueScript).GetMethod(commandType);

            if (methodInfo == null)
            {
                return null;
            }

            return (Func<string, Task>)Delegate.CreateDelegate(typeof(Func<string, Task>), null, methodInfo);
        }

        // ���̖��߂����s���܂��B
        public void NextCommand()
        {
            if (isEnd)
            {
                throw new Exception("���ׂĂ̖��߂��I����Ă��܂������߂̎��s���Ăяo����܂����B");
            }

            // �Ō�̖��߂�������I�������L�������܂��B
            if (currentCommandNum == commandList.commands.Count - 1)
            {
                isEnd = true;
                return;
            }

            // ���݂̖��ߔԍ��𑝂₵�܂��B
            currentCommandNum++;

            // ���߂̌^��ݒ肵�܂��B
            commandMethod = commandList.commands[currentCommandNum].type;
            // ���߂̈�����ݒ肵�Ď��s���܂��B
            commandMethod.DynamicInvoke(this, new string(commandList.commands[currentCommandNum].argument));

            // �Ō�̖��߂�������I�������L�������܂��B
            if (currentCommandNum == commandList.commands.Count - 1)
            {
                isEnd = true;
            }

            return;
        }

        // Dialogue�X�N���v�g���A�N�e�B�u�ɂ��܂��B
        public Task Inactive(string none)
        {
            dialogueGameObject.SetActive(false);

            return Task.CompletedTask;
        }

        // �e�L�X�g�{�b�N�X�ɕ�����\�����܂�
        public async Task Message(string messageText)
        {
            if (messageTextBox == null)
            {
                throw new ArgumentNullException("���b�Z�[�W�̕\�����ݒ肵�Ă�������");
            }

            isMessageEnd = false;

            // �����O��̃��b�Z�[�W�\�����܂��I����Ă��Ȃ��ꍇ�A�L�����Z�����đҋ@�𒆒f���܂�
            messageCancellationSource?.Cancel();
            messageCancellationSource = new CancellationTokenSource();

            CancellationToken cancellationToken = messageCancellationSource.Token;

            switch (currentDialogueState)
            {
                case DialogueState.Debug:
                    break;

                case DialogueState.Skip:
                    // �e�L�X�g�{�b�N�X����ɂ��܂�
                    messageTextBox.text = "";

                    for (int messageTextIndex = 0; messageTextIndex < messageText.Length; messageTextIndex++)
                    {
                        await Task.Delay(messageSpeed.skipSpeed / messageText.Length);
                        // �L�����Z���v�������������ꍇ�̓^�X�N���I�����܂�
                        if (cancellationToken.IsCancellationRequested)
                        {
                            return;
                        }

                        messageTextBox.text += messageText[messageTextIndex];
                    }

                    isMessageEnd = true;
                    break;

                case DialogueState.Auto:
                    if (messageSpeed.messageInterval != 0)
                    {
                        // �e�L�X�g�{�b�N�X����ɂ��܂�
                        messageTextBox.text = "";

                        for (int messageTextIndex = 0; messageTextIndex < messageText.Length; messageTextIndex++)
                        {
                            await Task.Delay(messageSpeed.messageInterval);
                            // �L�����Z���v�������������ꍇ�̓^�X�N���I�����܂�
                            if (cancellationToken.IsCancellationRequested)
                            {
                                return;
                            }

                            messageTextBox.text += messageText[messageTextIndex];
                        }

                        await Task.Delay(messageSpeed.autoSpeed);
                        // �L�����Z���v�������������ꍇ�̓^�X�N���I�����܂�
                        if (cancellationToken.IsCancellationRequested)
                        {
                            return;
                        }

                        isMessageEnd = true;
                    }
                    else
                    {
                        messageTextBox.text = messageText;

                        await Task.Delay(messageSpeed.autoSpeed);
                        // �L�����Z���v�������������ꍇ�̓^�X�N���I�����܂�
                        if (cancellationToken.IsCancellationRequested)
                        {
                            return;
                        }

                        isMessageEnd = true;
                    }
                    break;

                case DialogueState.Normal:
                    if (messageSpeed.messageInterval != 0)
                    {
                        // �e�L�X�g�{�b�N�X����ɂ��܂�
                        messageTextBox.text = "";

                        for (int messageTextIndex = 0; messageTextIndex < messageText.Length; messageTextIndex++)
                        {
                            await Task.Delay(messageSpeed.messageInterval);
                            // �L�����Z���v�������������ꍇ�̓^�X�N���I�����܂�
                            if (cancellationToken.IsCancellationRequested)
                            {
                                return;
                            }

                            messageTextBox.text += messageText[messageTextIndex];
                        }

                        isMessageEnd=true;
                    }
                    else
                    {
                        messageTextBox.text = messageText;

                        isMessageEnd = true;
                    }
                    break;
            }
        }

        // �����̕\���Ԋu��ݒ肵�܂��B
        public Task MessageInterval(string messageIntervalText)
        {
            // �n���ꂽ�����������ȊO��������ݒ肵�܂���B
            if (!int.TryParse(messageIntervalText, out int value))
            {
                throw new Exception("���b�Z�[�W�C���^�[�o���̐ݒ�l�ɐ����ł͂Ȃ����̂��n����܂����B : " + messageIntervalText);
            }

            messageSpeed.messageInterval = value;

            NextCommand();

            return Task.CompletedTask;
        }

        // �X�L�b�v���̑��x��ݒ肵�܂��B
        public Task SkipSpeed(string skipSpeedText)
        {
            // �n���ꂽ�����������ȊO��������ݒ肵�܂���B
            if (!int.TryParse(skipSpeedText, out int value))
            {
                throw new Exception("�X�L�b�v���x�̐ݒ�l�ɐ����ł͂Ȃ����̂��n����܂����B : " + skipSpeedText);
            }

            messageSpeed.skipSpeed = value;

            NextCommand();

            return Task.CompletedTask;
        }

        // �I�[�g���̑��x��ݒ肵�܂��B
        public Task AutoSpeed(string autoSpeedText)
        {
            // �n���ꂽ�����������ȊO��������ݒ肵�܂���B
            if (!int.TryParse(autoSpeedText, out int value))
            {
                throw new Exception("�I�[�g���x�̐ݒ�l�ɐ����ł͂Ȃ����̂��n����܂����B : " + autoSpeedText);
            }

            messageSpeed.autoSpeed = value;

            NextCommand();

            return Task.CompletedTask;
        }

        // �����҂̖��O��\�����܂��B
        public Task Name(string nameText)
        {
            if (nameTextBox == null)
            {
                throw new ArgumentNullException("�����҂̖��O�̕\�����ݒ肵�Ă�������");
            }

            nameTextBox.text = nameText;

            NextCommand();

            return Task.CompletedTask;
        }

        // ���b�Z�[�W�e�L�X�g�{�b�N�X�̃t�H���g�T�C�Y��ύX���܂��B
        public Task MessageFontSize(string fontSizeText)
        {
            // �n���ꂽ�����������ȊO��������ݒ肵�܂���B
            if (!int.TryParse(fontSizeText, out int value))
            {
                throw new Exception("���b�Z�[�W�t�H���g�T�C�Y�̐ݒ�l�ɐ����ł͂Ȃ����̂��n����܂����B : " + fontSizeText);
            }

            messageTextBox.fontSize = value;
            NextCommand();
            return Task.CompletedTask;
        }

        // �l�[���e�L�X�g�{�b�N�X�̃t�H���g�T�C�Y��ύX���܂��B
        public Task NameFontSize(string fontSizeText)
        {
            // �n���ꂽ�����������ȊO��������ݒ肵�܂���B
            if (!int.TryParse(fontSizeText, out int value))
            {
                throw new Exception("���b�Z�[�W�t�H���g�T�C�Y�̐ݒ�l�ɐ����ł͂Ȃ����̂��n����܂����B : " + fontSizeText);
            }

            nameTextBox.fontSize = value;
            NextCommand();
            return Task.CompletedTask;
        }

        // �L�����N�^�[�̏�Ԃ�ω������܂��B
        public Task CharacterMode(string characterNameAndModeNameText)
        {
            bool isCharacterName = true;
            string characterName = "";
            string modeName = "";

            for (int textIndex = 0; textIndex < characterNameAndModeNameText.Length; textIndex++)
            {
                if ((characterNameAndModeNameText[textIndex] == '_' || characterNameAndModeNameText[textIndex] == '�Q') && isCharacterName)
                {
                    textIndex++;
                    isCharacterName = false;
                }

                if (isCharacterName)
                {
                    characterName += characterNameAndModeNameText[textIndex];
                }
                else
                {
                    modeName += characterNameAndModeNameText[textIndex];
                }
            }

            if (characterName == "")
            {
                characterName = nameTextBox.text;
            }

            for (int characterIndex = 0; characterIndex < characters.Length; characterIndex++)
            {
                if (characters[characterIndex].CharacterName == characterName)
                {
                    characters[characterIndex].SetCharacterMode(modeName);

                    NextCommand();

                    return Task.CompletedTask;
                }
            }

            throw new ArgumentNullException("���݂��Ȃ��L�����N�^�[�������͑��݂��Ȃ����[�h�ł��B : " + characterNameAndModeNameText);
        }

        // �V�[�����ړ����܂��B
        public Task LoadScene(string sceneNameText)
        {
            SceneManager.LoadScene(sceneNameText);

            return Task.CompletedTask;
        }

        // ���[�U�[���쐬�������߂ł��B
        public Task UserCommand(string commandNameText)
        {
            // ���߂̖��̂���v�f�ԍ���ݒ肵�܂��B
            int index = userCommands.FindIndex(item => item.CommandName == commandNameText);

            if (index == -1)
            {
                throw new Exception("�쐬����ĂȂ����߂��Ԉ�������̂��n����܂����B : " + commandNameText);
            }

            userCommands[index].CommandEvent.Invoke();

            NextCommand();

            return Task.CompletedTask;
        }
    }
}