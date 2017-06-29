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
    // ファイルのハッシュテーブル。
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

    // １つのソースファイル。
    public class CodeFile
    {
        public FileInfo fileInfo;
        private bool isSource;
        /// 除外ファイルか。
        private bool isIgnore = false;
        /// このファイルがインクルードしているファイルの総数。
        public UInt32 includeCount = 0;
        /// このファイルが直接インクルードしているファイル達。
        public CodeFileTable includeCodeFiles = new CodeFileTable();
        /// このファイルを直接インクルードしているファイル達。
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

    // アナライザ本体
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
            // 初期化
            settingFile = aSettingFile;
            onAnalyzerStartFunc = aOnAnalyzerStartFunc;
            onCalcIncludeCountStartFunc = aOnCalcIncludeCountStartFunc;
            // 設定の読み込み
            try
            {
                loadSetting(aSettingFile);
            }
            catch (Exception aExp)
            {
                throw aExp;
            }
            // 強制インクルードファイル
            foreach (FileInfo fileInfo in forceIncludeFiles)
            {
                addCodeFileIfNeccesary(fileInfo);
            }
            // ソースファイルの追加
            foreach (DirectoryInfo dirInfo in targetDirs)
            {
                addCodeFiles(dirInfo);
            }
            // 除外フラグの設定
            foreach (FileInfo fileInfo in ignoreCodeFiles)
            {
                if (!codeFiles.IsExistCodeFile(fileInfo))
                {// 存在しない
                    continue;
                }
                setIgnoreFragRecursive(codeFiles.GetCodeFile(fileInfo));
            }
            // インクルードカウントの計算
            foreach (CodeFile sourceFile in codeFiles.GetCodeFiles())
            {
                onCalcIncludeCountStartFunc(sourceFile.fileInfo);
                CodeFileTable guardTable = new CodeFileTable();
                sourceFile.includeCount = calculateTotalIncludeCount(sourceFile, guardTable);
                // 強制インクルードファイル
                foreach (FileInfo fileInfo in forceIncludeFiles)
                {
                    sourceFile.includeCount += calculateTotalIncludeCountRev(codeFiles.GetCodeFile(fileInfo), guardTable);
                }
                if (sourceFile.IsSource())
                {// 補正
                    --sourceFile.includeCount;
                }
            }
        }

        private void loadSetting(FileInfo aSettingFile)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(aSettingFile.FullName);

            // ルートチェック
            if (xmlDoc.DocumentElement.Name != "depend_analyzer_setting")
            {
                throw new Exception("ルートノード名が正しくありません。" + xmlDoc.DocumentElement.Name);
            }

            // 各要素の展開
            foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Comment)
                {// コメントはパス。
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
                    throw new Exception("不明なノード名です。 " + node.Name);
                }
            }

            // チェック
            if (targetDirs.Count == 0)
            {
                throw new Exception("ターゲットディレクトリ（target_dirs）が１つも設定されていません。");
            }
            if (headerRegexString.Length == 0)
            {
                throw new Exception("ヘッダファイルの拡張子（header_extensions）が１つも指定されていません。");
            }
            if (sourceRegexString.Length == 0)
            {
                throw new Exception("ソースファイルの拡張子（source_extensions）が１つも指定されていません。");
            }

            // 拡張子文字列の設定
            sourceRegexString = "(" + sourceRegexString + @")$";
            headerRegexString = "(" + headerRegexString + @")$";
        }

        // dirタグの処理。
        private void tagProcessNamedDir(ArrayList aTargetList, XmlNode aParentNode)
        {
            foreach (XmlNode pathNode in aParentNode.ChildNodes)
            {
                if (pathNode.NodeType == XmlNodeType.Comment)
                {// コメントはパス。
                    continue;
                }
                if (pathNode.Name != "dir")
                {
                    throw new Exception(aParentNode.Name + "に不明なノードが存在します。 " + @"'" + pathNode.Name + @"'");
                }
                aTargetList.Add(new DirectoryInfo(pathNode.InnerXml));
            }
        }

        // extensionタグの処理。
        private void tagProcessNamedExtension(ref string aTargetStr, XmlNode aParentNode)
        {
            foreach (XmlNode extNode in aParentNode.ChildNodes)
            {
                if (extNode.NodeType == XmlNodeType.Comment)
                {// コメントはパス。
                    continue;
                }
                if (extNode.Name != "extension")
                {
                    throw new Exception(aParentNode.Name + "に不明なノードが存在します。 " + @"'" + extNode.Name + @"'");
                }

                if (aTargetStr.Length != 0)
                {
                    aTargetStr += @"|";
                }
                aTargetStr += @"\" + extNode.InnerXml;
            }
        }

        // fileタグの処理。
        private void tagProcessNamedFile(ArrayList aTargetList, XmlNode aParentNode)
        {
            foreach (XmlNode pathNode in aParentNode.ChildNodes)
            {
                if (pathNode.NodeType == XmlNodeType.Comment)
                {// コメントはパス。
                    continue;
                }
                if (pathNode.Name != "file")
                {
                    throw new Exception(aParentNode.Name + "に不明なノードが存在します。 " + @"'" + pathNode.Name + @"'");
                }
                aTargetList.Add(new FileInfo(pathNode.InnerXml));
            }
        }

        // ファイルの取得。
        public ICollection GetCodeFiles()
        {
            return codeFiles.GetCodeFiles();
        }

        // ヘッダファイルの総数の取得。
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

        // ソースファイルの総数の取得。
        public uint GetSourceFilesCount()
        {
            return (uint)(GetCodeFiles().Count) - GetHeaderFilesCount();
        }

        // 指定ディレクトリ以下のコードファイルを追加。
        private void addCodeFiles(DirectoryInfo aRootDir)
        {
            // コードファイルの追加
            foreach (FileInfo fileInfo in aRootDir.GetFiles())
            {
                if (!Regex.IsMatch(fileInfo.Name, sourceRegexString)
                    && !Regex.IsMatch(fileInfo.Name, headerRegexString)
                    )
                {// 収集対象のファイルではない
                    continue;
                }
                // 必要なら追加
                addCodeFileIfNeccesary(fileInfo);
            }
            // 再帰的に
            foreach (DirectoryInfo subDirInfo in aRootDir.GetDirectories())
            {
                // 隠しフォルダはパス
                if ((subDirInfo.Attributes & FileAttributes.Hidden) != 0)
                {
                    continue;
                }
                // 探索
                addCodeFiles(subDirInfo);
            }
        }

        // インクルードされている数を数える。
        private uint calculateTotalIncludeCount(CodeFile aCodeFile, CodeFileTable aGuardTable)
        {
            if (aGuardTable.IsExistCodeFile(aCodeFile.fileInfo))
            {// 既に計算済み
                return 0;
            }
            aGuardTable.AddCodeFileRef(aCodeFile);

            uint cnt = 0;
            if (aCodeFile.IsSource())
            {// ソースファイルを数に含める
                ++cnt;
            }

            foreach (CodeFile subCodeFile in aCodeFile.referencedCodeFiles.GetCodeFiles())
            {
                cnt += calculateTotalIncludeCount(subCodeFile, aGuardTable);
            }
            return cnt;
        }

        // calculateTotalIncludeCountとは逆の方向で計算。
        private uint calculateTotalIncludeCountRev(CodeFile aCodeFile, CodeFileTable aGuardTable)
        {
            if (aGuardTable.IsExistCodeFile(aCodeFile.fileInfo))
            {// 既に計算済み
                return 0;
            }
            aGuardTable.AddCodeFileRef(aCodeFile);

            uint cnt = 0;
            if (aCodeFile.IsSource())
            {// ソースファイルを数に含める
                ++cnt;
            }

            foreach (CodeFile subCodeFile in aCodeFile.includeCodeFiles.GetCodeFiles())
            {
                cnt += calculateTotalIncludeCountRev(subCodeFile, aGuardTable);
            }
            return cnt;
        }

        // ignoreフラグをつける。
        private void setIgnoreFragRecursive(CodeFile aCodeFile)
        {
            if (aCodeFile.IsIgnore())
            {// 既にフラグがたっている。
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
            {// 既にある
                return;
            }
            CodeFile codeFile = new CodeFile(aFileInfo, Regex.IsMatch(aFileInfo.Name, sourceRegexString));
            codeFiles.AddCodeFileRef(codeFile);

            // コールバック
            onAnalyzerStartFunc(aFileInfo);

            // 解析
            Regex regex = new Regex(@"^(#include)( )+([""<])(?<1>([a-zA-Z0-9_/.]+))(["">])");
            string[] lines = File.ReadAllLines(aFileInfo.FullName);
            foreach (string line in lines)
            {
                Match match = regex.Match(line);
                if (match.Success)
                {
                    // 該当するヘッダファイルを探す
                    bool isFound = false;
                    string headerPath = match.Groups[1].ToString();
                    foreach (DirectoryInfo dirInfo in includeDirs)
                    {
                        if (!dirInfo.Exists)
                        {// ディレクトリがない
                            continue;
                        }
                        FileInfo headerFile = new FileInfo(dirInfo.FullName + @"\" + headerPath);
                        if (!headerFile.Exists)
                        {// ファイル見つからず
                            continue;
                        }
                        isFound = true;

                        // まだなければ作成
                        addCodeFileIfNeccesary(headerFile);

                        // インクルードの登録
                        codeFile.includeCodeFiles.AddCodeFileRef(getCodeFile(headerFile));
                        
                        // 参照の登録
                        getCodeFile(headerFile).referencedCodeFiles.AddCodeFileRef(codeFile);
                    }
                    if (!isFound)
                    {
                        //System.Console.WriteLine("[警告] " + codeFile.fileInfo.FullName + "がインクルードするファイル" + headerPath + "が見つかりませんでした。");
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
