namespace LibraryManager.EntityFramework
{
   public interface IAddNewObject<ObjectType>
   {
      /// <summary>
      /// Kết quả
      ///     return new object được thêm vào
      ///     null nếu không có object nào được thêm
      /// </summary>
      ObjectType Result { get; set; }

      System.Windows.Input.ICommand OKCommand { get; set; }
      System.Windows.Input.ICommand CancelCommand { get; set; }
   }
}