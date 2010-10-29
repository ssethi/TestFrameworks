using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using mscoree;
using System.Windows;

namespace TestPrism.Shell.Utility
{
	class ListProcessAppDomains
	{
		//public static IList<AppDomain> GetAppDomains()
		//{
		//    IList<AppDomain> _IList = new List<AppDomain>();
		//    IntPtr enumHandle = IntPtr.Zero;

		//    mscoree.CorRuntimeHostClass host = new mscoree.CorRuntimeHostClass();

		//    try
		//    {
		//        host.EnumDomains(out enumHandle);
		//        object domain = null;
		//        while (true)
		//        {
		//            host.NextDomain(enumHandle, out domain);
		//            if (domain == null)
		//                break;

		//            AppDomain appDomain = (AppDomain)domain;
		//            _IList.Add(appDomain);
		//        }

		//        return _IList;
		//    }

		//    catch (Exception e)
		//    {
		//        MessageBox.Show(e.ToString());
		//        return null;
		//    }

		//    finally
		//    {
		//        host.CloseEnum(enumHandle);
		//        Marshal.ReleaseComObject(host);
		//    }
		//}
	}
}
