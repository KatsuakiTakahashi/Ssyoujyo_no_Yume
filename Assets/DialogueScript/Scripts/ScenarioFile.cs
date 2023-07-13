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
            if (scenarioFileName == "")
            {
                throw new System.Exception("�V�i���I�t�@�C���̃t�@�C�������w�肵�Ă��������B");
            }

            // �t�@�C��������t�@�C���p�X���쐬���܂��B�t�@�C���p�X��ύX�������ꍇ�͂�����ύX���Ă��������B
            string scenarioFilePath = "Assets/DialogueScript/Scenarios/" + scenarioFileName;

            if (!File.Exists(scenarioFilePath))
            {
                throw new System.Exception("�w�肳�ꂽ�t�@�C����������܂���ł����B\n�t�@�C���� : " + scenarioFileName + "\n�t�@�C���p�X : " + scenarioFilePath);
            }

            // �t�@�C���l�[������g���q�𔻕ʂ��Đݒ肵�܂��B
            switch (Path.GetExtension(scenarioFileName))
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

            // �t�@�C���̃o�C�g������G���R�[�f�B���O�𔻕ʂ��܂��B
            Encoding encoding;
            byte[] scenarioFileBytes = File.ReadAllBytes(scenarioFilePath);
            using (MemoryStream stream = new MemoryStream(scenarioFileBytes))
            {
                using (StreamReader reader = new StreamReader(stream, detectEncodingFromByteOrderMarks: true))
                {
                    // StreamReader��CurrentEncoding�v���p�e�B�ŃG���R�[�f�B���O���擾
                    encoding = reader.CurrentEncoding;
                }
            }

            // scenario�ɃV�i���I�t�@�C���̒��g�����܂��B
            using (StreamReader streamReader = new(scenarioFilePath, encoding))
            {
                this.scenario = streamReader.ReadToEnd();
                streamReader.Close();
            }
        }
    }
}