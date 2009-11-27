// RichEdit50PropPage.cpp : Implementation of the CRichEdit50PropPage property page class.

#include "stdafx.h"
#include "RichEdit50.h"
#include "RichEdit50PropPage.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


IMPLEMENT_DYNCREATE(CRichEdit50PropPage, COlePropertyPage)



// Message map

BEGIN_MESSAGE_MAP(CRichEdit50PropPage, COlePropertyPage)
END_MESSAGE_MAP()



// Initialize class factory and guid

IMPLEMENT_OLECREATE_EX(CRichEdit50PropPage, "RICHEDIT50.RichEdit50PropPage.1",
	0xa6c82e69, 0xf480, 0x406d, 0x88, 0xc, 0xa1, 0x2e, 0x91, 0xb, 0x4d, 0x4d)



// CRichEdit50PropPage::CRichEdit50PropPageFactory::UpdateRegistry -
// Adds or removes system registry entries for CRichEdit50PropPage

BOOL CRichEdit50PropPage::CRichEdit50PropPageFactory::UpdateRegistry(BOOL bRegister)
{
	if (bRegister)
		return AfxOleRegisterPropertyPageClass(AfxGetInstanceHandle(),
			m_clsid, IDS_RICHEDIT50_PPG);
	else
		return AfxOleUnregisterClass(m_clsid, NULL);
}



// CRichEdit50PropPage::CRichEdit50PropPage - Constructor

CRichEdit50PropPage::CRichEdit50PropPage() :
	COlePropertyPage(IDD, IDS_RICHEDIT50_PPG_CAPTION)
{
}



// CRichEdit50PropPage::DoDataExchange - Moves data between page and properties

void CRichEdit50PropPage::DoDataExchange(CDataExchange* pDX)
{
	DDP_PostProcessing(pDX);
}



// CRichEdit50PropPage message handlers
