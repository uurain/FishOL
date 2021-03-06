﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class NetworkMgrWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(NetworkMgr), typeof(UnityEngine.MonoBehaviour));
		L.RegFunction("AddEvent", AddEvent);
		L.RegFunction("OnSkillObjectX", OnSkillObjectX);
		L.RegFunction("OnInit", OnInit);
		L.RegFunction("Unload", Unload);
		L.RegFunction("CallMethod", CallMethod);
		L.RegFunction("ConnectServer", ConnectServer);
		L.RegFunction("ShutDown", ShutDown);
		L.RegFunction("AddReceiveCallBack", AddReceiveCallBack);
		L.RegFunction("SendMsg", SendMsg);
		L.RegFunction("SendMsgNet", SendMsgNet);
		L.RegFunction("ParseMsg", ParseMsg);
		L.RegFunction("DisConnected", DisConnected);
		L.RegFunction("Connected", Connected);
		L.RegFunction("__eq", op_Equality);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddEvent(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			int arg0 = (int)LuaDLL.luaL_checknumber(L, 1);
			LuaByteBuffer arg1 = new LuaByteBuffer(ToLua.CheckByteBuffer(L, 2));
			NetworkMgr.AddEvent(arg0, arg1);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnSkillObjectX(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 3);
			NetworkMgr obj = (NetworkMgr)ToLua.CheckObject<NetworkMgr>(L, 1);
			ushort arg0 = (ushort)LuaDLL.luaL_checknumber(L, 2);
			System.IO.MemoryStream arg1 = (System.IO.MemoryStream)ToLua.CheckObject<System.IO.MemoryStream>(L, 3);
			obj.OnSkillObjectX(arg0, arg1);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnInit(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			NetworkMgr obj = (NetworkMgr)ToLua.CheckObject<NetworkMgr>(L, 1);
			obj.OnInit();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Unload(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			NetworkMgr obj = (NetworkMgr)ToLua.CheckObject<NetworkMgr>(L, 1);
			obj.Unload();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CallMethod(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);
			NetworkMgr obj = (NetworkMgr)ToLua.CheckObject<NetworkMgr>(L, 1);
			string arg0 = ToLua.CheckString(L, 2);
			object[] arg1 = ToLua.ToParamsObject(L, 3, count - 2);
			object[] o = obj.CallMethod(arg0, arg1);
			ToLua.Push(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ConnectServer(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 3);
			NetworkMgr obj = (NetworkMgr)ToLua.CheckObject<NetworkMgr>(L, 1);
			string arg0 = ToLua.CheckString(L, 2);
			ushort arg1 = (ushort)LuaDLL.luaL_checknumber(L, 3);
			obj.ConnectServer(arg0, arg1);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ShutDown(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			NetworkMgr obj = (NetworkMgr)ToLua.CheckObject<NetworkMgr>(L, 1);
			obj.ShutDown();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddReceiveCallBack(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 3);
			NetworkMgr obj = (NetworkMgr)ToLua.CheckObject<NetworkMgr>(L, 1);
			ushort arg0 = (ushort)LuaDLL.luaL_checknumber(L, 2);
			NFSDK.NFCMessageDispatcher.MessageHandler arg1 = (NFSDK.NFCMessageDispatcher.MessageHandler)ToLua.CheckDelegate<NFSDK.NFCMessageDispatcher.MessageHandler>(L, 3);
			obj.AddReceiveCallBack(arg0, arg1);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SendMsg(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 3);
			NetworkMgr obj = (NetworkMgr)ToLua.CheckObject<NetworkMgr>(L, 1);
			int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
			LuaByteBuffer arg1 = new LuaByteBuffer(ToLua.CheckByteBuffer(L, 3));
			obj.SendMsg(arg0, arg1);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SendMsgNet(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 3);
			NetworkMgr obj = (NetworkMgr)ToLua.CheckObject<NetworkMgr>(L, 1);
			ushort arg0 = (ushort)LuaDLL.luaL_checknumber(L, 2);
			System.IO.MemoryStream arg1 = (System.IO.MemoryStream)ToLua.CheckObject<System.IO.MemoryStream>(L, 3);
			obj.SendMsgNet(arg0, arg1);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ParseMsg(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 3);
			NetworkMgr obj = (NetworkMgr)ToLua.CheckObject<NetworkMgr>(L, 1);
			ushort arg0 = (ushort)LuaDLL.luaL_checknumber(L, 2);
			System.IO.MemoryStream arg1 = (System.IO.MemoryStream)ToLua.CheckObject<System.IO.MemoryStream>(L, 3);
			obj.ParseMsg(arg0, arg1);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DisConnected(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 3);
			NetworkMgr obj = (NetworkMgr)ToLua.CheckObject<NetworkMgr>(L, 1);
			object arg0 = ToLua.ToVarObject(L, 2);
			NFSDK.DisConnectedEventArgs arg1 = (NFSDK.DisConnectedEventArgs)ToLua.CheckObject<NFSDK.DisConnectedEventArgs>(L, 3);
			obj.DisConnected(arg0, arg1);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Connected(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 3);
			NetworkMgr obj = (NetworkMgr)ToLua.CheckObject<NetworkMgr>(L, 1);
			object arg0 = ToLua.ToVarObject(L, 2);
			NFSDK.ConnectedEventArgs arg1 = (NFSDK.ConnectedEventArgs)ToLua.CheckObject<NFSDK.ConnectedEventArgs>(L, 3);
			obj.Connected(arg0, arg1);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int op_Equality(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			UnityEngine.Object arg0 = (UnityEngine.Object)ToLua.ToObject(L, 1);
			UnityEngine.Object arg1 = (UnityEngine.Object)ToLua.ToObject(L, 2);
			bool o = arg0 == arg1;
			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}
}

