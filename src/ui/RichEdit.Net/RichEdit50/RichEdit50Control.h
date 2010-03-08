#pragma once

#include "resource.h"

using namespace System;
using namespace System::ComponentModel;
using namespace System::Collections;
using namespace System::Windows::Forms;
using namespace System::Data;
using namespace System::Drawing;
using namespace System::Runtime::InteropServices;
using namespace System::Runtime::Remoting::Messaging;

#include "RichEditControl50W.h"

namespace RichEdit {
	namespace Net {

		/// <summary>
		/// Summary for RichEdit50Control
		/// </summary>
		///
		/// WARNING: If you change the name of this class, you will need to change the
		///          'Resource File Name' property for the managed resource compiler tool
		///          associated with all .resx files this class depends on.  Otherwise,
		///          the designers will not be able to interact properly with localized
		///          resources associated with this form.
		public __gc class RichEdit50Control : public System::Windows::Forms::UserControl
		{
		public:
			RichEdit50Control(void)
			{
				InitializeComponent();
				m_pCtrl = new CRichEditControl50W();
				m_pThisWnd = new CWnd();
				m_bModified = false;
				m_pText = NULL;
			}

		protected:
			/// <summary>
			/// Clean up any resources being used.
			/// </summary>
			~RichEdit50Control()
			{
			}

		private:

#pragma region Windows Form Designer generated code
			/// <summary>
			/// Required method for Designer support - do not modify
			/// the contents of this method with the code editor.
			/// </summary>
			void InitializeComponent(void)
			{
				this->SuspendLayout();
				// 
				// RichEdit50Control
				// 
				this->AutoScaleDimensions = System::Drawing::SizeF(6, 13);
				this->AutoScaleMode = System::Windows::Forms::AutoScaleMode::Font;
				this->Name = S"RichEdit50Control";
				this->Load += new System::EventHandler(this, &RichEdit50Control::RichEdit50Control_Load);
				this->Resize += new System::EventHandler(this, &RichEdit50Control::RichEdit50Control_Resize);
				this->ResumeLayout(false);
			}
#pragma endregion

		protected:
			void Dispose(bool b)
			{
				UserControl::Dispose(b);

				if (m_pThisWnd != NULL)
				{
					m_pThisWnd->Detach();
					delete m_pThisWnd;
					m_pThisWnd = NULL;
				}

				if (m_pCtrl != NULL)
				{
					m_pCtrl->DestroyWindow();
				}
				m_pCtrl = NULL;

				if (m_pText != NULL)
				{
					delete m_pText;
					m_pText = NULL;
				}
			}

			void OnHandleCreated(EventArgs* e)
			{
				UserControl::OnHandleCreated(e);

				System::Diagnostics::Debug::Assert(m_pCtrl->GetSafeHwnd() == NULL);

				AFX_MANAGE_STATE(AfxGetStaticModuleState());

				m_pThisWnd->Attach((HWND)get_Handle().ToPointer());

				//Set Options for the Rich Edit Control 
				DWORD w_RichEd = WS_CHILD | WS_VISIBLE | WS_CLIPSIBLINGS | WS_VSCROLL | ES_AUTOVSCROLL | WS_HSCROLL | ES_AUTOHSCROLL | ES_MULTILINE;

				//Create Rich Edit Control to fill view
				m_pCtrl->Create(w_RichEd, CRect(0, 0, 0, 0), m_pThisWnd, 100);

				m_pCtrl->Initialize();
			}

			void WndProc(System::Windows::Forms::Message __gc * m)
			{
				AFX_MANAGE_STATE(AfxGetStaticModuleState());

				if (m->Msg == WM_NOTIFY)
				{
					NMHDR * pNMHDR = (NMHDR *)m->LParam.ToPointer();

					if (pNMHDR->code == EN_SELCHANGE)
					{
						CString strResult = 
							m_pCtrl->GetRtfTextFrom50WControl(SF_RTF);

						if (m_pText == NULL)
						{
							m_bModified = true;
						}
						else
						{
							m_bModified = (strResult.Compare(*m_pText) != 0);
						}
					}
				}

				UserControl::WndProc(m);
			}

		private:
			CRichEditControl50W * m_pCtrl;
			CWnd * m_pThisWnd;
			bool m_bModified;
			CString * m_pText;

		public:
			[property: CategoryAttribute("RichEdit")]
			[property: BrowsableAttribute(false)]
			[property: DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility::Hidden)]
			__property void set_Rtf(String* rtf)
			{
				if (!m_pCtrl)
					throw new ObjectDisposedException(__typeof(RichEdit50Control)->ToString());

				AFX_MANAGE_STATE(AfxGetStaticModuleState());

				if (!::IsWindow(m_pCtrl->m_hWnd))
					return;

				CString newVal(rtf);

				m_pCtrl->SetRtfTextTo50WControl(newVal,
					SF_RTF);

				m_bModified = true;
			}

