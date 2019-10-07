﻿using Atlas.UI.Windows;
using System.Windows.Interop;

namespace Atlas.UI.Extensions
{
    public static class WindowExtensions
    {
        public static void SetMaximization(this Window window, bool enable)
        {
            SetWindowStyle(window, 0x00010000, enable);
        }

        public static void SetResizing(this Window window, bool enable)
        {
            SetWindowStyle(window, 0x00040000, enable);
        }

        public static void SetBorder(this Window window, bool enable)
        {
            SetWindowStyle(window, 0x00800000, enable);
        }

        public static void SetDialogMode(this Window window)
        {
            SetWindowStyle(window, 0x00C00000, false);
            SetWindowStyle(window, 0x00800000, true);
            SetWindowStyle(window, 0x00400000, true);
        }

        private static void SetWindowStyle(Window window, uint styleConst, bool enable)
        {
            var handle = new WindowInteropHelper(window).Handle;
            var currentWindowLong = WinAPI.GetWindowLong(handle, WinAPI.GWL_STYLE);

            if (!enable)
            {
                currentWindowLong &= ~styleConst;
            }
            else
            {
                currentWindowLong |= styleConst;
            }

            WinAPI.SetWindowLong(handle, WinAPI.GWL_STYLE, currentWindowLong);
        }
    }
}
