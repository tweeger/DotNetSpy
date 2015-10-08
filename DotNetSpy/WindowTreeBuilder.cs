using System;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;

using DotNetSpy.Properties;

namespace DotNetSpy
{
	/// <summary>
	/// Summary description for WindowTreeBuilder.
	/// </summary>
	public class WindowTreeBuilder
    {
        private Hashtable hwndNodeMap = new Hashtable();

        public WindowTreeBuilder(IntPtr hRootWnd)
        {
            this.RootNode = this.AddWindow(hRootWnd, NativeMethods.IsDotNetWindow(hRootWnd));
        }

        private WindowTreeNode rootNode = null;
		public WindowTreeNode RootNode
		{
			get
			{
				return rootNode;
			}
            private set
            {
                if (value != rootNode)
                {
                    this.rootNode = value;
                }
            }
		}

        public void RemoveWindow(IntPtr hwnd)
		{
			WindowTreeNode node =  (WindowTreeNode)hwndNodeMap[hwnd];
			
			if (node != null)
			{
				hwndNodeMap.Remove(hwnd);
				node.Remove();
			}
		}
        public bool HasManagedChild(WindowTreeNode parentNode)
		{
			bool ret = false;
			
			foreach(WindowTreeNode node in parentNode.Nodes)
			{
				if (node.IsManaged || HasManagedChild(node))
				{
					ret = true;
					break;
				}
			}

			return ret;
		}
		
		public void BuildAllWindowsTree()
		{
            if (NativeMethods.GetDesktopWindow() != rootNode.Hwnd)
            {
                IntPtr param = IntPtr.Zero;
                if (NativeMethods.IsDotNetWindow(rootNode.Hwnd)) param = new IntPtr(1);
                NativeMethods.EnumChildWindows(rootNode.Hwnd, new WindowEnumProc(this.OnEnumWindow), param);
            }
            else
            {
                this.AddAllProcessWindows();
            }

            this.BuildWindowTree();
        }
        public void FilterUnmanagedWindows()
        {
            WindowTreeBuilder.FilterUnmanagedWindows(rootNode);
        }
        public static void FilterUnmanagedWindows(WindowTreeNode parentNode)
        {
            for (int i = 0; i < parentNode.Nodes.Count; i++)
            {
                WindowTreeNode node = (WindowTreeNode)parentNode.Nodes[i];

                if (!node.IsManaged)// && !HasManagedChild(node))
                {
                    parentNode.Nodes.RemoveAt(i);
                    i--;
                }
                else
                {
                    FilterUnmanagedWindows(node);
                }
            }
        }

        private void AddAllProcessWindows()
        {
            foreach (Process proc in Process.GetProcesses())
            {
                this.AddAllProcessWindows(proc);
            }
        }
        private void AddAllProcessWindows(Process proc)
        {
            IntPtr param = IntPtr.Zero;
            if (NativeMethods.IsDotNetProcess(proc.Id)) param = new IntPtr(1);
            foreach (ProcessThread pt in proc.Threads)
            {
                NativeMethods.EnumThreadWindows(pt.Id, new WindowEnumProc(this.OnEnumThreadWindow), param);
            }
        }

        private int OnEnumThreadWindow(IntPtr hwnd, IntPtr lParam)
        {
            AddWindow(hwnd, lParam == IntPtr.Zero ? false : true);
            NativeMethods.EnumChildWindows(hwnd, new WindowEnumProc(this.OnEnumWindow), lParam);
            return 1;
        }
        private int OnEnumWindow(IntPtr hwnd, IntPtr lParam)
        {
            AddWindow(hwnd, lParam == IntPtr.Zero ? false : true);
            return 1;
        }

        private WindowTreeNode AddWindow(IntPtr hwnd, bool isManaged)
        {
            WindowTreeNode node = new WindowTreeNode(hwnd, isManaged);
            if (!isManaged)
            {
                NativeWindow native = new NativeWindow();
                native.Handle = hwnd;
                native.ClassName = node.WindowClassName;
                native.Text = node.WindowText;

                node.Tag = native;
            }

            if (!hwndNodeMap.ContainsKey(hwnd)) hwndNodeMap[hwnd] = node;

            return node;
        }
        private void BuildWindowTree()
        {
            hwndNodeMap.Remove(this.RootNode.Hwnd);
            foreach (WindowTreeNode node in hwndNodeMap.Values)
            {
                this.AddWindowNode(node);
            }
        }
        private WindowTreeNode AddWindowNode(WindowTreeNode node)
        {
            IntPtr hwndParent = NativeMethods.GetParent(node.Hwnd);
            if (hwndNodeMap.ContainsKey(hwndParent))
            {
                WindowTreeNode parentNode = (WindowTreeNode)hwndNodeMap[hwndParent];
                if (parentNode != null) parentNode.Nodes.Add(node);
            }
            else
            {
                this.RootNode.Nodes.Add(node);
            }

            return node;
        }
	}
}
