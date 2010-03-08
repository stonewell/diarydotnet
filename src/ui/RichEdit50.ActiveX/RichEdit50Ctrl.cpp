// RichEdit50Ctrl.cpp : Implementation of the CRichEdit50Ctrl ActiveX Control class.

#include "stdafx.h"
#include "RichEdit50.h"
#include "RichEdit50Ctrl.h"
#include "RichEdit50PropPage.h"


#ifdef _DEBUG
#define new DEBUG_NEW
#endif


IMPLEMENT_DYNCREATE(CRichEdit50Ctrl, COleControl)



// Message map

BEGIN_MESSAGE_MAP(CRichEdit50Ctrl, COleControl)
	ON_MESSAGE(OCM_COMMAND, &CRichEdit50Ctrl::OnOcmCommand)
	ON_MESSAGE(WM_COMMAND, &CRichEdit50Ctrl::OnOcmCommand)
	ON_OLEVERB(AFX_IDS_VERB_PROPERTIES, OnProperties)
	ON_WM_CREATE()
	ON_WM_SIZE()
END_MESSAGE_MAP()



// Dispatch map

BEGIN_DISPATCH_MAP(CRichEdit50Ctrl, COleControl)
	DISP_FUNCTION_ID(CRichEdit50Ctrl, "AboutBox", DISPID_ABOUTBOX, AboutBox, VT_EMPTY, VTS_NONE)
	DISP_STOCKPROP_TEXT()
	DISP_PROPERTY_EX_ID(CRichEdit50Ctrl, "Rtf", dispidRTFText, GetRTFText, SetRTFText, VT_BSTR)
	DISP_PROPERTY_EX_ID(CRichEdit50Ctrl, "SelectRtf", dispidSelectRTF, GetSelectRTF, SetSelectRTF, VT_BSTR)
	DISP_PROPERTY_EX_ID(CRichEdit50Ctrl, "Modified", dispidModified, GetModified, SetModified, VT_BOOL)
	DISP_FUNCTION_ID(CRichEdit50Ctrl, "CanPaste", dispidCanPaste, CanPaste, VT_BOOL, VTS_I4)
	DISP_FUNCTION_ID(CRichEdit50Ctrl, "Paste", dispidPaste, Paste, VT_EMPTY, VTS_I4)
	DISP_PROPERTY_EX_ID(CRichEdit50Ctrl, "Text", dispidText, GetText, SetText, VT_BSTR)
	DISP_PROPERTY_EX_ID(CRichEdit50Ctrl, "SelectText", dispidSelectText, GetSelectText, SetSelectText, VT_BSTR)
	DISP_FUNCTION_ID(CRichEdit50Ctrl, "Cut", dispidCut, Cut, VT_EMPTY, VTS_NONE)
	DISP_FUNCTION_ID(CRichEdit50Ctrl, "Copy", dispidCopy, Copy, VT_EMPTY, VTS_NONE)
END_DISPATCH_MAP()



// Event map

BEGIN_EVENT_MAP(CRichEdit50Ctrl, COleControl)
END_EVENT_MAP()



// Property pages

// TODO: Add more property pages as needed.  Remember to increase the count!
BEGIN_PROPPAGEIDS(CRichEdit50Ctrl, 1)
PROPPAGEID(CRichEdit50PropPage::guid)
END_PROPPAGEIDS(CRichEdit50Ctrl)



// Initialize class factory and guid
IMPLEMENT_OLECREATE_EX(CRichEdit50Ctrl, "RICHEDIT50.RichEdit50Ctrl.1",
					   0x47aefb7f, 0xe430, 0x48db, 0xa7, 0x2, 0x60, 0xf3, 0xc0, 0x45, 0xd6, 0x12)

// Type library ID and version
IMPLEMENT_OLETYPELIB(CRichEdit50Ctrl, _tlid, _wVerMajor, _wVerMinor)

// Interface IDs
const IID BASED_CODE IID_DRichEdit50 =
{ 0x323BB75D, 0xFB0F, 0x4A26, { 0x98, 0xB8, 0x7E, 0xF, 0x36, 0x3E, 0x9, 0x64 } };
const IID BASED_CODE IID_DRichEdit50Events =
{ 0x5F1F819F, 0x698F, 0x4A36, { 0x8B, 0x38, 0x40, 0x5E, 0xFB, 0xB9, 0x82, 0x27 } };

// Control type information

static const DWORD BASED_CODE _dwRichEdit50OleMisc =
OLEMISC_ACTIVATEWHENVISIBLE |
OLEMISC_SETCLIENTSITEFIRST |
OLEMISC_INSIDEOUT |
OLEMISC_CANTLINKINSIDE |
OLEMISC_RECOMPOSEONRESIZE;

IMPLEMENT_OLECTLTYPE(CRichEdit50Ctrl, IDS_RICHEDIT50, _dwRichEdit50OleMisc)



