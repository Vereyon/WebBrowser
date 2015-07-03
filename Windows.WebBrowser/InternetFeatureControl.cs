using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Windows.WebBrowser
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// Based on: http://msdn.microsoft.com/en-us/library/ee330732(v=vs.85).aspx
    /// </remarks>
    public class InternetFeatureControl
    {

        public const string FeatureControlKey = "SOFTWARE\\Microsoft\\Internet Explorer\\Main\\FeatureControl";

        /// <summary>
        /// Returns the path of the executable that started this application.
        /// </summary>
        /// <returns></returns>
        public static string ExecutablePath()
        {

            string executableName;

            executableName = Path.GetFileName(Application.ExecutablePath);
            return executableName;
        }

        
        /// <summary>
        /// Opens or creates the registry key controlling the specified feature.
        /// </summary>
        /// <param name="featureName"></param>
        /// <returns></returns>
        public static RegistryKey OpenFeatureControlKey(string featureName)
        {
            var key = FeatureControlKey + "\\" + featureName;
            return Registry.CurrentUser.CreateSubKey(key);
        }

        public static void SetBrowserEmulation(BrowserEmulationMode mode)
        {
            SetBrowserEmulation(ExecutablePath(), mode);
        }

        /// <summary>
        /// Sets the IE browser emaulation mode for the specified executable.
        /// </summary>
        /// <param name="executableName"></param>
        /// <param name="mode"></param>
        public static void SetBrowserEmulation(string executableName, BrowserEmulationMode mode)
        {

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION"))
            {
                if (key == null)
                    throw new IOException("Unable to open FEATURE_BROWSER_EMULATION registry key for writing.");
                key.SetValue(executableName, mode, RegistryValueKind.DWord);
            }
        }

        public static void SetGpuRendering(bool enabled)
        {
            SetGpuRendering(ExecutablePath(), enabled);
        }

        /// <summary>
        /// Internet Explorer 9. The FEATURE_GPU_RENDERING feature enables Internet Explorer to use a graphics processing unit (GPU) to render content. This dramatically improves performance for webpages that are rich in graphics.
        /// By default, this feature is enabled for Internet Explorer and disabled for applications hosting the WebBrowser Control.
        /// </summary>
        /// <param name="executableName"></param>
        /// <param name="enabled"></param>
        public static void SetGpuRendering(string executableName, bool enabled)
        {

            int dwValue;

            if (enabled)
                dwValue = 0x00000001;
            else
                dwValue = 0x00000000;

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_GPU_RENDERING"))
            {
                if (key == null)
                    throw new IOException("Unable to open FEATURE_GPU_RENDERING registry key for writing.");
                key.SetValue(executableName, dwValue, RegistryValueKind.DWord);
            }
        }

        public static bool GetGpuRendering(string executableName)
        {

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_GPU_RENDERING"))
            {
                if (key == null)
                    throw new IOException("Unable to open FEATURE_GPU_RENDERING registry key for writing.");
                var value = key.GetValue(executableName);
                if (value == null)
                    return false;
                if(value is int)
                    return ((int)value) == 1;
                return false;
            }
        }
    }

    /// <summary>
    /// Windows Internet Explorer 8 and later. The FEATURE_BROWSER_EMULATION feature defines the default emulation mode for Internet Explorer and supports the following values.
    /// </summary>
    public enum BrowserEmulationMode : long
    {

        /// <summary>
        /// Internet Explorer 11. Webpages are displayed in IE11 edge mode, regardless of the !DOCTYPE directive. 
        /// </summary>
        Ie11Force = 0x2af9,

        /// <summary>
        /// IE11. Webpages containing standards-based !DOCTYPE directives are displayed in IE11 edge mode. Default value for IE11.
        /// </summary>
        Ie11FollowDoctype = 0x2af8,

        /// <summary>
        /// Internet Explorer 10. Webpages are displayed in IE10 Standards mode, regardless of the !DOCTYPE directive. 
        /// </summary>
        Ie10Force = 0x2711,

        /// <summary>
        /// Internet Explorer 10. Webpages containing standards-based !DOCTYPE directives are displayed in IE10 Standards mode. Default value for Internet Explorer 10.
        /// </summary>
        Ie10FollowDoctype = 0x02710,

        /// <summary>
        /// Windows Internet Explorer 9. Webpages are displayed in IE9 Standards mode, regardless of the !DOCTYPE directive.
        /// </summary>
        Ie9Force = 0x270f,

        /// <summary>
        /// Internet Explorer 9. Webpages containing standards-based !DOCTYPE directives are displayed in IE9 mode. Default value for Internet Explorer 9.
        /// </summary>
        Ie9FollowDoctype = 0x2328,
    }
}
