//------------------------------------------------------------
　TexTarga Command-line Tool
　Ver.1.0
　作者：秋野ぺんぎん
　http://www.10106.net/~hoboaki/wiki/index.php?GameDevelopment%2FTexTarga%2Fver1
//------------------------------------------------------------

概要：
TexTarga画像データを扱うコマンドラインツールです。

動作確認環境：
Microsoft WindowsXP SP2 and Microsoft Visual Studio 2005
MacOSX 10.5 and XCode3.0

ビルド方法：
<Windows>
Win32/textga_cmd.slnをビルド。
textga.exeが生成されます。
<MacOSX>
MacOSX/textga_cmd.xcodebuildをビルド。
UniversalBinaryのtextgaが生成されます。

ビルド設定：
Debug - 最適化なし。アサート有効。
Develop - 最適化あり。アサート有効。
Release - 最適化あり。アサート無効。

使い方：
textga helpを実行して確認してください。
Webにコマンド例が書かれているので、そちらを確認するのもいいかもしれません。

Thanks：
squish - DXT Compression Library
http://www.sjbrown.co.uk/?code=squish

//------------------------------------------------------------
// EOF