// CRichEdit50Ctrl::CRichEdit50CtrlFactory::UpdateRegistry -
// Adds or removes system registry entries for CRichEdit50Ctrl

BOOL CRichEdit50Ctrl::CRichEdit50CtrlFactory::UpdateRegistry(BOOL bRegister)
{
	// TODO: Verify that your control follows apartment-model threading rules.
	// Refer to MFC TechNote 64 for more information.
	// If your control does not conform to the apartment-model rules, then
	// you must modify the code below, changing the 6th parameter from
	// afxRegApartmentThreading to 0.

	if (bRegister)
		return AfxOleRegisterControlClass(
		AfxGetInstanceHandle(),
		m_clsid,
		m_lpszProgID,
		IDS_RICHEDIT50,
		IDB_RICHEDIT50,
		afxRegApartmentThreading,
		_dwRichEdit50OleMisc,
		_tlid,
		_wVerMajor,
		_wVerMinor);
	else
		return AfxOleUnregisterClass(m_clsid, m_lpszProgID);
}



// CRichEdit50Ctrl::CRichEdit50Ctrl - Constructor

CRichEdit50Ctrl::CRichEdit50Ctrl()
{
	InitializeIIDs(&IID_DRichEdit50, &IID_DRichEdit50Events);
	// TODO: Initialize your control's instance data here.
}



// CRichEdit50Ctrl::~CRichEdit50Ctrl - Destructor

CRichEdit50Ctrl::~CRichEdit50Ctrl()
{
}



// CRichEdit50Ctrl::OnDraw - Drawing function

void CRichEdit50Ctrl::OnDraw(
							 CDC* pdc, const CRect& rcBounds, const CRect& rcInvalid)
{
	COleControl::OnDraw(pdc, rcBounds, rcInvalid);
}



// CRichEdit50Ctrl::DoPropExchange - Persistence support

void CRichEdit50Ctrl::DoPropExchange(CPropExchange* pPX)
{
	ExchangeVersion(pPX, MAKELONG(_wVerMinor, _wVerMajor));
	COleControl::DoPropExchange(pPX);

	// TODO: Call PX_ functions for each persistent custom property.
}



// CRichEdit50Ctrl::OnResetState - Reset control to default state

void CRichEdit50Ctrl::OnResetState()
{
	COleControl::OnResetState();  // Resets defaults found in DoPropExchange

	// TODO: Reset any other control state here.
}



// CRichEdit50Ctrl::AboutBox - Display an "About" box to the user

void CRichEdit50Ctrl::AboutBox()
{
	CDialog dlgAbout(IDD_ABOUTBOX_RICHEDIT50);
	dlgAbout.DoModal();
}



// CRichEdit50Ctrl::PreCreateWindow - Modify parameters for CreateWindowEx

BOOL CRichEdit50Ctrl::PreCreateWindow(CREATESTRUCT& cs)
{
	return COleControl::PreCreateWindow(cs);
}

// CRichEdit50Ctrl::IsSubclassedControl - This is a subclassed control

BOOL CRichEdit50Ctrl::IsSubclassedControl()
{
	return FALSE;
}

// CRichEdit50Ctrl::OnOcmCommand - Handle command messages

LRESULT CRichEdit50Ctrl::OnOcmCommand(WPARAM wParam, LPARAM lParam)
{
#ifdef _WIN32
	WORD wNotifyCode = HIWORD(wParam);
#else
	WORD wNotifyCode = HIWORD(lParam);
#endif

	AfxMessageBox(L"Hello");
	return 0;
}

// CRichEdit50Ctrl message handlers

int CRichEdit50Ctrl::OnCreate(LPCREATESTRUCT lpCreateStruct)
{
	if (COleControl::OnCreate(lpCreateStruct) == -1)
		return -1;

	//Set Options for the Rich Edit Control 
	DWORD w_RichEd = WS_CHILD | WS_VISIBLE | WS_CLIPSIBLINGS | WS_VSCROLL | ES_AUTOVSCROLL | WS_HSCROLL | ES_AUTOHSCROLL | ES_MULTILINE;

	//Create Rich Edit Control to fill view
	if (!m_REControl50W.Create(w_RichEd, CRect(0, 0, 0, 0), this, 100))
	{
		TRACE0("Failed to create view window\n");
		return -1;
	}

	// Send Initialization messages to the window
	m_REControl50W.LimitText50W(-1);	//Set the control to accept the maximum amount of text

	DWORD REOptions = (ECO_AUTOVSCROLL | 
		ECO_AUTOHSCROLL |
		ECO_NOHIDESEL | 
		ECO_SAVESEL | 
		ECO_SELECTIONBAR |
		ECO_WANTRETURN);

	//Set other options
	m_REControl50W.SetOptions50W(	
		ECOOP_OR,	//The type of operation
		REOptions );	//Options

	//Set the contol to automatically detect URLs
	m_REControl50W.SendMessage( EM_AUTOURLDETECT, TRUE, 0);  

	//Set the event masks for the rich edit control
	m_REControl50W.SetEventMask50W(
		ENM_SELCHANGE | ENM_LINK 	//New event mask for the rich edit control
		);

	//Set the default character formatting...
	//see RichEditControl50W.cpp for function definition
	m_REControl50W.SetDefaultCharFormat50W(	
		CFM_COLOR | CFM_BOLD | CFM_SIZE | CFM_FACE | CFM_BACKCOLOR,	//Mask options 
		RGB(0,0,0),			//Text Color	
		!CFE_BOLD,			//Text Effects
		L"",		//Font name
		200,				//Font yHeight
		GetSysColor(COLOR_WINDOW));	//Font background color

	SetModifiedFlag(false);

	return 0;
}

