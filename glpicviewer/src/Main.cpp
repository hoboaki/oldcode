/**
 * @file
 * @brief �A�v���P�[�V�����̊J�n�ӏ����L�q����B
 */

//----------------------------------------------------------------
#include "GUIMainWindow.h"

//----------------------------------------------------------------
class MyApp: public wxApp {
public:
    bool OnInit();
};

IMPLEMENT_APP(MyApp)

//----------------------------------------------------------------
bool MyApp::OnInit()
{
    wxInitAllImageHandlers();
    GUIMainWindow* frame = new GUIMainWindow(NULL, wxID_ANY, wxEmptyString);
    SetTopWindow(frame);
    frame->Show();
    return true;
}

//----------------------------------------------------------------
// EOF
