using System;
using System.Runtime.InteropServices;
using System.Reflection.Emit;

using DBus;

namespace DBus.DBusType
{
  /// <summary>
  /// 64-bit unsigned integer.
  /// </summary>
  public class UInt64 : IDBusType
  {
    public const char Code = 't';
    private System.UInt64 val;
    
    private UInt64()
    {
    }
    
    public UInt64(System.UInt64 val, Service service) 
    {
      this.val = val;
    }

    public UInt64(IntPtr iter, Service service)
    {
      this.val = dbus_message_iter_get_uint64(iter);
    }
    
    public void Append(IntPtr iter)
    {
      if (!dbus_message_iter_append_uint64(iter, val))
	throw new ApplicationException("Failed to append UINT64 argument:" + val);
    }

    public static bool Suits(System.Type type) 
    {
      switch (type.ToString()) {
      case "System.UInt64":
      case "System.UInt64&":
	return true;
      }
      
      return false;
    }

    public static void EmitMarshalIn(ILGenerator generator, Type type)
    {
      if (type.IsByRef) {
	generator.Emit(OpCodes.Ldind_I8);
      }
    }

    public static void EmitMarshalOut(ILGenerator generator, Type type, bool isReturn) 
    {
      generator.Emit(OpCodes.Unbox, type);
      generator.Emit(OpCodes.Ldind_I8);
      if (!isReturn) {
	generator.Emit(OpCodes.Stind_I8);
      }
    }
    
    public object Get() 
    {
      return this.val;
    }

    public object Get(System.Type type)
    {
      switch (type.ToString()) 
	{
	case "System.UInt64":
	case "System.UInt64&":
	  return this.val;
	default:
	  throw new ArgumentException("Cannot cast DBus.Type.UInt64 to type '" + type.ToString() + "'");
	}
    }    

    [DllImport("dbus-1")]
    private extern static System.UInt64 dbus_message_iter_get_uint64(IntPtr iter);
 
    [DllImport("dbus-1")]
    private extern static bool dbus_message_iter_append_uint64(IntPtr iter, System.UInt64 value);
  }
}