using LibraryManager.EntityFramework.Model;
using LibraryManager.Utility;
using System.Collections.ObjectModel;

namespace LibraryManager.EntityFramework.ViewModel
{
   public class PageMemberBorrowListVM : BaseViewModel
   {
      public ObservableCollection<BorrowDTO> ListBookBorrow { get => listBookBorrow; set { listBookBorrow = value; OnPropertyChanged(); } }

      public PageMemberBorrowListVM(string memberId)
      {
         ListBookBorrow = BorrowDAL.Instance.GetListByMemberId(memberId);
      }

      private ObservableCollection<BorrowDTO> listBookBorrow;
   }
}