namespace LibraryManager.EntityFramework
{
   public interface IObjectManager
   {
      /// <summary>
      /// Thay đổi object được chọn trên listview
      /// </summary>
      System.Windows.Input.ICommand ObjectSelectedChangedCommand { get; set; }

      /// <summary>
      /// Thêm đối tượng mới
      /// </summary>
      System.Windows.Input.ICommand AddCommand { get; set; }

      /// <summary>
      /// Cập nhật thông tin đối tượng
      /// </summary>
      System.Windows.Input.ICommand UpdateCommand { get; set; }

      /// <summary>
      /// Thay đổi trạng thái (status) của đối tượng
      /// </summary>
      System.Windows.Input.ICommand StatusChangeCommand { get; set; }

      /// <summary>
      /// Xóa đối tượng
      /// </summary>
      System.Windows.Input.ICommand DeleteCommand { get; set; }
   }
}