using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Collections.Specialized;

namespace DependAnalyzer
{
    // �t�@�C���̃n�b�V���e�[�u���B
    public class CodeFileTable
    {
        private Hashtable table = new Hashtable();

        public ICollection GetCodeFiles()
        {
            return table.Values;
        }

        public bool IsExistCodeFile(FileInfo aFileInfo)
        {
            return GetCodeFile(aFileInfo) != null;
        }

        public CodeFile GetCodeFile(FileInfo aFileInfo)
        {
            return (CodeFile)table[aFileInfo.FullName];
        }

        public void AddCodeFileRef(CodeFile aFile)
        {
            table[aFile.fileInfo.FullName] = aFile;
        }
    };

    // �P�̃\�[�X�t�@�C���B
    public class CodeFile
    {
        public FileInfo fileInfo;
        private bool isSource;
        /// ���O�t�@�C�����B
        private bool isIgnore = false;
        /// ���̃t�@�C�����C���N���[�h���Ă���t�@�C���̑����B
        public UInt32 includeCount = 0;
        /// ���̃t�@�C�������ڃC���N���[�h���Ă���t�@�C���B�B
        public CodeFileTable includeCodeFiles = new CodeFileTable();
        /// ���̃t�@�C���𒼐ڃC���N���[�h���Ă���t�@�C���B�B
        public CodeFileTable referencedCodeFiles = new CodeFileTable();

        public CodeFile(FileInfo aFileInfo,bool aIsSource)
        {
            isSource = aIsSource;
            fileInfo = aFileInfo;
        }

        public bool IsHeader()
        {
            return !isSource;
        }

        public bool IsSource()
        {
            return isSource;
        }

        public bool IsIgnore()
        {
            return isIgnore;
        }

        public void SetIgnore()
        {
            isIgnore = true;
        }
    };

    // �A�i���C�U�{��
    public class Analyzer
    {
        public delegate void CodeFileCallBack(FileInfo file);

        private CodeFileTable codeFiles = new CodeFileTable();
        private ArrayList targetDirs = new ArrayList();
        private ArrayList includeDirs = new ArrayList();
        private ArrayList forceIncludeFiles = new ArrayList();
        private ArrayList ignoreCodeFiles = new ArrayList();
        private string sourceRegexString = @"";
        private string headerRegexString = @"";
        public FileInfo settingFile;
        private CodeFileCallBack onAnalyzerStartFunc;
        private CodeFileCallBack onCalcIncludeCountStartFunc;

        public Analyzer(FileInfo aSettingFile
            , CodeFileCallBack aOnAnalyzerStartFunc
            , CodeFileCallBack aOnCalcIncludeCountStartFunc
            )
        {
            // ������
            settingFile = aSettingFile;
            onAnalyzerStartFunc = aOnAnalyzerStartFunc;
            onCalcIncludeCountStartFunc = aOnCalcIncludeCountStartFunc;
            // �ݒ�̓ǂݍ���
            try
            {
                loadSetting(aSettingFile);
            }
            catch (Exception aExp)
            {
                throw aExp;
            }
            // �����C���N���[�h�t�@�C��
            foreach (FileInfo fileInfo in forceIncludeFiles)
            {
                addCodeFileIfNeccesary(fileInfo);
            }
            // �\�[�X�t�@�C���̒ǉ�
            foreach (DirectoryInfo dirInfo in targetDirs)
            {
                addCodeFiles(dirInfo);
            }
            // ���O�t���O�̐ݒ�
            foreach (FileInfo fileInfo in ignoreCodeFiles)
            {
                if (!codeFiles.IsExistCodeFile(fileInfo))
                {// ���݂��Ȃ�
                    continue;
                }
                setIgnoreFragRecursive(codeFiles.GetCodeFile(fileInfo));
            }
            // �C���N���[�h�J�E���g�̌v�Z
            foreach (CodeFile sourceFile in codeFiles.GetCodeFiles())
            {
                onCalcIncludeCountStartFunc(sourceFile.fileInfo);
                CodeFileTable guardTable = new CodeFileTable();
                sourceFile.includeCount = calculateTotalIncludeCount(sourceFile, guardTable);
                // �����C���N���[�h�t�@�C��
                foreach (FileInfo fileInfo in forceIncludeFiles)
                {
                    sourceFile.includeCount += calculateTotalIncludeCountRev(codeFiles.GetCodeFile(fileInfo), guardTable);
                }
                if (sourceFile.IsSource())
                {// �␳
                    --sourceFile.includeCount;
                }
            }
        }

        private void loadSetting(FileInfo aSettingFile)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(aSettingFile.FullName);

            // ���[�g�`�F�b�N
            if (xmlDoc.DocumentElement.Name != "depend_analyzer_setting")
            {
                throw new Exception("���[�g�m�[�h��������������܂���B" + xmlDoc.DocumentElement.Name);
            }

