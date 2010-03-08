#pragma once

// RichEdit50Ctrl.h : Declaration of the CRichEdit50Ctrl ActiveX Control class.
#include "RichEditControl50W.h"

// CRichEdit50Ctrl : See RichEdit50Ctrl.cpp for implementation.

class CRichEdit50Ctrl : public COleControl
{
	DECLARE_DYNCREATE(CRichEdit50Ctrl)

// Constructor
public:
	CRichEdit50Ctrl();

// Overrides
public:
	virtual void OnDraw(CDC* pdc, const CRect& rcBounds, const CRect& rcInvalid);
	virtual BOOL PreCreateWindow(CREATESTRUCT& cs);
	virtual void DoPropExchange(CPropExchange* pPX);
	virtual void OnResetState();

// Implementation
protected:
	~CRichEdit50Ctrl();

	DECLARE_OLECREATE_EX(CRichEdit50Ctrl)    // Class factory and guid
	DECLARE_OLETYPELIB(CRichEdit50Ctrl)      // GetTypeInfo
	DECLARE_PROPPAGEIDS(CRichEdit50Ctrl)     // Property page IDs
	DECLARE_OLECTLTYPE(CRichEdit50Ctrl)		// Type name and misc status

	// Subclassed control support
	BOOL IsSubclassedControl();
	LRESULT OnOcmCommand(WPARAM wParam, LPARAM lParam);

// Message maps
	DECLARE_MESSAGE_MAP()

// Dispatch maps
	DECLARE_DISPATCH_MAP()

	afx_msg void AboutBox();

// Event maps
	DECLARE_EVENT_MAP()

// Dispatch and event IDs
public:
	enum {
		dispidCopy = 9L,
		dispidCut = 8L,
		dispidSelectText = 7,
		dispidText = 6,
		dispidPaste = 5L,
		dispidCanPaste = 4L,
		dispidModified = 3,
		dispidSelectRTF = 2,
		dispidRTFText = 1
	};

private:
	CRichEditControl50W m_REControl50W;
	CString m_Text;
public:
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg void OnSize(UINT nType, int cx, int cy);
protected:
	BSTR GetRTFText();
	void SetRTFText(LPCTSTR newVal);
	BSTR GetSelectRTF(void);
	void SetSelectRTF(LPCTSTR newVal);
	VARIANT_BOOL GetModified(void);
	void SetModified(VARIANT_BOOL newVal);
	VARIANT_BOOL CanPaste(LONG formatId);
	void Paste(LONG formatId);
	BSTR GetText(void);
	void SetText(LPCTSTR newVal);
	BSTR GetSelectText(void);
	void SetSelectText(LPCTSTR newVal);
	void Cut(void);
	void Copy(void);
//	virtual BOOL OnNotify(WPARAM wParam, LPARAM lParam, LRESULT* pResult);
	virtual BOOL OnNotify(WPARAM wParam, LPARAM lParam, LRESULT* pResult);
};

