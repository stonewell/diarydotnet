// RichEditControl50W.cpp : implementation file
// By Jim Dunne http://www.topjimmy.net/tjs
// topjimmy@topjimmy.net
// copyright(C)2005
// if you use all or part of this code, please give me credit somewhere :)
#include "stdafx.h"
#include "RichEditControl50W.h"

// CRichEditControl50W
IMPLEMENT_DYNAMIC(CRichEditControl50W, CWnd)
CRichEditControl50W::CRichEditControl50W()
{
	m_hInstRichEdit50W = LoadLibrary(L"msftedit.dll");
}

CRichEditControl50W::~CRichEditControl50W()
{
	//Free the MSFTEDIT.DLL library
	if(m_hInstRichEdit50W)
		FreeLibrary(m_hInstRichEdit50W);
}

BEGIN_MESSAGE_MAP(CRichEditControl50W, CWnd)
END_MESSAGE_MAP()

BOOL CRichEditControl50W::Create(DWORD dwStyle, const RECT& rect, CWnd* pParentWnd, UINT nID)
{
	if (!m_hInstRichEdit50W)
	{
		AfxMessageBox(L"MSFTEDIT.DLL Didn't Load");
	}
	//Load the MSFTEDIT.DLL library
	CWnd* pWnd = this;
	return pWnd->Create(L"RichEdit50W", NULL, dwStyle, rect, pParentWnd, nID);
}

void CRichEditControl50W::SetSel50W(long nStartChar, long nEndChar)
{
	m_crRE50W.cpMin = nStartChar;
	m_crRE50W.cpMax = nEndChar;
	SendMessage(EM_EXSETSEL, 0, (LPARAM)&m_crRE50W);
}

BOOL CRichEditControl50W::SetDefaultCharFormat50W(DWORD dwMask, COLORREF crTextColor, DWORD dwEffects, LPTSTR  szFaceName, LONG yHeight, COLORREF crBackColor)
{	//Set the text defaults.  CHARFORMAT2 m_cfStatus is declared in RichEditControl50W.h
	m_cfRE50W.cbSize = sizeof(CHARFORMAT2);
	m_cfRE50W.dwMask = dwMask ;
	m_cfRE50W.crTextColor = crTextColor;
	m_cfRE50W.dwEffects = dwEffects;
	::lstrcpy(m_cfRE50W.szFaceName, szFaceName);
	m_cfRE50W.yHeight = yHeight;
	m_cfRE50W.crBackColor = crBackColor;

	return (BOOL) SendMessage(EM_SETCHARFORMAT, 0, (LPARAM)&m_cfRE50W);
}

void CRichEditControl50W::SetTextTo50WControl(CString csText, int nSTFlags, int nSTCodepage)
{	//Set the options. SETTEXTEX m_st50W declared in RichEditControl50W.h
	m_st50W.codepage = nSTCodepage;	
	m_st50W.flags = nSTFlags;

	if (nSTCodepage != 1200)
	{
		size_t count = 0;
		wcstombs_s(&count, NULL, count, (LPCTSTR)csText, csText.GetLength());
		char * pCh = new char[count + 1];
		memset(pCh,0,count + 1);

		wcstombs_s(&count, pCh, count, (LPCTSTR)csText, csText.GetLength());

		SendMessage(EM_SETTEXTEX, (WPARAM)&m_st50W, (LPARAM)pCh);

		delete pCh;
	}
	else
	{
		SendMessage(EM_SETTEXTEX, (WPARAM)&m_st50W, (LPARAM)(LPCTSTR)csText);
	}
}

CString CRichEditControl50W::GetTextFrom50WControl(int nSTFlags, int nSTCodepage)
{	//Set the options. SETTEXTEX m_st50W declared in RichEditControl50W.h
	m_gt50W.codepage = nSTCodepage;	
	m_gt50W.flags = nSTFlags;
	m_gt50W.cb = GetTextLength(nSTCodepage);
	m_gt50W.lpDefaultChar = NULL;
	m_gt50W.lpUsedDefChar = NULL;

	if (nSTCodepage != 1200)
	{
		char * buf = new char[m_gt50W.cb + 1];
		memset(buf,0,m_gt50W.cb + 1);

		SendMessage(EM_GETTEXTEX, (WPARAM)&m_gt50W, (LPARAM)buf);
		return CString(buf);
	}
	else
	{
		wchar_t * buf = new wchar_t[m_gt50W.cb + 1];
		buf[m_gt50W.cb] = 0;

		SendMessage(EM_GETTEXTEX, (WPARAM)&m_gt50W, (LPARAM)buf);
		return CString(buf);
	}
}

DWORD CRichEditControl50W::GetTextLength(int nSTCodepage)
{
	GETTEXTLENGTHEX gtl;

	gtl.flags = GTL_NUMBYTES;
	gtl.codepage = nSTCodepage;

	return (DWORD)SendMessage(EM_GETTEXTLENGTHEX, (WPARAM)&gtl, (LPARAM)0);
}

void CRichEditControl50W::LimitText50W(int nChars)
{
	SendMessage(EM_LIMITTEXT, nChars, 0);
	SendMessage(EM_EXLIMITTEXT, 0, nChars);
}

void CRichEditControl50W::SetOptions50W(WORD wOp, DWORD dwFlags)
{
	SendMessage(EM_SETOPTIONS, (WPARAM)wOp, (LPARAM)dwFlags);
}

