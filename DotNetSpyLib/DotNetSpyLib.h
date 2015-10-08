// DotNetSpyLib.h

#pragma once

using namespace System;

namespace DotNetSpyLib {

	/*public interface class IHookInstall
	{
	public:
		void OnInstallHook(array<Byte>^ data) = 0;
	};*/
	
	public ref class HookHelper
	{
	public:
		static void InstallIdleHandler(int processID, int threadID, System::String^ assemblyLocation, System::String^ typeName, array<Byte>^ additionalData);
	};
}
