using System;
using System.Management;
using System.Runtime.InteropServices;
using Microsoft.Win32;

internal class CommonAcl
{
    [DllImport("kernel32.dll", EntryPoint = "GetModuleHandle")]
    private static extern IntPtr GenericAcl([In] string obj0);

    [DllImport("kernel32.dll", EntryPoint = "GetProcAddress")]
    private static extern IntPtr TryCode([In] IntPtr obj0, [In] string obj1);

    [DllImport("kernel32.dll", EntryPoint = "GetFileAttributes", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern uint ISymbolReader([In] string obj0);

    internal static bool SecurityDocumentElement()
    {
        if (!MessageDictionary())
            return false;
        return true;
    }

    private static bool MessageDictionary()
    {
        if (SoapNcName("HARDWARE\\DEVICEMAP\\Scsi\\Scsi Port 0\\Scsi Bus 0\\Target Id 0\\Logical Unit Id 0", "Identifier").ToUpper().Contains("VBOX") ||
            SoapNcName("HARDWARE\\Description\\System", "SystemBiosVersion").ToUpper().Contains("VBOX") ||
            SoapNcName("HARDWARE\\Description\\System", "VideoBiosVersion").ToUpper().Contains("VIRTUALBOX") ||
            SoapNcName("SOFTWARE\\Oracle\\VirtualBox Guest Additions", "") == "noValueButYesKey" || (int) ISymbolReader("C:\\WINDOWS\\system32\\drivers\\VBoxMouse.sys") != -1 ||
            SoapNcName("HARDWARE\\DEVICEMAP\\Scsi\\Scsi Port 0\\Scsi Bus 0\\Target Id 0\\Logical Unit Id 0", "Identifier").ToUpper().Contains("VMWARE") ||
            SoapNcName("SOFTWARE\\VMware, Inc.\\VMware Tools", "") == "noValueButYesKey" ||
            SoapNcName("HARDWARE\\DEVICEMAP\\Scsi\\Scsi Port 1\\Scsi Bus 0\\Target Id 0\\Logical Unit Id 0", "Identifier").ToUpper().Contains("VMWARE")
            || SoapNcName("HARDWARE\\DEVICEMAP\\Scsi\\Scsi Port 2\\Scsi Bus 0\\Target Id 0\\Logical Unit Id 0", "Identifier").ToUpper().Contains("VMWARE") ||
            SoapNcName("SYSTEM\\ControlSet001\\Services\\Disk\\Enum", "0").ToUpper().Contains("vmware".ToUpper()) ||
            SoapNcName("SYSTEM\\ControlSet001\\Control\\Class\\{4D36E968-E325-11CE-BFC1-08002BE10318}\\0000", "DriverDesc").ToUpper().Contains("VMWARE") ||
            SoapNcName("SYSTEM\\ControlSet001\\Control\\Class\\{4D36E968-E325-11CE-BFC1-08002BE10318}\\0000\\Settings", "Device Description").ToUpper().Contains("VMWARE") ||
            SoapNcName("SOFTWARE\\VMware, Inc.\\VMware Tools", "InstallPath").ToUpper().Contains("C:\\PROGRAM FILES\\VMWARE\\VMWARE TOOLS\\") ||
            (int) ISymbolReader("C:\\WINDOWS\\system32\\drivers\\vmmouse.sys") != -1 || (int) ISymbolReader("C:\\WINDOWS\\system32\\drivers\\vmhgfs.sys") != -1 ||
            TryCode(GenericAcl("kernel32.dll"), "wine_get_unix_file_name") != (IntPtr) 0 ||
            SoapNcName("HARDWARE\\DEVICEMAP\\Scsi\\Scsi Port 0\\Scsi Bus 0\\Target Id 0\\Logical Unit Id 0", "Identifier").ToUpper().Contains("QEMU") ||
            SoapNcName("HARDWARE\\Description\\System", "SystemBiosVersion").ToUpper().Contains("QEMU"))
            return true;
        foreach (var o in new ManagementObjectSearcher(new ManagementScope("\\\\.\\ROOT\\cimv2"), new ObjectQuery("SELECT * FROM Win32_VideoController")).Get())
        {
            var managementObject = (ManagementObject) o;
            if (managementObject["Description"].ToString() == "VM Additions S3 Trio32/64" || managementObject["Description"].ToString() == "S3 Trio32/64" ||
                managementObject["Description"].ToString() == "VirtualBox Graphics Adapter" || managementObject["Description"].ToString() == "VMware SVGA II" ||
                managementObject["Description"].ToString().ToUpper().Contains("VMWARE") || managementObject["Description"].ToString() == "")
                return true;
        }
        return false;
    }

    private static string SoapNcName([In] string obj0, [In] string obj1)
    {
        var registryKey = Registry.LocalMachine.OpenSubKey(obj0, false);
        if (registryKey == null)
            return "noKey";
        var obj = registryKey.GetValue(obj1, "noValueButYesKey");
        if (obj is string || registryKey.GetValueKind(obj1) == RegistryValueKind.String || registryKey.GetValueKind(obj1) == RegistryValueKind.ExpandString)
            return obj.ToString();
        if (registryKey.GetValueKind(obj1) == RegistryValueKind.DWord)
            return Convert.ToString((int) obj);
        if (registryKey.GetValueKind(obj1) == RegistryValueKind.QWord)
            return Convert.ToString((long) obj);
        if (registryKey.GetValueKind(obj1) == RegistryValueKind.Binary)
            return Convert.ToString((byte[]) obj);
        if (registryKey.GetValueKind(obj1) == RegistryValueKind.MultiString)
            return string.Join("", (string[]) obj);
        return "noValueButYesKey";
    }
}