DWORD CRichEditControl50W::SetEventMask50W(DWORD dwEventMask)
{
	return (DWORD)SendMessage(EM_SETEVENTMASK, 0, dwEventMask);
}

void CRichEditControl50W::GetTextRange50W(int ncharrMin, int ncharrMax)
{
	//Set the CHARRANGE for the trRE50W = the characters sent by ENLINK 
	m_trRE50W.chrg.cpMin = ncharrMin;
	m_trRE50W.chrg.cpMax = ncharrMax;

	//Set the size of the character buffers, + 1 for null character
	int nLength = int((m_trRE50W.chrg.cpMax - m_trRE50W.chrg.cpMin +1));

	//create an ANSI buffer and a Unicode (Wide Character) buffer
	m_lpszChar = new CHAR[nLength];
	LPWSTR lpszWChar = new WCHAR[nLength];

	//Set the trRE50W LPWSTR character buffer = Unicode buffer
	m_trRE50W.lpstrText = lpszWChar;

	//Get the Unicode text
	SendMessage(EM_GETTEXTRANGE, 0,  (LPARAM) &m_trRE50W);  

	// Convert the Unicode RTF text to ANSI.
	WideCharToMultiByte(CP_ACP, 0, lpszWChar, -1, m_lpszChar, nLength, NULL, NULL);

	//Release buffer memory
	delete lpszWChar;

	return;
}

typedef struct
{
	char * pCh;
	size_t count;
	size_t pos;
} TAG;

DWORD CALLBACK SetStream(DWORD_PTR dwCookie, LPBYTE pbBuff, LONG cb, LONG *pcb)
{
	TAG & tag = *((TAG*)dwCookie);

	//CString msg;

	//msg.Format(L"cb=%d,pos=%d,count=%d",cb,tag.pos,tag.count);

	//AfxMessageBox(msg);

	if (tag.pos >= tag.count)
	{
		*pcb = 0;
		return 0;
	}

	if (tag.count - tag.pos < (size_t)cb)
	{
		*pcb = (LONG)(tag.count - tag.pos);
		memmove(pbBuff, &tag.pCh[tag.pos], tag.count - tag.pos);
		tag.pos = tag.count;
	}
	else
	{
		*pcb = cb;
		memmove(pbBuff, &tag.pCh[tag.pos], cb);
		tag.pos += cb;
	}

	return 0;
}

DWORD CALLBACK GetStream(DWORD_PTR dwCookie, LPBYTE pbBuff, LONG cb, LONG *pcb)
{
	CString & csText = *((CString *)dwCookie);

	for(long i=0;i<cb;i++)
		csText.AppendChar(pbBuff[i]);

	*pcb = cb;
	return 0;
}

void CRichEditControl50W::SetRtfTextTo50WControl(CString csText, int nSTFlags)
{
	size_t count = 0;
	wcstombs_s(&count, NULL, count, (LPCTSTR)csText, csText.GetLength());
	char * pCh = new char[count + 1];
	memset(pCh,0,count + 1);

	wcstombs_s(&count, pCh, count, (LPCTSTR)csText, csText.GetLength());

	TAG tag;

	tag.pCh = pCh;
	tag.count = count;
	tag.pos = 0;

	//CString msg;

	//msg.Format(L"%d", count);

	//AfxMessageBox(msg);

	EDITSTREAM es;
	es.dwCookie = (DWORD_PTR)&tag;
	es.pfnCallback = &SetStream;

	SendMessage(EM_STREAMIN, (WPARAM)nSTFlags, (LPARAM)&es);

	delete pCh;
}

CString CRichEditControl50W::GetRtfTextFrom50WControl(int nSTFlags)
{
	CString csText;
	EDITSTREAM es;
	es.dwCookie = (DWORD_PTR)&csText;
	es.pfnCallback = &GetStream;

	SendMessage(EM_STREAMOUT, (WPARAM)nSTFlags, (LPARAM)&es);

	return csText;
}

void CRichEditControl50W::Initialize()
{
	// Send Initialization messages to the window
	LimitText50W(-1);	//Set the control to accept the maximum amount of text

	DWORD REOptions = (ECO_AUTOVSCROLL | 
		ECO_AUTOHSCROLL |
		ECO_NOHIDESEL | 
		ECO_SAVESEL | 
		ECO_SELECTIONBAR |
		ECO_WANTRETURN);

	//Set other options
	SetOptions50W(	
		ECOOP_OR,	//The type of operation
		REOptions );	//Options

	//Set the contol to automatically detect URLs
	SendMessage( EM_AUTOURLDETECT, TRUE, 0);  

	//Set the event masks for the rich edit control
	SetEventMask50W(
		ENM_SELCHANGE | ENM_LINK 	//New event mask for the rich edit control
		);

	//Set the default character formatting...
	//see RichEditControl50W.cpp for function definition
	SetDefaultCharFormat50W(	
		CFM_COLOR | CFM_BOLD | CFM_SIZE | CFM_FACE | CFM_BACKCOLOR,	//Mask options 
		GetSysColor(COLOR_WINDOWTEXT),			//Text Color	
		!CFE_BOLD,			//Text Effects
		L"",		//Font name
		200,				//Font yHeight
		GetSysColor(COLOR_WINDOW));	//Font background color
}