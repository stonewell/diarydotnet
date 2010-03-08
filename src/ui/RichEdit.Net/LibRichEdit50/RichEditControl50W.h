// By Jim Dunne http://www.topjimmy.net/tjs
// topjimmy@topjimmy.net
// copyright(C)2005
// if you use all or part of this code, please give me credit somewhere :)
#pragma once
// CRichEditControl50W
class CRichEditControl50W : public CWnd
{
	DECLARE_DYNAMIC(CRichEditControl50W)

protected:
	DECLARE_MESSAGE_MAP()
	CHARRANGE m_crRE50W;
	CHARFORMAT2 m_cfRE50W;
	SETTEXTEX m_st50W;
	GETTEXTEX m_gt50W;

	HINSTANCE m_hInstRichEdit50W; 
	TEXTRANGEW m_trRE50W;
	LPSTR m_lpszChar;

// Constructors
public:
	CRichEditControl50W();

	virtual BOOL Create(DWORD dwStyle, const RECT& rect, CWnd* pParentWnd, UINT nID);

	void SetSel50W(long nStartChar, long nEndChar);
	BOOL SetDefaultCharFormat50W(DWORD dwMask, COLORREF crTextColor, DWORD dwEffects, LPTSTR szFaceName, LONG yHeight, COLORREF crBackColor);
	void SetTextTo50WControl(CString csText, int nSTFlags, int nSTCodepage);
	CString GetTextFrom50WControl(int nSTFlags, int nSTCodepage);
	void LimitText50W(int nChars);
	void SetOptions50W(WORD wOp, DWORD dwFlags);
	DWORD SetEventMask50W(DWORD dwEventMask);
	void GetTextRange50W(int ncharrMin, int ncharrMax);
	DWORD GetTextLength(int nSTCodepage);

	void SetRtfTextTo50WControl(CString csText, int nSTFlags);
	CString GetRtfTextFrom50WControl(int nSTFlags);

	void Initialize();
	virtual ~CRichEditControl50W();
protected:
};


