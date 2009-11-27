

/* this ALWAYS GENERATED file contains the IIDs and CLSIDs */

/* link this file in with the server and any clients */


 /* File created by MIDL compiler version 6.00.0366 */
/* at Wed Apr 30 09:26:44 2008
 */
/* Compiler settings for .\RichEdit50.idl:
    Oicf, W1, Zp8, env=Win32 (32b run)
    protocol : dce , ms_ext, c_ext
    error checks: allocation ref bounds_check enum stub_data 
    VC __declspec() decoration level: 
         __declspec(uuid()), __declspec(selectany), __declspec(novtable)
         DECLSPEC_UUID(), MIDL_INTERFACE()
*/
//@@MIDL_FILE_HEADING(  )

#pragma warning( disable: 4049 )  /* more than 64k source lines */


#ifdef __cplusplus
extern "C"{
#endif 


#include <rpc.h>
#include <rpcndr.h>

#ifdef _MIDL_USE_GUIDDEF_

#ifndef INITGUID
#define INITGUID
#include <guiddef.h>
#undef INITGUID
#else
#include <guiddef.h>
#endif

#define MIDL_DEFINE_GUID(type,name,l,w1,w2,b1,b2,b3,b4,b5,b6,b7,b8) \
        DEFINE_GUID(name,l,w1,w2,b1,b2,b3,b4,b5,b6,b7,b8)

#else // !_MIDL_USE_GUIDDEF_

#ifndef __IID_DEFINED__
#define __IID_DEFINED__

typedef struct _IID
{
    unsigned long x;
    unsigned short s1;
    unsigned short s2;
    unsigned char  c[8];
} IID;

#endif // __IID_DEFINED__

#ifndef CLSID_DEFINED
#define CLSID_DEFINED
typedef IID CLSID;
#endif // CLSID_DEFINED

#define MIDL_DEFINE_GUID(type,name,l,w1,w2,b1,b2,b3,b4,b5,b6,b7,b8) \
        const type name = {l,w1,w2,{b1,b2,b3,b4,b5,b6,b7,b8}}

#endif !_MIDL_USE_GUIDDEF_

MIDL_DEFINE_GUID(IID, LIBID_RichEdit50Lib,0xB3D949B0,0x26D2,0x415F,0xA3,0xCC,0x3A,0x02,0x32,0xA7,0xB3,0xCF);


MIDL_DEFINE_GUID(IID, DIID__DRichEdit50,0x323BB75D,0xFB0F,0x4A26,0x98,0xB8,0x7E,0x0F,0x36,0x3E,0x09,0x64);


MIDL_DEFINE_GUID(IID, DIID__DRichEdit50Events,0x5F1F819F,0x698F,0x4A36,0x8B,0x38,0x40,0x5E,0xFB,0xB9,0x82,0x27);


MIDL_DEFINE_GUID(CLSID, CLSID_RichEdit50,0x47AEFB7F,0xE430,0x48DB,0xA7,0x02,0x60,0xF3,0xC0,0x45,0xD6,0x12);

#undef MIDL_DEFINE_GUID

#ifdef __cplusplus
}
#endif



