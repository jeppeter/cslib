using System;
using System.IO;
using System.Runtime.InteropServices;

[Flags]
public enum LoadLibraryFlags : uint
{
    NoFlags = 0x00000000,
    DontResolveDllReferences = 0x00000001,
    LoadIgnoreCodeAuthzLevel = 0x00000010,
    LoadLibraryAsDatafile = 0x00000002,
    LoadLibraryAsDatafileExclusive = 0x00000040,
    LoadLibraryAsImageResource = 0x00000020,
    LoadWithAlteredSearchPath = 0x00000008
}

class Program
{
    private const string Kernel32LibraryName = "kernel32.dll";
    [DllImportAttribute(Kernel32LibraryName)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool FreeLibrary(IntPtr hModule);
    public static IntPtr LoadLibrary(string lpFileName)
    {
        return LoadLibraryEx(lpFileName, 0, LoadLibraryFlags.NoFlags);
    }

    [DllImportAttribute(Kernel32LibraryName, SetLastError = true)]
    public static extern IntPtr LoadLibraryEx(String fileName, int hFile, LoadLibraryFlags dwFlags);

    [DllImport("dbgeng.dll")]
    internal static extern int DebugCreate(ref Guid InterfaceId, [MarshalAs(UnmanagedType.IUnknown)] out object Interface);
    [DllImport(Kernel32LibraryName)]
    static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

    static void Main(string[] args)
    {
        IntPtr dbgeng = LoadLibrary(Path.Combine(Environment.SystemDirectory, "dbgeng.dll"));
        if (dbgeng == IntPtr.Zero)
            throw new Exception();

        IntPtr debugCreate = GetProcAddress(dbgeng, "DebugCreate");
        if (debugCreate == IntPtr.Zero)
            throw new Exception();

        FreeLibrary(dbgeng);

        Guid guid = new Guid("27fe5639-8407-4f47-8364-ee118fb08ac8");
        object obj;
        int res = DebugCreate(ref guid, out obj);
        if (res != 0){
            throw new Exception();
        }
        Console.Out.WriteLine("call DebugCreate success");
    }
}