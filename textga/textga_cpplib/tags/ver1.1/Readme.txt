//------------------------------------------------------------
　TexTarga C++ Library 
　Ver.1.0
　作者：秋野ぺんぎん
　http://www.10106.net/~hoboaki/wiki/index.php?GameDevelopment%2FTexTarga%2Fver1
//------------------------------------------------------------

概要：
TexTarga画像データを扱うC++ライブラリです。

動作確認環境：
Microsoft WindowsXP SP2 and Microsoft Visual Studio 2005
MacOSX 10.5 and XCode3.0

ビルド方法：
<Windows>
Win32/textga_cpplib.slnをビルド。
dll版とstaticLibrary版が選べます。
必要なものをお使いください。
<MacOSX>
MacOSX/textga_cpplib.xcodebuildをビルド。
ppc用.a、i386用.a、UniversalBinary版.framewokが選べます。
必要なものをお使いください。

ビルド設定：
Debug - 最適化なし。アサート有効。
Develop - 最適化あり。アサート有効。
Release - 最適化あり。アサート無効。

使い方：
includeディレクトリをインクルードパスに設定してください。
textga/textga.h　をインクルードすることで全ての機能が使えます。

サンプルコード：
include/textga/textga.h　にコメントとして書いてあります。

//------------------------------------------------------------
// EOF
