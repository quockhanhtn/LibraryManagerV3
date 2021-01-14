using System;
using System.Text;
using System.Text.RegularExpressions;

namespace LibraryManager.Utility
{
   /// <summary>
   /// Hỗ trợ format, kiểm tra chuỗi
   /// </summary>
   public static class StringHelper
   {
      /// <summary>
      /// Chuyển chuỗi thành decimal
      /// </summary>
      /// <param name="str">Chuỗi cần chuyển thành decimal</param>
      /// <returns>0 nếu không chuyển được, giá trị của chuỗi nếu chuyển thành công</returns>
      public static decimal ToDecimal(string str)
      {
         decimal result = 0;
         decimal.TryParse(str, out result);
         return result;
      }

      /// <summary>
      /// Chuyển chuỗi thành int
      /// </summary>
      /// <param name="str">Chuỗi cần chuyển thành int</param>
      /// <returns>0 nếu không chuyển được, giá trị của chuỗi nếu chuyển thành công</returns>
      public static int ToInt(string str)
      {
         int result = 0;
         int.TryParse(str, out result);
         return result;
      }

      public static int StringToInt(string str)
      {
         int intNumber;
         if (!int.TryParse(str, out intNumber))
         {
            for (int i = 0; i < str.Length; i++)
            {
               if (!char.IsDigit(str[i]))
               {
                  str = str.Remove(i, 1);
                  i--;
               }
            }
         }

         if (!int.TryParse(str, out intNumber)) { intNumber = 0; }
         return intNumber;
      }

      public static double StringToDouble(string str)
      {
         double doubleNumber;
         if (!double.TryParse(str, out doubleNumber))
         {
            while (str.IndexOf(".") != str.LastIndexOf("."))
            {
               str = str.Remove(str.LastIndexOf("."), 1);
            }
            for (int i = 0; i < str.Length; i++)
            {
               if ((!char.IsDigit(str[i])) && (str[i] != '.'))
               {
                  str = str.Remove(i, 1);
                  i--;
               }
            }
         }

         if (!double.TryParse(str, out doubleNumber)) { doubleNumber = 0; }
         return doubleNumber;
      }

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