void CRichEdit50Ctrl::OnSize(UINT nType, int cx, int cy)
{
	COleControl::OnSize(nType, cx, cy);

	::SetWindowPos(m_REControl50W.m_hWnd, 
		0,0,0,cx,cy,
		SWP_NOZORDER | SWP_NOMOVE);
}

BSTR CRichEdit50Ctrl::GetRTFText()
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	CString strResult = 
		m_REControl50W.GetRtfTextFrom50WControl(SF_RTF);

	return strResult.AllocSysString(); 
}

void CRichEdit50Ctrl::SetRTFText(LPCTSTR newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	m_REControl50W.SetRtfTextTo50WControl(newVal,
		SF_RTF);

	SetModifiedFlag();
}

BSTR CRichEdit50Ctrl::GetSelectRTF(void)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	CString strResult = 
		m_REControl50W.GetRtfTextFrom50WControl(SF_RTF | SFF_SELECTION);

	return strResult.AllocSysString();
}

void CRichEdit50Ctrl::SetSelectRTF(LPCTSTR newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	m_REControl50W.SetRtfTextTo50WControl(newVal,
		SF_RTF | SFF_SELECTION);

	SetModifiedFlag();
}

VARIANT_BOOL CRichEdit50Ctrl::GetModified(void)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	return IsModified();
}

void CRichEdit50Ctrl::SetModified(VARIANT_BOOL newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	SetModifiedFlag(newVal);

	if (newVal == false)
	{
		m_Text = 
			m_REControl50W.GetRtfTextFrom50WControl(SF_RTF);
	}
}

VARIANT_BOOL CRichEdit50Ctrl::CanPaste(LONG formatId)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	LRESULT l = m_REControl50W.SendMessage(EM_CANPASTE, (WPARAM)formatId, (LPARAM)0);

	return l != 0;
}

void CRichEdit50Ctrl::Paste(LONG formatId)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	REPASTESPECIAL rsp;
	rsp.dwAspect = DVASPECT_CONTENT;
	rsp.dwParam = 0;

	m_REControl50W.SendMessage(EM_PASTESPECIAL, (WPARAM)formatId, (LPARAM)&rsp);

	SetModifiedFlag();
}

BSTR CRichEdit50Ctrl::GetText(void)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	CString strResult = m_REControl50W.GetTextFrom50WControl(GT_DEFAULT, 1200);

	return strResult.AllocSysString();
}

void CRichEdit50Ctrl::SetText(LPCTSTR newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	m_REControl50W.SetTextTo50WControl(newVal,	//Write the text in m_csMessage to the RE Control 
		ST_DEFAULT,	//	SETTEXT flags value
		1200);

	SetModifiedFlag();
}

BSTR CRichEdit50Ctrl::GetSelectText(void)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	CString strResult = m_REControl50W.GetTextFrom50WControl(GT_SELECTION, 1200);

	return strResult.AllocSysString();
}

void CRichEdit50Ctrl::SetSelectText(LPCTSTR newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	m_REControl50W.SetTextTo50WControl(newVal,	//Write the text in m_csMessage to the RE Control 
		ST_SELECTION,	//	SETTEXT flags value
		1200);

	SetModifiedFlag();
}

void CRichEdit50Ctrl::Cut(void)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	m_REControl50W.SendMessage(WM_CUT);

	SetModifiedFlag();
}

void CRichEdit50Ctrl::Copy(void)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	m_REControl50W.SendMessage(WM_COPY);
}

BOOL CRichEdit50Ctrl::OnNotify(WPARAM wParam, LPARAM lParam, LRESULT* pResult)
{
	NMHDR * pNMHDR = (NMHDR *)lParam;

	if (pNMHDR->code == EN_SELCHANGE)
	{
		CString strResult = 
			m_REControl50W.GetRtfTextFrom50WControl(SF_RTF);

		SetModifiedFlag(strResult.Compare(m_Text) != 0);
	}

	return COleControl::OnNotify(wParam, lParam, pResult);
}
