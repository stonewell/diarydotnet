// Win32Functions.h

#pragma once

using namespace System;
using namespace System::ComponentModel;
using namespace System::Collections;
using namespace System::Data;
using namespace System::Runtime::InteropServices;
using namespace System::Runtime::Remoting::Messaging;

namespace Win32Functions {

	public class Win32Func
	{
	public:
			static IntPtr ExtractIcon(String^ path)
			{
				AFX_MANAGE_STATE(AfxGetStaticModuleState());

				CString strPath(path);

				WORD iIcon;

				LPTSTR buf = strPath.GetBuffer(255);

				HICON hIcon = ExtractAssociatedIcon(AfxGetInstanceHandle(),
					buf,&iIcon);

				strPath.ReleaseBuffer();

				return (IntPtr)hIcon;
			}

	};
}
