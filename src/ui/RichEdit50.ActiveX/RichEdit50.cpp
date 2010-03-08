// RichEdit50.cpp : Implementation of CRichEdit50App and DLL registration.

#include "stdafx.h"
#include "RichEdit50.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


CRichEdit50App theApp;

const GUID CDECL BASED_CODE _tlid =
		{ 0xB3D949B0, 0x26D2, 0x415F, { 0xA3, 0xCC, 0x3A, 0x2, 0x32, 0xA7, 0xB3, 0xCF } };
const WORD _wVerMajor = 1;
const WORD _wVerMinor = 0;



// CRichEdit50App::InitInstance - DLL initialization

BOOL CRichEdit50App::InitInstance()
{
	BOOL bInit = COleControlModule::InitInstance();

	if (bInit)
	{
		// TODO: Add your own module initialization code here.
	}

	return bInit;
}



// CRichEdit50App::ExitInstance - DLL termination

int CRichEdit50App::ExitInstance()
{
	// TODO: Add your own module termination code here.

	return COleControlModule::ExitInstance();
}



// DllRegisterServer - Adds entries to the system registry

STDAPI DllRegisterServer(void)
{
	AFX_MANAGE_STATE(_afxModuleAddrThis);

	if (!AfxOleRegisterTypeLib(AfxGetInstanceHandle(), _tlid))
		return ResultFromScode(SELFREG_E_TYPELIB);

	if (!COleObjectFactoryEx::UpdateRegistryAll(TRUE))
		return ResultFromScode(SELFREG_E_CLASS);

	return NOERROR;
}



// DllUnregisterServer - Removes entries from the system registry

STDAPI DllUnregisterServer(void)
{
	AFX_MANAGE_STATE(_afxModuleAddrThis);

	if (!AfxOleUnregisterTypeLib(_tlid, _wVerMajor, _wVerMinor))
		return ResultFromScode(SELFREG_E_TYPELIB);

	if (!COleObjectFactoryEx::UpdateRegistryAll(FALSE))
		return ResultFromScode(SELFREG_E_CLASS);

	return NOERROR;
}
