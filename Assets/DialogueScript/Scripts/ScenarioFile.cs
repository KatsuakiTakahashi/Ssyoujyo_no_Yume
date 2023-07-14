using UnityEngine;
using System.IO;
using System.Text;

namespace DialogueScript
{
    public class ScenarioFile
    {
        // �V�i���I�t�@�C���̒��g�ł��B
        public readonly string scenario = "";

        // �V�i���I�t�@�C���̊g���q�̏��ł��B
        public enum Format
        {
            txt,
            csv,
        }
        // ���݂̃V�i���I�t�@�C���̊g���q�ł��B
        public Format ScenarioFormat { get; private set; } = Format.txt;

        // �t�@�C���l�[�����玩���I�Ɋg���q�𔻕ʂ����g��scenario�ɓ���܂��B
        public ScenarioFile(string scenarioFileName)
        {
            if (string.IsNullOrEmpty(scenarioFileName))
            {
                throw new System.Exception("�V�i���I�t�@�C���̃t�@�C�������w�肵�Ă��������B");
            }

            // �t�@�C���p�X���쐬���܂��B
            string scenarioFilePath = Path.Combine(Application.streamingAssetsPath, scenarioFileName);

            if (!File.Exists(scenarioFilePath))
            {
                throw new System.Exception("�w�肳�ꂽ�t�@�C����������܂���ł����B\n�t�@�C���� : " + scenarioFileName + "\n�t�@�C���p�X : " + scenarioFilePath);
            }

            // �t�@�C���l�[������g���q�𔻕ʂ��Đݒ肵�܂��B
            string extension = Path.GetExtension(scenarioFileName);
            switch (extension)
            {
                case ".txt":
                    ScenarioFormat = Format.txt;
                    break;
                case ".csv":
                    ScenarioFormat = Format.csv;
                    break;
                default:
                    throw new System.Exception("�V�i���I�t�@�C���͊g���q��.txt��.csv�̂��̂��g�p���Ă��������B");
            }

            // �V�i���I�t�@�C���̒��g���o�C�i���f�[�^�Ƃ��ēǂݍ��݂܂��B
            byte[] scenarioFileBytes = File.ReadAllBytes(scenarioFilePath);
            using (MemoryStream stream = new MemoryStream(scenarioFileBytes))
            {
                // �t�@�C���̃o�C�g������G���R�[�f�B���O�𔻕ʂ��܂��B
                Encoding encoding;
                using (StreamReader reader = new StreamReader(stream, detectEncodingFromByteOrderMarks: true))
                {
                    // StreamReader��CurrentEncoding�v���p�e�B�ŃG���R�[�f�B���O���擾
                    encoding = reader.CurrentEncoding;
                }

                // �o�C�i���f�[�^���G���R�[�f�B���O�Ńf�R�[�h���A������Ƃ��Ċi�[���܂��B
                this.scenario = encoding.GetString(scenarioFileBytes);
            }
        }
    }
}