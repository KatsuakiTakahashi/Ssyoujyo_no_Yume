using System.Collections.Generic;

namespace DialogueScript
{
    public class CommandList
    {
        // ���߂̎�ނƖ��߂̈����̃f�[�^�������X�g�ł�
        public readonly List<Command> commands = new();

        public CommandList(string scenario, ScenarioFile.Format format)
        {
            if (scenario == "")
            {
                throw new System.Exception("�V�i���I�̒��g�����݂��܂���B");
            }

            // ���ߌ^�̐ݒ蒆���ǂ���
            bool isCommandType = true;

            // ���݂̖��ߌ^�Ƃ��̈���
            string currentCommandType = "";
            string currentCommandArgument = "";

            // �擪�����̋󔒕������폜���R�}���h���쐬�����̂����X�g�Ɋi�[���܂�
            void CommandSet()
            {
                commands.Add(new Command(currentCommandType, currentCommandArgument));

                currentCommandType = "";
                currentCommandArgument = "";
            }

            switch (format)
            {
                // �V�i���I�t�@�C���̊g���q��.txt�������ꍇ�̔���
                case ScenarioFile.Format.txt:
                    // �V�i���I�̍Ō�܂Ŕ���
                    for (int scenarioIndex = 0; scenarioIndex < scenario.Length; scenarioIndex++)
                    {
                        // �擪��','��������܂�
                        if (scenarioIndex == 0 && scenario[scenarioIndex] == '<')
                        {
                            scenarioIndex++;
                        }

                        // ���ߒ��ł͂Ȃ��Ƃ���'<'�������疽�߂��ǂ������ʊJ�n
                        if (scenario[scenarioIndex] == '<' && isCommandType != true)
                        {
                            if (!(scenarioIndex == scenario.Length - 1))
                            {
                                scenarioIndex++;
                            }

                            // �A������'<'�������Ă����ꍇ���߂ł͂Ȃ��Ɣ���
                            if (scenario[scenarioIndex] != '<')
                            {
                                isCommandType = true;

                                // ���߂�ݒ肵���߂Ɩ��߂̈�����������
                                CommandSet();
                            }
                        }
                        // ���ߒ��������Ƃ���'>'�������疽�ߏI���Ɣ���
                        else if (scenario[scenarioIndex] == '>' && isCommandType == true)
                        {
                            if (!(scenarioIndex == scenario.Length - 1))
                            {
                                scenarioIndex++;
                                isCommandType = false;
                            }
                        }
                        // ���ߒ��������疽�߂ɓ��́B
                        if (isCommandType)
                        {
                            currentCommandType += scenario[scenarioIndex];
                        }
                        // ���ߒ�����Ȃ������疽�߂̈����ɓ��́B
                        else
                        {
                            currentCommandArgument += scenario[scenarioIndex];
                        }

                        // �V�i���I�̍Ō�܂œ��B�����疽�߂�ݒ�
                        if (scenarioIndex == scenario.Length - 1)
                        {
                            // ���߂�ݒ肵���߂Ɩ��߂̈�����������
                            CommandSet();
                        }
                    }
                    break;

                // �V�i���I�t�@�C���̊g���q��.csv�������ꍇ�̔���
                case ScenarioFile.Format.csv:
                    // �V�i���I�̍Ō�܂Ŕ���
                    for (int scenarioIndex = 0; scenarioIndex < scenario.Length; scenarioIndex++)
                    {
                        // ���s�R�[�h��LF�������ꍇ�̔���
                        if (scenario[scenarioIndex] == '\n')
                        {
                            if (isCommandType)
                            {
                                isCommandType = false;
                            }
                            else
                            {
                                isCommandType = true;

                                // ���߂�ݒ肵���߂Ɩ��߂̈�����������
                                CommandSet();
                            }
                        }

                        // ���s�R�[�h��CRLF�������ꍇ�̔���
                        if (scenario[scenarioIndex] == '\r')
                        {
                            if (!(scenarioIndex == scenario.Length - 1))
                            {
                                scenarioIndex++;
                            }

                            if (scenario[scenarioIndex] == '\n')
                            {
                                if (isCommandType)
                                {
                                    isCommandType = false;
                                }
                                else
                                {
                                    isCommandType = true;

                                    // ���߂�ݒ肵���߂Ɩ��߂̈�����������
                                    CommandSet();
                                }
                            }
                        }

                        // ','���������ɖ��߂����ʊJ�n
                        if (scenario[scenarioIndex] == ',')
                        {
                            if (!(scenarioIndex == scenario.Length - 1))
                            {
                                scenarioIndex++;
                            }

                            // �A������','�����ĂȂ��Ƃ��͖��ߔ����ύX
                            if (scenario[scenarioIndex] != ',')
                            {
                                if (isCommandType)
                                {
                                    isCommandType = false;
                                }
                                else
                                {
                                    isCommandType = true;

                                    // ���߂�ݒ肵���߂Ɩ��߂̈�����������
                                    CommandSet();
                                }
                            }
                        }

                        // ���ߒ��������疽�߂ɓ��́B
                        if (isCommandType)
                        {
                            currentCommandType += scenario[scenarioIndex];
                        }
                        // ���ߒ�����Ȃ������疽�߂̈����ɓ��́B
                        else
                        {
                            currentCommandArgument += scenario[scenarioIndex];
                        }

                        // �V�i���I�̍Ō�܂œ��B�����疽�߂�ݒ�
                        if (scenarioIndex == scenario.Length - 1)
                        {
                            // ���߂�ݒ肵���߂Ɩ��߂̈�����������
                            CommandSet();
                        }
                    }
                    break;
            }
        }
    }
}
