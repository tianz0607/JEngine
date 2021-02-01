﻿using System;
using LitJson;
using UnityEngine;
using JEngine.Helper;
using System.Threading;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Stack;
using ILRuntime.CLR.TypeSystem;
using System.Collections.Generic;
using System.Threading.Tasks;
using ILRuntime.Runtime.Generated;
using ILRuntime.Runtime.Intepreter;
using JEngine.Core;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;

public static class InitILrt
{
    public static void InitializeILRuntime(AppDomain appdomain)
    {
#if DEBUG && (UNITY_EDITOR || UNITY_ANDROID || UNITY_IPHONE)
        //由于Unity的Profiler接口只允许在主线程使用，为了避免出异常，需要告诉ILRuntime主线程的线程ID才能正确将函数运行耗时报告给Profiler
        appdomain.UnityMainThreadID = Thread.CurrentThread.ManagedThreadId;
        appdomain.DebugService.StartDebugService(56000);
#endif

        RegisterCrossBindingAdaptorHelper.HelperRegister(appdomain);
        RegisterCLRMethodRedirectionHelper.HelperRegister(appdomain);
        RegisterMethodDelegateHelper.HelperRegister(appdomain);
        RegisterFunctionDelegateHelper.HelperRegister(appdomain);
        RegisterDelegateConvertorHelper.HelperRegister(appdomain);
        RegisterLitJsonHelper.HelperRegister(appdomain);
        RegisterValueTypeBinderHelper.HelperRegister(appdomain);

        //Protobuf适配
        ProtoBuf.PType.RegisterILRuntimeCLRRedirection(appdomain);
        
        //LitJson适配
        JsonMapper.RegisterILRuntimeCLRRedirection(appdomain);
        
        //CLR绑定
        CLRBindings.Initialize(appdomain);
        Init.Inited = true;
    }
}
