using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DialogueScript
{
    public class DialogueScript : MonoBehaviour
    {
        // �V�i���I�t�@�C���̃t�@�C�������w�肵�Ă��������B
        private static readonly string scenarioFileName = "Scenario.txt";

        // �V�i���I�t�@�C������f�[�^�����o���܂�
        private static readonly ScenarioFile scenarioFile = new(scenarioFileName);

        // �V�i���I�t�@�C���̃f�[�^����R�}���h���X�g���쐬���܂�
        private readonly CommandList commandList = new(scenarioFile.scenario, scenarioFile.ScenarioFormat);
        private Func<string, Task> commandMethod = null;

        // ���݂̖��ߔԍ�
        private int currentCommandNum = -1;
        // �I������
        private bool isEnd = false;

        // �V�i���I�����������삷�邩�m�F�o���܂��B
        [SerializeField]
        private bool isDebugMode = false;

        private enum DialogueState
        {
            Debug,
            Hide,
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
        // ���ꂼ��̃��b�Z�[�W���x��ݒ肵�Ă��������B
        [SerializeField]
        private MessageSpeed messageSpeed;

        // ��̃��b�Z�[�W�̏I������ł�
        private bool isMessageEnd = false;
        private CancellationTokenSource messageCancellationSource;

        [SerializeField]
        private GameObject dialogueGameObject = null;
        // ���b�Z�[�W�̕\������w�肵�Ă�������
        [SerializeField]
        private GameObject messageTextBox = null;
        private TextMeshProUGUI messageTextArea = null;

        // �����҂̖��O�̕\������w�肵�Ă�������
        [SerializeField]
        private GameObject nameTextBox = null;
        private TextMeshProUGUI nameTextArea = null;

        // ���b�Z�[�W���I�������Ƃ��ɕ\�������A�C�R���ł��B
        [SerializeField]
        private GameObject messageEndIcon = null;

        private Character[] characters = null;

        // ���߂�ǉ��������ꍇ�͂����Őݒ���s���Ă��������B
        [SerializeField]
        private List<UserCommand> userCommands = new();

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

        private void Awake()
        {
            messageTextArea = messageTextBox.GetComponentInChildren<TextMeshProUGUI>();
            nameTextArea = nameTextBox.GetComponentInChildren<TextMeshProUGUI>();

            characters = FindObjectsOfType<Character>();

            if (messageEndIcon != null && messageEndIcon.activeSelf)
            {
                messageEndIcon.SetActive(false);
            }
        }

        private void Start()
        {
            Debug.Log("�V�i���I : \n" + scenarioFile.scenario);

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

        // ���s���̏������s���܂�
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

                case DialogueState.Hide:
                    UpdateHideState();
                    break;

                case DialogueState.Skip:
                    UpdateSkipState();
                    break;

                case DialogueState.Auto:
                    UpdateAutoState();
                    break;

                case DialogueState.Normal:
                    UpdateNormalState();
                    break;
            }
        }

        private void UpdateHideState()
        {
            if (Input.anyKeyDown)
            {
                nameTextBox.SetActive(true);
                messageTextBox.SetActive(true);
                currentDialogueState = DialogueState.Normal;
            }
        }

        private void UpdateSkipState()
        {
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
        }

        private void UpdateAutoState()
        {
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
        }

        private void UpdateNormalState()
        {
            if (Input.GetButtonDown("Cancel"))
            {
                nameTextBox.SetActive(false);
                messageTextBox.SetActive(false);
                currentDialogueState = DialogueState.Hide;
            }
            else if (Input.GetButtonDown("Fire1"))
            {
                currentDialogueState = DialogueState.Skip;
                NextCommand();
            }
            else if (Input.GetButtonDown("Fire2"))
            {
                currentDialogueState = DialogueState.Auto;
            }
            else if (Input.GetButtonDown("Submit"))
            {
                if (isMessageEnd)
                {
                    NextCommand();
                }
            }
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

        private void OnDisable()
        {
            TaskCancell();
        }

        // Dialogue�X�N���v�g�̃A�N�e�B�u��Ԃ�ݒ肵�܂��B
        public Task SetActive(string boolText)
        {
            if (boolText == "true" || boolText == "True")
            {
                dialogueGameObject.SetActive(true);
                enabled = true;
                NextCommand();
            }
            else if (boolText == "false" || boolText == "False")
            {
                dialogueGameObject.SetActive(false);
                TaskCancell();
                enabled = false;
            }
            else
            {
                throw new Exception("<SetActive>�ɑ����e�L�X�g�� true �������� false ��ݒ肵�Ă��������B");
            }

            return Task.CompletedTask;
        }

        private void TaskCancell()
        {
            // �^�X�N�̃L�����Z�����s���܂�
            if (messageCancellationSource != null)
            {
                messageCancellationSource.Cancel();
                messageCancellationSource = null;
            }

            if (currentDialogueState != DialogueState.Debug)
            {
                currentDialogueState = DialogueState.Normal;
            }
        }

        //////////////////////////////�ȉ��_�C�A���O�̂��߂̃R�}���h//////////////////////////////

        // �e�L�X�g�{�b�N�X�ɕ�����\�����܂�
        public async Task Message(string messageText)
        {
            if (messageTextBox == null)
            {
                throw new ArgumentNullException("���b�Z�[�W�̕\�����ݒ肵�Ă�������");
            }

            SetMessageEnd(false);

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
                    messageTextArea.text = "";

                    for (int messageTextIndex = 0; messageTextIndex < messageText.Length; messageTextIndex++)
                    {
                        await Task.Delay(messageSpeed.skipSpeed / messageText.Length);
                        // �L�����Z���v�������������ꍇ�̓^�X�N���I�����܂�
                        if (cancellationToken.IsCancellationRequested)
                        {
                            return;
                        }

                        messageTextArea.text += messageText[messageTextIndex];
                    }

                    SetMessageEnd(true);
                    break;

                case DialogueState.Auto:
                    if (messageSpeed.messageInterval != 0)
                    {
                        // �e�L�X�g�{�b�N�X����ɂ��܂�
                        messageTextArea.text = "";

                        for (int messageTextIndex = 0; messageTextIndex < messageText.Length; messageTextIndex++)
                        {
                            await Task.Delay(messageSpeed.messageInterval);
                            // �L�����Z���v�������������ꍇ�̓^�X�N���I�����܂�
                            if (cancellationToken.IsCancellationRequested)
                            {
                                return;
                            }

                            messageTextArea.text += messageText[messageTextIndex];
                        }

                        await Task.Delay(messageSpeed.autoSpeed);
                        // �L�����Z���v�������������ꍇ�̓^�X�N���I�����܂�
                        if (cancellationToken.IsCancellationRequested)
                        {
                            return;
                        }

                        SetMessageEnd(true);
                    }
                    else
                    {
                        messageTextArea.text = messageText;

                        await Task.Delay(messageSpeed.autoSpeed);
                        // �L�����Z���v�������������ꍇ�̓^�X�N���I�����܂�
                        if (cancellationToken.IsCancellationRequested)
                        {
                            return;
                        }

                        SetMessageEnd(true);
                    }
                    break;

                case DialogueState.Normal:
                    if (messageSpeed.messageInterval != 0)
                    {
                        // �e�L�X�g�{�b�N�X����ɂ��܂�
                        messageTextArea.text = "";

                        for (int messageTextIndex = 0; messageTextIndex < messageText.Length; messageTextIndex++)
                        {
                            await Task.Delay(messageSpeed.messageInterval);
                            // �L�����Z���v�������������ꍇ�̓^�X�N���I�����܂�
                            if (cancellationToken.IsCancellationRequested)
                            {
                                return;
                            }

                            messageTextArea.text += messageText[messageTextIndex];
                        }

                        SetMessageEnd(true);
                    }
                    else
                    {
                        messageTextArea.text = messageText;

                        SetMessageEnd(true);
                    }
                    break;
            }
        }

        private void SetMessageEnd(bool setBool)
        {
            isMessageEnd = setBool;
            if (messageEndIcon != null && currentDialogueState != DialogueState.Hide)
            {
                messageEndIcon.SetActive(setBool);
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

            nameTextArea.text = nameText;

            NextCommand();

            return Task.CompletedTask;
        }

        // ���b�Z�[�W�e�L�X�g�{�b�N�X�̕\����\����ݒ肵�܂��B
        public Task MessageTextBox(string boolText)
        {
            if (boolText == "true" || boolText == "True")
            {
                messageTextBox.SetActive(true);
            }
            else if (boolText == "false" || boolText == "False")
            {
                messageTextBox.SetActive(false);
            }
            else
            {
                throw new Exception("<MessageTextBox>�ɑ����e�L�X�g�� true �������� false ��ݒ肵�Ă��������B");
            }

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

            messageTextArea.fontSize = value;
            NextCommand();
            return Task.CompletedTask;
        }

        // �l�[���e�L�X�g�{�b�N�X�̕\����\����ݒ肵�܂��B
        public Task NameTextBox(string boolText)
        {
            if (boolText == "true" || boolText == "True")
            {
                nameTextBox.SetActive(true);
            }
            else if (boolText == "false" || boolText == "False")
            {
                nameTextBox.SetActive(false);
            }
            else
            {
                throw new Exception("<MessageTextBox>�ɑ����e�L�X�g�� true �������� false ��ݒ肵�Ă��������B");
            }

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

            nameTextArea.fontSize = value;
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
                characterName = nameTextArea.text;
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