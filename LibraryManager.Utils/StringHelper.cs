using System;
using System.Text;
using System.Text.RegularExpressions;

namespace LibraryManager.Utils
{
   /// <summary>
   /// Hỗ trợ format, kiểm tra chuỗi
   /// </summary>
   public static class StringHelper
   {
      /// <summary>
      /// Kiểm tra định dạng chuỗi có phải là địa chỉ email hay không ?
      /// </summary>
      /// <param name="str">Chuỗi cần kiểm tra</param>
      /// <returns>True nếu là địa chỉ email hợp lệ, ngược lại là False</returns>
      public static bool IsEmail(string str)
      {
         if (str.Length < 5) { return false; }
         //Có nhiều ký tự @
         if (str.IndexOf("@") != str.LastIndexOf("@")) { return false; }
         //Không có ký tự @
         if (str.IndexOf("@") == -1) { return false; }
         //Không có dấu . phía sau @
         if (str.IndexOf(".") < str.IndexOf("@")) { return false; }

         return true;
      }

      /// <summary>
      /// Kiểm tra định dạng chuỗi "str" có phải là số điện thoại
      /// Số điện thoại đúng : có ít nhất 10 số, kí tự đầu tiên có thể là số hoặc dấu "+"
      /// </summary>
      /// <param name="str"></param>
      /// <returns></returns>
      public static bool IsPhone(string str)
      {
         if (str.Length < 10) { return false; }

         if (!(str[0] == '+' && char.IsDigit(str[0]))) { return false; }

         for (int i = 1; i < str.Length; i++) { if (!char.IsDigit(str[i])) { return false; } }

         return true;
      }
   }
}