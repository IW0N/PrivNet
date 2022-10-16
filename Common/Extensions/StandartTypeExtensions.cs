using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Extensions
{
    public static class StandartTypeExtensions
    {
        public static string ToBase64(this byte[] bytes)
        => bytes!=null&&bytes.Length>0?Convert.ToBase64String(bytes):"";
        public static byte[] FromBase64(this string? base64)=>
            !string.IsNullOrEmpty(base64)?Convert.FromBase64String(base64):null;
        public static T ChangeType<T>(this object val) => (T)Convert.ChangeType(val, typeof(T));
        public static void CopyToWithOffset(this byte[] mainArr, byte[] arr,int offset)
        {
           
            for (int i=0;i<mainArr.Length;i++)
            {
                arr[i+offset] = mainArr[i];
            }
            
        }
    }
}
