// -*- C++ -*- generated by wxGlade 0.5 on Wed Sep 19 23:11:56 2007 from C:\Documents and Settings\akiharu\My Documents\projects\glpicviewer\data\wxGlade\MainWindow.wxg

#include <wx/wx.h>
#include <wx/image.h>
// begin wxGlade: ::dependencies
// end wxGlade


#ifndef GUIMAINWINDOW_H
#define GUIMAINWINDOW_H


class GUIMainWindow: public wxFrame {
public:
    // begin wxGlade: GUIMainWindow::ids
    // end wxGlade

    GUIMainWindow(wxWindow* parent, int id, const wxString& title, const wxPoint& pos=wxDefaultPosition, const wxSize& size=wxDefaultSize, long style=wxDEFAULT_FRAME_STYLE);

private:
    // begin wxGlade: GUIMainWindow::methods
    void set_properties();
    void do_layout();
    // end wxGlade

protected:
    // begin wxGlade: GUIMainWindow::attributes
    wxMenuBar* menubar_;
    // end wxGlade
}; // wxGlade: end class


#endif // GUIMAINWINDOW_H