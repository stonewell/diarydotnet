

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


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


/* verify that the <rpcndr.h> version is high enough to compile this file*/
#ifndef __REQUIRED_RPCNDR_H_VERSION__
#define __REQUIRED_RPCNDR_H_VERSION__ 440
#endif

#include "rpc.h"
#include "rpcndr.h"

#ifndef __RichEdit50idl_h__
#define __RichEdit50idl_h__

#if defined(_MSC_VER) && (_MSC_VER >= 1020)
#pragma once
#endif

/* Forward Declarations */ 

#ifndef ___DRichEdit50_FWD_DEFINED__
#define ___DRichEdit50_FWD_DEFINED__
typedef interface _DRichEdit50 _DRichEdit50;
#endif 	/* ___DRichEdit50_FWD_DEFINED__ */


#ifndef ___DRichEdit50Events_FWD_DEFINED__
#define ___DRichEdit50Events_FWD_DEFINED__
typedef interface _DRichEdit50Events _DRichEdit50Events;
#endif 	/* ___DRichEdit50Events_FWD_DEFINED__ */


#ifndef __RichEdit50_FWD_DEFINED__
#define __RichEdit50_FWD_DEFINED__

#ifdef __cplusplus
typedef class RichEdit50 RichEdit50;
#else
typedef struct RichEdit50 RichEdit50;
#endif /* __cplusplus */

#endif 	/* __RichEdit50_FWD_DEFINED__ */


#ifdef __cplusplus
extern "C"{
#endif 

void * __RPC_USER MIDL_user_allocate(size_t);
void __RPC_USER MIDL_user_free( void * ); 


#ifndef __RichEdit50Lib_LIBRARY_DEFINED__
#define __RichEdit50Lib_LIBRARY_DEFINED__

/* library RichEdit50Lib */
/* [control][helpstring][helpfile][version][uuid] */ 


EXTERN_C const IID LIBID_RichEdit50Lib;

#ifndef ___DRichEdit50_DISPINTERFACE_DEFINED__
#define ___DRichEdit50_DISPINTERFACE_DEFINED__

/* dispinterface _DRichEdit50 */
/* [helpstring][uuid] */ 


EXTERN_C const IID DIID__DRichEdit50;

#if defined(__cplusplus) && !defined(CINTERFACE)

    MIDL_INTERFACE("323BB75D-FB0F-4A26-98B8-7E0F363E0964")
    _DRichEdit50 : public IDispatch
    {
    };
    
#else 	/* C style interface */

    typedef struct _DRichEdit50Vtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            _DRichEdit50 * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            _DRichEdit50 * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            _DRichEdit50 * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            _DRichEdit50 * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            _DRichEdit50 * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            _DRichEdit50 * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            _DRichEdit50 * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        END_INTERFACE
    } _DRichEdit50Vtbl;

    interface _DRichEdit50
    {
        CONST_VTBL struct _DRichEdit50Vtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define _DRichEdit50_QueryInterface(This,riid,ppvObject)	\
    (This)->lpVtbl -> QueryInterface(This,riid,ppvObject)

#define _DRichEdit50_AddRef(This)	\
    (This)->lpVtbl -> AddRef(This)

#define _DRichEdit50_Release(This)	\
    (This)->lpVtbl -> Release(This)


#define _DRichEdit50_GetTypeInfoCount(This,pctinfo)	\
    (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo)

#define _DRichEdit50_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo)

#define _DRichEdit50_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)

#define _DRichEdit50_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)

#endif /* COBJMACROS */


#endif 	/* C style interface */


#endif 	/* ___DRichEdit50_DISPINTERFACE_DEFINED__ */


#ifndef ___DRichEdit50Events_DISPINTERFACE_DEFINED__
#define ___DRichEdit50Events_DISPINTERFACE_DEFINED__

/* dispinterface _DRichEdit50Events */
/* [helpstring][uuid] */ 


EXTERN_C const IID DIID__DRichEdit50Events;

#if defined(__cplusplus) && !defined(CINTERFACE)

    MIDL_INTERFACE("5F1F819F-698F-4A36-8B38-405EFBB98227")
    _DRichEdit50Events : public IDispatch
    {
    };
    
#else 	/* C style interface */

    typedef struct _DRichEdit50EventsVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            _DRichEdit50Events * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            _DRichEdit50Events * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            _DRichEdit50Events * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            _DRichEdit50Events * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            _DRichEdit50Events * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            _DRichEdit50Events * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            _DRichEdit50Events * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        END_INTERFACE
    } _DRichEdit50EventsVtbl;

    interface _DRichEdit50Events
    {
        CONST_VTBL struct _DRichEdit50EventsVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define _DRichEdit50Events_QueryInterface(This,riid,ppvObject)	\
    (This)->lpVtbl -> QueryInterface(This,riid,ppvObject)

#define _DRichEdit50Events_AddRef(This)	\
    (This)->lpVtbl -> AddRef(This)

#define _DRichEdit50Events_Release(This)	\
    (This)->lpVtbl -> Release(This)


#define _DRichEdit50Events_GetTypeInfoCount(This,pctinfo)	\
    (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo)

#define _DRichEdit50Events_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo)

#define _DRichEdit50Events_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)

#define _DRichEdit50Events_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)

#endif /* COBJMACROS */


#endif 	/* C style interface */


#endif 	/* ___DRichEdit50Events_DISPINTERFACE_DEFINED__ */


EXTERN_C const CLSID CLSID_RichEdit50;

#ifdef __cplusplus

class DECLSPEC_UUID("47AEFB7F-E430-48DB-A702-60F3C045D612")
RichEdit50;
#endif
#endif /* __RichEdit50Lib_LIBRARY_DEFINED__ */

/* Additional Prototypes for ALL interfaces */

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif


