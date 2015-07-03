using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
 
namespace Vereyon.Windows
{
    /// <summary>
    /// Based on: http://support.microsoft.com/kb/326201
    /// </summary>
    public static class WinInetCacheControl
    {
 
        /// <summary>
        /// Initiates the enumeration of the cache groups in the Internet cache.
        /// </summary>
        /// <param name="dwFlags"></param>
        /// <param name="dwFilter"></param>
        /// <param name="lpSearchCondition"></param>
        /// <param name="dwSearchCondition"></param>
        /// <param name="lpGroupId"></param>
        /// <param name="lpReserved"></param>
        /// <returns></returns>
        [DllImport(@"wininet",
            SetLastError = true,
            CharSet = CharSet.Auto,
            EntryPoint = "FindFirstUrlCacheGroup",
            CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr FindFirstUrlCacheGroup(
            int dwFlags,
            CacheGroupSearchFilter dwFilter,
            IntPtr lpSearchCondition,
            int dwSearchCondition,
            ref long lpGroupId,
            IntPtr lpReserved);
 
        /// <summary>
        /// Retrieves the next cache group in a cache group enumeration.
        /// </summary>
        /// <param name="hFind"></param>
        /// <param name="lpGroupId"></param>
        /// <param name="lpReserved"></param>
        /// <returns></returns>
        [DllImport(@"wininet",
            SetLastError = true,
            CharSet = CharSet.Auto,
            EntryPoint = "FindNextUrlCacheGroup",
            CallingConvention = CallingConvention.StdCall)]
        private static extern bool FindNextUrlCacheGroup(
            IntPtr hFind,
            ref long lpGroupId,
            IntPtr lpReserved);
 
        /// <summary>
        /// Releases the specified GROUPID and any associated state in the cache index file.
        /// </summary>
        /// <param name="GroupId"></param>
        /// <param name="dwFlags"></param>
        /// <param name="lpReserved"></param>
        /// <returns></returns>
        [DllImport(@"wininet",
            SetLastError = true,
            CharSet = CharSet.Auto,
            EntryPoint = "DeleteUrlCacheGroup",
            CallingConvention = CallingConvention.StdCall)]
        private static extern bool DeleteUrlCacheGroup(
            long GroupId,
            CacheGroupFlag dwFlags,
            IntPtr lpReserved);
 
        /// <summary>
        /// Begins the enumeration of the Internet cache.
        /// </summary>
        /// <param name="lpszUrlSearchPattern"></param>
        /// <param name="lpFirstCacheEntryInfo"></param>
        /// <param name="lpdwFirstCacheEntryInfoBufferSize"></param>
        /// <returns></returns>
        [DllImport(@"wininet",
            SetLastError = true,
            CharSet = CharSet.Auto,
            EntryPoint = "FindFirstUrlCacheEntry",
            CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr FindFirstUrlCacheEntry(
            [MarshalAs(UnmanagedType.LPTStr)] string lpszUrlSearchPattern,
            IntPtr lpFirstCacheEntryInfo,
            ref int lpdwFirstCacheEntryInfoBufferSize);
 
        /// <summary>
        /// Retrieves the next entry in the Internet cache.
        /// </summary>
        /// <param name="hFind"></param>
        /// <param name="lpNextCacheEntryInfo"></param>
        /// <param name="lpdwNextCacheEntryInfoBufferSize"></param>
        /// <returns></returns>
        [DllImport(@"wininet",
            SetLastError = true,
            CharSet = CharSet.Auto,
            EntryPoint = "FindNextUrlCacheEntry",
            CallingConvention = CallingConvention.StdCall)]
        private static extern bool FindNextUrlCacheEntry(
            IntPtr hFind,
            IntPtr lpNextCacheEntryInfo,
            ref int lpdwNextCacheEntryInfoBufferSize);
 
        /// <summary>
        /// Removes the file that is associated with the source name from the cache, if the file exists
        /// </summary>
        /// <param name="lpszUrlName"></param>
        /// <returns></returns>
        [DllImport(@"wininet",
            SetLastError = true,
            CharSet = CharSet.Auto,
            EntryPoint = "DeleteUrlCacheEntry",
            CallingConvention = CallingConvention.StdCall)]
        private static extern bool DeleteUrlCacheEntry(
            IntPtr lpszUrlName);

        [DllImport(@"wininet",
            SetLastError = true,
            CharSet = CharSet.Auto,
            EntryPoint = "FindCloseUrlCache",
            CallingConvention = CallingConvention.StdCall)]
        private static extern bool FindCloseUrlCache(
            IntPtr hFind);

        private enum Win32Error
        {

            /// <summary>
            ///  File not found.
            /// </summary>
            ERROR_FILE_NOT_FOUND = 0x2,

            /// <summary>
            /// There are no more files
            /// </summary>
            ERROR_NO_MORE_FILES = 0x12,

            /// <summary>
            /// The data area passed to a system call is too small.
            /// </summary>
            ERROR_INSUFFICIENT_BUFFER = 0x7A,

            /// <summary>
            /// No more data is available.
            /// </summary>
            ERROR_NO_MORE_ITEMS = 0x103,
        }

        private enum CacheGroupSearchFilter
        {
            /// <summary>
            /// Indicates that all of the cache groups in the user's system should be enumerated
            /// </summary>
            All = 0x0,
        }

        [Flags]
        private enum CacheGroupFlag
        {

            /// <summary>
            /// Indicates that all the cache entries that are associated with the cache group
            /// should be deleted, unless the entry belongs to another cache group.
            /// </summary>
            FlushUrlOnDelete = 0x2
        }

        /// <summary>
        /// Enumerates all url cache groups.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<long> UrlCacheGroups()
        {

            long cacheGroupId = 0;

            // Start the enumeration.
            var enumHandle = FindFirstUrlCacheGroup(0, CacheGroupSearchFilter.All, IntPtr.Zero, 0, ref cacheGroupId, IntPtr.Zero);

            // It is a valid case for the pointer to be zero if no filese were found. If the pointer is zero in other cases
            // something went wrong.
            if (enumHandle == IntPtr.Zero && Marshal.GetLastWin32Error() != (int)Win32Error.ERROR_NO_MORE_FILES)
                throw new Win32Exception("Unable to start enumerating url cache groups.");

            // We can't move to the next entry if the handle is null. This may be the case when no files were found
            // during the initial search. No cleanup is required.
            if (enumHandle == IntPtr.Zero)
                yield break;

            try
            {
                while (true)
                {

                    // Return the next cache group.
                    yield return cacheGroupId;

                    // Find the next url cache group.
                    var result = FindNextUrlCacheGroup(enumHandle, ref cacheGroupId, IntPtr.Zero);

                    // If the function returned false check if it is because no new files were found.
                    // In other cases something went wrong.
                    if (!result && ((int)Win32Error.ERROR_NO_MORE_ITEMS == Marshal.GetLastWin32Error()
                        || (int)Win32Error.ERROR_FILE_NOT_FOUND == Marshal.GetLastWin32Error()))
                        break;

                    else if (!result)
                        throw new Win32Exception("Unable to enumerate the next url cache group.");

                }

            }
            finally
            {

                // Release the enumeration handle.
                if (enumHandle != IntPtr.Zero)
                    FindCloseUrlCache(enumHandle);
            }
        }

        /// <summary>
        /// Enumerates all url cache entries.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<InternetCacheEntryInfoA> UrlCacheEntries()
        {

            int cacheEntryInfoBufferSize = 0;
            IntPtr cacheEntryInfoBuffer = IntPtr.Zero;
            InternetCacheEntryInfoA internetCacheEntry;

            // Start the enumeration. The first call to FindFirstUrlCacheEntry will return the required buffer size.
            var enumHandle = FindFirstUrlCacheEntry(null, IntPtr.Zero, ref cacheEntryInfoBufferSize);

            // It is a valid case for the pointer to be zero if no filese were found. No cleanup is required.
            if (enumHandle == IntPtr.Zero && Marshal.GetLastWin32Error() == (int)Win32Error.ERROR_NO_MORE_ITEMS)
                yield break;

            // We should be told that the buffer is not large enough. If this is not the case, something is wrong.
            if(enumHandle == IntPtr.Zero && Marshal.GetLastWin32Error() != (int)Win32Error.ERROR_INSUFFICIENT_BUFFER)
                throw new Win32Exception("Unable to start enumerating url cache entries.");

            // Allocate the desired buffer and try again. We should succeed this time.
            cacheEntryInfoBuffer = Marshal.AllocHGlobal(cacheEntryInfoBufferSize);
            enumHandle = FindFirstUrlCacheEntry(null, cacheEntryInfoBuffer, ref cacheEntryInfoBufferSize);
            if(enumHandle == IntPtr.Zero)
                throw new Win32Exception("Unable to start enumerating url cache entries.");

            try
            {
                while (true)
                {

                    // Dereference the entry buffer pointer and return the first result.
                    internetCacheEntry = (InternetCacheEntryInfoA)Marshal.PtrToStructure(cacheEntryInfoBuffer, typeof(InternetCacheEntryInfoA));
                    yield return internetCacheEntry;

                    // Find the next url cache entry.
                    var result = FindNextUrlCacheEntry(enumHandle, cacheEntryInfoBuffer, ref cacheEntryInfoBufferSize);

                    // If the function returned false check if it is because no new files were found.
                    if (!result && Marshal.GetLastWin32Error() == (int)Win32Error.ERROR_NO_MORE_ITEMS)
                        break;

                    // We may fail because the buffer is not large enough. If so, reallocate the buffer and try again.
                    if (!result && Marshal.GetLastWin32Error() == (int)Win32Error.ERROR_INSUFFICIENT_BUFFER)
                    {
                        cacheEntryInfoBuffer = Marshal.ReAllocHGlobal(cacheEntryInfoBuffer, (IntPtr)cacheEntryInfoBufferSize);
                        result = FindNextUrlCacheEntry(enumHandle, cacheEntryInfoBuffer, ref cacheEntryInfoBufferSize);
                    }

                    // If we failed again or failed in the first case due to a different cause, something is wrong.
                    if (!result)
                        throw new Win32Exception("Unable to enumerate the next url cache entry.");
                }

            }
            finally
            {

                // Release the enumeration handle and entry buffer.
                if (enumHandle != IntPtr.Zero)
                    FindCloseUrlCache(enumHandle);
                if (cacheEntryInfoBuffer != IntPtr.Zero)
                    Marshal.FreeHGlobal(cacheEntryInfoBuffer);
            }
        }

        /// <summary>
        /// Clears the WinInet cache.
        /// </summary>
        public static void ClearCache()
        {

            // Delete the groups first.
            // Groups may not always exist on the system.
            // For more information, visit the following Microsoft Web site:
            // http://msdn.microsoft.com/library/?url=/workshop/networking/wininet/overview/cache.asp            
            // By default, a URL does not belong to any group. Therefore, that cache may become
            // empty even when the CacheGroup APIs are not used because the existing URL does not belong to any group.
            foreach (var cacheGroupId in UrlCacheGroups())
            {
                DeleteUrlCacheGroup(cacheGroupId, CacheGroupFlag.FlushUrlOnDelete, IntPtr.Zero);
            }

            // Start to delete URLs that do not belong to any group.
            foreach (var sourceUrlPtry in UrlCacheEntries())
            {
                DeleteUrlCacheEntry(sourceUrlPtry.lpszSourceUrlName);
            }
        }
    }

    /// <summary>
    /// Contains information about an url entry in the WinInet cache.
    /// </summary>
    /// <remarks>
    /// Stripped the fixed field offsets and size as they may vary between 32 bit and 64 bit machines.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct InternetCacheEntryInfoA
    {

        public uint dwStructSize;

        public IntPtr lpszSourceUrlName;

        public IntPtr lpszLocalFileName;

        public uint CacheEntryType;

        public uint dwUseCount;

        public uint dwHitRate;

        public uint dwSizeLow;

        public uint dwSizeHigh;

        public System.Runtime.InteropServices.ComTypes.FILETIME LastModifiedTime;

        public System.Runtime.InteropServices.ComTypes.FILETIME ExpireTime;

        public System.Runtime.InteropServices.ComTypes.FILETIME LastAccessTime;

        public System.Runtime.InteropServices.ComTypes.FILETIME LastSyncTime;

        public IntPtr lpHeaderInfo;

        public uint dwHeaderInfoSize;

        public IntPtr lpszFileExtension;

        public uint dwExemptDeltaOrReserved;
    }
}