			[property: CategoryAttribute("RichEdit")]
			[property: BrowsableAttribute(false)]
			[property: DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility::Hidden)]
			__property String* get_Rtf()
			{
				if (!m_pCtrl)
					throw new ObjectDisposedException(__typeof(RichEdit50Control)->ToString());

				if (!::IsWindow(m_pCtrl->m_hWnd))
					return new String(_T(""));

				CString strResult = 
					m_pCtrl->GetRtfTextFrom50WControl(SF_RTF);

				return new String(strResult);
			}

			[property: CategoryAttribute("RichEdit")]
			[property: BrowsableAttribute(false)]
			[property: DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility::Hidden)]
			__property void set_SelectRtf(String* rtf)
			{
				if (!m_pCtrl)
					throw new ObjectDisposedException(__typeof(RichEdit50Control)->ToString());

				AFX_MANAGE_STATE(AfxGetStaticModuleState());

				if (!::IsWindow(m_pCtrl->m_hWnd))
					return;

				CString newVal(rtf);

				m_pCtrl->SetRtfTextTo50WControl(newVal,
					SF_RTF | SFF_SELECTION);

				m_bModified = true;
			}

			[property: CategoryAttribute("RichEdit")]
			[property: BrowsableAttribute(false)]
			[property: DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility::Hidden)]
			__property String* get_SelectRtf()
			{
				if (!m_pCtrl)
					throw new ObjectDisposedException(__typeof(RichEdit50Control)->ToString());

				if (!::IsWindow(m_pCtrl->m_hWnd))
					return new String(_T(""));

				CString strResult = 
					m_pCtrl->GetRtfTextFrom50WControl(SF_RTF | SFF_SELECTION);

				return new String(strResult);
			}	

			[property: CategoryAttribute("RichEdit")]
			[property: BrowsableAttribute(false)]
			[property: DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility::Hidden)]
			__property void set_Text(String* rtf)
			{
				if (!m_pCtrl)
					throw new ObjectDisposedException(__typeof(RichEdit50Control)->ToString());

				AFX_MANAGE_STATE(AfxGetStaticModuleState());
				if (!::IsWindow(m_pCtrl->m_hWnd))
					return;

				CString newVal(rtf);

				m_pCtrl->SetTextTo50WControl(newVal,
					ST_DEFAULT,
					1200);

				m_bModified = true;
			}

			[property: CategoryAttribute("RichEdit")]
			[property: BrowsableAttribute(false)]
			[property: DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility::Hidden)]
			__property String* get_Text()
			{
				if (!m_pCtrl)
					throw new ObjectDisposedException(__typeof(RichEdit50Control)->ToString());

				if (!::IsWindow(m_pCtrl->m_hWnd))
					return new String(_T(""));

				CString strResult = 
					m_pCtrl->GetTextFrom50WControl(GT_DEFAULT, 1200);

				return new String(strResult);
			}		

			[property: CategoryAttribute("RichEdit")]
			[property: BrowsableAttribute(false)]
			[property: DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility::Hidden)]
			__property void set_SelectText(String* rtf)
			{
				if (!m_pCtrl)
					throw new ObjectDisposedException(__typeof(RichEdit50Control)->ToString());

				AFX_MANAGE_STATE(AfxGetStaticModuleState());
				if (!::IsWindow(m_pCtrl->m_hWnd))
					return;

				CString newVal(rtf);

				m_pCtrl->SetTextTo50WControl(newVal,
					ST_SELECTION,
					1200);

				m_bModified = true;
			}

			[property: CategoryAttribute("RichEdit")]
			[property: BrowsableAttribute(false)]
			[property: DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility::Hidden)]
			__property String* get_SelectText()
			{
				if (!m_pCtrl)
					throw new ObjectDisposedException(__typeof(RichEdit50Control)->ToString());
				if (!::IsWindow(m_pCtrl->m_hWnd))
					return new String(_T(""));

				CString strResult = 
					m_pCtrl->GetTextFrom50WControl(GT_SELECTION, 1200);

				return new String(strResult);
			}	

			[property: CategoryAttribute("RichEdit")]
			[property: BrowsableAttribute(false)]
			[property: DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility::Hidden)]
			__property void set_Modified(bool bModified)
			{
				if (!m_pCtrl)
					throw new ObjectDisposedException(__typeof(RichEdit50Control)->ToString());

				AFX_MANAGE_STATE(AfxGetStaticModuleState());

				if (!::IsWindow(m_pCtrl->m_hWnd))
					return ;

				m_bModified = bModified;

				if (bModified == false)
				{
					if (m_pText != NULL)
						delete m_pText;

					m_pText = new CString(m_pCtrl->GetRtfTextFrom50WControl(SF_RTF));
				}
			}

			[property: CategoryAttribute("RichEdit")]
			[property: BrowsableAttribute(false)]
			[property: DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility::Hidden)]
			__property bool get_Modified()
			{
				if (!m_pCtrl)
					throw new ObjectDisposedException(__typeof(RichEdit50Control)->ToString());

				return m_bModified;
			}

			void Cut(void)
			{
				AFX_MANAGE_STATE(AfxGetStaticModuleState());
				if (!::IsWindow(m_pCtrl->m_hWnd))
					return;

				m_pCtrl->SendMessage(WM_CUT);

				m_bModified = true;
			}

			void Copy(void)
			{
				AFX_MANAGE_STATE(AfxGetStaticModuleState());
				if (!::IsWindow(m_pCtrl->m_hWnd))
					return;
				m_pCtrl->SendMessage(WM_COPY);
			}

			void Paste(DataFormats::Format * format)
			{
				AFX_MANAGE_STATE(AfxGetStaticModuleState());
				if (!::IsWindow(m_pCtrl->m_hWnd))
					return;

				REPASTESPECIAL rsp;
				rsp.dwAspect = DVASPECT_CONTENT;
				rsp.dwParam = 0;

				m_pCtrl->SendMessage(EM_PASTESPECIAL, (WPARAM)format->Id, (LPARAM)&rsp);

				m_bModified = true;
			}

			bool CanPaste(DataFormats::Format * format)
			{
				AFX_MANAGE_STATE(AfxGetStaticModuleState());
				if (!::IsWindow(m_pCtrl->m_hWnd))
					return false;

				LRESULT l = m_pCtrl->SendMessage(EM_CANPASTE, (WPARAM)format->Id, (LPARAM)0);

				return l != 0;
			}

		private: 
			System::Void RichEdit50Control_Load(System::Object*  sender, 
				System::EventArgs*  e) 
			{
				AFX_MANAGE_STATE(AfxGetStaticModuleState());
				if (!::IsWindow(m_pCtrl->m_hWnd))
					return;
				::SetWindowPos(m_pCtrl->m_hWnd, 
					0,0,0,this->Size.Width,this->Size.Height,
					SWP_NOZORDER | SWP_NOMOVE);
			}

		private: 
			System::Void RichEdit50Control_Resize(System::Object*  sender, 
				System::EventArgs*  e) 
			{
				AFX_MANAGE_STATE(AfxGetStaticModuleState());
				if (!::IsWindow(m_pCtrl->m_hWnd))
					return;

				::SetWindowPos(m_pCtrl->m_hWnd, 
					0,0,0,this->Size.Width,this->Size.Height,
					SWP_NOZORDER | SWP_NOMOVE);
			}
		};
	}
}