            // �e�v�f�̓W�J
            foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Comment)
                {// �R�����g�̓p�X�B
                    continue;
                }
                if (node.Name == "target_dirs")
                {
                    tagProcessNamedDir(targetDirs, node);
                }
                else if (node.Name == "include_dirs")
                {
                    tagProcessNamedDir(includeDirs, node);
                }
                else if (node.Name == "source_extensions")
                {
                    tagProcessNamedExtension(ref sourceRegexString, node);
                }
                else if (node.Name == "header_extensions")
                {
                    tagProcessNamedExtension(ref headerRegexString, node);
                }
                else if (node.Name == "ignore_code_files")
                {
                    tagProcessNamedFile(ignoreCodeFiles, node);
                }
                else if (node.Name == "force_include_files")
                {
                    tagProcessNamedFile(forceIncludeFiles, node);
                }
                else
                {
                    throw new Exception("�s���ȃm�[�h���ł��B " + node.Name);
                }
            }

            // �`�F�b�N
            if (targetDirs.Count == 0)
            {
                throw new Exception("�^�[�Q�b�g�f�B���N�g���itarget_dirs�j���P���ݒ肳��Ă��܂���B");
            }
            if (headerRegexString.Length == 0)
            {
                throw new Exception("�w�b�_�t�@�C���̊g���q�iheader_extensions�j���P���w�肳��Ă��܂���B");
            }
            if (sourceRegexString.Length == 0)
            {
                throw new Exception("�\�[�X�t�@�C���̊g���q�isource_extensions�j���P���w�肳��Ă��܂���B");
            }

            // �g���q������̐ݒ�
            sourceRegexString = "(" + sourceRegexString + @")$";
            headerRegexString = "(" + headerRegexString + @")$";
        }

        // dir�^�O�̏����B
        private void tagProcessNamedDir(ArrayList aTargetList, XmlNode aParentNode)
        {
            foreach (XmlNode pathNode in aParentNode.ChildNodes)
            {
                if (pathNode.NodeType == XmlNodeType.Comment)
                {// �R�����g�̓p�X�B
                    continue;
                }
                if (pathNode.Name != "dir")
                {
                    throw new Exception(aParentNode.Name + "�ɕs���ȃm�[�h�����݂��܂��B " + @"'" + pathNode.Name + @"'");
                }
                aTargetList.Add(new DirectoryInfo(pathNode.InnerXml));
            }
        }

        // extension�^�O�̏����B
        private void tagProcessNamedExtension(ref string aTargetStr, XmlNode aParentNode)
        {
            foreach (XmlNode extNode in aParentNode.ChildNodes)
            {
                if (extNode.NodeType == XmlNodeType.Comment)
                {// �R�����g�̓p�X�B
                    continue;
                }
                if (extNode.Name != "extension")
                {
                    throw new Exception(aParentNode.Name + "�ɕs���ȃm�[�h�����݂��܂��B " + @"'" + extNode.Name + @"'");
                }

                if (aTargetStr.Length != 0)
                {
                    aTargetStr += @"|";
                }
                aTargetStr += @"\" + extNode.InnerXml;
            }
        }

        // file�^�O�̏����B
        private void tagProcessNamedFile(ArrayList aTargetList, XmlNode aParentNode)
        {
            foreach (XmlNode pathNode in aParentNode.ChildNodes)
            {
                if (pathNode.NodeType == XmlNodeType.Comment)
                {// �R�����g�̓p�X�B
                    continue;
                }
                if (pathNode.Name != "file")
                {
                    throw new Exception(aParentNode.Name + "�ɕs���ȃm�[�h�����݂��܂��B " + @"'" + pathNode.Name + @"'");
                }
                aTargetList.Add(new FileInfo(pathNode.InnerXml));
            }
        }

        // �t�@�C���̎擾�B
        public ICollection GetCodeFiles()
        {
            return codeFiles.GetCodeFiles();
        }

        // �w�b�_�t�@�C���̑����̎擾�B
        public uint GetHeaderFilesCount()
        {
            uint cnt = 0;
            foreach (CodeFile file in GetCodeFiles())
            {
                if (file.IsHeader())
                {
                    ++cnt;
                }
            }
            return cnt;
        }

        // �\�[�X�t�@�C���̑����̎擾�B
        public uint GetSourceFilesCount()
        {
            return (uint)(GetCodeFiles().Count) - GetHeaderFilesCount();
        }

        // �w��f�B���N�g���ȉ��̃R�[�h�t�@�C����ǉ��B
        private void addCodeFiles(DirectoryInfo aRootDir)
        {
            // �R�[�h�t�@�C���̒ǉ�
            foreach (FileInfo fileInfo in aRootDir.GetFiles())
            {
                if (!Regex.IsMatch(fileInfo.Name, sourceRegexString)
                    && !Regex.IsMatch(fileInfo.Name, headerRegexString)
                    )
                {// ���W�Ώۂ̃t�@�C���ł͂Ȃ�
                    continue;
                }
                // �K�v�Ȃ�ǉ�
                addCodeFileIfNeccesary(fileInfo);
            }
            // �ċA�I��
            foreach (DirectoryInfo subDirInfo in aRootDir.GetDirectories())
            {
                // �B���t�H���_�̓p�X
                if ((subDirInfo.Attributes & FileAttributes.Hidden) != 0)
                {
                    continue;
                }
                // �T��
                addCodeFiles(subDirInfo);
            }
        }

        // �C���N���[�h����Ă��鐔�𐔂���B
        private uint calculateTotalIncludeCount(CodeFile aCodeFile, CodeFileTable aGuardTable)
        {
            if (aGuardTable.IsExistCodeFile(aCodeFile.fileInfo))
            {// ���Ɍv�Z�ς�
                return 0;
            }
            aGuardTable.AddCodeFileRef(aCodeFile);

            uint cnt = 0;
            if (aCodeFile.IsSource())
            {// �\�[�X�t�@�C���𐔂Ɋ܂߂�
                ++cnt;
            }

            foreach (CodeFile subCodeFile in aCodeFile.referencedCodeFiles.GetCodeFiles())
            {
                cnt += calculateTotalIncludeCount(subCodeFile, aGuardTable);
            }
            return cnt;
        }

        // calculateTotalIncludeCount�Ƃ͋t�̕����Ōv�Z�B
        private uint calculateTotalIncludeCountRev(CodeFile aCodeFile, CodeFileTable aGuardTable)
        {
            if (aGuardTable.IsExistCodeFile(aCodeFile.fileInfo))
            {// ���Ɍv�Z�ς�
                return 0;
            }
            aGuardTable.AddCodeFileRef(aCodeFile);

            uint cnt = 0;
            if (aCodeFile.IsSource())
            {// �\�[�X�t�@�C���𐔂Ɋ܂߂�
                ++cnt;
            }

            foreach (CodeFile subCodeFile in aCodeFile.includeCodeFiles.GetCodeFiles())
            {
                cnt += calculateTotalIncludeCountRev(subCodeFile, aGuardTable);
            }
            return cnt;
        }

        // ignore�t���O������B
        private void setIgnoreFragRecursive(CodeFile aCodeFile)
        {
            if (aCodeFile.IsIgnore())
            {// ���Ƀt���O�������Ă���B
                return;
            }
            aCodeFile.SetIgnore();

            foreach (CodeFile subCodeFile in aCodeFile.includeCodeFiles.GetCodeFiles())
            {
                setIgnoreFragRecursive(subCodeFile);
            }
        }

        private bool isExistCodeFile(FileInfo aFileInfo)
        {
            return codeFiles.IsExistCodeFile(aFileInfo);
        }

        private void addCodeFileIfNeccesary(FileInfo aFileInfo)
        {
            if (isExistCodeFile(aFileInfo))
            {// ���ɂ���
                return;
            }
            CodeFile codeFile = new CodeFile(aFileInfo, Regex.IsMatch(aFileInfo.Name, sourceRegexString));
            codeFiles.AddCodeFileRef(codeFile);

            // �R�[���o�b�N
            onAnalyzerStartFunc(aFileInfo);

            // ���
            Regex regex = new Regex(@"^(#include)( )+([""<])(?<1>([a-zA-Z0-9_/.]+))(["">])");
            string[] lines = File.ReadAllLines(aFileInfo.FullName);
            foreach (string line in lines)
            {
                Match match = regex.Match(line);
                if (match.Success)
                {
                    // �Y������w�b�_�t�@�C����T��
                    bool isFound = false;
                    string headerPath = match.Groups[1].ToString();
                    foreach (DirectoryInfo dirInfo in includeDirs)
                    {
                        if (!dirInfo.Exists)
                        {// �f�B���N�g�����Ȃ�
                            continue;
                        }
                        FileInfo headerFile = new FileInfo(dirInfo.FullName + @"\" + headerPath);
                        if (!headerFile.Exists)
                        {// �t�@�C�������炸
                            continue;
                        }
                        isFound = true;

                        // �܂��Ȃ���΍쐬
                        addCodeFileIfNeccesary(headerFile);

                        // �C���N���[�h�̓o�^
                        codeFile.includeCodeFiles.AddCodeFileRef(getCodeFile(headerFile));
                        
                        // �Q�Ƃ̓o�^
                        getCodeFile(headerFile).referencedCodeFiles.AddCodeFileRef(codeFile);
                    }
                    if (!isFound)
                    {
                        //System.Console.WriteLine("[�x��] " + codeFile.fileInfo.FullName + "���C���N���[�h����t�@�C��" + headerPath + "��������܂���ł����B");
                    }
                }
            }
        }

        private CodeFile getCodeFile(FileInfo aFileInfo)
        {
            return codeFiles.GetCodeFile(aFileInfo);
        }

    };

}
