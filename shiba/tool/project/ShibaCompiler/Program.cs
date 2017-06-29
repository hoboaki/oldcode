using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// �v���O�����{�́B
    /// </summary>
    class Program
    {
        //------------------------------------------------------------
        // �G���g���[�|�C���g�B
        static int Main(string[] aArgs)
        {
            // test
            if (false)
            {
                LexerTest.Execute();
                ParserTest.Execute();
                SymbolTreeTest.Execute();
            }

            // ��������
            if (aArgs.Length != 2)
            {// �������Ⴄ�B
                System.Console.Error.WriteLine("useage: ShibaCompiler.exe src_list_file_path output_dir_path");
                return -1;
            }
            string srcListFilePath = aArgs[0];
            string outputDirPath = aArgs[1];

            // �R���p�C��
            if (!Compiler.Execute(srcListFilePath, outputDirPath))
            {// ���s�B
                return -1;
            }

            // ����
            return 0;
        }
    }
}
