#pragma once

// RichEdit50PropPage.h : Declaration of the CRichEdit50PropPage property page class.


// CRichEdit50PropPage : See RichEdit50PropPage.cpp for implementation.

class CRichEdit50PropPage : public COlePropertyPage
{
	DECLARE_DYNCREATE(CRichEdit50PropPage)
	DECLARE_OLECREATE_EX(CRichEdit50PropPage)

// Constructor
public:
	CRichEdit50PropPage();

// Dialog Data
	enum { IDD = IDD_PROPPAGE_RICHEDIT50 };

// Implementation
protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

// Message maps
protected:
	DECLARE_MESSAGE_MAP()
};

